using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostLaser : MonoBehaviour {

    // Stripped down version of laserLogic with some changes

    // Configurable
    public float laserWidth = 0.05f;
    public float laserVisualHeight = 0.4f;
    public float laserTransparency = 0.5f;
    public Material laserMaterialGhostP1;
    public Material laserMaterialGhostP2;

    private int iterationLimit = 300;
    private int iterationCount = 0;
    private int laserLimit = 100;
    private int laserIndex = 0;
    private int laserCounter = 0;
    private List<List<laserNode>> lasers;   // Laser list for line rendering
    private List<laserNode> laserQueue;
    private laserGrid ghostLaserData;
    private Dictionary<XY, List<dirHeadPlayer>> refractHits;    // Refract collision dictionary
    private GameObject laserContainer;

    public static bool ghostUpdateNeeded = false;
    public static bool updateGhostLaser = false;

    private struct laserNode
    {
        private int X;
        private int Y;
        private float strength;
        private Direction laserHeading;
        private Direction marchDirection;
        private Player owner;
        private int laserIndex;
        private int laserSubIndex;

        public laserNode(int x, int y, float laserPower, Direction laserDirection, Direction marchDir, Player ownedBy, int index, int subIndex)
        {
            X = x;
            Y = y;
            strength = laserPower;
            laserHeading = laserDirection;
            marchDirection = marchDir;
            owner = ownedBy;
            laserIndex = index;
            laserSubIndex = subIndex;
        }

		private Building getBuilding(int x, int y, Player player = Player.World, bool flag = true)
        {
            if (player == Player.PlayerOne && inputController.cursorP1.x == x && inputController.cursorP1.y == y && validCursorBuilding(Player.PlayerOne)) return getCursorBuilding(Player.PlayerOne);
            if (player == Player.PlayerTwo && inputController.cursorP2.x == x && inputController.cursorP2.y == y && validCursorBuilding(Player.PlayerTwo)) return getCursorBuilding(Player.PlayerTwo);
			return gridManager.theGrid.getBuilding(x, y, flag);
        }

        // Get
        public int getX() { return X; }
        public int getY() { return Y; }
        public int getIndex() { return laserIndex; }
        public int getSubIndex() { return laserSubIndex; }
        public float getStrength() { return strength; }
        public Direction getHeading() { return laserHeading; }
        public Direction getMarchDir() { return marchDirection; }
        public Player getOwner() { return owner; }
		public bool onRedirect(Player player) { return getBuilding(X, Y, player, false) == Building.Redirecting; }
        // Set
        public void combineLasers(float str, Player laserOwner)
        {
            if (owner != laserOwner) owner = Player.Shared;
            strength += str;
        }

        public string toString()
        {
            return "Coords: (" + X + ", " + Y + ")  |  Strength: " + strength + "  |  Heading: " + laserHeading + "  |  MarchDirection: " + marchDirection + "  |  Owner: " + owner;
        }
    }

    public static Direction opposite(Direction dir)
    {
        switch (dir) {
            case Direction.NW: return Direction.SE;
            case Direction.NE: return Direction.SW;
            case Direction.SW: return Direction.NE;
            case Direction.SE: return Direction.NW;
            case Direction.Up: return Direction.Down;
            case Direction.Down: return Direction.Up;
            case Direction.Left: return Direction.Right;
            case Direction.Right: return Direction.Left;
        }
        return Direction.None;
    }

    private struct laserGrid
    {
        private List<laserNode>[,] grid;
        private int dimX;
        private int dimY;

        public laserGrid(int x, int y)
        {
            grid = new List<laserNode>[y, x];
            for (int row = 0; row < y; row++) {
                for (int col = 0; col < x; col++) {
                    grid[row, col] = new List<laserNode>();
                }
            }
            dimX = x;
            dimY = y;
        }

        private bool validateInput(int x, int y)
        {
            if (x < 0 || y < 0 || x >= dimX || y >= dimY) return false;
            return true;
        }


        public bool insertNode(int x, int y, float str, Direction heading, Direction marchDir, Player laserOwner, int indx, int subIndx)
        {
            if (!validateInput(x, y)) return false;
            grid[y, x].Add(new laserNode(x, y, str, heading, marchDir, laserOwner, indx, subIndx));
            return true;
        }

        public bool checkOpposing(int x, int y, Direction heading, Direction marchDir)
        {
            if (!validateInput(x, y)) return false;
            for (int i = 0; i < grid[y, x].Count; i++) {
                if (grid[y, x][i].getHeading() == opposite(heading) && grid[y, x][i].getMarchDir() != opposite(marchDir)) return true;// new int[] { grid[y, x][i].getIndex(), grid[y, x][i].getSubIndex() };
            }
            return false;
        }

        public int[] checkOverlapping(int x, int y, float str, Direction heading, Direction marchDir, Player laserOwner)
        {
            if (!validateInput(x, y)) return null;
            for (int i = 0; i < grid[y, x].Count; i++) {
                if (grid[y, x][i].getHeading() == heading && grid[y, x][i].getMarchDir() == marchDir) {
                    grid[y, x][i].combineLasers(str, laserOwner);
                    return new int[] { grid[y, x][i].getIndex(), grid[y, x][i].getSubIndex() };
                }
            }
            return null;
        }

        public int getDimX() { return dimX; }
        public int getDimY() { return dimY; }


    }


    // For storing when lasers hit resource or blocking blocks, base
    struct laserHit
    {
        public int X;
        public int Y;
        public bool weakSideHit;
        public float laserStrength;
        public Building buildingHit;
        public Player buildingOwner;

        public laserHit(int x, int y, bool weakHit, float laserPower, Building buildingType, Player buildingOwnedBy)
        {
            X = x;
            Y = y;
            weakSideHit = weakHit;
            laserStrength = laserPower;
            buildingHit = buildingType;
            buildingOwner = buildingOwnedBy;
        }
    }

    // Used for refraction loop checks
    public struct dirHeadPlayer
    {
        public Direction direction;
        public Direction heading;
        public Player player;

        public dirHeadPlayer(Direction d, Direction h, Player p)
        {
            direction = d;
            heading = h;
            player = p;
        }

    }

    // Used for refraction loop checks
    public bool opposites(Direction d1, Direction d2)
    {
        if (d1 == d2) return false;
        switch (d1) {
            case Direction.Up: return d2 == Direction.Down;
            case Direction.Down: return d2 == Direction.Up;
            case Direction.Left: return d2 == Direction.Right;
            case Direction.Right: return d2 == Direction.Left;
        }
        return false;
    }

    // Used for refraction loop checks
    public bool headingsOpposite(Direction h1, Direction h2)
    {
        if (h1 == h2) return false;
        switch (h1) {
            case Direction.NW: return h2 == Direction.SE;
            case Direction.NE: return h2 == Direction.SW;
            case Direction.SW: return h2 == Direction.NE;
            case Direction.SE: return h2 == Direction.NW;
        }
        return false;
    }

    // Gets the direction of a laser coming out of a refraction block
    public Direction getExit(Direction direction, Direction heading)
    {
        switch (direction) {
            case Direction.Up: return heading == Direction.NE ? Direction.Right : Direction.Left;
            case Direction.Down: return heading == Direction.SE ? Direction.Right : Direction.Left;
            case Direction.Right: return heading == Direction.NE ? Direction.Up : Direction.Down;
            case Direction.Left: return heading == Direction.NW ? Direction.Up : Direction.Down;
        }
        return direction;
    }

    void Awake()
    {
        laserIndex = -1;
        lasers = new List<List<laserNode>>();
        laserQueue = new List<laserNode>();
        ghostLaserData = new laserGrid(gridManager.theGrid.getDimX(), gridManager.theGrid.getDimY());
        laserContainer = new GameObject("ghostLaserContainer");
        laserContainer.transform.SetParent(gameObject.transform);
        refractHits = new Dictionary<XY, List<dirHeadPlayer>>();

        for (int i = 0; i < laserLimit; i++) {
            // Line object pool
            GameObject lineObject = new GameObject("lineObject");
            lineObject.transform.SetParent(laserContainer.transform);
            LineRenderer line = lineObject.AddComponent<LineRenderer>();
            line.startWidth = laserWidth;
            line.endWidth = laserWidth;
            line.numCapVertices = 0;
            line.material = laserMaterialGhostP1;
            line.enabled = false;
        }
    }

    void Update()
    {
        if (updateGhostLaser) simulateLasers(); // Update ghost laser if needed
    }

    private void drawLaser(laserNode start, laserNode end)
    {
        Vector3 startPos = coordToPos(start.getX(), start.getY(), start.getHeading(), start.getMarchDir(), true);

        if (((start.getOwner() == Player.PlayerOne && start.getX() == inputController.cursorP1.x && start.getY() == inputController.cursorP1.y && getCursorBuilding(Player.PlayerOne) == Building.Redirecting) || (start.getOwner() == Player.PlayerTwo && start.getX() == inputController.cursorP2.x && start.getY() == inputController.cursorP2.y && getCursorBuilding(Player.PlayerTwo) == Building.Redirecting))) {
            Direction dir = Direction.Up;
            switch (start.getMarchDir()) {
                case Direction.Down: dir = start.getHeading() == Direction.SE ? Direction.Right : Direction.Left; break;
                case Direction.Up: dir = start.getHeading() == Direction.NE ? Direction.Right : Direction.Left; break;
                case Direction.Left: dir = start.getHeading() == Direction.NW ? Direction.Up : Direction.Down; break;
                case Direction.Right: dir = start.getHeading() == Direction.NE ? Direction.Up : Direction.Down; break;
            }

            startPos = coordToPos(start.getX(), start.getY(), start.getHeading(), dir, true);
        }
        Vector3 endPos = coordToPos(end.getX(), end.getY(), end.getHeading(), end.getMarchDir(), false);
        Vector3 heightOffset = new Vector3(0, laserCounter * -0.01f, 0);
        laserContainer.transform.GetChild(laserCounter).GetComponent<LineRenderer>().SetPosition(0, startPos + heightOffset);
        laserContainer.transform.GetChild(laserCounter).GetComponent<LineRenderer>().SetPosition(1, endPos + heightOffset);
        laserContainer.transform.GetChild(laserCounter).GetComponent<LineRenderer>().enabled = true;
        laserContainer.transform.GetChild(laserCounter).GetComponent<LineRenderer>().material = start.getOwner() == Player.PlayerOne ? laserMaterialGhostP1 : laserMaterialGhostP2;
        laserContainer.transform.GetChild(laserCounter).GetComponent<LineRenderer>().startColor = new Color(1, 1, 1, laserTransparency);
        laserContainer.transform.GetChild(laserCounter).GetComponent<LineRenderer>().endColor = new Color(1, 1, 1, laserTransparency);
        laserCounter++;
    }

    private Vector3 coordToPos(int x, int y, Direction heading, Direction direction, bool start)
    {
        float xOff = 0, yOff = 0;

        if (start) {
			if (getBuilding(x, y, Player.World, false) == Building.Redirecting) {
                switch (heading) {
                    case Direction.NE: { if (direction == Direction.Right) { xOff = 0.5f; yOff = 0f; } else { xOff = 0f; yOff = 0.5f; } break; }
                    case Direction.NW: { if (direction == Direction.Left) { xOff = 0.5f; yOff = 0f; } else { xOff = 1f; yOff = 0.5f; } break; }
                    case Direction.SE: { if (direction == Direction.Right) { xOff = 0.5f; yOff = 1f; } else { xOff = 0f; yOff = 0.5f; } break; }
                    case Direction.SW: { if (direction == Direction.Left) { xOff = 0.5f; yOff = 1f; } else { xOff = 1f; yOff = 0.5f; } break; }
                }
            } else
                switch (heading) {
                    case Direction.NE: { if (direction == Direction.Right) { yOff += 0.5f; } else { xOff += 0.5f; } break; }
                    case Direction.SE: { if (direction == Direction.Right) { yOff += 0.5f; } else { xOff += 0.5f; yOff++; } break; }
                    case Direction.NW: { if (direction == Direction.Up) { xOff += 0.5f; } else { xOff++; yOff += 0.5f; } break; }
                    case Direction.SW: { if (direction == Direction.Down) { xOff += 0.5f; yOff++; } else { xOff++; yOff += 0.5f; } break; }
                }
        } else {
			if (getBuilding(x, y, Player.World, false) == Building.Redirecting) {
                switch (heading) {
                    case Direction.NE: { if (direction == Direction.Right) { xOff = 0.5f; yOff = 1f; } else { xOff = 1f; yOff = 0.5f; } break; }
                    case Direction.NW: { if (direction == Direction.Left) { xOff = 0.5f; yOff = 1f; } else { xOff = 0f; yOff = 0.5f; } break; }
                    case Direction.SE: { if (direction == Direction.Right) { xOff = 0.5f; yOff = 0f; } else { xOff = 1f; yOff = 0.5f; } break; }
                    case Direction.SW: { if (direction == Direction.Left) { xOff = 0.5f; yOff = 0f; } else { xOff = 0f; yOff = 0.5f; } break; }
                }
            } else
                switch (heading) {
                    case Direction.NE: { if (direction == Direction.Right) { xOff += 0.5f; yOff++; } else { xOff++; yOff += 0.5f; } break; }
                    case Direction.SE: { if (direction == Direction.Right) { xOff += 0.5f; } else { xOff++; yOff += 0.5f; } break; }
                    case Direction.NW: { if (direction == Direction.Up) { yOff += 0.5f; } else { xOff += 0.5f; yOff++; } break; }
                    case Direction.SW: { if (direction == Direction.Down) { yOff += 0.5f; } else { xOff += 0.5f; } break; }
                }
        }

        return new Vector3(x - (gridManager.theGrid.getDimX() / 2) + xOff, laserVisualHeight, y - (gridManager.theGrid.getDimY() / 2) + yOff);

    }

    private Vector3 directionToEular(Direction direction)
    {
        switch (direction) {
            case Direction.Left: return new Vector3(0, 90, 0);
            case Direction.Up: return new Vector3(0, 180, 0);
            case Direction.Right: return new Vector3(0, 270, 0);
        }
        return new Vector3(0, 0, 0);
    }

    private void simulateLasers()
    {
        // Reset laser data grid, causes ocasional garbage collection spike
        ghostLaserData = new laserGrid(gridManager.theGrid.getDimX(), gridManager.theGrid.getDimY());

        // Reset counters and check for placed lasers
        iterationCount = 0; laserIndex = -1;

        // Clear old lasers before starting again
        for (int i = 0; i < lasers.Count; i++) lasers[i].Clear();
        for (int i = 0; i < laserCounter; i++) laserContainer.transform.GetChild(i).GetComponent<LineRenderer>().enabled = false;
        laserCounter = 0;

        // Clear laser hits, refract hits
        refractHits.Clear();

        // Check p1 cursor if ghost laser simulation is necessary
		if (validCursorBuilding (Player.PlayerOne) && inputController.validPlacement (inputController.cursorP1.x, inputController.cursorP1.y, inputController.cursorP1.direction, getCursorBuilding (Player.PlayerOne)) && laserLogic.laserData.grid.GetLength (0) > 0)
			for (int i = 0; i < laserLogic.laserData.grid[inputController.cursorP1.y, inputController.cursorP1.x].Count; i++) {
                if (getCursorBuilding(Player.PlayerOne) == Building.Redirecting)
                    laserRedirect(inputController.cursorP1.x, inputController.cursorP1.y, 1f, laserLogic.laserData.grid[inputController.cursorP1.y, inputController.cursorP1.x][i].getHeading(), laserLogic.laserData.grid[inputController.cursorP1.y, inputController.cursorP1.x][i].getMarchDir(), Player.PlayerOne, 0, 0);
                else laserReflect(inputController.cursorP1.x, inputController.cursorP1.y, 1f, laserLogic.laserData.grid[inputController.cursorP1.y, inputController.cursorP1.x][i].getHeading(), laserLogic.laserData.grid[inputController.cursorP1.y, inputController.cursorP1.x][i].getMarchDir(), Player.PlayerOne, 0, 0);
                //laserQueue.Add(new laserNode(inputController.cursorP1.x, inputController.cursorP1.y, 1f, laserLogic.laserData.grid[inputController.cursorP1.x, inputController.cursorP1.y][i].getHeading(), laserLogic.laserData.grid[inputController.cursorP1.x, inputController.cursorP1.y][i].getMarchDir(), Player.PlayerOne, ++laserIndex, 0));
                // Loop through the simulation
                while (laserQueue.Count > 0) {
                    laserStep(laserQueue[0]);
                    laserQueue.RemoveAt(0);
                }
            }
        if (inputController.cursorP1.state == State.placeLaser || inputController.cursorP1.state == State.placingLaser) {
            Direction dir = inputController.cursorP1.direction;
            if (inputController.cursorP1.y < 2) dir = Direction.Up;
            else if (inputController.cursorP1.y > gridManager.theGrid.getDimY() - 3) dir = Direction.Down;
            if (dir == Direction.Up) addLaserToQueue(1, inputController.cursorP1.y + 1, 1f, Direction.NE, Direction.Right, Player.PlayerOne, 0, 0, true);
            else if (dir == Direction.Down) addLaserToQueue(1, inputController.cursorP1.y - 1, 1f, Direction.SE, Direction.Right, Player.PlayerOne, 0, 0, true);
            while (laserQueue.Count > 0) {
                laserStep(laserQueue[0]);
                laserQueue.RemoveAt(0);
            }
        }

        ghostLaserData = new laserGrid(gridManager.theGrid.getDimX(), gridManager.theGrid.getDimY());

        // Check p2 cursor if ghost laser simulation is necessary
        if (validCursorBuilding(Player.PlayerTwo) && inputController.validPlacement(inputController.cursorP2.x, inputController.cursorP2.y, inputController.cursorP2.direction, getCursorBuilding(Player.PlayerTwo)))
            for (int i = 0; i < laserLogic.laserData.grid[inputController.cursorP2.y, inputController.cursorP2.x].Count; i++) {
                if (getCursorBuilding(Player.PlayerTwo) == Building.Redirecting)
                    laserRedirect(inputController.cursorP2.x, inputController.cursorP2.y, 1f, laserLogic.laserData.grid[inputController.cursorP2.y, inputController.cursorP2.x][i].getHeading(), laserLogic.laserData.grid[inputController.cursorP2.y, inputController.cursorP2.x][i].getMarchDir(), Player.PlayerTwo, laserIndex, 0);
                else laserReflect(inputController.cursorP2.x, inputController.cursorP2.y, 1f, laserLogic.laserData.grid[inputController.cursorP2.y, inputController.cursorP2.x][i].getHeading(), laserLogic.laserData.grid[inputController.cursorP2.y, inputController.cursorP2.x][i].getMarchDir(), Player.PlayerTwo, laserIndex, 0);
           		//laserQueue.Add(new laserNode(inputController.cursorP2.x, inputController.cursorP2.y, 1f, laserLogic.laserData.grid[inputController.cursorP2.x, inputController.cursorP2.y][i].getHeading(), laserLogic.laserData.grid[inputController.cursorP2.x, inputController.cursorP2.y][i].getMarchDir(), Player.PlayerTwo, ++laserIndex, 0));
                // Loop through the simulation
                while (laserQueue.Count > 0) {
                    laserStep(laserQueue[0]);
                    laserQueue.RemoveAt(0);
                }
            }
        if (inputController.cursorP2.state == State.placeLaser || inputController.cursorP2.state == State.placingLaser) {
            Direction dir = inputController.cursorP2.direction;
            if (inputController.cursorP2.y < 2) dir = Direction.Up;
            else if (inputController.cursorP2.y > gridManager.theGrid.getDimY() - 3) dir = Direction.Down;
            if (dir == Direction.Up) addLaserToQueue(inputController.cursorP2.x-1, inputController.cursorP2.y + 1, 1f, Direction.NW, Direction.Left, Player.PlayerTwo, 0, 0, true);
            else if (dir == Direction.Down) addLaserToQueue(inputController.cursorP2.x - 1, inputController.cursorP2.y - 1, 1f, Direction.SW, Direction.Left, Player.PlayerTwo, 0, 0, true);
            while (laserQueue.Count > 0) {
                laserStep(laserQueue[0]);
                laserQueue.RemoveAt(0);
            }
        }

        // Trim down unused list elements
        for (int i = lasers.Count - 1; i >= 0; i--) {
            if (lasers[i].Count == 0) lasers.RemoveAt(i);
        }

        // Draw lasers
        for (int i = 0; i < lasers.Count; i++) {
            int last = lasers[i].Count - 1;
            if (lasers[i][0].getOwner() != lasers[i][last].getOwner()) {
                for (int j = last; j > 0; j--) {
                    if (lasers[i][j].getOwner() != Player.Shared) { drawLaser(lasers[i][0], lasers[i][j]); drawLaser(lasers[i][j + 1], lasers[i][last]); break; }
                }
            } else {
                drawLaser(lasers[i][0], lasers[i][last]);
            }
        }

        // Set needsUpdate to false
        updateGhostLaser = false;
    }


    private void laserStep(laserNode node) // Need to add: if strength <= 0 return
    {
        // Get data from node
        int x = node.getX(), y = node.getY(), indx = node.getIndex(), subIndx = node.getSubIndex();
        float strength = node.getStrength();
        Direction heading = node.getHeading(), direction = node.getMarchDir();
        Player player = node.getOwner();

        // Check if valid iteration
        if (x < 0 || y < 0 || x >= gridManager.theGrid.getDimX() || y >= gridManager.theGrid.getDimY() || iterationCount > iterationLimit) return;
        iterationCount++;

        // Check if opposing laser
        if (ghostLaserData.checkOpposing(x, y, heading, direction)) {
            return;
        }

        // Make sure lasers list is initialized
        while (lasers.Count <= laserIndex) lasers.Add(new List<laserNode>());

        // Check if on same path as existing laser
        int[] indeces = ghostLaserData.checkOverlapping(x, y, strength, heading, direction, player);
        if (indeces != null) {
            lasers[indeces[0]][indeces[1]] = new laserNode(x, y, lasers[indeces[0]][indeces[1]].getStrength() + strength, heading, direction, Player.Shared, indx, subIndx);
        } else {
            // Add node to list for line rendering later
            ghostLaserData.insertNode(x, y, strength, heading, direction, player, indx, subIndx);
            lasers[indx].Add(new laserNode(x, y, strength, heading, direction, player, indx, subIndx));
        }

        // Determine next laser step
        int newX; int newY; Direction newDir;
        float newStrength = strength;// - laserDecay;
        switch (heading) {
            case Direction.NE:
                {
                    if (direction == Direction.Right) {
                        newDir = Direction.Up;
                        newX = x; newY = y + 1;
                    } else {
                        newDir = Direction.Right;
                        newX = x + 1; newY = y;
                    }

                    laserSolver(newX, newY, newStrength, heading, newDir, player, indx, subIndx);
                    break;
                }
            case Direction.NW:
                {
                    if (direction == Direction.Left) {
                        newDir = Direction.Up;
                        newX = x; newY = y + 1;
                    } else {
                        newDir = Direction.Left;
                        newX = x - 1; newY = y;
                    }

                    laserSolver(newX, newY, newStrength, heading, newDir, player, indx, subIndx);
                    break;
                }
            case Direction.SE:
                {
                    if (direction == Direction.Right) {
                        newDir = Direction.Down;
                        newX = x; newY = y - 1;
                    } else {
                        newDir = Direction.Right;
                        newX = x + 1; newY = y;
                    }

                    laserSolver(newX, newY, newStrength, heading, newDir, player, indx, subIndx);
                    break;
                }
            case Direction.SW:
                {
                    if (direction == Direction.Left) {
                        newDir = Direction.Down;
                        newX = x; newY = y - 1;
                    } else {
                        newDir = Direction.Left;
                        newX = x - 1; newY = y;
                    }

                    laserSolver(newX, newY, newStrength, heading, newDir, player, indx, subIndx);
                    break;
                }
        }
    }

    private void laserSolver(int x, int y, float strength, Direction heading, Direction direction, Player player, int indx, int subIndx)
    {
        if (x < 0 || y < 0 || x >= gridManager.theGrid.getDimX() || y >= gridManager.theGrid.getDimY()) return;
		switch (getBuilding(x, y, player, false)) {
            case Building.Empty: addLaserToQueue(x, y, strength, heading, direction, player, indx, subIndx, false); break;
            case Building.Blocking: laserBlock(x, y, strength, heading, direction, player, indx, subIndx); break;
            case Building.Reflecting: laserReflect(x, y, strength, heading, direction, player, indx, subIndx); break;
            case Building.Refracting: laserRefract(x, y, strength, heading, direction, player, indx, subIndx); break;
            case Building.Redirecting: laserRedirect(x, y, strength, heading, direction, player, indx, subIndx); break;
            case Building.Portal: break; // Not implemented
            case Building.Resource: laserResource(x, y, strength, heading, direction, player, indx, subIndx); break;
            case Building.Base: laserBase(x, y, strength, heading, direction, player); break;
            case Building.Laser: addLaserToQueue(x, y, strength, heading, direction, player, indx, subIndx, false); break;

        }
    }

    // Determine how much power a building adds to the laser
    private float powerSolver(int x, int y)
    {
        return 0.15f;// + laserDecay; // WIP
    }

    // Adds laser node to queue
    private void addLaserToQueue(int x, int y, float strength, Direction heading, Direction direction, Player player, int indx, int subIndx, bool isNew)
    {
        if (!isNew)
            if (lasers.Count > indx && lasers[indx].Count > 0 && subIndx == 0 && lasers[indx][0].onRedirect(player)) isNew = true;
        // Add laser node to queue
        if (isNew) laserQueue.Add(new laserNode(x, y, strength, heading, direction, player, ++laserIndex, 0));
        else laserQueue.Add(new laserNode(x, y, strength, heading, direction, player, indx, subIndx + 1));
    }

    // What happens when a laser collides with a base
    private void laserBase(int x, int y, float strength, Direction heading, Direction direction, Player player)
    {
        return;
    }

    // What happens when a laser collides with a blocking block
    private void laserBlock(int x, int y, float strength, Direction heading, Direction direction, Player player, int indx, int subIndx)
    {
        return;
    }

    // What happens when a laser collides with a resource block
    private void laserResource(int x, int y, float strength, Direction heading, Direction direction, Player player, int indx, int subIndx)
    {
        return;
    }

    // What happens when a laser collides with a reflection block
    private void laserReflect(int x, int y, float strength, Direction heading, Direction direction, Player player, int indx, int subIndx)
    {
        switch (direction) {
			case Direction.Down: if (getBuilding(x, y + 1, player, false) != Building.Redirecting) addLaserToQueue(x, y + 1, strength + powerSolver(x, y), heading == Direction.SE ? Direction.NE : Direction.NW, Direction.Up, player, indx, subIndx, true); break;
			case Direction.Up: if (getBuilding(x, y - 1, player, false) != Building.Redirecting) addLaserToQueue(x, y - 1, strength + powerSolver(x, y), heading == Direction.NE ? Direction.SE : Direction.SW, Direction.Down, player, indx, subIndx, true); break;
			case Direction.Left: if (getBuilding(x + 1, y, player, false) != Building.Redirecting) addLaserToQueue(x + 1, y, strength + powerSolver(x, y), heading == Direction.SW ? Direction.SE : Direction.NE, Direction.Right, player, indx, subIndx, true); break;
			case Direction.Right: if (getBuilding(x - 1, y, player, false) != Building.Redirecting) addLaserToQueue(x - 1, y, strength + powerSolver(x, y), heading == Direction.SE ? Direction.SW : Direction.NW, Direction.Left, player, indx, subIndx, true); break;
        }
    }

    // What happens when a laser collides with a refraction block
    private void laserRefract(int x, int y, float strength, Direction heading, Direction direction, Player player, int indx, int subIndx)
    {
        // Infinite loop check;
        List<dirHeadPlayer> directionalHits;
        if (refractHits.TryGetValue(new XY(x, y), out directionalHits))
            foreach (dirHeadPlayer hit in directionalHits) {
                //laserHits.Add(new laserHit(x, y, true, strength, Building.Refracting, gridManager.theGrid.getOwner(x, y))); return;
                //if (!(opposites(hit.direction, direction) && headingsOpposite(hit.heading, heading))) { laserHits.Add(new laserHit(x, y, true, strength, Building.Refracting, gridManager.theGrid.getOwner(x, y))); return; }
            } else directionalHits = new List<dirHeadPlayer>();
        directionalHits.Add(new dirHeadPlayer(getExit(direction, heading), heading, player));
        if (refractHits.ContainsKey(new XY(x, y))) refractHits[new XY(x, y)] = directionalHits;
        else refractHits.Add(new XY(x, y), directionalHits);

        addLaserToQueue(x, y, strength, heading, direction, player, indx, subIndx, false);

        switch (direction) {
            case Direction.Down: addLaserToQueue(x, y + 1, strength + powerSolver(x, y), heading == Direction.SE ? Direction.NE : Direction.NW, Direction.Up, player, indx, subIndx, true); break;
            case Direction.Up: addLaserToQueue(x, y - 1, strength + powerSolver(x, y), heading == Direction.NE ? Direction.SE : Direction.SW, Direction.Down, player, indx, subIndx, true); break;
            case Direction.Left: addLaserToQueue(x + 1, y, strength + powerSolver(x, y), heading == Direction.SW ? Direction.SE : Direction.NE, Direction.Right, player, indx, subIndx, true); break;
            case Direction.Right: addLaserToQueue(x - 1, y, strength + powerSolver(x, y), heading == Direction.SE ? Direction.SW : Direction.NW, Direction.Left, player, indx, subIndx, true); break;
        }
    }

    // What happens when a laser collides with a redirect block
    private void laserRedirect(int x, int y, float strength, Direction heading, Direction direction, Player player, int indx, int subIndx)
    {
        // damage hit if side hit
        bool damageHit = false;
        Direction dir = Direction.Up;
        if (player == Player.PlayerOne && inputController.cursorP1.x == x && inputController.cursorP1.y == y && validCursorBuilding(Player.PlayerOne)) dir = inputController.cursorP1.direction;
        else if (player == Player.PlayerTwo && inputController.cursorP2.x == x && inputController.cursorP2.y == y && validCursorBuilding(Player.PlayerTwo)) dir = inputController.cursorP2.direction;
        else dir = gridManager.theGrid.getDirection(x, y);
        if (dir == Direction.Up || dir == Direction.Down) { if (direction == Direction.Right || direction == Direction.Left) damageHit = true; } else { if (direction == Direction.Up || direction == Direction.Down) damageHit = true; }

        if (!damageHit) {
            switch (direction) {
                case Direction.Down: addLaserToQueue(x, y, strength + powerSolver(x, y), heading, heading == Direction.SE ? Direction.Right : Direction.Left, player, indx, subIndx, true); break;
                case Direction.Up: addLaserToQueue(x, y, strength + powerSolver(x, y), heading, heading == Direction.NE ? Direction.Right : Direction.Left, player, indx, subIndx, true); break;
                case Direction.Left: addLaserToQueue(x, y, strength + powerSolver(x, y), heading, heading == Direction.NW ? Direction.Up : Direction.Down, player, indx, subIndx, true); break;
                case Direction.Right: addLaserToQueue(x, y, strength + powerSolver(x, y), heading, heading == Direction.NE ? Direction.Up : Direction.Down, player, indx, subIndx, true); break;
            }
        }
    }

    // Returns ghost building or gridmanager building
	private Building getBuilding(int x, int y, Player player = Player.World, bool flag = true)
    {
        if (player == Player.PlayerOne && inputController.cursorP1.x == x && inputController.cursorP1.y == y && validCursorBuilding(Player.PlayerOne)) return getCursorBuilding(Player.PlayerOne);
        if (player == Player.PlayerTwo && inputController.cursorP2.x == x && inputController.cursorP2.y == y && validCursorBuilding(Player.PlayerTwo)) return getCursorBuilding(Player.PlayerTwo);
        return gridManager.theGrid.getBuilding(x, y, flag);
    }

    // Check if current selected building is supported by the ghost laser
    public static bool validCursorBuilding(Player player = Player.World)
    {
        if (player == Player.PlayerOne) {
            if ((inputController.cursorP1.state == State.idle || inputController.cursorP1.state == State.placing) && (inputController.cursorP1.selection == Building.Reflecting || inputController.cursorP1.selection == Building.Refracting || inputController.cursorP1.selection == Building.Redirecting)) return true;
            if ((inputController.cursorP1.state == State.moving || inputController.cursorP1.state == State.placingMove) && (inputController.cursorP1.moveBuilding == Building.Reflecting || inputController.cursorP1.moveBuilding == Building.Refracting || inputController.cursorP1.moveBuilding == Building.Redirecting)) return true;
        } else if (player == Player.PlayerTwo) {
            if ((inputController.cursorP2.state == State.idle || inputController.cursorP2.state == State.placing) && (inputController.cursorP2.selection == Building.Reflecting || inputController.cursorP2.selection == Building.Refracting || inputController.cursorP2.selection == Building.Redirecting)) return true;
            if ((inputController.cursorP2.state == State.moving || inputController.cursorP2.state == State.placingMove) && (inputController.cursorP2.moveBuilding == Building.Reflecting || inputController.cursorP2.moveBuilding == Building.Refracting || inputController.cursorP2.moveBuilding == Building.Redirecting)) return true;
        }
        return false;
    }

    // Return the proper building for use with the ghost laser depending on player and state
    public static Building getCursorBuilding(Player player = Player.World)
    {
        if (player == Player.PlayerOne) {
            if (inputController.cursorP1.state == State.idle || inputController.cursorP1.state == State.placing) return inputController.cursorP1.selection;
            if (inputController.cursorP1.state == State.moving || inputController.cursorP1.state == State.placingMove) return inputController.cursorP1.moveBuilding;
        } else if (player == Player.PlayerTwo) {
            if (inputController.cursorP2.state == State.idle || inputController.cursorP2.state == State.placing) return inputController.cursorP2.selection;
            if (inputController.cursorP2.state == State.moving || inputController.cursorP2.state == State.placingMove) return inputController.cursorP2.moveBuilding;
        }
        return Building.Empty;
    }

}

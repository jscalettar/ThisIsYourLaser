using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserLogic : MonoBehaviour
{

    // Configurable
    //public float laserDecay = 0.025f;
    public float laserWidth = 0.05f;
    public float laserVisualHeight = 0.5f;
    public float laserIntensity = 0.5f;
    public float intervalInMinutes1;
    public float intervalInMinutes2;
    public float intervalInMinutes3;
    public float resourceRate = 0.1f;
    public Material laserMaterialP1;
    public Material laserMaterialP2;
    public Material laserMaterialCombined;
    public GameObject hitEffect;
    public static float laserPowerMultiplier = 1.0f;

    // Can change laser heading like this:
    // laserLogic.laserHeadingP1 = Direction.SE;  or  laserLogic.laserHeadingP2 = Direction.SW;
    public static Direction laserHeadingP1 = Direction.NE;
    public static Direction laserHeadingP2 = Direction.NW;

    // Private Variables
    private int laserStartP1 = 0;
    private int laserStartP2 = 0;
    
    private float intervalInSeconds1;
    private float intervalInSeconds2;
    private float intervalInSeconds3;
    //private int laserIndex = 0;
    private int iterationLimit = 300;
    private int iterationCount = 0;
    private int laserLimit = 100;
    private int laserIndex = 0;
    private int laserCounter = 0;
    private int particleCounter = 0;
    private List<List<laserNode>> lasers;   // Laser list for line rendering
    private List<laserNode> laserQueue;
    public static laserGrid laserData;
    private List<laserHit> laserHits;       // Laser-building collision list
    private Dictionary<XY, List<dirHeadPlayer>> refractHits;    // Refract collision dictionary
    private Dictionary<XY, List<dirHeadPlayer>> particleHits;    // Particle collision dictionary
    private GameObject laserContainer;
    private GameObject particleContainer;

    public struct laserNode
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

        // Get
        public int getX() { return X; }
        public int getY() { return Y; }
        public int getIndex() { return laserIndex; }
        public int getSubIndex() { return laserSubIndex; }
        public float getStrength() { return strength; }
        public Direction getHeading() { return laserHeading; }
        public Direction getMarchDir() { return marchDirection; }
        public Player getOwner() { return owner; }
        public bool onRedirect() { return gridManager.theGrid.getBuilding(X, Y) == Building.Redirecting; }
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

    public struct laserGrid
    {
        public List<laserNode>[,] grid;
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

        private Direction opposite(Direction dir)
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
        // Conversion to seconds
        float intervalInSeconds1 = 60 / intervalInMinutes1;
        float intervalInSeconds2 = 60 / intervalInMinutes2;
        float intervalInSeconds3 = 60 / intervalInMinutes3;

        laserIndex = -1;
        lasers = new List<List<laserNode>>();
        laserQueue = new List<laserNode>();
        laserHits = new List<laserHit>();
        laserData = new laserGrid(gridManager.theGrid.getDimX(), gridManager.theGrid.getDimY());
        laserContainer = new GameObject("laserContainer");
        laserContainer.transform.SetParent(gameObject.transform);
        particleContainer = new GameObject("particleContainer");
        particleContainer.transform.SetParent(gameObject.transform);
        refractHits = new Dictionary<XY, List<dirHeadPlayer>>();
        particleHits = new Dictionary<XY, List<dirHeadPlayer>>();

        for (int i = 0; i < laserLimit; i++) {
            // Line object pool
            GameObject lineObject = new GameObject("lineObject");
            lineObject.transform.SetParent(laserContainer.transform);
            LineRenderer line = lineObject.AddComponent<LineRenderer>();
            line.startWidth = laserWidth;
            line.endWidth = laserWidth;
            line.numCapVertices = 5;
            line.material = laserMaterialP1;
            line.enabled = false;
            // Particle object pool
            GameObject particleObject = Instantiate(hitEffect);
            particleObject.transform.SetParent(particleContainer.transform);
            particleObject.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        }
    }

    void LateUpdate()
    {
        if (Time.realtimeSinceStartup >= intervalInSeconds1) {
            laserPowerMultiplier = 2;
        }
        else if (Time.realtimeSinceStartup >= intervalInSeconds2) {
            laserPowerMultiplier = 3;
        }
        else if (Time.realtimeSinceStartup >= intervalInSeconds3) {
            laserPowerMultiplier = 4;
        }
       if (gridManager.theGrid.updateLaser()) simulateLasers(); // Update laser if needed

        // Generate Resources, apply damage
        foreach (laserHit hit in laserHits) {
            if (hit.buildingHit == Building.Resource && !hit.weakSideHit) {
                // Add Resources
				GameObject prefab = null;
				gridManager.theGrid.prefabDictionary.TryGetValue(new XY(hit.X, hit.Y), out prefab);
				if (prefab != null && prefab.transform.GetChild(0) != null) prefab = prefab.transform.GetChild(0).gameObject;
				if (prefab != null && prefab.GetComponent<ParticleSystem>() != null && !prefab.GetComponent<ParticleSystem>().isEmitting) prefab.GetComponent<ParticleSystem>().Emit(1);

                if (hit.buildingOwner == Player.PlayerOne && gridManager.theGrid.getResourcesP1() != gridManager.theGrid.getResourceLimit()) { gridManager.theGrid.addResources(hit.laserStrength * Time.deltaTime * resourceRate, 0); floatingNumbers.floatingNumbersStruct.checkResource(new XY(hit.X,hit.Y), gridManager.theGrid.getResourcesP1(), Player.PlayerOne, State.idle); }
                else if (hit.buildingOwner == Player.PlayerTwo && gridManager.theGrid.getResourcesP2() != gridManager.theGrid.getResourceLimit()) { gridManager.theGrid.addResources(0, hit.laserStrength * Time.deltaTime * resourceRate); floatingNumbers.floatingNumbersStruct.checkResource(new XY(hit.X, hit.Y), gridManager.theGrid.getResourcesP2(), Player.PlayerTwo, State.idle); }
            } else if (hit.weakSideHit) {
                // Apply Damage
                gridManager.theGrid.applyDamage(hit.X, hit.Y, hit.laserStrength * laserPowerMultiplier * Time.deltaTime);
            }
        }
        //setupManager smanager = new setupManager();
        // Generate Resources passively
        if (gridManager.theGrid.baseHealthP1() > 0f && gridManager.theGrid.baseHealthP2() > 0f) gridManager.theGrid.addResources(Time.deltaTime * resourceRate/2, Time.deltaTime * resourceRate/2);

        if (ghostLaser.ghostUpdateNeeded) { ghostLaser.updateGhostLaser = true; ghostLaser.ghostUpdateNeeded = false; }
    }

    private void drawLaser(laserNode start, laserNode end)
    {
        Vector3 startPos = coordToPos(start.getX(), start.getY(), start.getHeading(), start.getMarchDir(), true);
        Vector3 endPos = coordToPos(end.getX(), end.getY(), end.getHeading(), end.getMarchDir(), false);
        laserContainer.transform.GetChild(laserCounter).GetComponent<LineRenderer>().SetPosition(0, startPos);
        laserContainer.transform.GetChild(laserCounter).GetComponent<LineRenderer>().SetPosition(1, endPos);
        laserContainer.transform.GetChild(laserCounter).GetComponent<LineRenderer>().enabled = true;
        laserContainer.transform.GetChild(laserCounter).GetComponent<LineRenderer>().startColor = new Color(1, 1, 1, start.getStrength() * laserIntensity);
        laserContainer.transform.GetChild(laserCounter).GetComponent<LineRenderer>().endColor = new Color(1, 1, 1, end.getStrength() * laserIntensity);
        if (start.getOwner() == Player.Shared) laserContainer.transform.GetChild(laserCounter).GetComponent<LineRenderer>().material = laserMaterialCombined;
        else laserContainer.transform.GetChild(laserCounter).GetComponent<LineRenderer>().material = start.getOwner() == Player.PlayerOne ? laserMaterialP1 : laserMaterialP2;
        laserCounter++;
    }

    private Vector3 coordToPos(int x, int y, Direction heading, Direction direction, bool start)
    {
        float xOff = 0, yOff = 0;

        if (start) {
            if (gridManager.theGrid.getBuilding(x, y) == Building.Redirecting) {
                switch (heading) {
                    case Direction.NE: { if (direction == Direction.Right) { xOff = 0.5f; yOff = 0f; } else { xOff = 0f; yOff = 0.5f; } break; }
                    case Direction.NW: { if (direction == Direction.Left) { xOff = 0.5f; yOff = 0f; } else { xOff = 1f; yOff = 0.5f; } break; }
                    case Direction.SE: { if (direction == Direction.Right) { xOff = 0.5f; yOff = 1f; } else { xOff = 0f; yOff = 0.5f; } break; }
                    case Direction.SW: { if (direction == Direction.Left) { xOff = 0.5f; yOff = 1f; } else { xOff = 1f; yOff = 0.5f; } break; }
                }
            }
            else switch (heading) {
                    case Direction.NE: { if (direction == Direction.Right) { yOff += 0.5f; } else { xOff += 0.5f; } break; }
                    case Direction.SE: { if (direction == Direction.Right) { yOff += 0.5f; } else { xOff += 0.5f; yOff++; } break; }
                    case Direction.NW: { if (direction == Direction.Up) { xOff += 0.5f; } else { xOff++; yOff += 0.5f; } break; }
                    case Direction.SW: { if (direction == Direction.Down) { xOff += 0.5f; yOff++; } else { xOff++; yOff += 0.5f; } break; }
                }
        } else {
            if (gridManager.theGrid.getBuilding(x, y) == Building.Redirecting) {
                switch (heading) {
                    case Direction.NE: { if (direction == Direction.Right) { xOff = 0.5f; yOff = 1f; } else { xOff = 1f; yOff = 0.5f; } break; }
                    case Direction.NW: { if (direction == Direction.Left) { xOff = 0.5f; yOff = 1f; } else { xOff = 0f; yOff = 0.5f; } break; }
                    case Direction.SE: { if (direction == Direction.Right) { xOff = 0.5f; yOff = 0f; } else { xOff = 1f; yOff = 0.5f; } break; }
                    case Direction.SW: { if (direction == Direction.Left) { xOff = 0.5f; yOff = 0f; } else { xOff = 0f; yOff = 0.5f; } break; }
                }
            }
            else switch (heading) {
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
        laserData = new laserGrid(gridManager.theGrid.getDimX(), gridManager.theGrid.getDimY());

        // Reset counters and check for placed lasers
        iterationCount = 0; laserIndex = -1;
        bool p1LaserFound = false, p2LaserFound = false;
        for (int i = 0; i < gridManager.theGrid.getDimY(); i++) {
            if (gridManager.theGrid.getBuilding(0, i) == Building.Laser) { laserStartP1 = i; p1LaserFound = true; }
            if (gridManager.theGrid.getBuilding(gridManager.theGrid.getDimX() - 1, i) == Building.Laser) { laserStartP2 = i; p2LaserFound = true; }
        }

        // Clear old lasers before starting again
        for (int i = 0; i < lasers.Count; i++) lasers[i].Clear();
        for (int i = 0; i < laserCounter; i++) laserContainer.transform.GetChild(i).GetComponent<LineRenderer>().enabled = false;
        for (int i = 0; i < particleCounter; i++) particleContainer.transform.GetChild(i).transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        laserCounter = 0;
        particleCounter = 0;

        // Clear laser hits, refract hits
        laserHits.Clear();
        refractHits.Clear();
        particleHits.Clear();

        // Simulate each player's laser
        if (p1LaserFound) laserQueue.Add(new laserNode(0, laserStartP1, 1f, laserHeadingP1, Direction.Right, Player.PlayerOne, ++laserIndex, 0));
        if (p2LaserFound) laserQueue.Add(new laserNode(gridManager.theGrid.getDimX() - 1, laserStartP2, 1f, laserHeadingP2, Direction.Left, Player.PlayerTwo, ++laserIndex, 0));

        // Loop through the simulation
        while (laserQueue.Count > 0) {
            laserStep(laserQueue[0]);
            laserQueue.RemoveAt(0);
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

        // Particles
        int dimX = gridManager.theGrid.getDimX(); int dimY = gridManager.theGrid.getDimY();
        foreach (KeyValuePair<XY, List<dirHeadPlayer>> keyValue in particleHits) {
            for (int i = 0; i < keyValue.Value.Count; i++) {
                Transform particle = particleContainer.transform.GetChild(particleCounter);
                particle.localPosition = new Vector3((-dimX / 2) + keyValue.Key.x + 0.5f, 0.01f, (-dimY / 2) + keyValue.Key.y + 0.5f); ;
                particle.localEulerAngles = directionToEular(keyValue.Value[i].direction);
                //particle.transform.GetChild(0).GetComponent<ParticleSystem>(). set color? // hit.Value.player == Player.PlayerOne ? red : green;
                particle.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                particleCounter++;
            }
        }

        // Set needsUpdate to false
        gridManager.theGrid.updateFinished();
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
        if (laserData.checkOpposing(x, y, heading, direction)) {
            return;
        }

        // Make sure lasers list is initialized
        while (lasers.Count <= laserIndex) lasers.Add(new List<laserNode>());

        // Check if on same path as existing laser
        int[] indeces = laserData.checkOverlapping(x, y, strength, heading, direction, player);
        if (indeces != null) {
            lasers[indeces[0]][indeces[1]] = new laserNode(x, y, lasers[indeces[0]][indeces[1]].getStrength() + strength, heading, direction, Player.Shared, indx, subIndx);
        } else {
            // Add node to list for line rendering later
            laserData.insertNode(x, y, strength, heading, direction, player, indx, subIndx);
            lasers[indx].Add(new laserNode(x, y, strength, heading, direction, player, indx, subIndx));
        }

        // Determine next laser step
        int newX; int newY; Direction newDir;

        float newStrength = strength;

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
        switch (gridManager.theGrid.getBuilding(x, y)) {
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
   /* private float powerSolver(int x, int y)
    {
        return 0.15f + laserDecay; // WIP
    }
    */

    private float multiplyPower(float curStrength, float powerLevel) {
        return curStrength * powerLevel;
    }

    // Adds laser node to queue
    private void addLaserToQueue(int x, int y, float strength, Direction heading, Direction direction, Player player, int indx, int subIndx, bool isNew)
    {
        if (subIndx == 0 && lasers[indx][0].onRedirect()) isNew = true;
        // Add laser node to queue
        if (isNew) laserQueue.Add(new laserNode(x, y, strength, heading, direction, player, ++laserIndex, 0));
        else laserQueue.Add(new laserNode(x, y, strength, heading, direction, player, indx, subIndx + 1));
    }

    // What happens when a laser collides with a base
    private void laserBase(int x, int y, float strength, Direction heading, Direction direction, Player player)
    {
        laserHits.Add(new laserHit(x, y, true, strength, Building.Base, gridManager.theGrid.getOwner(x, y)));
        if (!particleHits.ContainsKey(new XY(x, y))) particleHits.Add(new XY(x, y), new List<dirHeadPlayer>());
        particleHits[new XY(x, y)].Add(new dirHeadPlayer(direction, heading, player));
    }

    // What happens when a laser collides with a blocking block
    private void laserBlock(int x, int y, float strength, Direction heading, Direction direction, Player player, int indx, int subIndx)
    {
        GridItem gi = gridManager.theGrid.getCellInfo(x, y);
        //if (opposites(gi.direction, direction)) {
            laserHits.Add(new laserHit(x, y, true, strength, Building.Blocking, gi.owner));
            if (!particleHits.ContainsKey(new XY(x, y))) particleHits.Add(new XY(x, y), new List<dirHeadPlayer>());
            particleHits[new XY(x, y)].Add(new dirHeadPlayer(direction, heading, player));
        //}
    }

    // What happens when a laser collides with a resource block
    private void laserResource(int x, int y, float strength, Direction heading, Direction direction, Player player, int indx, int subIndx)
    {
        GridItem gi = gridManager.theGrid.getCellInfo(x, y);
        bool weakHit = true;
        if (opposites(gi.direction, direction)) weakHit = false;
        laserHits.Add(new laserHit(x, y, weakHit, strength, Building.Resource, gi.owner));
        if (weakHit) {
            if (!particleHits.ContainsKey(new XY(x, y))) particleHits.Add(new XY(x, y), new List<dirHeadPlayer>());
            particleHits[new XY(x, y)].Add(new dirHeadPlayer(direction, heading, player));
        }
    }

    // What happens when a laser collides with a reflection block
    private void laserReflect(int x, int y, float strength, Direction heading, Direction direction, Player player, int indx, int subIndx)
    {
        // Need to add if (direction == building weak side), add damageHit, break;
        GridItem gi = gridManager.theGrid.getCellInfo(x, y);
        if (opposites(gi.direction, direction) || gi.direction == direction) {
            laserHits.Add(new laserHit(x, y, true, strength, Building.Reflecting, gi.owner));
            if (!particleHits.ContainsKey(new XY(x, y))) particleHits.Add(new XY(x, y), new List<dirHeadPlayer>());
            particleHits[new XY(x, y)].Add(new dirHeadPlayer(direction, heading, player));
        }

        switch (direction)
            {
            /*
            case Direction.Down: if(gridManager.theGrid.getBuilding(x, y + 1) != Building.Redirecting) addLaserToQueue(x, y + 1, strength + powerSolver(x, y), heading == Direction.SE ? Direction.NE : Direction.NW, Direction.Up, player, indx, subIndx, true); break;
            case Direction.Up: if (gridManager.theGrid.getBuilding(x, y - 1) != Building.Redirecting) addLaserToQueue(x, y - 1, strength + powerSolver(x, y), heading == Direction.NE ? Direction.SE : Direction.SW, Direction.Down, player, indx, subIndx, true); break;
            case Direction.Left: if (gridManager.theGrid.getBuilding(x + 1, y) != Building.Redirecting) addLaserToQueue(x + 1, y, strength + powerSolver(x, y), heading == Direction.SW ? Direction.SE : Direction.NE, Direction.Right, player, indx, subIndx, true); break;
            case Direction.Right: if (gridManager.theGrid.getBuilding(x - 1, y) != Building.Redirecting) addLaserToQueue(x - 1, y, strength + powerSolver(x, y), heading == Direction.SE ? Direction.SW : Direction.NW, Direction.Left, player, indx, subIndx, true); break;
            */

            case Direction.Down: if (gridManager.theGrid.getBuilding(x, y + 1) != Building.Redirecting) addLaserToQueue(x, y + 1, strength, heading == Direction.SE ? Direction.NE : Direction.NW, Direction.Up, player, indx, subIndx, true); break;
            case Direction.Up: if (gridManager.theGrid.getBuilding(x, y - 1) != Building.Redirecting) addLaserToQueue(x, y - 1, strength, heading == Direction.NE ? Direction.SE : Direction.SW, Direction.Down, player, indx, subIndx, true); break;
            case Direction.Left: if (gridManager.theGrid.getBuilding(x + 1, y) != Building.Redirecting) addLaserToQueue(x + 1, y, strength, heading == Direction.SW ? Direction.SE : Direction.NE, Direction.Right, player, indx, subIndx, true); break;
            case Direction.Right: if (gridManager.theGrid.getBuilding(x - 1, y) != Building.Redirecting) addLaserToQueue(x - 1, y, strength, heading == Direction.SE ? Direction.SW : Direction.NW, Direction.Left, player, indx, subIndx, true); break;
        }

    }

    // What happens when a laser collides with a refraction block
    private void laserRefract(int x, int y, float strength, Direction heading, Direction direction, Player player, int indx, int subIndx)
    {
        // Infinite loop check;
        List<dirHeadPlayer> directionalHits;
        if (refractHits.TryGetValue(new XY(x, y), out directionalHits))
            foreach (dirHeadPlayer hit in directionalHits) {
                laserHits.Add(new laserHit(x, y, true, strength, Building.Refracting, gridManager.theGrid.getOwner(x, y))); return;
                //if (!(opposites(hit.direction, direction) && headingsOpposite(hit.heading, heading))) { laserHits.Add(new laserHit(x, y, true, strength, Building.Refracting, gridManager.theGrid.getOwner(x, y))); return; }
            }
        else directionalHits = new List<dirHeadPlayer>();
        directionalHits.Add(new dirHeadPlayer(getExit(direction, heading), heading, player));
        if (refractHits.ContainsKey(new XY(x, y))) refractHits[new XY(x, y)] = directionalHits;
        else refractHits.Add(new XY(x, y), directionalHits);

        // Need to add particles somewhere

        addLaserToQueue(x, y, strength, heading, direction, player, indx, subIndx, false);

        switch (direction) {
            /*
            case Direction.Down: addLaserToQueue(x, y + 1, strength + powerSolver(x, y), heading == Direction.SE ? Direction.NE : Direction.NW, Direction.Up, player, indx, subIndx, true); break;
            case Direction.Up: addLaserToQueue(x, y - 1, strength + powerSolver(x, y), heading == Direction.NE ? Direction.SE : Direction.SW, Direction.Down, player, indx, subIndx, true); break;
            case Direction.Left: addLaserToQueue(x + 1, y, strength + powerSolver(x, y), heading == Direction.SW ? Direction.SE : Direction.NE, Direction.Right, player, indx, subIndx, true); break;
            case Direction.Right: addLaserToQueue(x - 1, y, strength + powerSolver(x, y), heading == Direction.SE ? Direction.SW : Direction.NW, Direction.Left, player, indx, subIndx, true); break
            */

            case Direction.Down: addLaserToQueue(x, y + 1, strength, heading == Direction.SE ? Direction.NE : Direction.NW, Direction.Up, player, indx, subIndx, true); break;
            case Direction.Up: addLaserToQueue(x, y - 1, strength, heading == Direction.NE ? Direction.SE : Direction.SW, Direction.Down, player, indx, subIndx, true); break;
            case Direction.Left: addLaserToQueue(x + 1, y, strength, heading == Direction.SW ? Direction.SE : Direction.NE, Direction.Right, player, indx, subIndx, true); break;
            case Direction.Right: addLaserToQueue(x - 1, y, strength, heading == Direction.SE ? Direction.SW : Direction.NW, Direction.Left, player, indx, subIndx, true); break;
        }
    }

    // What happens when a laser collides with a redirect block
    private void laserRedirect(int x, int y, float strength, Direction heading, Direction direction, Player player, int indx, int subIndx)
    {
        // Add damage hit if side hit
        bool damageHit = false;
        GridItem gi = gridManager.theGrid.getCellInfo(x, y);
        if (gi.direction == Direction.Up || gi.direction == Direction.Down) { if (direction == Direction.Right || direction == Direction.Left) damageHit = true; } else { if (direction == Direction.Up || direction == Direction.Down) damageHit = true; }


        if (!damageHit) {
            switch (direction) {
                /*
                case Direction.Down: addLaserToQueue(x, y, strength + powerSolver(x, y), heading, heading == Direction.SE ? Direction.Right : Direction.Left, player, indx, subIndx, true); break;
                case Direction.Up: addLaserToQueue(x, y , strength + powerSolver(x, y), heading, heading == Direction.NE ? Direction.Right : Direction.Left, player, indx, subIndx, true); break;
                case Direction.Left: addLaserToQueue(x, y, strength + powerSolver(x, y), heading, heading == Direction.NW ? Direction.Up : Direction.Down, player, indx, subIndx, true); break;
                case Direction.Right: addLaserToQueue(x, y, strength + powerSolver(x, y), heading, heading == Direction.NE ? Direction.Up : Direction.Down, player, indx, subIndx, true); break;
                */

                case Direction.Down: addLaserToQueue(x, y, strength, heading, heading == Direction.SE ? Direction.Right : Direction.Left, player, indx, subIndx, true); break;
                case Direction.Up: addLaserToQueue(x, y, strength, heading, heading == Direction.NE ? Direction.Right : Direction.Left, player, indx, subIndx, true); break;
                case Direction.Left: addLaserToQueue(x, y, strength, heading, heading == Direction.NW ? Direction.Up : Direction.Down, player, indx, subIndx, true); break;
                case Direction.Right: addLaserToQueue(x, y, strength, heading, heading == Direction.NE ? Direction.Up : Direction.Down, player, indx, subIndx, true); break;
            }
        } else {
            laserHits.Add(new laserHit(x, y, true, strength, Building.Redirecting, gi.owner));
            if (!particleHits.ContainsKey(new XY(x, y))) particleHits.Add(new XY(x, y), new List<dirHeadPlayer>());
            particleHits[new XY(x, y)].Add(new dirHeadPlayer(direction, heading, player));
        }
    }

    // DEBUG LASER    
    /*void OnDrawGizmos()
    {
        if (lasers != null) {
            float dimX = gridManager.theGrid.getDimX();
            float dimY = gridManager.theGrid.getDimY();
            Gizmos.color = new Color(1f, 0, 0, 1f);
            for (int i = 0; i < lasers.Count; i++) {
                for (int j = 0; j < lasers[i].Count; j++) {
                    Gizmos.color = lasers[i][j].getOwner() == Player.PlayerOne ? new Color(1f, 0, 0, lasers[i][j].getStrength()) : new Color(0f, 1f, 0f, lasers[i][j].getStrength());
                    Gizmos.DrawSphere(new Vector3((-dimX / 2) + lasers[i][j].getX() + 0.5f, 0.5f * 0.5f, (-dimY / 2) + lasers[i][j].getY() + 0.5f), 0.25f);
                    UnityEditor.Handles.Label(new Vector3((-dimX / 2) + lasers[i][j].getX() + 0.40f, 0.5f * 0.5f, (-dimY / 2) + lasers[i][j].getY() + 0.25f), lasers[i][j].getStrength().ToString("F2"));
                }
            }
        }
    }*/
    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Readme: Don't bother trying to understand this yet lol
// Issues: Refraction blocks can cause infinite laser loop
// Also need to combine lasers (and their strength) in some cases.
// No building hit data generated yet.

public class laserLogic : MonoBehaviour {

    // Configurable
    public float laserDecay = 0.025f;

    // Hardcoded for now
    private Direction laserHeadingP1 = Direction.NE;
    private Direction laserHeadingP2 = Direction.NW;

    // Private Variables
    private int laserStartP1 = 0;
    private int laserStartP2 = 0;
    private int laserIndex = 0;
    private int recursionLimit = 300;
    private int recursionCount = 0;
    private List<List<laserNode>> lasers; // Laser list

    struct laserNode
    {
        private int X;
        private int Y;
        private float strength;
        private Direction laserHeading;
        private Direction marchDirection;
        private Player owner;

        public laserNode(int x, int y, float laserPower, Direction laserDirection, Direction marchDir, Player ownedBy)
        {
            X = x;
            Y = y;
            strength = laserPower;
            laserHeading = laserDirection;
            marchDirection = marchDir;
            owner = ownedBy;
        }

        public int getX() { return X; }
        public int getY() { return Y; }
        public float getStrength() { return strength; }
        public Direction getHeading() { return laserHeading; }
        public Direction getMarchDir() { return marchDirection; }
        public Player getOwner() { return owner; }

        public string toString()
        {
            return "Coords: (" + X + ", " + Y + ")  |  Strength: " + strength + "  |  Heading: " + laserHeading + "  |  MarchDirection: " + marchDirection + "  |  Owner: " + owner;
        }
    }

    // For storing when lasers hit resource or blocking blocks
    struct laserHit
    {
        private int X;
        private int Y;
        private float laserStrength;
        private Building buildingHit;
        private Player laserOwner;

        public laserHit(int x, int y, float laserPower, Building buildingType, Player laserOwnedBy)
        {
            X = x;
            Y = y;
            laserStrength = laserPower;
            buildingHit = buildingType;
            laserOwner = laserOwnedBy;
        }
    }

    void Awake()
    {
        lasers = new List<List<laserNode>>();
    }

	void Start () {
        simulateLasers();
    }
	
	// Update is called once per frame
	void LateUpdate () {
        simulateLasers();
    }

    private void simulateLasers()
    {
        recursionCount = 0;
        bool p1LaserFound = false, p2LaserFound = false;
        for (int i = 0; i < gridManager.theGrid.getDimY(); i++) {
            if (gridManager.theGrid.getBuilding(0, i) == Building.Laser) { laserStartP1 = i; p1LaserFound = true; }
            if (gridManager.theGrid.getBuilding(gridManager.theGrid.getDimX() - 1, i) == Building.Laser) { laserStartP2 = i; p2LaserFound = true; }
        }

        // Clear old lasers before starting again
        laserIndex = -1;
        for (int i = 0; i < lasers.Count; i++) lasers[i].Clear();

        // Simulate each player's laser
        if (p1LaserFound) laserStep(0, laserStartP1, 1f, laserHeadingP1, Direction.Right, Player.PlayerOne, true);
        if (p2LaserFound) laserStep(gridManager.theGrid.getDimX() - 1, laserStartP2, 1f, laserHeadingP2, Direction.Left, Player.PlayerTwo, true);

        // Trim down unused list elements
        while (lasers.Count > laserIndex+1) {
            lasers.RemoveAt(lasers.Count-1);
        }
    }

    private void laserStep(int x, int y, float strength, Direction heading, Direction direction, Player player, bool newLaser)
    {
        if (x < 0 || y < 0 || x >= gridManager.theGrid.getDimX() || y >= gridManager.theGrid.getDimY() || recursionCount > recursionLimit) return;
        recursionCount++;

        // add laser node to list
        if (newLaser) {
            laserIndex++;
            if (lasers.Count == laserIndex) lasers.Add(new List<laserNode>());
        }
        lasers[laserIndex].Add(new laserNode(x, y, strength, heading, direction, player));


        

        int newX; int newY; Direction newDir;
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

                    laserSolver(newX, newY, strength, heading, newDir, player);
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

                    laserSolver(newX, newY, strength, heading, newDir, player);
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

                    laserSolver(newX, newY, strength, heading, newDir, player);
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

                    laserSolver(newX, newY, strength, heading, newDir, player);
                    break;
                }
        }
    }

    private void laserSolver(int x, int y, float strength, Direction heading, Direction direction, Player player)
    {
        // if laser is out of bounds return
        if (x < 0 || y < 0 || x >= gridManager.theGrid.getDimX() || y >= gridManager.theGrid.getDimY()) return;
        switch (gridManager.theGrid.getBuilding(x, y)) {
            case Building.Empty: laserStep(x, y, strength - laserDecay, heading, direction, player, false); break;
            case Building.Blocking: break; // need to push laser hit
            case Building.Reflecting: laserReflect(x, y, strength - laserDecay, heading, direction, player); break;
            case Building.Refracting: laserStep(x, y, strength - laserDecay, heading, direction, player, false); laserReflect(x, y, strength - laserDecay, heading, direction, player); break;
            case Building.Redirecting: break;
            case Building.Portal: break;
            case Building.Resource: laserStep(x, y, strength - laserDecay, heading, direction, player, false); break; // need to push laser hit
            case Building.Base: break;
            case Building.Laser: laserStep(x, y, strength - laserDecay, heading, direction, player, false); break;

        }
    }

    private float powerSolver(int x, int y)
    {
        return 0.15f;
    }

    private void laserReflect(int x, int y, float strength, Direction heading, Direction direction, Player player)
    {
        switch (direction) {
            case Direction.Down: laserStep(x, y+1, strength + powerSolver(x, y), heading == Direction.SE ? Direction.NE : Direction.NW, Direction.Up, player, true); break;
            case Direction.Up: laserStep(x, y-1, strength + powerSolver(x, y), heading == Direction.NE ? Direction.SE : Direction.SW, Direction.Down, player, true); break;
            case Direction.Left: laserStep(x+1, y, strength + powerSolver(x, y), heading == Direction.SW ? Direction.SE : Direction.NE, Direction.Right, player, true); break;
            case Direction.Right: laserStep(x-1, y, strength + powerSolver(x, y), heading == Direction.SE ? Direction.SW : Direction.NW, Direction.Left, player, true); break;
        }
    }
    
    void OnDrawGizmos()
    {
        if (lasers != null) {
            float dimX = gridManager.theGrid.getDimX();
            float dimY = gridManager.theGrid.getDimY();
            Gizmos.color = new Color(1f, 0, 0, 1f);
            for (int i = 0; i < lasers.Count; i++) {
                for (int j = 0; j < lasers[i].Count; j++) {
                    Gizmos.color = lasers[i][j].getOwner() == Player.PlayerOne ? new Color(1f, 0, 0, lasers[i][j].getStrength()) : new Color(0f, 1f, 0f, lasers[i][j].getStrength());
                    Gizmos.DrawSphere(new Vector3((-dimX / 2) + lasers[i][j].getX() + 0.5f, 0.5f * 0.5f, (-dimY / 2) + lasers[i][j].getY() + 0.5f), 0.25f);
                }
            }
        }
    }
    
}

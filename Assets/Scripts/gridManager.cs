using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

// README: ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                                  //
//  Grid coordinates start from the bottom left corner. IE: Increasing X values go right, increasing Y values go up.                //
//  Here is a list of accessible methods:                                                                                           //
//                                                                                                                                  //
//  getCellInfo(int x, int y)                                               - returns GridItem at specified coords.                 //
//  placeBuilding(int x, int y, Building newBuilding, Player playerID)      - Places a building into theGrid, rotation optional.    //
//  removeBuilding(int x, int y, Player playerID)                           - Removes a building, returns half of its resources.    //
//  destroyBuilding(int x, int y)                                           - Removes a building from theGrid.                      //
//  moveBuilding(int x, int y, int xNew, int yNew, Player playerID)         - Moves a building to a new location.                   //
//  upgradeBuilding(int x, int y, Player playerID)                          - Upgrades a building if it can be upgraded.            //
//                                                                                                                                  //
//  *** The above functions will return true if the succeed, or false if they fail.                                                 //
//  *** Functions are called like this: gridManager.theGrid.placeBuilding(x, y, Building.Blocking, Player.PlayerOne);               //
//  *** The player parameter should be the player trying to perform the action. IE: PlayerOne if PlayerOne is placing a building.   //
//                                                                                                                                  //
//  Example:                                                                                                                        //
//  if (gridManager.theGrid.placeBuilding(x, y, Building.Blocking, Player.PlayerOne, Direction.Up)) {                               //
//      print("Success");                                                                                                           //
//  } else {                                                                                                                        //
//      print("Failed to place building, make sure you select a valid location");                                                   //
//  }                                                                                                                               //
//                                                                                                                                  //
//  Example2:                                                                                                                       //
//  print(gridManager.theGrid.getCellInfo(x, y).toString()); // Use this for debugging grid cells.                                  //
//                                                                                                                                  //
//  Example3:                                                                                                                       //
//  foreach (Transform building in gridManager.theGrid.getBuildingContainer().transform)    // Print the health of all buildings    //
//      print(building.GetComponent<buildingParameters>().currentHP);                                                               //
//                                                                                                                                  //
//                                                                                                                                  //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public enum Building { Base, Laser, Blocking, Reflecting, Refracting, Redirecting, Resource, Portal, Empty };
public enum Player { World, PlayerOne, PlayerTwo, Shared }; // World Refers to neutral spaces owned by neither player
public enum Direction { None, NE, NW, SE, SW, Left, Right, Up, Down };

public struct XY
{
    public int x, y;
    public XY(int X, int Y)
    {
        x = X;
        y = Y;
    }
    public bool Equals(XY other)
    {
        return x == other.x && y == other.y;
    }
}

public struct GridItem
{
    public bool isEmpty;        // Empty grid cell?
    public Building building;   // Building type
    public Player owner;        // Who owns the block
    public Direction direction; // Used for laser block direction, other block rotations
    public int[] weakSides;     // {left, right, top, down} 1 for weak, 0 for not
    public byte level;          // Upgrade level
    public float health;

    public GridItem(bool emptyCell, Building buildingID, Player ownedBy, Direction facingDirection, float hitpoints)
    {
        isEmpty = emptyCell;
        building = buildingID;
        level = 0;
        owner = ownedBy;
        direction = facingDirection;
        weakSides = new int[4] { 0, 0, 0, 0 };
        health = hitpoints;
    }

    public string toString()    // Convert GridItem to string for easy printing
    {
        return "isEmpty: " + isEmpty + "  |  Building: " + building + "  |  Level: " + level + "  |  HP: " + health + "  |  Direction: " + direction + "  |  Owner: " + owner;
    }
}

public struct Grid
{
    private GridItem[,] grid;
    private GameObject buildingContainer;
    private GameObject[] buildingPrefabs;
    private Dictionary<XY, GameObject> prefabDictionary;
    private int dimX;
    private int dimY;
    private float resourcesP1;
    private float resourcesP2;
    private bool needsUpdate;
    private GameObject baseP1;
    private GameObject baseP2;

    public Grid(int x, int y, GameObject container, GameObject basePrefab, GameObject basePrefab2, GameObject laserPrefab, GameObject laserPrefab2, GameObject blockPrefab, GameObject blockPrefab2,
        GameObject reflectPrefab, GameObject reflectPrefab2, GameObject refractPrefab, GameObject refractPrefab2, GameObject redirectPrefab, GameObject redirectPrefab2, GameObject resourcePrefab,
        GameObject resourcePrefab2, GameObject portalPrefab, GameObject portalPrefab2, float resources, GameObject emptyHolder)
    {
        grid = new GridItem[y, x];
        for (int row = 0; row < y; row++) {
            for (int col = 0; col < x; col++) {
                grid[row, col] = new GridItem(true, Building.Empty, Player.World, Direction.None, 0);
                GameObject empty = MonoBehaviour.Instantiate(emptyHolder);    //makes transparent planes on each grid square
                empty.transform.localPosition = new Vector3((-x / 2) + col + 0.5f, 0, (-y / 2) + row + 0.5f);
            }
        }
        dimX = x;
        dimY = y;
        buildingContainer = container;
        buildingPrefabs = new GameObject[16]; // 8 Buildings x 2 Players
        buildingPrefabs[(int)Building.Base] = basePrefab;
        buildingPrefabs[(int)Building.Laser] = laserPrefab;
        buildingPrefabs[(int)Building.Blocking] = blockPrefab;
        buildingPrefabs[(int)Building.Reflecting] = reflectPrefab;
        buildingPrefabs[(int)Building.Refracting] = refractPrefab;
        buildingPrefabs[(int)Building.Redirecting] = redirectPrefab;
        buildingPrefabs[(int)Building.Resource] = resourcePrefab;
        buildingPrefabs[(int)Building.Portal] = portalPrefab;
        buildingPrefabs[(int)Building.Base + 8] = basePrefab2;
        buildingPrefabs[(int)Building.Laser + 8] = laserPrefab2;
        buildingPrefabs[(int)Building.Blocking + 8] = blockPrefab2;
        buildingPrefabs[(int)Building.Reflecting + 8] = reflectPrefab2;
        buildingPrefabs[(int)Building.Refracting + 8] = refractPrefab2;
        buildingPrefabs[(int)Building.Redirecting + 8] = redirectPrefab2;
        buildingPrefabs[(int)Building.Resource + 8] = resourcePrefab2;
        buildingPrefabs[(int)Building.Portal + 8] = portalPrefab2;
        prefabDictionary = new Dictionary<XY, GameObject>(buildingPrefabs.Length);
        resourcesP1 = resources;
        resourcesP2 = resources;
        baseP1 = null;
        baseP2 = null;
        needsUpdate = false;
    }

    private bool validateInput(int x, int y)
    {
        if (x < 0 || y < 0 || x >= dimX || y >= dimY) return false;
        return true;
    }

    private Vector3 directionToEular(Direction direction) // obsolete?
    {
        switch (direction) {
            case Direction.Right: return new Vector3(90, 90, 0);
            case Direction.Down: return new Vector3(90, 180, 0);
            case Direction.Left: return new Vector3(90, 270, 0);
        }
        return new Vector3(90, 0, 0);
    }

    private int directionToIndex(Direction direction)
    {
        switch (direction) {
            case Direction.Right: return 1;
            case Direction.Down: return 3;
            case Direction.Left: return 0;
        }
        return 2;
    }

    public float getCost(Building building, int x = -1, Player player = Player.World, bool moving = false, bool removing = false, bool swaping = false)
    {
        if (building == Building.Empty) return 0f;
        float cost = buildingPrefabs[(int)building].GetComponent<buildingParameters>().cost;
        if (x == -1) return cost;
        if (player == Player.PlayerOne && x >= gridManager.theGrid.getDimX() / 2) cost *= 2f;
        else if (player == Player.PlayerTwo && x < gridManager.theGrid.getDimX() / 2) cost *= 2f;
        if (moving || removing) cost /= 2f;
        else if (swaping) cost /= 4f;
        return cost;
    }

    public GridItem getCellInfo(int x, int y) { return validateInput(x, y) ? grid[y, x] : new GridItem(true, Building.Empty, Player.World, Direction.None, 0); }
    public Building getBuilding(int x, int y) { return validateInput(x, y) ? grid[y, x].building : Building.Empty; }
    public Direction getDirection(int x, int y) { return validateInput(x, y) ? grid[y, x].direction : Direction.None; }
    public int getDimX() { return dimX; }
    public int getDimY() { return dimY; }
    public float getResourcesP1() { return resourcesP1; }
    public float getResourcesP2() { return resourcesP2; }
    public float baseHealthP1() { return baseP1 != null ? baseP1.GetComponent<buildingParameters>().currentHP : 0f; }
    public float baseHealthP2() { return baseP2 != null ? baseP2.GetComponent<buildingParameters>().currentHP : 0f; }
    public void addResources(float p1, float p2) { resourcesP1 += p1; resourcesP2 += p2; }
    public bool updateLaser() { return needsUpdate; }
    public void updateFinished() { needsUpdate = false; }
    public GameObject getBuildingContainer() { return buildingContainer; }

    public bool applyDamage(int x, int y, float damage)
    {
        if (!validateInput(x, y)) return false;
        if (!grid[y, x].isEmpty) {
            grid[y, x].health -= damage;
            prefabDictionary[new XY(x, y)].GetComponent<buildingParameters>().currentHP = grid[y, x].health;
            if (grid[y, x].health <= 0f) {
                if (getBuilding(x, y) == Building.Base) SceneManager.LoadScene("GameOver", LoadSceneMode.Single); // Add player specific win screen in future
                else destroyBuilding(x, y);
            }
        } else return false;
        return true;
    }

    public bool placeBuilding(int x, int y, Building newBuilding, Player playerID, Direction facing = Direction.Up)
    {
        if (!validateInput(x, y)) return false;
        if (grid[y, x].isEmpty && newBuilding != Building.Empty && (playerID == Player.PlayerOne ? resourcesP1 : resourcesP2) >= getCost(newBuilding)) {
            grid[y, x].isEmpty = false;
            grid[y, x].building = newBuilding;
            grid[y, x].owner = playerID;
            grid[y, x].direction = facing;
            // Add Weak Side(s)
            if (newBuilding == Building.Reflecting || newBuilding == Building.Blocking || newBuilding == Building.Resource) {
                //if ((int)facing > 4 && (int)facing < 9) grid[y, x].weakSides[(int)facing-5] = 1;
                if (facing == Direction.Left) grid[y, x].weakSides[1] = 1;
                if (facing == Direction.Right) grid[y, x].weakSides[0] = 1;
                if (facing == Direction.Up) grid[y, x].weakSides[3] = 1;
                if (facing == Direction.Down) grid[y, x].weakSides[2] = 1;
            }
            // Place Building Prefab
            GameObject building = MonoBehaviour.Instantiate(buildingPrefabs[(int)newBuilding + (playerID == Player.PlayerOne ? 0 : 8)]);
            grid[y, x].health = building.GetComponent<buildingParameters>().health; // Building starting health
            building.GetComponent<buildingParameters>().x = x;
            building.GetComponent<buildingParameters>().y = y;
            building.GetComponent<buildingParameters>().owner = playerID;
            building.GetComponent<buildingParameters>().currentHP = building.GetComponent<buildingParameters>().health;
            if (newBuilding == Building.Reflecting) { // This if statement will be removed once all buildings are set up properly
                building.AddComponent<SpriteRenderer>();
                building.GetComponent<SpriteRenderer>().sprite = building.GetComponent<buildingParameters>().sprites[directionToIndex(facing)];
                building.GetComponent<Renderer>().material.color = playerID == Player.PlayerOne ? new Vector4(1f, 0.7f, 0.7f, 1f) : new Vector4(0.7f, 1, 0.7f, 1f);
            }
            else building.GetComponent<Renderer>().material.color = playerID == Player.PlayerOne ? Color.red : Color.green; // Used for debugging, not necessary with final art
            building.transform.SetParent(buildingContainer.transform);
            building.transform.localPosition = new Vector3((-dimX / 2) + x + 0.5f, 0, (-dimY / 2) + y + 0.5f);
            building.transform.localEulerAngles = new Vector3(90, 0, 0);
            prefabDictionary.Add(new XY(x, y), building);
            // Subtract Cost From Resources
            if (playerID == Player.PlayerOne) resourcesP1 -= getCost(newBuilding, x, playerID);
            else resourcesP2 -= getCost(newBuilding, x, playerID);
            // Set base references for getting health later
            if (newBuilding == Building.Base) { if (playerID == Player.PlayerOne) baseP1 = building; else baseP2 = building; }
            // Specify that the board was updated and that laserLogic needs to run a simulation
            needsUpdate = true;
        } else return false;
        return true;
    }

    // removeBuilding, unlike destroyBuilding, restores half of a building's cost
    public bool removeBuilding(int x, int y, Player playerID)
    {
        if (!validateInput(x, y)) return false;
        if (!grid[y, x].isEmpty && (playerID == grid[y, x].owner || playerID == Player.World)) {
            if (grid[y, x].building == Building.Base) { if (grid[y, x].owner == Player.PlayerOne) baseP1 = null; else baseP2 = null; }  // Remove Base Reference
            grid[y, x].isEmpty = true;
            grid[y, x].building = Building.Empty;
            grid[y, x].owner = Player.World;
            grid[y, x].level = 0;
            grid[y, x].health = 0;
            // Reset Weak Sides
            for (int i = 4; i < 4; i++) grid[y, x].weakSides[i] = 0;
            // Give some resources back to player
            if (playerID == Player.PlayerOne) resourcesP1 += getCost(prefabDictionary[new XY(x, y)].GetComponent<buildingParameters>().building, x, playerID, false, true);
            else resourcesP2 += getCost(prefabDictionary[new XY(x, y)].GetComponent<buildingParameters>().building, x, playerID, false, true);
            // Remove Building Prefab
            MonoBehaviour.DestroyImmediate(prefabDictionary[new XY(x, y)]);
            prefabDictionary.Remove(new XY(x, y));
            // Specify that the board was updated and that laserLogic needs to run a simulation
            needsUpdate = true;
        } else return false;
        return true;
    }

    public bool destroyBuilding(int x, int y)
    {
        if (!validateInput(x, y)) return false;
        if (!grid[y, x].isEmpty) {
            if (grid[y, x].building == Building.Base) { if (grid[y, x].owner == Player.PlayerOne) baseP1 = null; else baseP2 = null; }  // Remove Base Reference
            grid[y, x].isEmpty = true;
            grid[y, x].building = Building.Empty;
            grid[y, x].owner = Player.World;
            grid[y, x].level = 0;
            grid[y, x].health = 0;
            // Reset Weak Sides
            for (int i = 4; i < 4; i++) grid[y, x].weakSides[i] = 0;
            // Remove Building Prefab
            MonoBehaviour.DestroyImmediate(prefabDictionary[new XY(x, y)]);
            prefabDictionary.Remove(new XY(x, y));
            // Specify that the board was updated and that laserLogic needs to run a simulation
            needsUpdate = true;
        } else return false;
        return true;
    }

    public bool moveBuilding(int x, int y, int xNew, int yNew, Player playerID, Direction rotation) // need to add rotation?
    {
        if (!validateInput(x, y) || !validateInput(xNew, yNew)) return false;
        if (!grid[y, x].isEmpty && (grid[yNew, xNew].isEmpty || (x == xNew && y == yNew)) && playerID == grid[y, x].owner) {
            grid[yNew, xNew].isEmpty = false;
            grid[yNew, xNew].building = grid[y, x].building;
            grid[yNew, xNew].owner = playerID;
            grid[yNew, xNew].direction = rotation;
            grid[yNew, xNew].weakSides = grid[y, x].weakSides;
            grid[yNew, xNew].level = grid[y, x].level;
            grid[yNew, xNew].health = grid[y, x].health;
            grid[y, x].isEmpty = true;
            grid[y, x].building = Building.Empty;
            grid[y, x].owner = Player.World;
            grid[y, x].weakSides = new int[] { 0, 0, 0, 0 };
            grid[y, x].level = 0;
            grid[y, x].health = 0;
            // Move Building Prefab
            GameObject building = prefabDictionary[new XY(x, y)];
            building.GetComponent<buildingParameters>().x = xNew;
            building.GetComponent<buildingParameters>().y = yNew;
            building.transform.localPosition = new Vector3((-dimX / 2) + xNew + 0.5f, 0, (-dimY / 2) + yNew + 0.5f);
            prefabDictionary.Remove(new XY(x, y));
            prefabDictionary.Add(new XY(xNew, yNew), building);
            // Subtract some resources for move
            if (playerID == Player.PlayerOne) resourcesP1 -= getCost(building.GetComponent<buildingParameters>().building, xNew, playerID, true);
            else resourcesP2 -= getCost(building.GetComponent<buildingParameters>().building, xNew, playerID, true);
            // Specify that the board was updated and that laserLogic needs to run a simulation
            needsUpdate = true;
        } else return false;
        return true;
    }

    public bool swapBuilding(int x, int y, int xNew, int yNew, Player playerID) // need to add rotation?
    {
        if (!validateInput(x, y) || !validateInput(xNew, yNew)) return false;
        if (!grid[y, x].isEmpty && !grid[yNew, xNew].isEmpty && playerID == grid[y, x].owner && playerID == grid[yNew, xNew].owner) {
            Building tempBuild = grid[yNew, xNew].building;
            Direction tempDir = grid[yNew, xNew].direction;
            int[] tempWeakSides = grid[yNew, xNew].weakSides;
            byte tempLevel = grid[yNew, xNew].level;
            float tempHealth = grid[yNew, xNew].health;
            grid[yNew, xNew].building = grid[y, x].building;
            grid[yNew, xNew].direction = grid[y, x].direction;
            grid[yNew, xNew].weakSides = grid[y, x].weakSides;
            grid[yNew, xNew].level = grid[y, x].level;
            grid[yNew, xNew].health = grid[y, x].health;
            grid[y, x].building = tempBuild;
            grid[y, x].direction = tempDir;
            grid[y, x].weakSides = tempWeakSides;
            grid[y, x].level = tempLevel;
            grid[y, x].health = tempHealth;
            // Swap Building Prefab
            GameObject building = prefabDictionary[new XY(x, y)];
            GameObject building2 = prefabDictionary[new XY(xNew, yNew)];
            building.GetComponent<buildingParameters>().x = xNew;
            building.GetComponent<buildingParameters>().y = yNew;
            building2.GetComponent<buildingParameters>().x = x;
            building2.GetComponent<buildingParameters>().y = y;
            building.transform.localPosition = new Vector3((-dimX / 2) + xNew + 0.5f, 0, (-dimY / 2) + yNew + 0.5f);
            building.transform.localPosition = new Vector3((-dimX / 2) + x + 0.5f, 0, (-dimY / 2) + y + 0.5f);
            prefabDictionary.Remove(new XY(x, y));
            prefabDictionary.Remove(new XY(xNew, yNew));
            prefabDictionary.Add(new XY(xNew, yNew), building);
            prefabDictionary.Add(new XY(x, y), building2);
            // Subtract some resources for swapped buildings (equal to the cost to individually move each building / 2)
            if (playerID == Player.PlayerOne) resourcesP1 -= getCost(building.GetComponent<buildingParameters>().building, xNew, playerID, false, false, true);
            else resourcesP2 -= getCost(building.GetComponent<buildingParameters>().building, xNew, playerID, false, false, true);
            if (playerID == Player.PlayerOne) resourcesP1 -= getCost(building2.GetComponent<buildingParameters>().building, x, playerID, false, false, true);
            else resourcesP2 -= getCost(building2.GetComponent<buildingParameters>().building, x, playerID, false, false, true);
            // Specify that the board was updated and that laserLogic needs to run a simulation
            needsUpdate = true;
        } else return false;
        return true;
    }

    /* Backlog
    private bool canBeUpgraded(GridItem item)
    {
        // Add upgrade conditions here
        return true;
    }

    public bool upgradeBuilding(int x, int y, Player playerID)
    {
        if (!validateInput(x, y)) return false;
        if (!grid[y, x].isEmpty && playerID == grid[y, x].owner && canBeUpgraded(grid[y, x])) {
            grid[y, x].level++;
        } else return false;
        return true;
    }*/
}

public class gridManager : MonoBehaviour
{
    public static Grid theGrid;
    public int boardWidth = 14;
    public int boardHeight = 10;
    public float startingResources = 20;
    public GameObject Base;
    public GameObject Base2;
    public GameObject Laser;
    public GameObject Laser2;
    public GameObject Block;
    public GameObject Block2;
    public GameObject Reflect;
    public GameObject Reflect2;
    public GameObject Refract;
    public GameObject Refract2;
    public GameObject Redirect;
    public GameObject Redirect2;
    public GameObject Resource;
    public GameObject Resource2;
    public GameObject Portal;
    public GameObject Portal2;

    public GameObject empty;
    private GameObject buildingContainer;

    void Awake()
    {
        buildingContainer = new GameObject("buildingContainer");
        buildingContainer.transform.SetParent(gameObject.transform);
        theGrid = new Grid(boardWidth, boardHeight, buildingContainer, Base, Base2, Laser, Laser2, Block, Block2, Reflect, Reflect2, Refract, Refract2, Redirect, Redirect2, Resource, Resource2, Portal, Portal2, startingResources, empty);


    }

    // Debug building placements
    // Comment out to hide gizmos
    void OnDrawGizmos()
    {
        // Note: one grid cell = 1x1 meter in unity
        int dimX = theGrid.getDimX();
        int dimY = theGrid.getDimY();

        for (int row = 0; row < theGrid.getDimY(); row++) {
            for (int col = 0; col < theGrid.getDimX(); col++) {
                Gizmos.color = new Color(1f, 1f, 1f, .8f);
                //Gizmos.DrawCube(new Vector3((-dimX/2)+col+0.5f, -0.5f, (-dimY/2)+row+0.5f), new Vector3(.8f, 1f, .8f));
                if (theGrid.getCellInfo(col, row).isEmpty) {
                    Gizmos.color = new Color(1f, 1f, 1f, .8f);
                    //Gizmos.DrawCube(new Vector3((-dimX / 2) + col + 0.5f, 0.5f, (-dimY / 2) + row + 0.5f), Vector3.one);
                }
            }
        }
    }

}

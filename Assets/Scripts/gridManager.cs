using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// README: ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                                  //
//  Grid coordinates start from the bottom left corner. IE: Increasing X values go right, increasing Y values go up.                //
//  Here is a list of accessible methods:                                                                                           //
//                                                                                                                                  //
//  getCellInfo(int x, int y)                                               - returns GridItem at specified coords.                 //
//  placeBuilding(int x, int y, Building newBuilding, Player playerID)      - Places a building into theGrid.                       //
//  destroyBuilding(int x, int y, Player playerID)                          - Removes a building from theGrid.                      //
//  moveBuilding(int x, int y, int xNew, int yNew, Player playerID)         - Moves a building to a new location.                   //
//  upgradeBuilding(int x, int y, Player playerID)                          - Upgrades a building if it can be upgraded.            //
//                                                                                                                                  //
//  *** The above functions will return true if the succeed, or false if they fail.                                                 //
//  *** Functions are called like this: gridManager.theGrid.placeBuilding(x, y, Building.Blocking, Player.PlayerOne);               //
//  *** The player parameter should be the player trying to perform the action. IE: PlayerOne if PlayerOne is placing a building.   //
//                                                                                                                                  //
//  Example:                                                                                                                        //
//  if (gridManager.theGrid.placeBuilding(x, y, Building.Blocking, Player.PlayerOne)) {                                             //
//      decrementResources(Building.Blocking, Player.PlayerOne, x); // x can be used to determine if on enemy side (increased cost) //
//  } else {                                                                                                                        //
//      print("Failed to place building, make sure you select a valid location");                                                   //
//  }                                                                                                                               //
//                                                                                                                                  //
//  Example2:                                                                                                                       //
//  print(gridManager.theGrid.getCellInfo(x, y).toString()); // Use this for debugging grid cells.                                  //
//                                                                                                                                  //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public enum Building { Empty, Base, Blocking, Reflecting, Refracting, Redirecting, Portal, Resource, Laser };
public enum Player { World, PlayerOne, PlayerTwo, Shared }; // World Refers to neutral spaces owned by neither player
public enum Direction { None, NE, NW, SE, SW, Left, Right, Up, Down };

public struct GridItem
{
    public bool isEmpty;        // Empty grid cell?
    public Building building;   // Building type
    public Player owner;        // Who owns the block
    public Direction direction; // Used for laser block direction, other block rotations
    public byte level;          // Upgrade level

    public GridItem(bool emptyCell, Building buildingID, Player ownedBy, Direction facingDirection, byte upgradeLevel)
    {
        isEmpty = emptyCell;
        building = buildingID;
        level = upgradeLevel;
        owner = ownedBy;
        direction = facingDirection;
    }

    public string toString()    // Convert GridItem to string for easy printing
    {
        return "isEmpty: " + isEmpty + "  |  Building: " + building + "  |  Level: " + level + "  |  Direction: " + direction + "  |  Owner: " + owner;
    }
}

public struct Grid
{
    private GridItem[,] grid;
    private int dimX;
    private int dimY;

    public Grid(int x, int y)
    {
        grid = new GridItem[y, x];
        for (int row = 0; row < y; row++) {
            for (int col = 0; col < x; col++) {
                grid[row, col] = new GridItem(true, Building.Empty, Player.World, Direction.None, 0);
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

    public GridItem getCellInfo(int x, int y) { return validateInput(x, y) ? grid[y, x] : new GridItem(true, Building.Empty, Player.World, Direction.None, 0); }
    public Building getBuilding(int x, int y) { return validateInput(x, y) ? grid[y, x].building : Building.Empty; }
    public Direction getDirection(int x, int y) { return validateInput(x, y) ? grid[y, x].direction : Direction.None; }
    public int getDimX() { return dimX; }
    public int getDimY() { return dimY; }

    public bool placeBuilding(int x, int y, Building newBuilding, Player playerID, Direction facing = Direction.None)
    {
        if (!validateInput(x, y)) return false;
        if (grid[y, x].isEmpty) {
            grid[y, x].isEmpty = false;
            grid[y, x].building = newBuilding;
            grid[y, x].owner = playerID;
            grid[y, x].direction = facing;
        } else return false;
        return true;
    }

    public bool destroyBuilding(int x, int y, Player playerID)
    {
        if (!validateInput(x, y)) return false;
        if (!grid[y, x].isEmpty && (playerID == grid[y, x].owner || playerID == Player.World)) {
            grid[y, x].isEmpty = true;
            grid[y, x].building = Building.Empty;
            grid[y, x].owner = Player.World;
            grid[y, x].level = 0;
        } else return false;
        return true;
    }

    public bool moveBuilding(int x, int y, int xNew, int yNew, Player playerID)
    {
        if (!validateInput(x, y) || !validateInput(xNew, yNew)) return false;
        if (!grid[y, x].isEmpty && grid[yNew, xNew].isEmpty && playerID == grid[y, x].owner) {
            grid[yNew, xNew].isEmpty = false;
            grid[yNew, xNew].building = grid[y, x].building;
            grid[yNew, xNew].owner = playerID;
            grid[yNew, xNew].direction = grid[y, x].direction;
            grid[y, x].isEmpty = true;
            grid[y, x].building = Building.Empty;
            grid[y, x].owner = Player.World;
            grid[y, x].level = 0;
        } else return false;
        return true;
    }

    public bool swapBuilding(int x, int y, int xNew, int yNew, Player playerID)
    {
        if (!validateInput(x, y) || !validateInput(xNew, yNew)) return false;
        if (!grid[y, x].isEmpty && !grid[yNew, xNew].isEmpty && playerID == grid[y, x].owner && playerID == grid[yNew, xNew].owner)
        {//only swaps building types and directions since it is still not empty and it needs to be the same owner for both
            Building tempBuild = grid[yNew, xNew].building;
            Direction tempDir = grid[yNew, xNew].direction;
            grid[yNew, xNew].building = grid[y, x].building;
            grid[yNew, xNew].direction = grid[y, x].direction;
            grid[y, x].building = tempBuild;
            grid[y, x].direction = tempDir;
        } else return false;
        return true;
    }

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
    }
}

public class gridManager : MonoBehaviour {

    public static Grid theGrid;
    public int boardWidth = 14;
    public int boardHeight = 10;

    void Awake () {
        theGrid = new Grid(boardWidth, boardHeight);
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
                if ((row%2 == 0 && col%2 == 0) || (row % 2 != 0 && col % 2 != 0)) Gizmos.color = new Color(1f, 1f, 1f, 1f);
                else Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                Gizmos.DrawCube(new Vector3((-dimX/2)+col+0.5f, -0.5f, (-dimY/2)+row+0.5f), Vector3.one);
                if (!theGrid.getCellInfo(col, row).isEmpty) {
                    Gizmos.color = theGrid.getCellInfo(col, row).owner == Player.PlayerOne ? new Color(1f, 0, 0, 1f) : new Color(0, 1f, 0, 1f);
                    //Gizmos.DrawCube(new Vector3((-dimX / 2) + col + 0.5f, 0.5f, (-dimY / 2) + row + 0.5f), Vector3.one);
                }
            }
        }
    }
    
}

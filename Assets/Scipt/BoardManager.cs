using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Building { Empty, Base, Blocking, Reflecting, Refracting, Redirecting, Portal, Resource, Laser };
public enum Player { World, PlayerOne, PlayerTwo };

public struct GridItem
{
	public bool isEmpty;
	public Building building;
	public byte level;
	public Player owner;

	public GridItem(bool emptyCell, Building buildingID, byte upgradeLevel, Player ownedBy)
	{
		isEmpty = emptyCell;
		building = buildingID;
		level = upgradeLevel;
		owner = ownedBy;
	}

	public string toString()
	{
		return "isEmpty: " + isEmpty + "  |  Building: " + building + "  |  Level: " + level;
	}
}

public struct Grid
{
	private GridItem[,] grid;
	private int dimX;
	private int dimY;
	private float scale;

	public Grid(int x, int y, float scaleF)
	{
		grid = new GridItem[y, x];
		for (int row = 0; row < y; row++) {
			for (int col = 0; col < x; col++) {
				grid[row, col] = new GridItem(true, Building.Empty, 0, Player.World);
			}
		}
		dimX = x;
		dimY = y;
		scale = scaleF;
	}

	public GridItem getCellInfo(int x, int y)
	{
		return grid[y, x];
	}

	public int getDimX() { return dimX; }
	public int getDimY() { return dimY; }
	public float getScale() { return scale; }

	public bool placeBuilding(int x, int y, Building newBuilding, Player playerID)
	{
		if (grid[y, x].isEmpty) {
			grid[y, x].isEmpty = false;
			grid[y, x].building = newBuilding;
			grid[y, x].owner = playerID;
		} else return false;
		return true;
	}

	public bool destroyBuilding(int x, int y, Player playerID)
	{
		if (!grid[y, x].isEmpty && playerID == grid[y, x].owner) {
			grid[y, x].isEmpty = true;
			grid[y, x].building = Building.Empty;
			grid[y, x].owner = Player.World;
		} else return false;
		return true;
	}

	public bool moveBuilding(int x, int y, int xNew, int yNew, Player playerID)
	{
		if (!grid[y, x].isEmpty && grid[yNew, xNew].isEmpty && playerID == grid[y, x].owner) {
			grid[yNew, xNew].isEmpty = false;
			grid[yNew, xNew].building = grid[y, x].building;
			grid[yNew, xNew].owner = playerID;
			grid[y, x].isEmpty = true;
			grid[y, x].building = Building.Empty;
			grid[y, x].owner = Player.World;

		} else return false;
		return true;
	}

	public bool canBeUpgraded(GridItem item)
	{
		// Add upgrade conditions here
		return true;
	}

	public bool upgradeBuilding(int x, int y, Player playerID)
	{
		if (!grid[y, x].isEmpty && playerID == grid[y, x].owner && canBeUpgraded(grid[y, x])) {
			grid[y, x].level++;
		} else return false;
		return true;
	}
}


public class BoardManager : MonoBehaviour {
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;
    private const int TILE_DIM = 8;

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> chessmanPrefabs;
    private List<GameObject> activeChessman;

	private Grid theGrid;

	void Start () {
		theGrid = new Grid(14, 10, 1f);
		// ex: theGrid.placeBuilding(5, 3, Building.Blocking, Player.PlayerOne);
	}

    private void Update() {

        UpdateSelection();
        DrawChessboard();
    }

    private void UpdateSelection() {
        if (!Camera.main)
            return;

        RaycastHit hit;
        //Raycast returns bool of: 
        //(origin [camera array], info put into the out parameter to use later (if there is a collision), max distance, layer mask (only hit chess board not chess piece))
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane"))) {
            Debug.Log(hit.point);
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else{
            selectionX = -1;
            selectionY = -1;
        }
    }

    private void DrawChessboard() {
        Vector3 widthLine = Vector3.right * TILE_DIM; //Unity vector of length 1 pointing right * 8 (because there are 8 tiles in a chess game)
        Vector3 heightLine = Vector3.forward * TILE_DIM; //same as above, but pointing forward

        for(int i=0; i<=TILE_DIM ; i++) {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for(int j=0; j<=TILE_DIM; j++) {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }

        //Draw the selection
        if(selectionX >= 0 && selectionY >= 0) {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX, 
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX +1));
            Debug.DrawLine(
                Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cursor2 : MonoBehaviour
{
    // Public Vars
    public GameObject laserPrefab;
    //Should be changed to be a list of all possible buildings

    // Private Vars
    private bool moving = false;
    private bool posMove = true;
    private int speed = 10;
    private int buttonPress = 0;
    private int dimX;
    private int dimZ;
    private MoveDir dir = MoveDir.Up;
    public static Vector3 pos;
    private int currentBuilding = (int)Building.Laser;
    private int numberOfTypes = System.Enum.GetValues(typeof(Building)).Length;
    // Use this for initialization
    void Start()
    {
        dimX = gridManager.theGrid.getDimX() / 2;
        dimZ = gridManager.theGrid.getDimY() / 2;
    }

    // Update is called once per frame
    void Update()
    {
        //buildingControls();
        buttonPress--;
        if (posMove)
        {
            pos = transform.position;
            moveTower();
        }
        // Check if cursor is moving and doesn't move outside the board
        if (moving && pos.x >= -dimX && pos.x <= dimX
                   && pos.z >= -dimZ && pos.z <= dimZ)
        {
            if (/*curr.*/transform.position == pos)
            {
                moving = false;
                posMove = true;
                moveTower();
            }
            transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
        }
        // Moved outside of board, so return to last position
        else if (dir == MoveDir.Up)
        {
            pos += Vector3.back;
        }
        else if (dir == MoveDir.Down)
        {
            pos += Vector3.forward;
        }
        else if (dir == MoveDir.Left)
        {
            pos += Vector3.right;
        }
        else if (dir == MoveDir.Right)
        {
            pos += Vector3.left;
        }
    }
    private void moveTower()
    {
        if (buttonPress <= 0)
        {
            if (Input.GetKey(KeyCode.I))
            {
                if (dir != MoveDir.Up)
                {
                    buttonPress = 3;
                    dir = MoveDir.Up;
                }
                else
                {
                    posMove = false;
                    moving = true;
                    pos += Vector3.forward;
                }
            }
            else if (Input.GetKey(KeyCode.K))
            {
                if (dir != MoveDir.Down)
                {
                    buttonPress = 3;
                    dir = MoveDir.Down;
                }
                else
                {
                    posMove = false;
                    moving = true;
                    pos += Vector3.back;
                }
            }
            else if (Input.GetKey(KeyCode.J))
            {
                if (dir != MoveDir.Left)
                {
                    buttonPress = 3;
                    dir = MoveDir.Left;
                }
                else
                {
                    posMove = false;
                    moving = true;
                    pos += Vector3.left;
                }
            }
            else if (Input.GetKey(KeyCode.L))
            {
                if (dir != MoveDir.Right)
                {
                    buttonPress = 3;
                    dir = MoveDir.Right;
                }
                else
                {
                    posMove = false;
                    moving = true;
                    pos += Vector3.right;
                }
            }
        }
    }

    private void buildingControls() {
        if (Input.GetKeyDown("m")) {
            gridManager.theGrid.placeBuilding((int)(pos.x + 6.5), (int)(pos.y + 3.5), (Building)currentBuilding, Player.PlayerOne);
            print(currentBuilding);
        }
        else if (Input.GetKeyDown("o")) {
            currentBuilding += 1;
            if (currentBuilding == numberOfTypes) currentBuilding = 0;
        }
        else if (Input.GetKeyDown("u")) {
            currentBuilding -= 1;
            if (currentBuilding == -1) currentBuilding = currentBuilding - 1;
        }

    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class cursor2 : MonoBehaviour
{
    // Public Vars
    public Text playerState;
    //Should be changed to be a list of all possible buildings
    setupManager noP2Direction;

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
    public GameObject PauseMenu;

    // Use this for initialization
    void Start()
    {
        
        dimX = gridManager.theGrid.getDimX() / 2;
        dimZ = gridManager.theGrid.getDimY() / 2;
        PauseMenu = GameObject.Find("Pause Menu");
    }

    // Update is called once per frame
    void Update()
    {
        buildingControls();
        buttonPress--;
        if (PauseMenu.activeInHierarchy == false)
        {
            // Check if cursor is moving and doesn't move outside the board
            if (moving && pos.x >= -dimX && pos.x <= dimX
                   && pos.z >= -dimZ && pos.z <= dimZ)
            {
                if (transform.position == pos)
                {
                    moving = false;
                    posMove = true;
                    moveTower();
                }
                if(moving && pos.x >= -dimX && pos.x <= dimX
                   && pos.z >= -dimZ && pos.z <= dimZ)
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
            if (posMove)
            {
                pos = transform.position;
                moveTower();
            }
        }
    }
    private void moveTower()
    {
        if (noP2Direction == false)
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
    }

    private void buildingControls() {

        if (Input.GetKeyDown("p"))
        {
            gridManager.theGrid.destroyBuilding((int)(pos.x + 6.5), (int)(pos.z + 3.5), Player.PlayerTwo);
        }
    }
}
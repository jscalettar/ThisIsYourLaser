using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum MoveDir { Up, Down, Left, Right }

public class cursor1 : MonoBehaviour
{
    //public vars
    public static playerOneUI p1UI;
    setupManager noP1Direction;

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
    private int numberOfTypes = System.Enum.GetValues(typeof(Building)).Length -1;
    public GameObject PauseMenu;

    // Use this for initialization

    void Start()
    {
        //default values for UI  
        p1UI = gameObject.AddComponent<playerOneUI>();
        p1UI.playerState = GameObject.Find("playerOneState").GetComponent<Text>();
        p1UI.playerHealth = GameObject.Find("playerOneHealth").GetComponent<Text>();
        p1UI.currentSelection = GameObject.Find("playerOneSelection").GetComponent<Text>();
        p1UI.playerState.text = "Placing";
        dimX = gridManager.theGrid.getDimX() / 2;
        dimZ = gridManager.theGrid.getDimY() / 2;
        PauseMenu = GameObject.Find("Pause Menu");

    }

    // Update is called once per frame
    void Update()
    {
        Building currentTex = (Building)currentBuilding;
        buildingControls();
        p1UI.currentSelection.text = currentTex.ToString();
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
                //if next step is outside the game board, do not move.
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
        if (noP1Direction == false)
        {
            if (buttonPress <= 0)
            {
                if (Input.GetKey(KeyCode.W))
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
                else if (Input.GetKey(KeyCode.S))
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
                else if (Input.GetKey(KeyCode.A))
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
                else if (Input.GetKey(KeyCode.D))
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
    private void buildingControls()
    {
        
        if (Input.GetKeyDown("r"))//does not destroy instance
        {
            gridManager.theGrid.destroyBuilding((int)(pos.x + 6.5), (int)(pos.z + 3.5), Player.PlayerOne);
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum MoveDir { Up, Down, Left, Right }

public class cursor1 : MonoBehaviour
{
    //public vars
    public static playerOneUI p1UI;


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
        //default values for UI  
        p1UI = gameObject.AddComponent<playerOneUI>();
        p1UI.playerState = GameObject.Find("playerOneState").GetComponent<Text>();
        p1UI.playerHealth = GameObject.Find("playerOneHealth").GetComponent<Text>();
        p1UI.currentSelection = GameObject.Find("playerOneSelection").GetComponent<Text>();
        p1UI.playerState.text = "Placing";
        dimX = gridManager.theGrid.getDimX() / 2;
        dimZ = gridManager.theGrid.getDimY() / 2;


    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        //buildingControls();
=======
        Building currentTex = (Building)currentBuilding;
        buildingControls();
        p1UI.currentSelection.text = currentTex.ToString();
>>>>>>> origin/master
        buttonPress--;
        
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
    private void moveTower()
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
    private void buildingControls() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            gridManager.theGrid.placeBuilding((int)(pos.x + 6.5), (int)(pos.z + 3.5), (Building)currentBuilding, Player.PlayerOne);
            print(currentBuilding);
        }
        else if (Input.GetKeyDown("e")) {
            currentBuilding += 1;
            if (currentBuilding == numberOfTypes) currentBuilding = 0;
        }else if (Input.GetKeyDown("q")) {
            currentBuilding -= 1;
            if (currentBuilding == -1) currentBuilding = currentBuilding-1;
        }else if (Input.GetKeyDown("r"))
        {
            gridManager.theGrid.destroyBuilding((int)(pos.x + 6.5), (int)(pos.z + 3.5), Player.PlayerOne);
        }
        if (Input.GetKeyDown("r")) {
            p1UI.playerState.text = "swapping";
        }
        if (Input.GetKeyDown("t")) {
            p1UI.playerState.text = "deleting";
        }
        if (Input.GetKeyDown("f")) {
            p1UI.playerState.text = "placing";
        }

    }
}
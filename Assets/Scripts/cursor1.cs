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
    private int numberOfTypes = System.Enum.GetValues(typeof(Building)).Length -1;


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
        Building currentTex = (Building)currentBuilding;
        buildingControls();
        p1UI.currentSelection.text = currentTex.ToString();
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
    private void buildingControls()
    {
        if (Input.GetKeyDown("e"))
        {
            setupManager.i += 1;
            if (setupManager.i == numberOfTypes) setupManager.i = 1;
        }
        else if (Input.GetKeyDown("q"))
        {
            setupManager.i -= 1;
            if (setupManager.i == 0) setupManager.i = numberOfTypes - 1;
        }
        else if (Input.GetKeyDown("r"))//does not destroy instance
        {
            gridManager.theGrid.destroyBuilding((int)(pos.x + 6.5), (int)(pos.z + 3.5), Player.PlayerOne);
        }
        /*if (Input.GetKeyDown("r"))
        {
            p1UI.playerState.text = "swapping";
        }
        if (Input.GetKeyDown("t"))
        {
            p1UI.playerState.text = "deleting";
        }
        if (Input.GetKeyDown("f"))
        {
            p1UI.playerState.text = "placing";
        }*/
        if (setupManager.i == 1) { setupManager.selection = Building.Empty; i = 1; print("Free Move selection"); }
        if (setupManager.i == 2) { setupManager.selection = Building.Blocking; i = 2; print("Blocking Selected"); }
        if (setupManager.i == 3) { setupManager.selection = Building.Reflecting; i = 3; print("Reflecting Selected"); }
        if (setupManager.i == 4) { setupManager.selection = Building.Refracting; i = 4; print("Refracting Selected"); }
        if (setupManager.i == 5) { setupManager.selection = Building.Redirecting; i = 5; print("Redirecting Selected"); }
        if (setupManager.i == 6) { setupManager.selection = Building.Portal; i = 6; print("Portal Selected"); }
        if (setupManager.i == 7) { setupManager.selection = Building.Resource; i = 7; print("Resource Selected"); }
    }
}
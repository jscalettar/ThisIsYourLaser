using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//public enum MoveDir { Up, Down, Left, Right }

public class cursor2 : MonoBehaviour
{
    public GameObject cost;
    public int player;
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
    private int numberOfTypes = System.Enum.GetValues(typeof(Building)).Length - 1;
    //public GameObject PauseMenu;
    public Sprite[] Block;
    public Sprite[] Reflect;
    public Sprite[] Refract;
    public Sprite[] Redirect;
    public Sprite[] Resource;
    public Sprite[] Laser;
    public Sprite[][] Sprites;

    public Sprite[] UISprites;

    // Use this for initialization

    void Start()
    {
        Sprites = new Sprite[][] { Block, Reflect, Refract, Redirect, Resource, Laser };
        cost.GetComponent<TextMesh>().fontStyle = FontStyle.Bold;
    }
    private void Update()
    {
        if (player == 1)
        {
            if (inputController.cursorP1.state == State.idle)
            {
                cost.SetActive(true);
            }
            if (inputController.cursorP1.state == State.moving || inputController.cursorP1.state == State.placingMove) cost.GetComponent<TextMesh>().text = gridManager.theGrid.getCost(gridManager.theGrid.getBuilding(inputController.cursorP1.moveOrigin.x, inputController.cursorP1.moveOrigin.y), inputController.cursorP1.x, Player.PlayerOne, true).ToString("F1");
            else if (inputController.cursorP1.state == State.removing) cost.GetComponent<TextMesh>().text = gridManager.theGrid.getCost(inputController.cursorP1.selection, inputController.cursorP1.x, Player.PlayerOne, false, true).ToString("F1");
            else cost.GetComponent<TextMesh>().text = gridManager.theGrid.getCost(inputController.cursorP1.selection, inputController.cursorP1.x, Player.PlayerOne).ToString("F1");
        }
        else
        {
            if (inputController.cursorP2.state == State.idle)
            {
                cost.SetActive(true);
            }
            if (inputController.cursorP2.state == State.moving || inputController.cursorP2.state == State.placingMove) cost.GetComponent<TextMesh>().text = gridManager.theGrid.getCost(gridManager.theGrid.getBuilding(inputController.cursorP2.moveOrigin.x, inputController.cursorP2.moveOrigin.y), inputController.cursorP2.x, Player.PlayerTwo, true).ToString("F1");
            else if (inputController.cursorP2.state == State.removing) cost.GetComponent<TextMesh>().text = gridManager.theGrid.getCost(inputController.cursorP2.selection, inputController.cursorP2.x, Player.PlayerTwo, false, true).ToString("F1");
            else cost.GetComponent<TextMesh>().text = gridManager.theGrid.getCost(inputController.cursorP2.selection, inputController.cursorP2.x, Player.PlayerTwo).ToString("F1");
        }

    }
}
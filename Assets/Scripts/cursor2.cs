using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class cursor2 : MonoBehaviour
{
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
    }
    
}
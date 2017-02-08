using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Move 
///     player one with WASD
///     player two with IJKL
/// Place buildings and swap 
///     E for Player one
///     P for Player two
/// Numbers
///     2-7 select different buildings
///     1 sets to free selection allowing for swapping
/// There is no delete for these yet
/// </summary>



//This file will be replaced by Scott's actual keyboard control files once that is done
public class setupManager : MonoBehaviour
{

    public List<GameObject> goList;
    private GameObject[,] listPlace = new GameObject[14, 8];
    public Material p1Mat;
    public Material p2Mat;
    // location of the mouse on grid
    private Vector3 p1Pos;
    private Vector3 p2Pos;
    //if players can place base
    private bool pOneCanBase;
    private bool pTwoCanBase;
    //if players can place laser
    private bool pOneCanLaser;
    private bool pTwoCanLaser;
    //so players cant place both at same time and after laser phase its over
    private bool basePhase;
    private bool laserPhase;
    //wait for direction of block to be specified
    public bool noP1Direction;
    public bool noP2Direction;
    public static int i1;
    public static int i2;
    private bool haveSelected;
    public static Building p1selection;
    public static Building p2selection;
    private Vector2 selectedLoc;
    private GridItem selected;

	//Vars for UI
	public static playerOneUI p1UI;
    public static playerTwoUI p2UI;
    Building currentTextp1 = (Building)p1selection;
    Building currentTextp2 = (Building)p2selection;
    public GameObject PauseMenu;

	
    // Use this for initialization
    void Start()
    {
		//default values for Player 1 UI  
		p1UI = gameObject.AddComponent<playerOneUI>();
		p1UI.playerState = GameObject.Find("playerOneState").GetComponent<Text>();
		p1UI.playerHealth = GameObject.Find("playerOneHealth").GetComponent<Text>();
		p1UI.currentSelection = GameObject.Find("playerOneSelection").GetComponent<Text>();
		p1UI.playerState.text = "Placing";

        //default values for Player 2 UI  
        p2UI = gameObject.AddComponent<playerTwoUI>();
        p2UI.playerState = GameObject.Find("playerTwoState").GetComponent<Text>();
        p2UI.playerHealth = GameObject.Find("playerTwoHealth").GetComponent<Text>();
        p2UI.currentSelection = GameObject.Find("playerTwoSelection").GetComponent<Text>();
        p2UI.playerState.text = "Placing";

        //Setup to set bases
        pOneCanBase = true;
        pTwoCanBase = true;
        pOneCanLaser = false;
        pTwoCanLaser = false;
        basePhase = true;
        laserPhase = false;
        haveSelected = false;
        noP1Direction = true;
        noP2Direction = true;
        //selection = Building.Empty;
        p1selection = Building.Empty;
        p2selection = Building.Empty;
        i1 = 0;
        i2 = 0;

        PauseMenu = GameObject.Find("Pause Menu");
    }

    // Update is called once per frame
    void Update()
    {
        //updates UI
        Building currentTextp1 = (Building)p1selection;
        Building currentTextp2 = (Building)p2selection;
        p1UI.currentSelection.text = currentTextp1.ToString();
        p2UI.currentSelection.text = currentTextp2.ToString();

        if (PauseMenu.activeInHierarchy == false)
        {
            UpdateSelection();
            if (!pOneCanLaser && !pTwoCanLaser)//when players cant place lasers, not laser phase
            {
                laserPhase = false;
                pOneCanLaser = true;//changing one keeps it from coming back here
            }
            else if (!pOneCanBase && !pTwoCanBase)//when players cant place bases end basePhase start laserPhase
            {
                basePhase = false;
                pOneCanBase = true;//changing one keeps it from coming back here
                laserPhase = true;
                pOneCanLaser = true;
                pTwoCanLaser = true;
            }
            if (basePhase)
            {
                p1selection = Building.Base;
                p2selection = Building.Base;
                if (Input.GetKeyDown(KeyCode.E) && pOneCanBase && p1Pos.x == 0)//P1 base place
                {

                    PlaceBuild(Player.PlayerOne, Building.Base, 0, p1Pos, Direction.None);
                }
                else if (Input.GetKeyDown(KeyCode.O) && pTwoCanBase && p2Pos.x == 13)//P2 base place
                {
                    PlaceBuild(Player.PlayerTwo, Building.Base, 0, p2Pos, Direction.None);
                }
            }
            else if (laserPhase)

            {
                p1selection = Building.Laser;
                p2selection = Building.Laser;
                if (Input.GetKeyDown(KeyCode.E) && pOneCanLaser && p1Pos.x == 0)//P1 laser place
                {
                    PlaceBuild(Player.PlayerOne, Building.Laser, 1, p1Pos, Direction.None);
                    p1selection = Building.Empty;
                }
                else if (Input.GetKeyDown(KeyCode.O) && pTwoCanLaser && p2Pos.x == 13)//P2 laser place
                {
                    PlaceBuild(Player.PlayerTwo, Building.Laser, 1, p2Pos, Direction.None);
                    p2selection = Building.Empty;
                }
            }
            else if (!pOneCanBase && !pTwoCanBase)
                pOneCanLaser = pTwoCanLaser = true;
            if (haveSelected)//if you have an object selected
            {
                if (Input.GetKeyDown(KeyCode.Q))//P1 
                {
                    p1UI.playerState.text = "placing";
                    SwapBlock(p1Pos);
                }
                else if (Input.GetKeyDown(KeyCode.U))//P2 
                {
                    SwapBlock(p2Pos);
                }
            }
            else if (laserPhase == basePhase)//if no object is selected, select one
            {
                if (Input.GetKeyDown(KeyCode.Q))//P1 
                {
                    p1UI.playerState.text = "swapping";
                    SelectBlock(p1Pos);
                }
                else if (Input.GetKeyDown(KeyCode.U))//P2 
                {
                    SelectBlock(p2Pos);
                }
            }
            if (basePhase == laserPhase)//pick a building that you want to place
            {
                //Player 1 selection controls
                if (Input.GetKeyDown("1")) { p1selection = Building.Blocking; i1 = 2; print("Blocking Selected"); }
                if (Input.GetKeyDown("2")) { p1selection = Building.Reflecting; i1 = 3; print("Reflecting Selected"); }
                if (Input.GetKeyDown("3")) { p1selection = Building.Refracting; i1 = 4; print("Refracting Selected"); }
                if (Input.GetKeyDown("4")) { p1selection = Building.Redirecting; i1 = 5; print("Redirecting Selected"); }

                //Player 2 selection controls
                if (Input.GetKeyDown("7")) { p2selection = Building.Blocking; i2 = 2; print("Blocking Selected"); }
                if (Input.GetKeyDown("8")) { p2selection = Building.Reflecting; i2 = 3; print("Reflecting Selected"); }
                if (Input.GetKeyDown("9")) { p2selection = Building.Refracting; i2 = 4; print("Refracting Selected"); }
                if (Input.GetKeyDown("0")) { p2selection = Building.Redirecting; i2 = 5; print("Redirecting Selected"); }


                // PLAYER 1 BUILDING SELECTION
                if (noP1Direction == true) //need to select a direction for the P1 building
                {
                    if (Input.GetKeyDown(KeyCode.W)) //Up
                    {
                        PlaceBuild(Player.PlayerOne, p1selection, i1, p1Pos, Direction.Up);
                        noP1Direction = false;
                    }
                    else if (Input.GetKeyDown(KeyCode.A)) //Left
                    {
                        PlaceBuild(Player.PlayerOne, p1selection, i1, p1Pos, Direction.Left);
                        noP1Direction = false;
                    }
                    else if (Input.GetKeyDown(KeyCode.S)) //Down
                    {
                        PlaceBuild(Player.PlayerOne, p1selection, i1, p1Pos, Direction.Down);
                        noP1Direction = false;
                    }
                    else if (Input.GetKeyDown(KeyCode.D)) //Right
                    {
                        PlaceBuild(Player.PlayerOne, p1selection, i1, p1Pos, Direction.Right);
                        noP1Direction = false;
                    }
                }
                else //can place a new P1 building
                {
                    if (Input.GetKeyDown(KeyCode.E) && i1 > 0)
                    {
                        noP1Direction = true; //now you need to pick a direction
                                              //PlaceBuild(Player.PlayerOne, selection, i1, p1Pos);
                    }
                }

                // PLAYER 2 BUILDING SELECTION
                if (noP2Direction == true) //need to select a direction for the P2 building
                {
                    if (Input.GetKeyDown(KeyCode.I)) //Up
                    {
                        PlaceBuild(Player.PlayerTwo, p2selection, i2, p2Pos, Direction.Up);
                        noP2Direction = false;
                    }
                    else if (Input.GetKeyDown(KeyCode.J)) //Left
                    {
                        PlaceBuild(Player.PlayerTwo, p2selection, i2, p2Pos, Direction.Left);
                        noP2Direction = false;
                    }
                    else if (Input.GetKeyDown(KeyCode.K)) //Down
                    {
                        PlaceBuild(Player.PlayerTwo, p2selection, i2, p2Pos, Direction.Down);
                        noP2Direction = false;
                    }
                    else if (Input.GetKeyDown(KeyCode.L)) //Right
                    {
                        PlaceBuild(Player.PlayerTwo, p2selection, i2, p2Pos, Direction.Right);
                        noP2Direction = false;
                    }
                }
                else //can place a new P2 building
                {
                    if (Input.GetKeyDown(KeyCode.O) && i2 > 0)
                    {
                        noP2Direction = true; //now you need to pick a direction
                                              //PlaceBuild(Player.PlayerTwo, selection, i2, p2Pos, Direction.None);
                    }
                }
            }
        }
    }

    private void UpdateSelection()//gets the position on the grid, to be replaced with Scott's movement
    {
        p1Pos = new Vector3(cursor1.pos.x + 6.5f, 0, cursor1.pos.z + 3.5f);
        p2Pos = new Vector3(cursor2.pos.x + 6.5f, 0, cursor2.pos.z + 3.5f);
    }

    public void PlaceBuild(Player player, Building newBuild, int val, Vector3 pos, Direction facing)
    {
        if (gridManager.theGrid.placeBuilding((int)pos.x, (int)pos.z, newBuild, player, facing)) //place in grid
        {
            print(newBuild + " building placed with direction = " + facing);
            pos = new Vector3(pos.x - 6.5f, 0, pos.z - 3.5f);//cursor position is based on grid not world coordinates; adjust
            GameObject go = Instantiate(goList[val], pos, Quaternion.identity) as GameObject;//if it works, create an instance of object
            if (player == Player.PlayerOne)//if player one, make the mat red and update bools so you dont place more than one base/laser
            {
                MeshRenderer r = go.GetComponent<MeshRenderer>();
                r.material = p1Mat;
                if (newBuild == Building.Base)
                    pOneCanBase = !pOneCanBase;
                else if (newBuild == Building.Laser)
                    pOneCanLaser = !pOneCanLaser;
            }
            else//if player two, make the mat red and update bools so you dont place more than one base/laser
            {
                MeshRenderer r = go.GetComponent<MeshRenderer>();
                r.material = p2Mat;
                if (newBuild == Building.Base)
                    pTwoCanBase = !pTwoCanBase;
                else if (newBuild == Building.Laser)
                    pTwoCanLaser = !pTwoCanLaser;
            }
            go.transform.SetParent(transform);
            pos = new Vector3(pos.x + 6.5f, 0, pos.z + 3.5f);//want grid coordinates because negative numbers dont work for array
            listPlace[(int)pos.x,(int)pos.z] = go;
        }
        else
            print("dont work");
    }

    private void SelectBlock(Vector3 pos)
    {
        GridItem gi = gridManager.theGrid.getCellInfo((int)pos.x, (int)pos.z);
        if (gi.isEmpty)//if cell is empty nothing to select
            return;
        else//get grid items info
        {
            print(pos);
            haveSelected = true;
            selected = gi;
            selectedLoc = new Vector2(pos.x, pos.z);
        }
    }

    private void SwapBlock(Vector3 newPos)//this should be a fuction in the grid
    {
        if (selectedLoc.x == 0 && newPos.x > 0) return;
        if (selectedLoc.x == 13 && newPos.x < 13) return;
        GridItem temp = gridManager.theGrid.getCellInfo((int)newPos.x, (int)newPos.z);
        if (temp.owner == selected.owner)
        {//swap buildings
            if (gridManager.theGrid.swapBuilding((int)selectedLoc.x, (int)selectedLoc.y, (int)newPos.x, (int)newPos.z, selected.owner))
            {//connect the instances to the gridItem so when they swap the instance will also move
                GameObject go1 = listPlace[(int)selectedLoc.x, (int)selectedLoc.y];
                GameObject go2 = listPlace[(int)newPos.x, (int)newPos.z];
                listPlace[(int)selectedLoc.x, (int)selectedLoc.y] = listPlace[(int)newPos.x, (int)newPos.z];
                listPlace[(int)newPos.x, (int)newPos.z] = go1;
                newPos = new Vector3(newPos.x - 6.5f, 0, newPos.z - 3.5f);
                Vector3 loc = new Vector3(selectedLoc.x - 6.5f, 0, selectedLoc.y - 3.5f);
                go1.transform.position = newPos;
                go2.transform.position = loc;
                print("Swap Complete");
            }
            else
                print("Cannot complete swap");
            haveSelected = false;
        }
        else if (temp.owner == Player.World)
        {
            bool move = gridManager.theGrid.moveBuilding((int)selectedLoc.x, (int)selectedLoc.y, (int)newPos.x, (int)newPos.z, selected.owner);
            if (move)
            {//move the instance to new location
                listPlace[(int)newPos.x, (int)newPos.z] = listPlace[(int)selectedLoc.x, (int)selectedLoc.y];
                listPlace[(int)selectedLoc.x, (int)selectedLoc.y] = null;
                GameObject go = listPlace[(int)newPos.x, (int)newPos.z];
                newPos = new Vector3(newPos.x - 6.5f, 0, newPos.z - 3.5f);
                go.transform.position = newPos;
                print("Move complete");
            }
            else print("Cannot move");
            haveSelected = false;
        }
    }
}



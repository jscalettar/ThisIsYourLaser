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
///     O for Player two
/// Numbers
///     1-4, 7-0 select different buildings
///     Q, U used for swapping
/// There is no delete for these yet
/// </summary>



//This file will be replaced by Scott's actual keyboard control files once that is done
public class setupManager : MonoBehaviour
{
    //Change cursor stuff
    public Sprite P1BaseSprite;
    public Sprite P1BlockSprite;
    public Sprite P1LaserSprite;
    public Sprite P1ReflectSprite;
    public Sprite P1RefractSprite;
    public Sprite P1RedirectSprite;
    public GameObject P1Cursor;

    public Sprite P2BaseSprite;
    public Sprite P2BlockSprite;
    public Sprite P2LaserSprite;
    public Sprite P2ReflectSprite;
    public Sprite P2RefractSprite;
    public Sprite P2RedirectSprite;
    public GameObject P2Cursor;

    //public List<GameObject> goList;
    //private GameObject[,] listPlace = new GameObject[14, 8];
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
    private bool haveSelected1;
    private bool haveSelected2;
    public static Building selection1;
    public static Building selection2;
    private Vector2 selectedLoc1;
    private Vector2 selectedLoc2;
    private GridItem selected1;
    private GridItem selected2;

	//Vars for UI
	public static playerOneUI p1UI;
    public static playerTwoUI p2UI;
    Building currentTextp1 = (Building)selection1;
    Building currentTextp2 = (Building)selection2;
	
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
        noP1Direction = true;
        noP2Direction = true;
        haveSelected1 = false;
        haveSelected2 = false;
        selection1 = Building.Empty;
        selection2 = Building.Empty;
        i1 = 0;
        i2 = 0;

    }

    // Update is called once per frame
    void Update()
    {
		//updtes UI
		Building currentTextp1 = (Building)selection1;
        Building currentTextp2 = (Building)selection2;
        p1UI.currentSelection.text = currentTextp1.ToString();
        p2UI.currentSelection.text = currentTextp2.ToString();
		//Building currentText = (Building)selection1;

		//p1UI.currentSelection.text = currentText.ToString();

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
			selection1 = Building.Base;
            selection2 = Building.Base;
            if (Input.GetKeyDown(KeyCode.E) && pOneCanBase && p1Pos.x == 0)//P1 base place
            {
				
                PlaceBuild(Player.PlayerOne, selection1, 0, p1Pos, Direction.None);
            }
            else if (Input.GetKeyDown(KeyCode.O) && pTwoCanBase && p2Pos.x == 13)//P2 base place
            {
                PlaceBuild(Player.PlayerTwo, selection2, 0, p2Pos, Direction.None);
            }
        }
        else if (laserPhase)
			
        {
            P1Cursor.GetComponent<SpriteRenderer>().sprite = P1LaserSprite;
            P2Cursor.GetComponent<SpriteRenderer>().sprite = P2LaserSprite;
            selection1 = Building.Laser;
            selection2 = Building.Laser;
            if (Input.GetKeyDown(KeyCode.E) && pOneCanLaser && p1Pos.x == 0)//P1 laser place
            {
                PlaceBuild(Player.PlayerOne, selection1, 1, p1Pos, Direction.None);
				selection1 = Building.Empty;
            }
            else if (Input.GetKeyDown(KeyCode.O) && pTwoCanLaser && p2Pos.x == 13)//P2 laser place
            {
                PlaceBuild(Player.PlayerTwo, selection2, 1, p2Pos, Direction.None);
                selection2 = Building.Empty;
            }
        }
        if (haveSelected1)//if you have an object selected
        {
            if (Input.GetKeyDown(KeyCode.Q))//P1 
            {
                if (SwapBlock(p1Pos, selectedLoc1, selected1))
                {
                    haveSelected1 = false;
                }
				p1UI.playerState.text = "placing";
            }
        }
        else if (laserPhase == basePhase)//if no object is selected, select one
        {
            if (Input.GetKeyDown(KeyCode.Q))//P1 
            {
                SelectBlock(p1Pos, Player.PlayerOne);
				p1UI.playerState.text = "swapping";
            }
        }
        if (haveSelected2)//if you have an object selected
        {
           if (Input.GetKeyDown(KeyCode.U))//P2 
            {
                if (SwapBlock(p2Pos, selectedLoc2, selected2))
                {
                    haveSelected2 = false;
                }
            }
        }
        else if (laserPhase == basePhase)//if no object is selected, select one
        {
            if (Input.GetKeyDown(KeyCode.U))//P2 
            {
                SelectBlock(p2Pos, Player.PlayerTwo);
            }
        }
        if (basePhase == laserPhase)//pick a building that you want to place
        {
            //Player 1 selection controls
			if (Input.GetKeyDown("1")) { selection1 = Building.Blocking; i1 = 2; print ("Blocking Selected"); P1Cursor.GetComponent<SpriteRenderer>().sprite= P1BlockSprite; }
            if (Input.GetKeyDown("2")) { selection1 = Building.Reflecting; i1 = 3; print("Reflecting Selected"); P1Cursor.GetComponent<SpriteRenderer>().sprite = P1ReflectSprite; }
            if (Input.GetKeyDown("3")) { selection1 = Building.Refracting; i1 = 4; print("Refracting Selected"); P1Cursor.GetComponent<SpriteRenderer>().sprite = P1RefractSprite; }
            if (Input.GetKeyDown("4")) { selection1 = Building.Redirecting; i1 = 5; print("Redirecting Selected"); P1Cursor.GetComponent<SpriteRenderer>().sprite = P1RedirectSprite; }
            
            //Player 2 selection controls
            if (Input.GetKeyDown("7")) { selection2 = Building.Blocking; i2 = 2; print("Blocking Selected"); P2Cursor.GetComponent<SpriteRenderer>().sprite = P2BlockSprite; }
            if (Input.GetKeyDown("8")) { selection2 = Building.Reflecting; i2 = 3; print("Reflecting Selected"); P2Cursor.GetComponent<SpriteRenderer>().sprite = P2ReflectSprite; }
            if (Input.GetKeyDown("9")) { selection2 = Building.Refracting; i2 = 4; print("Refracting Selected"); P2Cursor.GetComponent<SpriteRenderer>().sprite = P2RefractSprite; }
            if (Input.GetKeyDown("0")) { selection2 = Building.Redirecting; i2 = 5; print("Redirecting Selected"); P2Cursor.GetComponent<SpriteRenderer>().sprite = P2RedirectSprite; }

            if (Input.GetKeyDown(KeyCode.E) && selection1 != Building.Empty)
            {
                noP1Direction = true;
            }else if (Input.GetKeyDown(KeyCode.O) && selection2 != Building.Empty)
            {
                noP2Direction = true;
            }
            // PLAYER 1 BUILDING SELECTION
            if (noP1Direction == true) //need to select a direction for the P1 building
            {
                if (Input.GetKeyDown(KeyCode.W)) //Up
                {
                    PlaceBuild(Player.PlayerOne, selection1, i1, p1Pos, Direction.Up);
                    noP1Direction = false;
                }
                else if (Input.GetKeyDown(KeyCode.A)) //Left
                {
                    PlaceBuild(Player.PlayerOne, selection1, i1, p1Pos, Direction.Left);
                    noP1Direction = false;
                }
                else if (Input.GetKeyDown(KeyCode.S)) //Down
                {
                    PlaceBuild(Player.PlayerOne, selection1, i1, p1Pos, Direction.Down);
                    noP1Direction = false;
                }
                else if (Input.GetKeyDown(KeyCode.D)) //Right
                {
                    PlaceBuild(Player.PlayerOne, selection1, i1, p1Pos, Direction.Right);
                    noP1Direction = false;
                }
            }

            // PLAYER 2 BUILDING SELECTION
            if (noP2Direction == true) //need to select a direction for the P2 building
            {
                if (Input.GetKeyDown(KeyCode.I)) //Up
                {
                    PlaceBuild(Player.PlayerTwo, selection2, i2, p2Pos, Direction.Up);
                    noP2Direction = false;
                }
                else if (Input.GetKeyDown(KeyCode.J)) //Left
                {
                    PlaceBuild(Player.PlayerTwo, selection2, i2, p2Pos, Direction.Left);
                    noP2Direction = false;
                }
                else if (Input.GetKeyDown(KeyCode.K)) //Down
                {
                    PlaceBuild(Player.PlayerTwo, selection2, i2, p2Pos, Direction.Down);
                    noP2Direction = false;
                }
                else if (Input.GetKeyDown(KeyCode.L)) //Right
                {
                    PlaceBuild(Player.PlayerTwo, selection2, i2, p2Pos, Direction.Right);
                    noP2Direction = false;
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
        //make it so lasers have to be put on the edge
        /*bool allow = false;
        if (newBuild != Building.Laser) allow = true;
        else if (player == Player.PlayerOne && newBuild == Building.Laser && (int)pos.x == 0) allow = true;
        else if (player == Player.PlayerTwo && newBuild == Building.Laser && (int)pos.x == 13) allow = true;
        else allow = false;*/
        if (gridManager.theGrid.placeBuilding((int)pos.x, (int)pos.z, newBuild, player, facing)) //place in grid
        {
            print(newBuild + " building placed with direction = " + facing);
            //pos = new Vector3(pos.x - 6.5f, 0, pos.z - 3.5f);//cursor position is based on grid not world coordinates; adjust
            //GameObject go = Instantiate(goList[val], pos, Quaternion.identity) as GameObject;//if it works, create an instance of object
            if (player == Player.PlayerOne)//if player one, make the mat red and update bools so you dont place more than one base/laser
            {
                //MeshRenderer r = go.GetComponent<MeshRenderer>();
                //r.material = p1Mat;
                if (newBuild == Building.Base)
                    pOneCanBase = !pOneCanBase;
                else if (newBuild == Building.Laser)
                    pOneCanLaser = !pOneCanLaser;
            }
            else//if player two, make the mat red and update bools so you dont place more than one base/laser
            {
                //MeshRenderer r = go.GetComponent<MeshRenderer>();
                //r.material = p2Mat;
                if (newBuild == Building.Base)
                    pTwoCanBase = !pTwoCanBase;
                else if (newBuild == Building.Laser)
                    pTwoCanLaser = !pTwoCanLaser;
            }
            //go.transform.SetParent(transform);
            //pos = new Vector3(pos.x + 6.5f, 0, pos.z + 3.5f);//want grid coordinates because negative numbers dont work for array
            //listPlace[(int)pos.x,(int)pos.z] = go;
        }
        else
            print("dont work");
    }

    private void SelectBlock(Vector3 pos, Player player)
    {
        GridItem gi = gridManager.theGrid.getCellInfo((int)pos.x, (int)pos.z);
        if (gi.isEmpty)//if cell is empty nothing to select
            return;
        else if (gi.building == Building.Base || gi.building == Building.Laser)
            return;
        else if (player == Player.PlayerOne)//get grid items info
        {
            print(pos);
            for (int i = 0; i < 4; i++) print(gi.weakSides[i]);
            haveSelected1 = true;
            selected1 = gi;
            selectedLoc1 = new Vector2(pos.x, pos.z);
        }else if(player == Player.PlayerTwo)
        {
            print(pos);
            for (int i = 0; i < 4; i++) print(gi.weakSides[i]);
            haveSelected2 = true;
            selected2 = gi;
            selectedLoc2 = new Vector2(pos.x, pos.z);
        }
    }

    private bool SwapBlock(Vector3 newPos, Vector2 oldPos, GridItem gridObj)//this should be a fuction in the grid
    {
        GridItem temp = gridManager.theGrid.getCellInfo((int)newPos.x, (int)newPos.z);
        if (temp.owner == gridObj.owner)
        {//swap buildings
            if (gridManager.theGrid.swapBuilding((int)oldPos.x, (int)oldPos.y, (int)newPos.x, (int)newPos.z, gridObj.owner))
            {//connect the instances to the gridItem so when they swap the instance will also move
                /*GameObject go1 = listPlace[(int)oldPos.x, (int)oldPos.y];
                GameObject go2 = listPlace[(int)newPos.x, (int)newPos.z];
                listPlace[(int)oldPos.x, (int)oldPos.y] = listPlace[(int)newPos.x, (int)newPos.z];
                listPlace[(int)newPos.x, (int)newPos.z] = go1;
                newPos = new Vector3(newPos.x - 6.5f, 0, newPos.z - 3.5f);
                Vector3 loc = new Vector3(oldPos.x - 6.5f, 0, oldPos.y - 3.5f);
                go1.transform.position = newPos;
                go2.transform.position = loc;*/
                print("Swap Complete");
            }
            else
                print("Cannot complete swap");
            return true;
        }
        else if (temp.owner == Player.World)
        {
            bool move = gridManager.theGrid.moveBuilding((int)oldPos.x, (int)oldPos.y, (int)newPos.x, (int)newPos.z, gridObj.owner);
            if (move)
            {//move the instance to new location
                /*listPlace[(int)newPos.x, (int)newPos.z] = listPlace[(int)oldPos.x, (int)oldPos.y];
                listPlace[(int)oldPos.x, (int)oldPos.y] = null;
                GameObject go = listPlace[(int)newPos.x, (int)newPos.z];
                newPos = new Vector3(newPos.x - 6.5f, 0, newPos.z - 3.5f);
                go.transform.position = newPos;*/
                print("Move complete");
            }
            else print("Cannot move");
            return true;
        }
        return false;
    }
}



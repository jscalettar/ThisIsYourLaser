using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Move 
///     player one with WASD
///     player two with IJKL
/// Place buildings and swap 
///     Space for Player one
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
    public static int i1;
    public static int i2;
    private bool haveSelected;
    public static Building selection;
    private Vector2 selectedLoc;
    private GridItem selected;

	//Vars for UI
	public static playerOneUI p1UI;

	

	Building currentText = (Building)selection;
    // Use this for initialization
    void Start()
    {
		//default values for UI  
		p1UI = gameObject.AddComponent<playerOneUI>();
		p1UI.playerState = GameObject.Find("playerOneState").GetComponent<Text>();
		p1UI.playerHealth = GameObject.Find("playerOneHealth").GetComponent<Text>();
		p1UI.currentSelection = GameObject.Find("playerOneSelection").GetComponent<Text>();
		p1UI.playerState.text = "Placing";

        //Setup to set bases
        pOneCanBase = true;
        pTwoCanBase = true;
        pOneCanLaser = false;
        pTwoCanLaser = false;
        basePhase = true;
        laserPhase = false;
        haveSelected = false;
        selection = Building.Empty;
        i1 = 0;
        i2 = 0;
    }

    // Update is called once per frame
    void Update()
    {
		//updtes UI
		Building currentText = (Building)selection;

		p1UI.currentSelection.text = currentText.ToString();
        

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
			selection = Building.Base;
            if (Input.GetKeyDown(KeyCode.E) && pOneCanBase && p1Pos.x == 0)//P1 base place
            {
				
                PlaceBuild(Player.PlayerOne, selection, 0, p1Pos);
            }
            else if (Input.GetKeyDown(KeyCode.O) && pTwoCanBase && p2Pos.x == 13)//P2 base place
            {
                PlaceBuild(Player.PlayerTwo, Building.Base, 0, p2Pos);
            }
        }
        else if (laserPhase)
			
        {
			selection = Building.Laser;
            if (Input.GetKeyDown(KeyCode.E) && pOneCanLaser && p1Pos.x == 0)//P1 laser place
            {
                PlaceBuild(Player.PlayerOne, Building.Laser, 1, p1Pos);
				selection = Building.Empty;
            }
            else if (Input.GetKeyDown(KeyCode.O) && pTwoCanLaser && p2Pos.x == 13)//P2 laser place
            {
                PlaceBuild(Player.PlayerTwo, Building.Laser, 1, p2Pos);
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
			if (Input.GetKeyDown("1")) { selection = Building.Blocking; i1 = 2; print ("Blocking Selected");}
            if (Input.GetKeyDown("2")) { selection = Building.Reflecting; i1 = 3; print("Reflecting Selected"); }
            if (Input.GetKeyDown("3")) { selection = Building.Refracting; i1 = 4; print("Refracting Selected"); }
            if (Input.GetKeyDown("4")) { selection = Building.Redirecting; i1 = 5; print("Redirecting Selected"); }
            //if (Input.GetKeyDown("5")) { selection = Building.Portal; i1 = 6; print("Portal Selected"); }

            if (Input.GetKeyDown("7")) { selection = Building.Blocking; i2 = 2; print("Blocking Selected"); }
            if (Input.GetKeyDown("8")) { selection = Building.Reflecting; i2 = 3; print("Reflecting Selected"); }
            if (Input.GetKeyDown("9")) { selection = Building.Refracting; i2 = 4; print("Refracting Selected"); }
            if (Input.GetKeyDown("0")) { selection = Building.Redirecting; i2 = 5; print("Redirecting Selected"); }
            //if (Input.GetKeyDown("-")) { selection = Building.Portal; i2 = 6; print("Portal Selected"); }

            if (Input.GetKeyDown(KeyCode.E) && i1 > 0)
            {
                PlaceBuild(Player.PlayerOne, selection, i1, p1Pos);
            }
            else if (Input.GetKeyDown(KeyCode.O) && i2 > 0)
            {
                PlaceBuild(Player.PlayerTwo, selection, i2, p2Pos);
            }
        }
    }

    private void UpdateSelection()//gets the position on the grid, to be replaced with Scott's movement
    {
        p1Pos = new Vector3(cursor1.pos.x + 6.5f, 0, cursor1.pos.z + 3.5f);
        p2Pos = new Vector3(cursor2.pos.x + 6.5f, 0, cursor2.pos.z + 3.5f);
    }

    public void PlaceBuild(Player player, Building newBuild, int val, Vector3 pos)
    {
        if (gridManager.theGrid.placeBuilding((int)pos.x, (int)pos.z, newBuild, player)) //place in grid
        {
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



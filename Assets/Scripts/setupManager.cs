using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This file will be replaced by Scott's actual keyboard control files once that is done
public class setupManager : MonoBehaviour {
    

    public GameObject Base;
    public GameObject Laser;
    public Material p1Mat;
    public Material p2Mat;
    private List<GameObject> instances = new List<GameObject>();
    private GridItem selected;
    private Vector2 selectedLoc;
    private bool haveSelected;
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
	public GameObject useThis;

    // Use this for initialization
    void Start () {
        //Setup to set bases
        pOneCanBase = true;
        pTwoCanBase = true;
        pOneCanLaser = false;
        pTwoCanLaser = false;
        basePhase = true;
        laserPhase = false;
        haveSelected = false;
	}

    // Update is called once per frame
    void Update()
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
            if (Input.GetKeyDown(KeyCode.Space) && pOneCanBase)//P1 base place
            {
                PlaceBaseLaser(Player.PlayerOne, Building.Base, Base, p1Pos);
            }
            else if (Input.GetKeyDown(KeyCode.P) && pTwoCanBase)//P2 base place
            {
                PlaceBaseLaser(Player.PlayerTwo, Building.Base, Base, p2Pos);
            }
        }
        else if (laserPhase)
        {
            if (Input.GetKeyDown(KeyCode.Space) && pOneCanLaser)//P1 laser place
            {
                PlaceBaseLaser(Player.PlayerOne, Building.Laser, Laser, p1Pos);
            }
            else if (Input.GetKeyDown(KeyCode.P) && pTwoCanLaser)//P2 laser place
            {
                PlaceBaseLaser(Player.PlayerTwo, Building.Laser, Laser, p2Pos);
            }
        }
        else if (!pOneCanBase && !pTwoCanBase)
            pOneCanLaser = pTwoCanLaser = true;
        if (haveSelected)
        {
            if (Input.GetKeyDown(KeyCode.Space))//P1 laser place
            {
                SwapBlock(p1Pos);
            }
            else if (Input.GetKeyDown(KeyCode.P))//P2 laser place
            {
                SwapBlock(p2Pos);
            }
        }
        else if(laserPhase == basePhase && !haveSelected)
        {
            if (Input.GetKeyDown(KeyCode.Space))//P1 laser place
            {
                SelectBlock(p1Pos);
            }
            else if (Input.GetKeyDown(KeyCode.P))//P2 laser place
            {
                SelectBlock(p2Pos);
            }
        }
    }

    private void UpdateSelection()//gets the position on the grid, to be replaced with Scott's movement
    {
        //p1Pos = cursor1.pos;
        //p2Pos = cursor2.pos;
    }

    void PlaceBaseLaser(Player player, Building newBuild, GameObject build, Vector3 pos)
    {

        if (gridManager.theGrid.placeBuilding((int)pos.x, (int)pos.y, newBuild, player))//place in grid
        {
            GameObject go = Instantiate(build, pos, Quaternion.identity) as GameObject;//if it works, create an instance of object
            if(player == Player.PlayerOne)
            {
                MeshRenderer r = go.GetComponent<MeshRenderer>();
                r.material = p1Mat;
                if(newBuild == Building.Base)
                    pOneCanBase = !pOneCanBase;
                else
                    pOneCanLaser = !pOneCanLaser;
            }
            else
            {
                MeshRenderer r = go.GetComponent<MeshRenderer>();
                r.material = p2Mat;
                if (newBuild == Building.Base)
                    pTwoCanBase = !pTwoCanBase;
                else
                    pTwoCanLaser = !pTwoCanLaser;
            }
            go.transform.SetParent(transform);
            instances.Add(go);
        }
        else
            print("dont work");
    }

    private void SelectBlock(Vector3 pos)
    {
        GridItem gi = gridManager.theGrid.getCellInfo((int)pos.x, (int)pos.y);
        if (gi.isEmpty)
            return;
        else
        {
            haveSelected = true;
            selected = gi;
            selectedLoc = new Vector2(pos.x, pos.y);
        }
    }

    private void SwapBlock(Vector3 newPos)//this should be a fuction in the grid
    {
        GridItem temp = gridManager.theGrid.getCellInfo((int)newPos.x, (int)newPos.y);
        if(temp.owner == selected.owner)
        {
            gridManager.theGrid.destroyBuilding((int)newPos.x, (int)newPos.y, temp.owner);
            gridManager.theGrid.destroyBuilding((int)selectedLoc.x, (int)selectedLoc.y, temp.owner);
            bool place1 = gridManager.theGrid.placeBuilding((int)newPos.x, (int)newPos.y, selected.building, selected.owner);
            bool place2 = gridManager.theGrid.placeBuilding((int)selectedLoc.x, (int)selectedLoc.y, temp.building, temp.owner);
            if (place1 && place2)//connect the instances to the gridItem so when they swap the instance will also move
                print("Swap Complete");
            else
                print ("Cannot complete swap");
            haveSelected = false;
        }
        else if(temp.owner == Player.World){
            bool move = gridManager.theGrid.moveBuilding((int)selectedLoc.x, (int)selectedLoc.y, (int)newPos.x, (int)newPos.y, selected.owner);
            if (move)
                print("Move complete");
            else print("Cannot move");
            haveSelected = false;
        }
    }
}

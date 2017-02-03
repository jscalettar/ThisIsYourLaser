using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This file will be replaced by Scott's actual keyboard control files once that is done
public class setupManager : MonoBehaviour
{

    public List<GameObject> goList;
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
    private int i;
    private bool haveSelected;
    private Building selection;
    private Vector2 selectedLoc;
    private GridItem selected;

    // Use this for initialization
    void Start()
    {
        //Setup to set bases
        pOneCanBase = true;
        pTwoCanBase = true;
        pOneCanLaser = false;
        pTwoCanLaser = false;
        basePhase = true;
        laserPhase = false;
        haveSelected = false;
        selection = Building.Empty;
        i = 0;
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
                PlaceBuild(Player.PlayerOne, Building.Base, 0, p1Pos);
            }
            else if (Input.GetKeyDown(KeyCode.P) && pTwoCanBase)//P2 base place
            {
                PlaceBuild(Player.PlayerTwo, Building.Base, 0, p2Pos);
            }
        }
        else if (laserPhase)
        {
            if (Input.GetKeyDown(KeyCode.Space) && pOneCanLaser)//P1 laser place
            {
                PlaceBuild(Player.PlayerOne, Building.Laser, 1, p1Pos);
            }
            else if (Input.GetKeyDown(KeyCode.P) && pTwoCanLaser)//P2 laser place
            {
                PlaceBuild(Player.PlayerTwo, Building.Laser, 1, p2Pos);
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
        else if (laserPhase == basePhase && selection == Building.Empty)
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
        if (basePhase == laserPhase)
        {

            if (Input.GetKeyDown("1")) { selection = Building.Empty; i = 1; print("Free Move selection"); }
            if (Input.GetKeyDown("2")) { selection = Building.Blocking; i = 2; print("Blocking Selected"); }
            if (Input.GetKeyDown("3")) { selection = Building.Reflecting; i = 3; print("Reflecting Selected"); }
            if (Input.GetKeyDown("4")) { selection = Building.Refracting; i = 4; print("Refracting Selected"); }
            if (Input.GetKeyDown("5")) { selection = Building.Redirecting; i = 5; print("Redirecting Selected"); }
            if (Input.GetKeyDown("6")) { selection = Building.Portal; i = 6; print("Portal Selected"); }
            if (Input.GetKeyDown("7")) { selection = Building.Resource; i = 7; print("Resource Selected"); }

            if (Input.GetKeyDown(KeyCode.Space) && i > 1)
            {
                print(i);
                PlaceBuild(Player.PlayerOne, selection, i, p1Pos);
            }
            else if (Input.GetKeyDown(KeyCode.P) && i > 1)
            {
                PlaceBuild(Player.PlayerTwo, selection, i, p2Pos);
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
        print(pos);
        if (gridManager.theGrid.placeBuilding((int)pos.x, (int)pos.z, newBuild, player)) //place in grid
        {
            pos = new Vector3(pos.x - 6.5f, 0, pos.z - 3.5f);
            GameObject go = Instantiate(goList[val], pos, Quaternion.identity) as GameObject;//if it works, create an instance of object
            if (player == Player.PlayerOne)
            {
                MeshRenderer r = go.GetComponent<MeshRenderer>();
                r.material = p1Mat;
                if (newBuild == Building.Base)
                    pOneCanBase = !pOneCanBase;
                else if (newBuild == Building.Laser)
                    pOneCanLaser = !pOneCanLaser;
            }
            else
            {
                MeshRenderer r = go.GetComponent<MeshRenderer>();
                r.material = p2Mat;
                if (newBuild == Building.Base)
                    pTwoCanBase = !pTwoCanBase;
                else if (newBuild == Building.Laser)
                    pTwoCanLaser = !pTwoCanLaser;
            }
            go.transform.SetParent(transform);
        }
        else
            print("dont work");
    }

    private void SelectBlock(Vector3 pos)
    {
        GridItem gi = gridManager.theGrid.getCellInfo((int)pos.x, (int)pos.z);
        if (gi.isEmpty)
            return;
        else
        {
            haveSelected = true;
            selected = gi;
            selectedLoc = new Vector2(pos.x, pos.z);
            print("objLoc: " + selectedLoc);
        }
    }

    private void SwapBlock(Vector3 newPos)//this should be a fuction in the grid
    {
        GridItem temp = gridManager.theGrid.getCellInfo((int)newPos.x, (int)newPos.z);
        if (temp.owner == selected.owner)
        {
            print("newLoc: " + newPos);
            bool swap = gridManager.theGrid.swapBuilding((int)selectedLoc.x, (int)selectedLoc.y, (int)newPos.x, (int)newPos.z, selected.owner);
            if (swap)//connect the instances to the gridItem so when they swap the instance will also move
                print("Swap Complete");
            else
                print("Cannot complete swap");
            haveSelected = false;
        }
        else if (temp.owner == Player.World)
        {
            print("newLoc: " + newPos);
            bool move = gridManager.theGrid.moveBuilding((int)selectedLoc.x, (int)selectedLoc.y, (int)newPos.x, (int)newPos.z, selected.owner);
            if (move)
                print("Move complete");
            else print("Cannot move");
            haveSelected = false;
        }
    }
}



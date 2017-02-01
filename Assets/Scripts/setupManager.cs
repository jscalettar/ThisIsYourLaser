using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This file will be replaced by Scott's actual keyboard control files once that is done
public class setupManager : MonoBehaviour {

    public List<GameObject> goList;
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

    private Building selection;

    // Use this for initialization
    void Start () {
        //Setup to set bases
        pOneCanBase = true;
        pTwoCanBase = true;
        pOneCanLaser = false;
        pTwoCanLaser = false;
        basePhase = true;
        laserPhase = false;
        selection = Building.Empty;
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
                PlaceBuild(Player.PlayerOne, Building.Base, goList[0], p1Pos);
                pOneCanBase = !pOneCanBase;
            }
            else if (Input.GetKeyDown(KeyCode.P) && pTwoCanBase)//P2 base place
            {
                PlaceBuild(Player.PlayerTwo, Building.Base, goList[0], p2Pos);
                pTwoCanBase = !pTwoCanBase;
            }
        }
        else if (laserPhase)
        {
            if (Input.GetKeyDown(KeyCode.Space) && pOneCanLaser)//P1 laser place
            {
                PlaceBuild(Player.PlayerOne, Building.Laser, goList[1], p1Pos);
                pOneCanLaser = !pOneCanLaser;
            }
            else if (Input.GetKeyDown(KeyCode.P) && pTwoCanLaser)//P2 laser place
            {
                PlaceBuild(Player.PlayerTwo, Building.Laser, goList[1], p2Pos);
                pTwoCanLaser = !pTwoCanLaser;
            }
        }
        else if (!pOneCanBase && !pTwoCanBase)
                pOneCanLaser = pTwoCanLaser = true;
        if(basePhase == laserPhase)
        {
            int i = 0;

            if (Input.GetKeyDown("1")) { selection = Building.Empty; print("Free Move selection"); }
            if (Input.GetKeyDown("2")) { selection = Building.Blocking; i = 2; print("Blocking Selected"); }
            if (Input.GetKeyDown("3")) { selection = Building.Reflecting; i = 3; print("Reflecting Selected"); }
            if (Input.GetKeyDown("4")) { selection = Building.Refracting; i = 4; print("Refracting Selected"); }
            if (Input.GetKeyDown("5")) { selection = Building.Redirecting; i = 5; print("Redirecting Selected"); }
            if (Input.GetKeyDown("6")) { selection = Building.Portal; i = 6; print("Portal Selected"); }
            if (Input.GetKeyDown("7")) { selection = Building.Resource; i = 7; print("Resource Selected"); }

            if (Input.GetMouseButtonDown(0) && i > 1)
            {
                PlaceBuild(Player.PlayerOne, selection, goList[i], p1Pos);
            }
            else if (Input.GetMouseButtonDown(1) && i > 1)
            {
                PlaceBuild(Player.PlayerTwo, selection, goList[i], p2Pos);
            }
        }
    }

    private void UpdateSelection()//gets the position on the grid, to be replaced with Scott's movement
    {
        p1Pos = cursor1.pos;
        p2Pos = cursor2.pos;
    }

    void PlaceBuild(Player player, Building newBuild, GameObject build, Vector3 pos)
    {

        if (gridManager.theGrid.placeBuilding((int)pos.x, (int)pos.y, newBuild, player))//place in grid
        {
            GameObject go = Instantiate(build, pos, Quaternion.identity) as GameObject;//if it works, create an instance of object
            go.transform.SetParent(transform);
        }
        else
            print("dont work");
    }
}

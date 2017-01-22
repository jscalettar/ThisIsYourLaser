using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This file will be replaced by Scott's actual keyboard control files once that is done
public class setupManager : MonoBehaviour {
    
    public GameObject Base;
    public GameObject Laser;
    // location of the mouse on grid
    private float selectionX = -1;
    private float selectionY = -1;
    //if players can place base
    private bool pOneCanBase;
    private bool pTwoCanBase;
    //if players can place laser
    private bool pOneCanLaser;
    private bool pTwoCanLaser;
    //so players cant place both at same time and after laser phase its over
    private bool basePhase;
    private bool laserPhase; 

    // Use this for initialization
    void Start () {
        //Setup to set bases
        pOneCanBase = true;
        pTwoCanBase = true;
        pOneCanLaser = false;
        pTwoCanLaser = false;
        basePhase = true;
        laserPhase = false;

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
            if (Input.GetMouseButtonDown(0) && pOneCanBase)//P1 base place
            {
                PlaceBaseLaser(Player.PlayerOne, Building.Base, Base);
                pOneCanBase = !pOneCanBase;
            }
            else if (Input.GetMouseButtonDown(1) && pTwoCanBase)//P2 base place
            {
                PlaceBaseLaser(Player.PlayerTwo, Building.Base, Base);
                pTwoCanBase = !pTwoCanBase;
            }
        }
        else if (laserPhase)
        {
            if (Input.GetMouseButtonDown(0) && pOneCanLaser)//P1 laser place
            {
                PlaceBaseLaser(Player.PlayerOne, Building.Laser, Laser);
                pOneCanLaser = !pOneCanLaser;
            }
            else if (Input.GetMouseButtonDown(1) && pTwoCanLaser)//P2 laser place
            {
                PlaceBaseLaser(Player.PlayerTwo, Building.Laser, Laser);
                pTwoCanLaser = !pTwoCanLaser;
            }
        }
            else if (!pOneCanBase && !pTwoCanBase)
                pOneCanLaser = pTwoCanLaser = true;
    }

    private void UpdateSelection()//gets the position on the grid, to be replaced with Scott's movement
    {
        if (!Camera.main)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    void PlaceBaseLaser(Player player, Building newBuild, GameObject build)
    {
        Vector3 tileCenter = new Vector3(selectionX+.5f, 0, selectionY+.5f);// get center of tiles
        print(tileCenter);

        if (gridManager.theGrid.placeBuilding((int)selectionX, (int)selectionY, newBuild, player))//place in grid
        {
            GameObject go = Instantiate(build, tileCenter, Quaternion.identity) as GameObject;//if it works, create an instance of object
            go.transform.SetParent(transform);
        }
        else
            print("dont work");
    }
}

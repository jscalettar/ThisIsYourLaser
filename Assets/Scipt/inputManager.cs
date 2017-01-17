using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This file will be replaced by Scott's actual keyboard control files once that is done
public class inputManager : MonoBehaviour {
    //We should make this a list of all building prefabs so the player can scroll through them
    public GameObject laserPrefab;

	// Use this for initialization
	void Start () {
		//Here we can probably set the state of the game to "first move" where the player is
        //only allowed to place the base, then the laser
	}

    // Update is called once per frame
    void Update() {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

        if (Input.GetMouseButtonDown(0)) {
            Vector3 wordPos;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f)) {
                wordPos = hit.point;
            }
            else {
                wordPos = Camera.main.ScreenToWorldPoint(mousePos);
            }
            Instantiate(laserPrefab, wordPos, Quaternion.identity);
        }
    }
}

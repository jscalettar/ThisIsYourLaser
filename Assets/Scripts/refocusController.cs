using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//This fixes a bug that causes controller input to stop working
public class refocusController : MonoBehaviour {

    GameObject lastSelectedObject;
    public GameObject eventSystem;
    public GameObject eventSystem2;

	void Start() {
        lastSelectedObject = new GameObject();
    }
    
	void Update () {
		if(EventSystem.current.currentSelectedGameObject == null) {
            EventSystem.current.SetSelectedGameObject(lastSelectedObject);
        } else {
            lastSelectedObject = EventSystem.current.currentSelectedGameObject;
        }

        if (Input.GetKeyDown("joystick 1 button 7")) { eventSystem2.GetComponent<EventSystem>().enabled = false; eventSystem.GetComponent<EventSystem>().enabled = true; }
        if (Input.GetKeyDown("joystick 2 button 7")) { eventSystem.GetComponent<EventSystem>().enabled = false; eventSystem2.GetComponent<EventSystem>().enabled = true; }
    }
}

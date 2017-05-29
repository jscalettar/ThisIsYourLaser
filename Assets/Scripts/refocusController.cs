using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//This fixes a bug that causes controller input to stop working
public class refocusController : MonoBehaviour {

    GameObject lastSelectedObject;
    GameObject firstSelectedObject;

    public Button currentSelected;
     
    public GameObject eventSystem;
    public GameObject eventSystem2;
    public GameObject eventSystem3;


    void Start() {
        firstSelectedObject = new GameObject();
        firstSelectedObject = EventSystem.current.currentSelectedGameObject;
        lastSelectedObject = new GameObject();
    }
    
	void Update () {

        if (EventSystem.current.currentSelectedGameObject == null) {
            EventSystem.current.SetSelectedGameObject(lastSelectedObject);
        } else if(lastSelectedObject != null){
            lastSelectedObject = EventSystem.current.currentSelectedGameObject;
        }

        if (EventSystem.current.IsPointerOverGameObject()) {
            EventSystem.current.SetSelectedGameObject(null);
        }
        if (Input.GetJoystickNames().Length > 0) {
            print(Input.GetJoystickNames());
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetKeyDown("joystick 1 button 7")) { eventSystem3.GetComponent<EventSystem>().enabled = false; eventSystem2.GetComponent<EventSystem>().enabled = false; eventSystem.GetComponent<EventSystem>().enabled = true; }
        if (Input.GetKeyDown("joystick 2 button 7")) { eventSystem3.GetComponent<EventSystem>().enabled = false; eventSystem.GetComponent<EventSystem>().enabled = false; eventSystem2.GetComponent<EventSystem>().enabled = true; }
        if (Input.anyKey) { eventSystem2.GetComponent<EventSystem>().enabled = false; eventSystem.GetComponent<EventSystem>().enabled = false; eventSystem3.GetComponent<EventSystem>().enabled = true; }
    }
}

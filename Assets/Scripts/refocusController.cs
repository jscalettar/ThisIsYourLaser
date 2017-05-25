using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//This fixes a bug that causes controller input to stop working
public class refocusController : MonoBehaviour {

    GameObject lastSelectedObject;

	void Start() {
        lastSelectedObject = new GameObject();
    }
    
	void Update () {
		if(EventSystem.current.currentSelectedGameObject == null) {
            EventSystem.current.SetSelectedGameObject(lastSelectedObject);
        } else {
            lastSelectedObject = EventSystem.current.currentSelectedGameObject;
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Still need to remove player 2 from tutorial, simplify it etc.
// Movement event popup triggers not yet implemented
// Building destruction related popups, and initial building spawns working

public class TutorialFramework : MonoBehaviour {

    public static bool tutorialActive = false;
    public GameObject Popup;    // Generic game object for displaying popups

    private TutorialModule activeTutorial;

    public void buildingDestructionEvent(XY pos, Building building)
    {
        if (activeTutorial.specificDestroyed.Count > 0) {
            for (int i = 0; i < activeTutorial.specificDestroyed.Count; i++) {
                if (activeTutorial.specificDestroyed[i] != null && activeTutorial.specificDestroyed[i].pos == pos) {
                    displayPopup(activeTutorial.specificDestroyed[i].popup, pos.x, pos.y); activeTutorial.specificDestroyed[i] = null; return;
                }
            }
        }
        if (activeTutorial.baseDestroyed != null && building == Building.Base) {
            displayPopup(activeTutorial.baseDestroyed); activeTutorial.baseDestroyed = null; return;
        } else if (activeTutorial.firstDestroyed != null) {
            displayPopup(activeTutorial.firstDestroyed); activeTutorial.firstDestroyed = null; return;
        }
    }
        
    private void displayPopup(Sprite tex, int x = -1, int y = -1)
    {
        if (x > 0 && y > 0) Popup.transform.localPosition = gridManager.theGrid.coordsToWorld(x, y);
        else Popup.transform.localPosition = Vector3.zero;

        Popup.GetComponent<SpriteRenderer>().sprite = tex;
        Popup.GetComponent<SpriteRenderer>().enabled = true;

        Time.timeScale = 0;
    }

    private void closePopup()
    {
        // Add functionality for sequence of popups?
        Time.timeScale = 1;
        Popup.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void nextTutorialLevel() // WIP
    {
        if (activeTutorial.moduleOrderIndex + 1 >= transform.childCount) return;
        activeTutorial = transform.GetChild(activeTutorial.moduleOrderIndex+1).GetComponent<TutorialModule>();
        // board reset needs to go here
        spawnInitialCreatures();
    }

    private void spawnInitialCreatures()
    {
        if (activeTutorial != null && activeTutorial.spawnList != null && activeTutorial.spawnList.Count > 0) {
            for (int i = 0; i < activeTutorial.spawnList.Count; i++) {
                TutorialModule.SpawnItem item = activeTutorial.spawnList[i];
                gridManager.theGrid.placeBuilding(item.x, item.y, item.building, item.player, item.direction, true);
            }
        }
    }

	void Update () {
        // Check if popup is active, and close it if cancel pressed
        if (Time.timeScale == 0 && (Input.GetButtonDown("cancel_1") || Input.GetButtonDown("cancel_2"))) closePopup();
    }

    void Start ()
    {
        spawnInitialCreatures();
    }

    // Toggle Tutorial Game State
    void OnEnable()
    {
        Popup.GetComponent<SpriteRenderer>().enabled = false;
        activeTutorial = transform.GetChild(0).GetComponent<TutorialModule>();
        tutorialActive = true;
    }

    void OnDisable()
    {
        tutorialActive = false;
    }
}

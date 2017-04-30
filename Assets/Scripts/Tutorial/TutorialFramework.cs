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
    public GameObject Board;    // Board with gridManager on it

    private TutorialModule activeTutorial;
    private bool endFlag = false;

    // -----------------------------------------------------------------------------------------------------------------------------------------------

    public void buildingDestructionEvent(XY pos, Building building)
    {
        if (activeTutorial.specificDestroyed.Count > 0) {
            for (int i = 0; i < activeTutorial.specificDestroyed.Count; i++) {
                if (activeTutorial.specificDestroyed[i] != null && activeTutorial.specificDestroyed[i].pos == pos) {
                    if (activeTutorial.specificDestroyed[i].endTrigger) endFlag = true;
                    displayPopup(activeTutorial.specificDestroyed[i].popup, pos.x, pos.y);
                    activeTutorial.specificDestroyed[i] = null;
                    return;
                }
            }
        }
        if (activeTutorial.baseDestroyed != null && building == Building.Base) {
            displayPopup(activeTutorial.baseDestroyed); activeTutorial.baseDestroyed = null;
            if (activeTutorial.endOnBaseDestruction) endFlag = true; return;
        } else if (activeTutorial.firstDestroyed != null) {
            displayPopup(activeTutorial.firstDestroyed); activeTutorial.firstDestroyed = null;
            if (activeTutorial.endOnFirstDestroyed) endFlag = true; return;
        }
    }

    public void moveEvent(inputController.Cursor cursor, inputController.Cursor cursorLast) // cursor move event
    {

    }

    public void placedEvent(XY pos, Building building)
    {

    }

    public void placingEvent(XY pos, Building building)
    {

    }

    public void movedEvent(XY pos, Building building)
    {

    }

    public void movingEvent(XY pos, Building building)
    {

    }

    public void movingPlacingEvent(XY pos, Building building)
    {

    }

    public void removedEvent(XY pos, Building building)
    {

    }

    public void removingEvent(XY pos, Building building)
    {

    }

    // -----------------------------------------------------------------------------------------------------------------------------------------------

    private void displayPopup(Sprite tex, int x = -1, int y = -1, bool destroy = false)
    {
        if (x > 0 && y > 0) Popup.transform.localPosition = gridManager.theGrid.coordsToWorld(x, y);
        else Popup.transform.localPosition = Vector3.zero;

        Popup.GetComponent<SpriteRenderer>().sprite = tex;
        Popup.GetComponent<SpriteRenderer>().enabled = true;

        Time.timeScale = 0;
    }

    private void closePopup()
    {
        Time.timeScale = 1;
        Popup.GetComponent<SpriteRenderer>().enabled = false;
        if (endFlag) { endFlag = false; Invoke("nextTutorialLevel", 2); }
    }

    private void nextTutorialLevel()
    {
        endFlag = false;
        if (activeTutorial.moduleOrderIndex + 1 >= transform.childCount) return;
        activeTutorial = transform.GetChild(activeTutorial.moduleOrderIndex + 1).GetComponent<TutorialModule>();
        Transform toDelete = Board.transform.Find("ObjectHolder");
        if (toDelete != null) Destroy(toDelete.gameObject);
        foreach (var item in gridManager.theGrid.prefabDictionary) Destroy(item.Value);
        Board.GetComponent<gridManager>().initGrid();
        Board.GetComponent<inputController>().initCursors();
        spawnInitialCreatures();
    }

    // -----------------------------------------------------------------------------------------------------------------------------------------------

    private void spawnInitialCreatures()
    {
        if (activeTutorial != null && activeTutorial.spawnList != null && activeTutorial.spawnList.Count > 0) {
            for (int i = 0; i < activeTutorial.spawnList.Count; i++) {
                TutorialModule.SpawnItem item = activeTutorial.spawnList[i];
                gridManager.theGrid.placeBuilding(item.x, item.y, item.building, item.player, item.direction, true);
            }
        }
    }

    private void displayInitialPopups()
    {
        if (Popup.GetComponent<SpriteRenderer>().enabled == false) {
            if (activeTutorial.initialPopup != null) { displayPopup(activeTutorial.initialPopup); activeTutorial.initialPopup = null; }
            else if (activeTutorial.initialPopup2 != null) { displayPopup(activeTutorial.initialPopup2); activeTutorial.initialPopup2 = null; }
            else if (activeTutorial.initialPopup3 != null) { displayPopup(activeTutorial.initialPopup3); activeTutorial.initialPopup3 = null; }
        }
    }

    public void setupCursorState()
    {
        inputController.p2HasPlacedBase = true;
        switch (activeTutorial.initialState) {
            case startState.placeBase: inputController.cursorP1.state = State.placeBase; break;
            case startState.placeLaser: inputController.cursorP1.state = State.placeLaser; inputController.p1HasPlacedBase = true; break;
            case startState.idle: inputController.cursorP1.state = State.idle; inputController.p1HasPlacedBase = true; break;
        }
        if ((inputController.cursorP1.state == State.placeBase || inputController.cursorP1.state == State.placeLaser) && (activeTutorial.initialSelection == Building.Base || activeTutorial.initialSelection == Building.Laser)) return;
        inputController.cursorP1.selection = activeTutorial.initialSelection;
    }

    // -----------------------------------------------------------------------------------------------------------------------------------------------

    void Update () {
        // Check if popup is active, and close it if cancel pressed
        if (Time.timeScale == 0 && (Input.GetButtonDown("cancel_1") || Input.GetButtonDown("cancel_2"))) closePopup();

        // Check if initial popups exist, and if so display them
        if (activeTutorial.initialPopup != null || activeTutorial.initialPopup2 != null || activeTutorial.initialPopup3 != null) displayInitialPopups();
    }

    void Start ()
    {
        spawnInitialCreatures();
        displayInitialPopups();
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

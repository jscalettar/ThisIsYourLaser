using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialFramework : MonoBehaviour {

    public static bool tutorialActive = false;
    public static bool skipFrame = false;       // used to skip input for a frame after a popup closes which prevents issues
    public static bool initialized = false;
    public GameObject Popup;    // Generic game object for displaying popups
    public GameObject Board;    // Board with gridManager on it

    private TutorialModule activeTutorial;
    private GameObject Highlight;
    public GameObject BottomImage;
    private bool endFlag = false;

    // -----------------------------------------------------------------------------------------------------------------------------------------------

    public void buildingDestructionEvent(XY pos, Building building)
    {
        if (activeTutorial.specificDestroyed.Count > 0) {
            for (int i = 0; i < activeTutorial.specificDestroyed.Count; i++) {
                if (activeTutorial.specificDestroyed[i] != null && activeTutorial.specificDestroyed[i].pos == pos) {
                    if (activeTutorial.specificDestroyed[i].endTrigger) endFlag = true;
                    displayPopup(activeTutorial.specificDestroyed[i].popup);
                    activeTutorial.specificDestroyed.RemoveAt(i);
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
        if (cursor.x > cursorLast.x)
        {
            if (activeTutorial.movedRight != null)
                displayPopup(activeTutorial.movedRight); activeTutorial.movedRight = null;
        } else if (cursor.x < cursorLast.x)
        {
            if (activeTutorial.movedLeft != null)
                displayPopup(activeTutorial.movedLeft); activeTutorial.movedLeft = null;
        } else if (cursor.y > cursorLast.y)
        {
            if (activeTutorial.movedUp != null)
                displayPopup(activeTutorial.movedUp); activeTutorial.movedUp = null;
        } else if (cursor.y < cursorLast.y)
        {
            if (activeTutorial.movedDown != null)
                displayPopup(activeTutorial.movedDown); activeTutorial.movedDown = null;
        }
        if (activeTutorial.endOnAllDirectionsMoved && activeTutorial.movedRight == null && activeTutorial.movedLeft == null && activeTutorial.movedUp == null && activeTutorial.movedDown == null) endFlag = true;

    }

    public void placedEvent(XY pos, Building building)
    {
        if (activeTutorial.specificInteraction.Count > 0)
        {
            for (int i = 0; i < activeTutorial.specificInteraction.Count; i++)
            {
                if (activeTutorial.specificInteraction[i] != null && (activeTutorial.specificInteraction[i].building == building || activeTutorial.specificInteraction[i].building == Building.Any) && activeTutorial.specificInteraction[i] != null && activeTutorial.specificInteraction[i].type == tutorialTrigger.placed && (activeTutorial.specificInteraction[i].pos == pos || activeTutorial.specificInteraction[i].pos == new XY(-1, -1)))
                {
                    if (activeTutorial.specificInteraction[i].endTrigger) endFlag = true;
                    displayPopup(activeTutorial.specificInteraction[i].popup);
                    activeTutorial.specificInteraction.RemoveAt(i);
                    return;
                }
            }
        }
        if (activeTutorial.firstPlaced != null)
        {
            displayPopup(activeTutorial.firstPlaced); activeTutorial.firstPlaced = null;
            if (activeTutorial.endOnPlaced) endFlag = true; return;
        }
    }

    public void placingEvent(XY pos, Building building)
    {
        if (activeTutorial.specificInteraction.Count > 0)
        {
            for (int i = 0; i < activeTutorial.specificInteraction.Count; i++)
            {
                if (activeTutorial.specificInteraction[i] != null && (activeTutorial.specificInteraction[i].building == building || activeTutorial.specificInteraction[i].building == Building.Any) && activeTutorial.specificInteraction[i] != null && activeTutorial.specificInteraction[i].type == tutorialTrigger.placing && (activeTutorial.specificInteraction[i].pos == pos || activeTutorial.specificInteraction[i].pos == new XY(-1, -1)))
                {
                    if (activeTutorial.specificInteraction[i].endTrigger) endFlag = true;
                    displayPopup(activeTutorial.specificInteraction[i].popup);
                    activeTutorial.specificInteraction.RemoveAt(i);
                    return;
                }
            }
        }
        if (activeTutorial.firstPlacing != null)
        {
            displayPopup(activeTutorial.firstPlacing); activeTutorial.firstPlacing = null;
        }
    }

    public void movedEvent(XY pos, Building building)
    {
        if (activeTutorial.specificInteraction.Count > 0) {
            for (int i = 0; i < activeTutorial.specificInteraction.Count; i++) {
                if (activeTutorial.specificInteraction[i] != null && (activeTutorial.specificInteraction[i].building == building || activeTutorial.specificInteraction[i].building == Building.Any) && activeTutorial.specificInteraction[i] != null && activeTutorial.specificInteraction[i].type == tutorialTrigger.moved && (activeTutorial.specificInteraction[i].pos == pos || activeTutorial.specificInteraction[i].pos == new XY(-1, -1))) {
                    if (activeTutorial.specificInteraction[i].endTrigger) endFlag = true;
                    displayPopup(activeTutorial.specificInteraction[i].popup);
                    activeTutorial.specificInteraction.RemoveAt(i);
                    return;
                }
            }
        }
        if (activeTutorial.firstMoved != null) {
            displayPopup(activeTutorial.firstMoved); activeTutorial.firstMoved = null;
        }
    }

    public void movingEvent(XY pos, Building building)
    {

    }

    public void movingPlacingEvent(XY pos, Building building)
    {

    }

    public void removedEvent(XY pos, Building building)
    {
        if (activeTutorial.specificInteraction.Count > 0) {
            for (int i = 0; i < activeTutorial.specificInteraction.Count; i++) {
                if (activeTutorial.specificInteraction[i] != null && (activeTutorial.specificInteraction[i].building == building || activeTutorial.specificInteraction[i].building == Building.Any) && activeTutorial.specificInteraction[i] != null && activeTutorial.specificInteraction[i].type == tutorialTrigger.removed && (activeTutorial.specificInteraction[i].pos == pos || activeTutorial.specificInteraction[i].pos == new XY(-1, -1))) {
                    if (activeTutorial.specificInteraction[i].endTrigger) endFlag = true;
                    displayPopup(activeTutorial.specificInteraction[i].popup);
                    activeTutorial.specificInteraction.RemoveAt(i);
                    return;
                }
            }
        }
        if (activeTutorial.firstRemoved != null) {
            displayPopup(activeTutorial.firstRemoved); activeTutorial.firstRemoved = null;
        }
    }

    public void removingEvent(XY pos, Building building)
    {

    }

    // -----------------------------------------------------------------------------------------------------------------------------------------------

    private void displayPopup(Texture2D tex, int x = -1, int y = -1, bool destroy = false)
    {
        if (tex == null) return;

        if (x > 0 && y > 0) Popup.transform.localPosition = gridManager.theGrid.coordsToWorld(x, y);
        else Popup.transform.localPosition = Vector3.zero;
        if (tex != null){
            Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), (Screen.height / 1920f) * 200f);
            Popup.GetComponent<SpriteRenderer>().sprite = sprite;
            Popup.GetComponent<SpriteRenderer>().enabled = true;
        }
        Time.timeScale = 0;
    }

    private void closePopup()
    {
        skipFrame = true;
        Time.timeScale = 1;
        Popup.GetComponent<SpriteRenderer>().enabled = false;
        if (endFlag) { endFlag = false; Invoke("nextTutorialLevel", 3); }
    }

    private void nextTutorialLevel()
    {
        endFlag = false;
        if (activeTutorial.moduleOrderIndex + 1 >= transform.childCount) {
            tutorialToInstructionFlag.instance.flag = true;
            SceneManager.LoadScene("TitleScreen");
            return;
        }
        activeTutorial = transform.GetChild(activeTutorial.moduleOrderIndex + 1).GetComponent<TutorialModule>();
        Transform toDelete = Board.transform.Find("ObjectHolder");
        if (toDelete != null) Destroy(toDelete.gameObject);
        foreach (var item in gridManager.theGrid.prefabDictionary) Destroy(item.Value);
        Board.GetComponent<inputController>().initCursors();
        Board.GetComponent<gridManager>().initGrid();
        spawnInitialCreatures();
        gridManager.theGrid.queueUpdate();
        if (Highlight != null) { DestroyImmediate(Highlight); }
        createHighlightSquare();
        drawBottomImage();
        Limicator.limicatorObj.reset();
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
            case startState.placeLaser: inputController.cursorP1.state = State.placeLaser; inputController.p1HasPlacedBase = true; inputController.p2HasPlacedBase = true; break;
            case startState.idle: inputController.cursorP1.state = State.idle; inputController.p1HasPlacedBase = true; inputController.p2HasPlacedBase = true; break;
        }
        if ((inputController.cursorP1.state == State.placeBase || inputController.cursorP1.state == State.placeLaser) && (activeTutorial.initialSelection == Building.Base || activeTutorial.initialSelection == Building.Laser)) return;
        inputController.cursorP1.selection = activeTutorial.initialSelection;
    }

    private void createHighlightSquare()
    {
        if (activeTutorial.highlightPos == new XY(-1, -1) || activeTutorial.highlightTexture == null) {
            if (Highlight != null) {
                Destroy(Highlight); Highlight = null;
            }
            return;
        }
        int x = activeTutorial.highlightPos.x;
        int y = activeTutorial.highlightPos.y;
        if (Highlight == null) Highlight = new GameObject("Highlight Sprite");
        if (Highlight.GetComponent<SpriteRenderer>() == null) Highlight.AddComponent<SpriteRenderer>();
        Highlight.transform.position = gridManager.theGrid.coordsToWorld(x, y);
        Highlight.transform.eulerAngles = new Vector3(90f, 0f, 0f);
        Sprite sprite = Sprite.Create(activeTutorial.highlightTexture, new Rect(0.0f, 0.0f, 256, 256), new Vector2(0.5f, 0.5f), 210f);
        Highlight.GetComponent<SpriteRenderer>().sprite = sprite;
        Highlight.GetComponent<SpriteRenderer>().enabled = true;
        Highlight.GetComponent<SpriteRenderer>().sortingOrder = -1;
    }

    private void drawBottomImage()
    {
        if (activeTutorial.bottomImage == null) {
            if (BottomImage != null && BottomImage.GetComponent<MeshRenderer>() != null) {
                BottomImage.GetComponent<MeshRenderer>().enabled = false;
            }
            return;
        }

        if (BottomImage != null && BottomImage.GetComponent<MeshRenderer>() != null) {
            BottomImage.GetComponent<MeshRenderer>().enabled = true;
            BottomImage.GetComponent<MeshRenderer>().material.mainTexture = activeTutorial.bottomImage;
        }
    }

    // -----------------------------------------------------------------------------------------------------------------------------------------------

    void Update () {
        // Check if popup is active, and close it if cancel pressed
		if (Time.timeScale == 0 && (Input.GetButtonDown("place_1") || Input.GetKeyDown("return"))) closePopup();

        // Check if initial popups exist, and if so display them
        if (initialized && (activeTutorial.initialPopup != null || activeTutorial.initialPopup2 != null || activeTutorial.initialPopup3 != null)) displayInitialPopups();
    }

    void Start ()
    {
        Invoke("init", 0.1f);
    }

    public void init()
    {
        spawnInitialCreatures();
        displayInitialPopups();
        createHighlightSquare();
        drawBottomImage();
        initialized = true;
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
        initialized = false;
        if (Highlight != null) Destroy(Highlight);
    }
}

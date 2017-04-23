using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GUI_Script : MonoBehaviour {

    public GameObject canvas;
    public static playerOneUI p1UI;
    public static playerTwoUI p2UI;
    public static UIScript UI;

    public GameObject limicator;
    public GameObject[,] stones;
    public int p1StonesPlaced;
    public int p2StonesPlaced;
    // Use this for initialization

    void Start () {
        //define UI
        UI = gameObject.AddComponent<UIScript>();
        UI.BlockingImage = GameObject.Find("BlockingImage").GetComponent<Image>();
        UI.ReflectingImage = GameObject.Find("ReflectingImage").GetComponent<Image>();
        UI.RefractingImage = GameObject.Find("RefractingImage").GetComponent<Image>();
        UI.RedirectingImage = GameObject.Find("RedirectingImage").GetComponent<Image>();
        UI.ResourceImage = GameObject.Find("ResourceImage").GetComponent<Image>();

        UI.ResourcePlaceCost = GameObject.Find("ResourcePlaceCost").GetComponent<Text>();
        UI.BlockPlaceCost = GameObject.Find("BlockPlaceCost").GetComponent<Text>();
        UI.ReflectPlaceCost = GameObject.Find("ReflectPlaceCost").GetComponent<Text>();
        UI.RefractPlaceCost = GameObject.Find("RefractPlaceCost").GetComponent<Text>();
        UI.RedirectPlaceCost = GameObject.Find("RedirectPlaceCost").GetComponent<Text>();

        UI.playerOneResource = GameObject.Find("playerOneResource").GetComponent<Text>();
        UI.playerTwoResource = GameObject.Find("playerTwoResource").GetComponent<Text>();

        //default values for Player 1 UI  
        /*p1UI = gameObject.AddComponent<playerOneUI>();
        //p1UI.playerState = GameObject.Find("playerOneState").GetComponent<Text>();
        p1UI.playerHealth = GameObject.Find("playerOneHealth").GetComponent<Text>();
        p1UI.currentResource = GameObject.Find("playerOneResource").GetComponent<Text>();
        //p1UI.playerState.text = "Placing";
        p1UI.p1ResourceBackground = GameObject.Find("p1ResourceBackground").GetComponent<Image>();
        p1UI.p1BlockBackground = GameObject.Find("p1BlockBackground").GetComponent<Image>();
        p1UI.p1ReflectBackground = GameObject.Find("p1ReflectBackground").GetComponent<Image>();
        p1UI.p1RefractBackground = GameObject.Find("p1RefractBackground").GetComponent<Image>();
        p1UI.p1RedirectBackground = GameObject.Find("p1RedirectBackground").GetComponent<Image>();

        p1UI.p1ResourcePlaceCost = GameObject.Find("p1ResourcePlaceCost").GetComponent<Text>();
        p1UI.p1ResourceMoveCost = GameObject.Find("p1ResourceMoveCost").GetComponent<Text>();
        p1UI.p1ResourceRemoveCost = GameObject.Find("p1ResourceRemoveCost").GetComponent<Text>();
        p1UI.p1BlockPlaceCost = GameObject.Find("p1BlockPlaceCost").GetComponent<Text>();
        p1UI.p1BlockMoveCost = GameObject.Find("p1BlockMoveCost").GetComponent<Text>();
        p1UI.p1BlockRemoveCost = GameObject.Find("p1BlockRemoveCost").GetComponent<Text>();
        p1UI.p1ReflectPlaceCost = GameObject.Find("p1ReflectPlaceCost").GetComponent<Text>();
        p1UI.p1ReflectMoveCost = GameObject.Find("p1ReflectMoveCost").GetComponent<Text>();
        p1UI.p1ReflectRemoveCost = GameObject.Find("p1ReflectRemoveCost").GetComponent<Text>();
        p1UI.p1RefractPlaceCost = GameObject.Find("p1RefractPlaceCost").GetComponent<Text>();
        p1UI.p1RefractMoveCost = GameObject.Find("p1RefractMoveCost").GetComponent<Text>();
        p1UI.p1RefractRemoveCost = GameObject.Find("p1RefractRemoveCost").GetComponent<Text>();
        p1UI.p1RedirectPlaceCost = GameObject.Find("p1RedirectPlaceCost").GetComponent<Text>();
        p1UI.p1RedirectMoveCost = GameObject.Find("p1RedirectMoveCost").GetComponent<Text>();
        p1UI.p1RedirectRemoveCost = GameObject.Find("p1RedirectRemoveCost").GetComponent<Text>();*/

        //default values for Player 2 UI  
        /*p2UI = gameObject.AddComponent<playerTwoUI>();
        //p2UI.playerState = GameObject.Find("playerTwoState").GetComponent<Text>();
        p2UI.playerHealth = GameObject.Find("playerTwoHealth").GetComponent<Text>();
        p2UI.currentResource = GameObject.Find("playerTwoResource").GetComponent<Text>();
        //p2UI.playerState.text = "Placing";
        p2UI.p2ResourceBackground = GameObject.Find("p2ResourceBackground").GetComponent<Image>();
        p2UI.p2BlockBackground = GameObject.Find("p2BlockBackground").GetComponent<Image>();
        p2UI.p2ReflectBackground = GameObject.Find("p2ReflectBackground").GetComponent<Image>();
        p2UI.p2RefractBackground = GameObject.Find("p2RefractBackground").GetComponent<Image>();
        p2UI.p2RedirectBackground = GameObject.Find("p2RedirectBackground").GetComponent<Image>();

        p2UI.p2ResourcePlaceCost = GameObject.Find("p2ResourcePlaceCost").GetComponent<Text>();
        p2UI.p2ResourceMoveCost = GameObject.Find("p2ResourceMoveCost").GetComponent<Text>();
        p2UI.p2ResourceRemoveCost = GameObject.Find("p2ResourceRemoveCost").GetComponent<Text>();
        p2UI.p2BlockPlaceCost = GameObject.Find("p2BlockPlaceCost").GetComponent<Text>();
        p2UI.p2BlockMoveCost = GameObject.Find("p2BlockMoveCost").GetComponent<Text>();
        p2UI.p2BlockRemoveCost = GameObject.Find("p2BlockRemoveCost").GetComponent<Text>();
        p2UI.p2ReflectPlaceCost = GameObject.Find("p2ReflectPlaceCost").GetComponent<Text>();
        p2UI.p2ReflectMoveCost = GameObject.Find("p2ReflectMoveCost").GetComponent<Text>();
        p2UI.p2ReflectRemoveCost = GameObject.Find("p2ReflectRemoveCost").GetComponent<Text>();
        p2UI.p2RefractPlaceCost = GameObject.Find("p2RefractPlaceCost").GetComponent<Text>();
        p2UI.p2RefractMoveCost = GameObject.Find("p2RefractMoveCost").GetComponent<Text>();
        p2UI.p2RefractRemoveCost = GameObject.Find("p2RefractRemoveCost").GetComponent<Text>();
        p2UI.p2RedirectPlaceCost = GameObject.Find("p2RedirectPlaceCost").GetComponent<Text>();
        p2UI.p2RedirectMoveCost = GameObject.Find("p2RedirectMoveCost").GetComponent<Text>();
        p2UI.p2RedirectRemoveCost = GameObject.Find("p2RedirectRemoveCost").GetComponent<Text>();

        stones = new GameObject[2, 10];
        if (limicator != null) {
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 10; j++) {
                    GameObject limit = Instantiate(limicator);    //makes transparent planes on each grid square
                    limit.transform.localPosition = i == 0 ? new Vector3(-6.3f, 0f, 3.3f - j * (.75f)) : new Vector3(6.3f, 0f, 3.3f - j * (.75f));
                    limit.transform.Rotate(90, 0, 0);
                    limit.transform.localScale = new Vector3(.1f, .1f, .1f);
                    stones[i, j] = limit;
                }
            }
        }
        p1StonesPlaced = 0;
        p2StonesPlaced = 0;*/
    }
	
	// Update is called once per frame
	void Update () {
        /*p1UI.playerHealth.text = "Base HP: " + (Mathf.Round(gridManager.theGrid.baseHealthP1() * 2) / 2f).ToString("F1");
        p2UI.playerHealth.text = "Base HP: " + (Mathf.Round(gridManager.theGrid.baseHealthP2() * 2) / 2f).ToString("F1");
        p1UI.currentResource.text = "Resources: " + (Mathf.Floor(gridManager.theGrid.getResourcesP1() * 2) / 2f).ToString("F1");
        p2UI.currentResource.text = "Resources: " + (Mathf.Floor(gridManager.theGrid.getResourcesP2() * 2) / 2f).ToString("F1");*/

        UI.playerOneResource.text = "Resources: " + (Mathf.Floor(gridManager.theGrid.getResourcesP1() * 2) / 2f).ToString("F1");
        UI.playerTwoResource.text = "Resources: " + (Mathf.Floor(gridManager.theGrid.getResourcesP2() * 2) / 2f).ToString("F1");

        /*p1UI.p1ResourceBackground.color = Color.white; p1UI.p1BlockBackground.color = Color.white; p1UI.p1ReflectBackground.color = Color.white;
        p1UI.p1RefractBackground.color = Color.white; p1UI.p1RedirectBackground.color = Color.white;
        p2UI.p2ResourceBackground.color = Color.white; p2UI.p2BlockBackground.color = Color.white; p2UI.p2ReflectBackground.color = Color.white;
        p2UI.p2RefractBackground.color = Color.white; p2UI.p2RedirectBackground.color = Color.white;*/


        /*if (inputController.cursorP1.state != State.placeBase && inputController.cursorP1.state != State.placeLaser && inputController.cursorP1.state != State.placingLaser)
            switch (inputController.cursorP1.selection) {
                case Building.Blocking: p1UI.p1BlockBackground.color = new Color(1f, 0.8f, 0.8f); break;
                case Building.Reflecting: p1UI.p1ReflectBackground.color = new Color(1f, 0.8f, 0.8f); break;
                case Building.Refracting: p1UI.p1RefractBackground.color = new Color(1f, 0.8f, 0.8f); break;
                case Building.Redirecting: p1UI.p1RedirectBackground.color = new Color(1f, 0.8f, 0.8f); break;
                case Building.Resource: p1UI.p1ResourceBackground.color = new Color(1f, 0.8f, 0.8f); break;
            }

        if (inputController.cursorP2.state != State.placeBase && inputController.cursorP2.state != State.placeLaser && inputController.cursorP2.state != State.placingLaser)
            switch (inputController.cursorP2.selection) {
                case Building.Blocking: p2UI.p2BlockBackground.color = new Color(0.8f, 1f, 0.8f); break;
                case Building.Reflecting: p2UI.p2ReflectBackground.color = new Color(0.8f, 1f, 0.8f); break;
                case Building.Refracting: p2UI.p2RefractBackground.color = new Color(0.8f, 1f, 0.8f); break;
                case Building.Redirecting: p2UI.p2RedirectBackground.color = new Color(0.8f, 1f, 0.8f); break;
                case Building.Resource: p2UI.p2ResourceBackground.color = new Color(0.8f, 1f, 0.8f); break;
            }*/

        //PLAYER 1--------
        // Set all backgrounds to white
        /*p1UI.p1ResourceBackground.color = Color.white; p1UI.p1BlockBackground.color = Color.white; p1UI.p1ReflectBackground.color = Color.white;
        p1UI.p1RefractBackground.color = Color.white; p1UI.p1RedirectBackground.color = Color.white;
        // Highlight selection
        if (inputController.cursorP1.selection == Building.Blocking) p1UI.p1BlockBackground.color = Color.red;
        if (inputController.cursorP1.selection == Building.Reflecting) p1UI.p1ReflectBackground.color = Color.red;
        if (inputController.cursorP1.selection == Building.Refracting) p1UI.p1RefractBackground.color = Color.red;
        if (inputController.cursorP1.selection == Building.Redirecting) p1UI.p1RedirectBackground.color = Color.red;
        if (inputController.cursorP1.selection == Building.Resource) p1UI.p1ResourceBackground.color = Color.red;
        // Grey-out resources player can't afford
        if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne) > gridManager.theGrid.getResourcesP1()) p1UI.p1BlockBackground.color = Color.grey;
        if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne) > gridManager.theGrid.getResourcesP1()) p1UI.p1ReflectBackground.color = Color.grey;
        if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne) > gridManager.theGrid.getResourcesP1()) p1UI.p1RefractBackground.color = Color.grey;
        if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne) > gridManager.theGrid.getResourcesP1()) p1UI.p1RedirectBackground.color = Color.grey;
        if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne) > gridManager.theGrid.getResourcesP1()) p1UI.p1ResourceBackground.color = Color.grey;

        //PLAYER 2--------
        // Set all backgrounds to white
        p2UI.p2ResourceBackground.color = Color.white; p2UI.p2BlockBackground.color = Color.white; p2UI.p2ReflectBackground.color = Color.white;
        p2UI.p2RefractBackground.color = Color.white; p2UI.p2RedirectBackground.color = Color.white;
        // Highlight selection
        if (inputController.cursorP2.selection == Building.Blocking) p2UI.p2BlockBackground.color = Color.green;
        if (inputController.cursorP2.selection == Building.Reflecting) p2UI.p2ReflectBackground.color = Color.green;
        if (inputController.cursorP2.selection == Building.Refracting) p2UI.p2RefractBackground.color = Color.green;
        if (inputController.cursorP2.selection == Building.Redirecting) p2UI.p2RedirectBackground.color = Color.green;
        if (inputController.cursorP2.selection == Building.Resource) p2UI.p2ResourceBackground.color = Color.green;
        // Grey-out resources player can't afford
        if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo) > gridManager.theGrid.getResourcesP2()) p2UI.p2BlockBackground.color = Color.grey;
        if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo) > gridManager.theGrid.getResourcesP2()) p2UI.p2ReflectBackground.color = Color.grey;
        if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo) > gridManager.theGrid.getResourcesP2()) p2UI.p2RefractBackground.color = Color.grey;
        if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo) > gridManager.theGrid.getResourcesP2()) p2UI.p2RedirectBackground.color = Color.grey;
        if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo) > gridManager.theGrid.getResourcesP2()) p2UI.p2ResourceBackground.color = Color.grey; */

        UI.BlockPlaceCost.text = gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne).ToString();
        UI.ReflectPlaceCost.text = gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne).ToString();
        UI.RefractPlaceCost.text = gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne).ToString();
        UI.RedirectPlaceCost.text = gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne).ToString();
        UI.ResourcePlaceCost.text = gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne).ToString();

        /*Text[] elements = p1UI.p1BlockBackground.GetComponentsInChildren<Text>();
        int counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne).ToString();
                        if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne) == 20) p1UI.p1BlockPlaceCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne) == 10) p1UI.p1BlockPlaceCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne) == 25) p1UI.p1BlockPlaceCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne) == 30) p1UI.p1BlockPlaceCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne) == 35) p1UI.p1BlockPlaceCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne) == 40) p1UI.p1BlockPlaceCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne) == 100) p1UI.p1BlockPlaceCost.color = Color.red; break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne, true) == 10) p1UI.p1BlockMoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne, true) == 5) p1UI.p1BlockMoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne, true) == 12.5) p1UI.p1BlockMoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne, true) == 15) p1UI.p1BlockMoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne, true) == 17.5) p1UI.p1BlockMoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne, true) == 20) p1UI.p1BlockMoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne, true) == 50) p1UI.p1BlockMoveCost.color = Color.red; break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne, false, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne, false, true) == 10) p1UI.p1BlockRemoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne, false, true) == 5) p1UI.p1BlockRemoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne, false, true) == 12.5) p1UI.p1BlockRemoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne, false, true) == 15) p1UI.p1BlockRemoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne, false, true) == 17.5) p1UI.p1BlockRemoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne, false, true) == 20) p1UI.p1BlockRemoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne, false, true) == 50) p1UI.p1BlockRemoveCost.color = Color.red; break;
                }
            counter++;
            }
        }

        elements = p1UI.p1ReflectBackground.GetComponentsInChildren<Text>(); counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne).ToString();
                        if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne) == 12) p1UI.p1ReflectPlaceCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne) == 6) p1UI.p1ReflectPlaceCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne) == 15) p1UI.p1ReflectPlaceCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne) == 18) p1UI.p1ReflectPlaceCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne) == 21) p1UI.p1ReflectPlaceCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne) == 24) p1UI.p1ReflectPlaceCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne) == 60) p1UI.p1ReflectPlaceCost.color = Color.red; break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne, true) == 6) p1UI.p1ReflectMoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne, true) == 3) p1UI.p1ReflectMoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne, true) == 7.5) p1UI.p1ReflectMoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne, true) == 9) p1UI.p1ReflectMoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne, true) == 10.5) p1UI.p1ReflectMoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne, true) == 12) p1UI.p1ReflectMoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne, true) == 30) p1UI.p1ReflectMoveCost.color = Color.red; break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne, false, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 6) p1UI.p1ReflectRemoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 3) p1UI.p1ReflectRemoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 7.5) p1UI.p1ReflectRemoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 9) p1UI.p1ReflectRemoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 10.5) p1UI.p1ReflectRemoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 12) p1UI.p1ReflectRemoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 30) p1UI.p1ReflectRemoveCost.color = Color.red; break;
                }
                counter++;
            }
        }

        elements = p1UI.p1RefractBackground.GetComponentsInChildren<Text>(); counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne).ToString();
                        if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne) == 8) p1UI.p1RefractPlaceCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne) == 4) p1UI.p1RefractPlaceCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne) == 10) p1UI.p1RefractPlaceCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne) == 12) p1UI.p1RefractPlaceCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne) == 14) p1UI.p1RefractPlaceCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne) == 16) p1UI.p1RefractPlaceCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne) == 40) p1UI.p1RefractPlaceCost.color = Color.red; break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne, true) == 4) p1UI.p1RefractMoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne, true) == 2) p1UI.p1RefractMoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne, true) == 5) p1UI.p1RefractMoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne, true) == 6) p1UI.p1RefractMoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne, true) == 7) p1UI.p1RefractMoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne, true) == 8) p1UI.p1RefractMoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne, true) == 20) p1UI.p1RefractMoveCost.color = Color.red; break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne, false, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 4) p1UI.p1RefractRemoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 2) p1UI.p1RefractRemoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 5) p1UI.p1RefractRemoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 6) p1UI.p1RefractRemoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 7) p1UI.p1RefractRemoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 8) p1UI.p1RefractRemoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 20) p1UI.p1RefractRemoveCost.color = Color.red; break;
                }
                counter++;
            }
        }

        elements = p1UI.p1RedirectBackground.GetComponentsInChildren<Text>(); counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne).ToString();
                        if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne) == 12) p1UI.p1RedirectPlaceCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne) == 6) p1UI.p1RedirectPlaceCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne) == 15) p1UI.p1RedirectPlaceCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne) == 18) p1UI.p1RedirectPlaceCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne) == 21) p1UI.p1RedirectPlaceCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne) == 24) p1UI.p1RedirectPlaceCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne) == 60) p1UI.p1RedirectPlaceCost.color = Color.red; break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne, true) == 6) p1UI.p1RedirectMoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne, true) == 3) p1UI.p1RedirectMoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne, true) == 7.5) p1UI.p1RedirectMoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne, true) == 9) p1UI.p1RedirectMoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne, true) == 10.5) p1UI.p1RedirectMoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne, true) == 12) p1UI.p1RedirectMoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne, true) == 30) p1UI.p1RedirectMoveCost.color = Color.red; break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne, false, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 6) p1UI.p1RedirectRemoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 3) p1UI.p1RedirectRemoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 7.5) p1UI.p1RedirectRemoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 9) p1UI.p1RedirectRemoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 10.5) p1UI.p1RedirectRemoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 12) p1UI.p1RedirectRemoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne, false, true) == 30) p1UI.p1RedirectRemoveCost.color = Color.red; break;
                }
                counter++;
            }
        }

        elements = p1UI.p1ResourceBackground.GetComponentsInChildren<Text>(); counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne).ToString();
                        if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne) == 8) p1UI.p1ResourcePlaceCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne) == 4) p1UI.p1ResourcePlaceCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne) == 10) p1UI.p1ResourcePlaceCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne) == 12) p1UI.p1ResourcePlaceCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne) == 14) p1UI.p1ResourcePlaceCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne) == 16) p1UI.p1ResourcePlaceCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne) == 40) p1UI.p1ResourcePlaceCost.color = Color.red; break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne, true) == 4) p1UI.p1ResourceMoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne, true) == 2) p1UI.p1ResourceMoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne, true) == 5) p1UI.p1ResourceMoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne, true) == 6) p1UI.p1ResourceMoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne, true) == 7) p1UI.p1ResourceMoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne, true) == 8) p1UI.p1ResourceMoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne, true) == 20) p1UI.p1ResourceMoveCost.color = Color.red; break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne, false, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne, false, true) == 4) p1UI.p1ResourceRemoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne, false, true) == 2) p1UI.p1ResourceRemoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne, false, true) == 5) p1UI.p1ResourceRemoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne, false, true) == 6) p1UI.p1ResourceRemoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne, false, true) == 7) p1UI.p1ResourceRemoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne, false, true) == 8) p1UI.p1ResourceRemoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne, false, true) == 20) p1UI.p1ResourceRemoveCost.color = Color.red; break;
                }
                counter++;
            }
        }












        elements = p2UI.p2BlockBackground.GetComponentsInChildren<Text>(); counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo).ToString();
                        if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo) == 20) p2UI.p2BlockPlaceCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo) == 10) p2UI.p2BlockPlaceCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo) == 25) p2UI.p2BlockPlaceCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo) == 30) p2UI.p2BlockPlaceCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo) == 35) p2UI.p2BlockPlaceCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo) == 40) p2UI.p2BlockPlaceCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo) == 100) p2UI.p2BlockPlaceCost.color = Color.red; break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo, true) == 10) p2UI.p2BlockMoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo, true) == 5) p2UI.p2BlockMoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo, true) == 12.5) p2UI.p2BlockMoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo, true) == 15) p2UI.p2BlockMoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo, true) == 17.5) p2UI.p2BlockMoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo, true) == 20) p2UI.p2BlockMoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo, true) == 50) p2UI.p2BlockMoveCost.color = Color.red; break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo, false, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 10) p2UI.p2BlockRemoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 5) p2UI.p2BlockRemoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 12.5) p2UI.p2BlockRemoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 15) p2UI.p2BlockRemoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 17.5) p2UI.p2BlockRemoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 20) p2UI.p2BlockRemoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 50) p2UI.p2BlockRemoveCost.color = Color.red; break;
                }
                counter++;
            }
        }

        elements = p2UI.p2ReflectBackground.GetComponentsInChildren<Text>(); counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo).ToString();
                        if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo) == 12) p2UI.p2ReflectPlaceCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo) == 6) p2UI.p2ReflectPlaceCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo) == 15) p2UI.p2ReflectPlaceCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo) == 18) p2UI.p2ReflectPlaceCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo) == 21) p2UI.p2ReflectPlaceCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo) == 24) p2UI.p2ReflectPlaceCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo) == 60) p2UI.p2ReflectPlaceCost.color = Color.red; break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo, true) == 6) p2UI.p2ReflectMoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo, true) == 3) p2UI.p2ReflectMoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo, true) == 7.5) p2UI.p2ReflectMoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo, true) == 9) p2UI.p2ReflectMoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo, true) == 10.5) p2UI.p2ReflectMoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo, true) == 12) p2UI.p2ReflectMoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo, true) == 30) p2UI.p2ReflectMoveCost.color = Color.red; break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo, false, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 6) p2UI.p2ReflectRemoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 3) p2UI.p2ReflectRemoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 7.5) p2UI.p2ReflectRemoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 9) p2UI.p2ReflectRemoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 10.5) p2UI.p2ReflectRemoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 12) p2UI.p2ReflectRemoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 30) p2UI.p2ReflectRemoveCost.color = Color.red; break;
                }
                counter++;
            }
        }

        elements = p2UI.p2RefractBackground.GetComponentsInChildren<Text>(); counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo).ToString();
                        if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo) == 8) p2UI.p2RefractPlaceCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo) == 4) p2UI.p2RefractPlaceCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo) == 10) p2UI.p2RefractPlaceCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo) == 12) p2UI.p2RefractPlaceCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo) == 14) p2UI.p2RefractPlaceCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo) == 16) p2UI.p2RefractPlaceCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo) == 40) p2UI.p2RefractPlaceCost.color = Color.red; break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo, true) == 4) p2UI.p2RefractMoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo, true) == 2) p2UI.p2RefractMoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo, true) == 5) p2UI.p2RefractMoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo, true) == 6) p2UI.p2RefractMoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo, true) == 7) p2UI.p2RefractMoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo, true) == 8) p2UI.p2RefractMoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo, true) == 20) p2UI.p2RefractMoveCost.color = Color.red; break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo, false, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 4) p2UI.p2RefractRemoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 2) p2UI.p2RefractRemoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 5) p2UI.p2RefractRemoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 6) p2UI.p2RefractRemoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 7) p2UI.p2RefractRemoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 8) p2UI.p2RefractRemoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 20) p2UI.p2RefractRemoveCost.color = Color.red; break;
                }
                counter++;
            }
        }

        elements = p2UI.p2RedirectBackground.GetComponentsInChildren<Text>(); counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo).ToString();
                        if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo) == 12) p2UI.p2RedirectPlaceCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo) == 6) p2UI.p2RedirectPlaceCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo) == 15) p2UI.p2RedirectPlaceCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo) == 18) p2UI.p2RedirectPlaceCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo) == 21) p2UI.p2RedirectPlaceCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo) == 24) p2UI.p2RedirectPlaceCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo) == 60) p2UI.p2RedirectPlaceCost.color = Color.red; break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo, true) == 6) p2UI.p2RedirectMoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo, true) == 3) p2UI.p2RedirectMoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo, true) == 7.5) p2UI.p2RedirectMoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo, true) == 9) p2UI.p2RedirectMoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo, true) == 10.5) p2UI.p2RedirectMoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo, true) == 12) p2UI.p2RedirectMoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo, true) == 30) p2UI.p2RedirectMoveCost.color = Color.red; break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo, false, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 6) p2UI.p2RedirectRemoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 3) p2UI.p2RedirectRemoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 7.5) p2UI.p2RedirectRemoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 9) p2UI.p2RedirectRemoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 10.5) p2UI.p2RedirectRemoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 12) p2UI.p2RedirectRemoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 30) p2UI.p2RedirectRemoveCost.color = Color.red; break;
                }
                counter++;
            }
        }

        elements = p2UI.p2ResourceBackground.GetComponentsInChildren<Text>(); counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo).ToString();
                        if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo) == 8) p2UI.p2ResourcePlaceCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo) == 4) p2UI.p2ResourcePlaceCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo) == 10) p2UI.p2ResourcePlaceCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo) == 12) p2UI.p2ResourcePlaceCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo) == 14) p2UI.p2ResourcePlaceCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo) == 16) p2UI.p2ResourcePlaceCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo) == 40) p2UI.p2ResourcePlaceCost.color = Color.red; break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo, true) == 4) p2UI.p2ReflectMoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo, true) == 2) p2UI.p2ResourceMoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo, true) == 5) p2UI.p2ResourceMoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo, true) == 6) p2UI.p2ResourceMoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo, true) == 7) p2UI.p2ResourceMoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo, true) == 8) p2UI.p2ResourceMoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo, true) == 20) p2UI.p2ResourceMoveCost.color = Color.red; break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo, false, true).ToString();
                        if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 4) p2UI.p2ResourceRemoveCost.color = new Color(.735f, .349f, .367f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 2) p2UI.p2ResourceRemoveCost.color = Color.black;
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 5) p2UI.p2ResourceRemoveCost.color = new Color(.949f, .235f, .235f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 6) p2UI.p2ResourceRemoveCost.color = new Color(.808f, .161f, .161f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 7) p2UI.p2ResourceRemoveCost.color = new Color(.612f, .074f, .102f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 8) p2UI.p2ResourceRemoveCost.color = new Color(.302f, 0.094f, .094f);
                        else if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo, false, true) == 20) p2UI.p2ResourceRemoveCost.color = Color.red; break;
                }
                counter++;
            }
        }*/











    }

   
}

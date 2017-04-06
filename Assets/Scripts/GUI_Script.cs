using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GUI_Script : MonoBehaviour {

    public GameObject canvas;
    public static playerOneUI p1UI;
    public static playerTwoUI p2UI;

    // Use this for initialization
    void Start () {
        //default values for Player 1 UI  
        p1UI = gameObject.AddComponent<playerOneUI>();
        p1UI.playerState = GameObject.Find("playerOneState").GetComponent<Text>();
        p1UI.playerHealth = GameObject.Find("playerOneHealth").GetComponent<Text>();
        p1UI.currentResource = GameObject.Find("playerOneResource").GetComponent<Text>();
        //p1UI.playerState.text = "Placing";
        p1UI.p1ResourceBackground = GameObject.Find("p1ResourceBackground").GetComponent<Image>();
        p1UI.p1BlockBackground = GameObject.Find("p1BlockBackground").GetComponent<Image>();
        p1UI.p1ReflectBackground = GameObject.Find("p1ReflectBackground").GetComponent<Image>();
        p1UI.p1RefractBackground = GameObject.Find("p1RefractBackground").GetComponent<Image>();
        p1UI.p1RedirectBackground = GameObject.Find("p1RedirectBackground").GetComponent<Image>();

        //default values for Player 2 UI  
        p2UI = gameObject.AddComponent<playerTwoUI>();
        p2UI.playerState = GameObject.Find("playerTwoState").GetComponent<Text>();
        p2UI.playerHealth = GameObject.Find("playerTwoHealth").GetComponent<Text>();
        p2UI.currentResource = GameObject.Find("playerTwoResource").GetComponent<Text>();
        //p2UI.playerState.text = "Placing";
        p2UI.p2ResourceBackground = GameObject.Find("p2ResourceBackground").GetComponent<Image>();
        p2UI.p2BlockBackground = GameObject.Find("p2BlockBackground").GetComponent<Image>();
        p2UI.p2ReflectBackground = GameObject.Find("p2ReflectBackground").GetComponent<Image>();
        p2UI.p2RefractBackground = GameObject.Find("p2RefractBackground").GetComponent<Image>();
        p2UI.p2RedirectBackground = GameObject.Find("p2RedirectBackground").GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        p1UI.playerHealth.text = "Base HP: " + (Mathf.Round(gridManager.theGrid.baseHealthP1() * 2) / 2f).ToString("F1");
        p2UI.playerHealth.text = "Base HP: " + (Mathf.Round(gridManager.theGrid.baseHealthP2() * 2) / 2f).ToString("F1");
        p1UI.currentResource.text = "Resources: " + (Mathf.Round(gridManager.theGrid.getResourcesP1() * 2) / 2f).ToString("F1");
        p2UI.currentResource.text = "Resources: " + (Mathf.Round(gridManager.theGrid.getResourcesP2() * 2) / 2f).ToString("F1");

        p1UI.p1ResourceBackground.color = Color.white; p1UI.p1BlockBackground.color = Color.white; p1UI.p1ReflectBackground.color = Color.white;
        p1UI.p1RefractBackground.color = Color.white; p1UI.p1RedirectBackground.color = Color.white;
        p2UI.p2ResourceBackground.color = Color.white; p2UI.p2BlockBackground.color = Color.white; p2UI.p2ReflectBackground.color = Color.white;
        p2UI.p2RefractBackground.color = Color.white; p2UI.p2RedirectBackground.color = Color.white;

        if (inputController.cursorP1.state != State.placeBase && inputController.cursorP1.state != State.placeLaser && inputController.cursorP1.state != State.placingLaser)
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
            }

    



        Text[] elements = p1UI.p1BlockBackground.GetComponentsInChildren<Text>();
        int counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne).ToString(); break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne, true).ToString(); break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne, false, true).ToString(); break;
                }
            counter++;
            }
        }

        elements = p1UI.p1ReflectBackground.GetComponentsInChildren<Text>(); counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne).ToString(); break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne, true).ToString(); break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne, false, true).ToString(); break;
                }
                counter++;
            }
        }

        elements = p1UI.p1RefractBackground.GetComponentsInChildren<Text>(); counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne).ToString(); break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne, true).ToString(); break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne, false, true).ToString(); break;
                }
                counter++;
            }
        }

        elements = p1UI.p1RedirectBackground.GetComponentsInChildren<Text>(); counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne).ToString(); break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne, true).ToString(); break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne, false, true).ToString(); break;
                }
                counter++;
            }
        }

        elements = p1UI.p1ResourceBackground.GetComponentsInChildren<Text>(); counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne).ToString(); break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne, true).ToString(); break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne, false, true).ToString(); break;
                }
                counter++;
            }
        }












        elements = p2UI.p2BlockBackground.GetComponentsInChildren<Text>(); counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo).ToString(); break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo, true).ToString(); break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo, false, true).ToString(); break;
                }
                counter++;
            }
        }

        elements = p2UI.p2ReflectBackground.GetComponentsInChildren<Text>(); counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo).ToString(); break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo, true).ToString(); break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo, false, true).ToString(); break;
                }
                counter++;
            }
        }

        elements = p2UI.p2RefractBackground.GetComponentsInChildren<Text>(); counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo).ToString(); break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo, true).ToString(); break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo, false, true).ToString(); break;
                }
                counter++;
            }
        }

        elements = p2UI.p2RedirectBackground.GetComponentsInChildren<Text>(); counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo).ToString(); break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo, true).ToString(); break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo, false, true).ToString(); break;
                }
                counter++;
            }
        }

        elements = p2UI.p2ResourceBackground.GetComponentsInChildren<Text>(); counter = 0;
        foreach (Text text in elements) {
            if (text.name.Contains("Cost")) {
                switch (counter) {
                    case 0: text.text = gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo).ToString(); break;
                    case 1: text.text = gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo, true).ToString(); break;
                    case 2: text.text = gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo, false, true).ToString(); break;
                }
                counter++;
            }
        }











    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GUI_Script : MonoBehaviour {

    public GameObject canvas;
    public static UIScript UI;
    
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

        UI.BlockPlaceCost.text = gridManager.theGrid.getCost(Building.Blocking).ToString();
        UI.ReflectPlaceCost.text = gridManager.theGrid.getCost(Building.Reflecting).ToString();
        UI.RefractPlaceCost.text = gridManager.theGrid.getCost(Building.Refracting).ToString();
        UI.RedirectPlaceCost.text = gridManager.theGrid.getCost(Building.Redirecting).ToString();
        UI.ResourcePlaceCost.text = gridManager.theGrid.getCost(Building.Resource).ToString();

		UI.SpriteP1 = GameObject.Find("Sprite1").GetComponent<SpriteRenderer>().material;

        if (GameObject.Find("Sprite2") != null)
		    UI.SpriteP2 = GameObject.Find("Sprite2").GetComponent<SpriteRenderer>().material;
    }
	
	// Update is called once per frame
	void Update () {
        UI.playerOneResource.text = (Mathf.Floor(gridManager.theGrid.getResourcesP1() * 2) / 2f).ToString("F1");
        UI.playerTwoResource.text = (Mathf.Floor(gridManager.theGrid.getResourcesP2() * 2) / 2f).ToString("F1");

		////////// Player 1 cursor color //////////
		UI.SpriteP1.color = new Vector4 (1f,1f,1f,0.5f); //default cursor color
		//Blocking Block selected
		if (inputController.cursorP1.selection == Building.Blocking) {
			//Can't afford
			if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne) > gridManager.theGrid.getResourcesP1()) {
				UI.SpriteP1.color = new Vector4 (0.3f,0.3f,0.3f,1f); //cursor quad is grey
			}
		}
		//Reflecting Block selected
		if (inputController.cursorP1.selection == Building.Reflecting) {
			//Can't afford
			if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne) > gridManager.theGrid.getResourcesP1()) {
				UI.SpriteP1.color = new Vector4 (0.3f,0.3f,0.3f,1f); //cursor quad is grey
			}
		}
		//Refracting Block selected
		if (inputController.cursorP1.selection == Building.Refracting) {
			//Can't afford
			if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne) > gridManager.theGrid.getResourcesP1()) {
				UI.SpriteP1.color = new Vector4 (0.3f,0.3f,0.3f,1f); //cursor quad is grey
			}
		}
		//Redirecting Block selected
		if (inputController.cursorP1.selection == Building.Redirecting) {
			//Can't afford
			if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne) > gridManager.theGrid.getResourcesP1()) {
				UI.SpriteP1.color = new Vector4 (0.3f,0.3f,0.3f,1f); //cursor quad is grey
			}
		}
		//Resource Block selected
		if (inputController.cursorP1.selection == Building.Resource) {
			//Can't afford
			if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne) > gridManager.theGrid.getResourcesP1()) {
				UI.SpriteP1.color = new Vector4 (0.3f,0.3f,0.3f,1f); //cursor quad is grey
			}
		}

        ////////// Player 2 cursor color //////////
        if (UI.SpriteP2 != null) {
            UI.SpriteP2.color = new Vector4(1f, 1f, 1f, 0.5f); //default cursor color
                                                               //Blocking Block selected
            if (inputController.cursorP2.selection == Building.Blocking) {
                //Can't afford
                if (gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP2.x, Player.PlayerTwo) > gridManager.theGrid.getResourcesP2()) {
                    UI.SpriteP2.color = new Vector4(0.3f, 0.3f, 0.3f, 1f); //cursor quad is grey
                }
            }
            //Reflecting Block selected
            if (inputController.cursorP2.selection == Building.Reflecting) {
                //Can't afford
                if (gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP2.x, Player.PlayerTwo) > gridManager.theGrid.getResourcesP2()) {
                    UI.SpriteP2.color = new Vector4(0.3f, 0.3f, 0.3f, 1f); //cursor quad is grey
                }
            }
            //Refracting Block selected
            if (inputController.cursorP2.selection == Building.Refracting) {
                //Can't afford
                if (gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP2.x, Player.PlayerTwo) > gridManager.theGrid.getResourcesP2()) {
                    UI.SpriteP2.color = new Vector4(0.3f, 0.3f, 0.3f, 1f); //cursor quad is grey
                }
            }
            //Redirecting Block selected
            if (inputController.cursorP2.selection == Building.Redirecting) {
                //Can't afford
                if (gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP2.x, Player.PlayerTwo) > gridManager.theGrid.getResourcesP2()) {
                    UI.SpriteP2.color = new Vector4(0.3f, 0.3f, 0.3f, 1f); //cursor quad is grey
                }
            }
            //Resource Block selected
            if (inputController.cursorP2.selection == Building.Resource) {
                //Can't afford
                if (gridManager.theGrid.getCost(Building.Resource, inputController.cursorP2.x, Player.PlayerTwo) > gridManager.theGrid.getResourcesP2()) {
                    UI.SpriteP2.color = new Vector4(0.3f, 0.3f, 0.3f, 1f); //cursor quad is grey
                }
            }
        }
    } 
}

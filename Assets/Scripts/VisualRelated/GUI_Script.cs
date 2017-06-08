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
		if (inputController.cursorP1.selection != Building.Laser && inputController.cursorP1.selection != Building.Base) {
			//Can't afford
			if ((gridManager.theGrid.getCost (inputController.cursorP1.selection, inputController.cursorP1.x, Player.PlayerOne) > gridManager.theGrid.getResourcesP1 ()) ||
                (!gridManager.theGrid.probeGrid(inputController.cursorP1.x, inputController.cursorP1.y, Direction.Up, Building.Blocking))) {
				UI.SpriteP1.color = new Vector4 (0.3f, 0.3f, 0.3f, 0.5f); //cursor quad is grey
			} else if (checkDanger(Player.PlayerOne)) {
				UI.SpriteP1.color = new Vector4 (1f, 0.3f, 0.3f, 0.5f); //cursor quad is grey
			}
		}

        ////////// Player 2 cursor color //////////
        if (UI.SpriteP2 != null) {
            UI.SpriteP2.color = new Vector4(1f, 1f, 1f, 0.5f); //default cursor color
			if (inputController.cursorP2.selection != Building.Laser && inputController.cursorP2.selection != Building.Base) {
				//Can't afford
				if ((gridManager.theGrid.getCost (inputController.cursorP2.selection, inputController.cursorP2.x, Player.PlayerTwo) > gridManager.theGrid.getResourcesP2 ()) ||
                (!gridManager.theGrid.probeGrid(inputController.cursorP2.x, inputController.cursorP2.y, Direction.Up, Building.Blocking))) {
					UI.SpriteP2.color = new Vector4 (0.3f, 0.3f, 0.3f, 0.5f); //cursor quad is grey
				} else if (checkDanger(Player.PlayerTwo)) {
					UI.SpriteP2.color = new Vector4 (1f, 0.3f, 0.3f, 0.5f); //cursor quad is grey
				}
			}
        }
    }

	bool checkDanger(Player player) {
		Direction dir = (player == Player.PlayerOne ? inputController.cursorP1.direction : inputController.cursorP2.direction);
		Building creature = (player == Player.PlayerOne ? inputController.cursorP1.selection : inputController.cursorP2.selection);
		if (player == Player.PlayerOne && (inputController.cursorP1.state == State.moving || inputController.cursorP1.state == State.placingMove)) {
			creature = inputController.cursorP1.moveBuilding;
		} else if (player == Player.PlayerTwo && (inputController.cursorP2.state == State.moving || inputController.cursorP2.state == State.placingMove)) {
			creature = inputController.cursorP2.moveBuilding;
		}
		List<Direction> laserDirs;
		if (player == Player.PlayerOne) laserDirs = laserLogic.laserData.getLaserDirections(inputController.cursorP1.x, inputController.cursorP1.y);
		else laserDirs = laserLogic.laserData.getLaserDirections(inputController.cursorP2.x, inputController.cursorP2.y);


		if (laserDirs != null) {
			switch (creature) {
			case Building.Blocking:
				{
					return true;
				}
			case Building.Reflecting:
				{
					foreach (var laserDir in laserDirs) {
						if (laserLogic.opposites(dir, laserDir) || dir == laserDir)
							return true;
					}
					return false;
				}
			case Building.Refracting:
				{
					Direction firstDir = laserDirs[0];
					for (int i = 1; i < laserDirs.Count; i++) {
						if (laserDirs [i] != laserDirs [0])
							return true;
					}
					return false;
				}
			case Building.Redirecting:
				{
					foreach (var laserDir in laserDirs) {
						if (dir == Direction.Up || dir == Direction.Down) {
							if (laserDir == Direction.Right || laserDir == Direction.Left)
								return true;
						} else {
							if (laserDir == Direction.Up || laserDir == Direction.Down)
								return true;
						}
					}
					return false;
				}
			case Building.Resource:
				{
					Direction firstDir = laserDirs[0];
					for (int i = 1; i < laserDirs.Count; i++) {
						if (laserDirs [i] != laserDirs [0])
							return true;
					}
					return false;
				}
			}
		}
		return false;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GUI_Script : MonoBehaviour {

    public GameObject canvas;
    public static playerOneUI p1UI;
    public static playerTwoUI p2UI;
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

        UI.BlockPlaceCost.text = gridManager.theGrid.getCost(Building.Blocking, inputController.cursorP1.x, Player.PlayerOne).ToString();
        UI.ReflectPlaceCost.text = gridManager.theGrid.getCost(Building.Reflecting, inputController.cursorP1.x, Player.PlayerOne).ToString();
        UI.RefractPlaceCost.text = gridManager.theGrid.getCost(Building.Refracting, inputController.cursorP1.x, Player.PlayerOne).ToString();
        UI.RedirectPlaceCost.text = gridManager.theGrid.getCost(Building.Redirecting, inputController.cursorP1.x, Player.PlayerOne).ToString();
        UI.ResourcePlaceCost.text = gridManager.theGrid.getCost(Building.Resource, inputController.cursorP1.x, Player.PlayerOne).ToString();
    }
	
	// Update is called once per frame
	void Update () {
        UI.playerOneResource.text = (Mathf.Floor(gridManager.theGrid.getResourcesP1() * 2) / 2f).ToString("F1");
        UI.playerTwoResource.text = (Mathf.Floor(gridManager.theGrid.getResourcesP2() * 2) / 2f).ToString("F1");
    } 
}

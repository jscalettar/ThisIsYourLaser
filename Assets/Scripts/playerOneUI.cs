using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerOneUI : MonoBehaviour {
    public Text playerState;
    public Text playerHealth;
    
	// Use this for initialization
	void Start () {
        GameObject p1 = GameObject.Find("Base");
        Health healthScript = p1.GetComponent<Health>();
        playerState = GameObject.Find("playerOneState").GetComponent<Text>();
        playerHealth = GameObject.Find("playerOneHealth").GetComponent<Text>();

        playerHealth.text = healthScript.health.ToString();
        playerState.text = "Placing";

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("r")) {
            playerState.text = "swapping";
        }
        if (Input.GetKeyDown("t")) {
            playerState.text = "deleting";
        }
        if (Input.GetKeyDown("f")) {
            playerState.text = "placing";
        }
    }
}

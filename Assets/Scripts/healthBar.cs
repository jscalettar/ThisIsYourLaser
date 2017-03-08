using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour {
    
    private float maxValue; // Max player health

    //// Player 1 variables
    [SerializeField]
    private Image p1HealthFill;
    private float p1value;      // Player's health value
    private float p1fillAmount; // Player's health on a scale of 0 to 1

    //// Player 2 variables
    [SerializeField]
    private Image p2HealthFill;
    private float p2value;      // Player's health value
    private float p2fillAmount; // Player's health on a scale of 0 to 1

    // Use this for initialization
    void Start () {
        maxValue = 20;
	}
	
	// Update is called once per frame
	void Update () {
        HandleBarP1();
        HandleBarP2();
    }

    private void HandleBarP1() {
        p1value = gridManager.theGrid.baseHealthP1();   // get health
        p1fillAmount = Map(p1value, 0, maxValue, 0, 1); // convert health

        if (p1fillAmount != p1HealthFill.fillAmount) {
            p1HealthFill.fillAmount = p1fillAmount;     // update fill
        }
    }

    private void HandleBarP2() {
        p2value = gridManager.theGrid.baseHealthP2();   // get health
        p2fillAmount = Map(p2value, 0, maxValue, 0, 1); // convert health

        if (p2fillAmount != p2HealthFill.fillAmount) {
            p2HealthFill.fillAmount = p2fillAmount;     // update fill
        }
    }

    //convert current health to a value between 0 and 1
    private float Map(float value, float inMin, float inMax, float outMin, float outMax) {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseDamageEffect : MonoBehaviour {

    public Sprite p1DamageSprite;
    public Sprite p2DamageSprite;

    public Color colorP1;
    public Color colorP2;

    public float alphaMax = 1f;
    public float alphaMin = 0.1f;
    public float pulseSpeed = 1f;

    private GameObject p1DamageIndicator;
    private GameObject p2DamageIndicator;

    private float opacityP1 = 0f;
    private float opacityP2 = 0f;

    private bool flagP1 = true;
    private bool flagP2 = true;

    void Start () {
        if (p1DamageSprite != null && p1DamageIndicator == null) {
            p1DamageIndicator = new GameObject("p1DamageIndicator");
            p1DamageIndicator.transform.parent = gameObject.transform;
            p1DamageIndicator.transform.localPosition = new Vector3(0f, 2f, 0f);
            p1DamageIndicator.transform.localScale = Vector3.one * 11f;
            p1DamageIndicator.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
            p1DamageIndicator.AddComponent<SpriteRenderer>();
            p1DamageIndicator.GetComponent<SpriteRenderer>().sprite = p1DamageSprite;
            p1DamageIndicator.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            p1DamageIndicator.GetComponent<SpriteRenderer>().sortingOrder = 10;
            p1DamageIndicator.GetComponent<SpriteRenderer>().material.color = colorP1;
        }
        if (p2DamageSprite != null && p2DamageIndicator == null) {
            p2DamageIndicator = new GameObject("p1DamageIndicator");
            p2DamageIndicator.transform.parent = gameObject.transform;
            p2DamageIndicator.transform.localPosition = new Vector3(0f, 2f, 0f);
            p2DamageIndicator.transform.localScale = Vector3.one * 11f;
            p2DamageIndicator.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
            p2DamageIndicator.AddComponent<SpriteRenderer>();
            p2DamageIndicator.GetComponent<SpriteRenderer>().sprite = p2DamageSprite;
            p2DamageIndicator.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            p2DamageIndicator.GetComponent<SpriteRenderer>().sortingOrder = 10;
            p2DamageIndicator.GetComponent<SpriteRenderer>().material.color = colorP2;
        }
    }

    void Update () {
		
        // P1
        if (gridManager.theGrid.baseP1 != null && gridManager.theGrid.baseP1.GetComponent<buildingParameters>().takingDamage) {
            if (opacityP1 < alphaMin) opacityP1 += pulseSpeed * Time.deltaTime;
            else if (flagP1) {
                opacityP1 += pulseSpeed * Time.deltaTime;
                if (opacityP1 >= alphaMax) flagP1 = false;
            } else {
                opacityP1 -= pulseSpeed * Time.deltaTime;
                if (opacityP1 <= alphaMin) flagP1 = true;
            }
        } else if (opacityP1 != 0f) {
            opacityP1 = Mathf.Max(0f, opacityP1 - (pulseSpeed * Time.deltaTime));
        }
        if (p1DamageIndicator != null && p1DamageIndicator.GetComponent<SpriteRenderer>() != null)
            p1DamageIndicator.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacityP1);

        // P2
        if (gridManager.theGrid.baseP2 != null && gridManager.theGrid.baseP2.GetComponent<buildingParameters>().takingDamage) {
            if (opacityP2 < alphaMin) opacityP2 += pulseSpeed * Time.deltaTime;
            else if (flagP2) {
                opacityP2 += pulseSpeed * Time.deltaTime;
                if (opacityP2 >= alphaMax) flagP2 = false;
            } else {
                opacityP2 -= pulseSpeed * Time.deltaTime;
                if (opacityP2 <= alphaMin) flagP2 = true;
            }
        } else if (opacityP2 != 0f) {
            opacityP2 = Mathf.Max(0f, opacityP2 - (pulseSpeed * Time.deltaTime));
        }
        if (p2DamageIndicator != null && p2DamageIndicator.GetComponent<SpriteRenderer>() != null)
            p2DamageIndicator.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacityP2);

    }
}

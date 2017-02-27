using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingParameters : MonoBehaviour {
    [HideInInspector]
    public int x = -1;
    [HideInInspector]
    public int y = -1;
    [HideInInspector]
    public Player owner = Player.World;
    [HideInInspector]
    public float currentHP = 0;

    public float health = 5f;
    public float cost = 2f;
    public Sprite[] sprites;
    public float scale = 1f;
    public float placementTime = 1f;
    public float removalTime = 1f;
    public float destructionTime = 0.5f;

}

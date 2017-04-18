using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public float health = 50f; // how much health the base has

    // Damage taken depending on amount
    public void RemoveHealth(float amount) {
        health -= amount;
        // Destroy base if health is depleted
        if (health <= 0){
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageAnimScript : MonoBehaviour {

    public static void damageAnimSolver()
    {
        foreach (var item in gridManager.theGrid.prefabDictionary) {
            if (item.Value != null && item.Value.GetComponent<Animator>() != null && item.Value.GetComponent<Animator>().HasState(0, Animator.StringToHash("damage"))) {
                if (item.Value.GetComponent<buildingParameters>().takingDamage == false) {
                    if (item.Value.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("damage")) {
                        if (item.Value.GetComponent<buildingParameters>().currentHP > 0f)
                            item.Value.GetComponent<Animator>().Play("idle");               // If creature alive, switch to idle state
                        else item.Value.GetComponent<Animator>().Stop();                    // If creature dead, stop anim instead of going idle
                    }
                } else {
                    if (!item.Value.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("damage")) {
                        item.Value.GetComponent<Animator>().Play("damage");                 // If taking damage and not playing anim yet, play damage anim
                    }
                }
            }
        }
    }

    public static void damageAnimResetBool()
    {
        foreach (var item in gridManager.theGrid.prefabDictionary) {
            if (item.Value != null) {
                item.Value.GetComponent<buildingParameters>().takingDamage = false;
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageAnimScript : MonoBehaviour {

    public static void damageAnimSolver()
    {
        foreach (var item in gridManager.theGrid.prefabDictionary) {
            if (item.Value != null && item.Value.GetComponent<Animator>() != null) {

                // Determine which states are needed depending on creature direction -------------------------------------------------------
                string damageName = "";
                string idleName = "";

                Building type = item.Value.GetComponent<buildingParameters>().buildingType;

                if (type == Building.Redirecting) {
                    switch (item.Value.GetComponent<buildingParameters>().direction) {
                        case Direction.Up: damageName = "damage"; idleName = "idle"; break;
                        case Direction.Down: damageName = "damage"; idleName = "idle"; break;
                        case Direction.Left: damageName = "damage_R"; idleName = "idle_R"; break;
                        case Direction.Right: damageName = "damage_R"; idleName = "idle_R"; break;
                    }
                } else if (type == Building.Reflecting || type == Building.Resource) {
                    switch (item.Value.GetComponent<buildingParameters>().direction) {
                        case Direction.Up: damageName = "damage_U"; idleName = "idle_U"; break;
                        case Direction.Down: damageName = "damage_D"; idleName = "idle_D"; break;
                        case Direction.Left: damageName = "damage_L"; idleName = "idle_L"; break;
                        case Direction.Right: damageName = "damage_R"; idleName = "idle_R"; break;
                    }
                } else {
                    damageName = "damage"; idleName = "idle";
                }

                // Make sure animator controller actually contains state, else skip it
                if (!item.Value.GetComponent<Animator>().HasState(0, Animator.StringToHash(idleName))) continue;
                if (!item.Value.GetComponent<Animator>().HasState(0, Animator.StringToHash(damageName))) continue;
                // -------------------------------------------------------------------------------------------------------------------------

                if (item.Value.GetComponent<buildingParameters>().takingDamage == false) {
                    // If not taking damage but not in idle anim state...
                    if (!item.Value.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(idleName)) {
                        if (item.Value.GetComponent<buildingParameters>().currentHP > 0f)
                            item.Value.GetComponent<Animator>().Play(idleName);             // If creature alive, switch to idle state
                        else item.Value.GetComponent<Animator>().Stop();                    // If creature dead, stop anim instead of going idle
                    }

                } else {
                    // If taking damage but not in damage anim state...
                    if (!item.Value.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(damageName)) {
                        item.Value.GetComponent<Animator>().Play(damageName);               // If taking damage and not playing anim yet, play damage anim
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

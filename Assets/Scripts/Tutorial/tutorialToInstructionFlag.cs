using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;


[CreateAssetMenu(menuName = "tutorialToInstructionFlag")]
public class tutorialToInstructionFlag : ScriptableObject {

    private static tutorialToInstructionFlag Instance;
    public static tutorialToInstructionFlag instance {
        get {
            if (!Instance) Instance = Resources.Load("tutorialToInstructionFlag") as tutorialToInstructionFlag;
            return Instance;
        } }

    public bool flag;
}

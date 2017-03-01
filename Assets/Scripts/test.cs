using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindowsInput;
using System;

public class test : MonoBehaviour {
    List<Action> test1 = new List<Action>();
    List<Action> test2 = new List<Action>();
    List<Action> test3 = new List<Action>();
    List<Action> test4 = new List<Action>();
    List<Action> test5 = new List<Action>();
    List<Action> test6= new List<Action>();
    List<Action> test7= new List<Action>();
    List<Action> test8 = new List<Action>();

    // Use this for initialization
    void Start () {
        //Test 1 - 
        test1.Add( ()=>InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
        test1.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
        test1.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));

        //Test 2 - Test for base + laser placement
        
        //P1 Place Base
        test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
        test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
        test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
        
        //P2 Place Base
        test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
        test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
        test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));

        //P1 Place Laser
        test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
        test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
        test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
        test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));

        //P2 Place laser
        test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
        test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
        test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
        test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));

        //Test 3 - Test for placing every block
        test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_1));
        test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_2));
        test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_3));
    }
	
	// Update is called once per frame
	void Update () {
        getTest();	
	}

	IEnumerator doTest(List<Action> keyPressList) {
		foreach(var keypress in keyPressList) {
            keypress();
            yield return new WaitForSeconds(1);
        }
        yield break;
	}

    void getTest() {
        if (Input.GetKeyDown("[1]")){
            StartCoroutine(doTest(test1));
        }
        if (Input.GetKeyDown("[2]")) {
            StartCoroutine(doTest(test2));
        }
        if (Input.GetKeyDown("[3]")) {
            StartCoroutine(doTest(test3));
        }
    }
}

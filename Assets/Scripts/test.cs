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

        //Test 3 - Test for placing reflect in all directions
            //P1 Place Base
            test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
            test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
            test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));

            //P2 Place Base
            test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
            test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
            test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));

            //P1 Place Laser
            test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
            test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
            test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
            test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));

            //P2 Place laser
            test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
            test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
            test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
            test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));

            //P1 Place reflect in all directions
            test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D));
            test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D));
            test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
            test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
            test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
            
            

    }
	
	// Update is called once per frame
	void Update () {
        getTest();	
	}

	IEnumerator doTest(List<Action> keyPressList) {
		foreach(var keypress in keyPressList) {
            keypress();
            yield return new WaitForSeconds(.1f);
        }
        yield return new WaitForSeconds(5);
        Application.CaptureScreenshot("testScreenshots/test.png");
        yield break;
	}

    void getTest() {
        if (Input.GetKeyDown("[1]")){
            print("Starting test");
            StartCoroutine(doTest(test1));
        }
        if (Input.GetKeyDown("[2]")) {
            print("Starting base/laser placement test");
            StartCoroutine(doTest(test2));
          
        }
        if (Input.GetKeyDown("[3]")) {
            print("Starting reflect block placement test");
            StartCoroutine(doTest(test3));
        }
    }
}

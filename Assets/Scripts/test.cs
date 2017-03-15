using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindowsInput;
using System;

public class test : MonoBehaviour {
    public float sec;
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

                //Place Base
                test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));

                // Place Laser
                test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test2.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));

        //Test 3 - Test for all block effects
                //Place Base
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));

                // Place Laser
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));





         // Place Reflect
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_J));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_J));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_2));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_8));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_A));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_L));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));

        // Place Refract
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_3));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_9));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_J));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_A));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_L));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));

        //Place Redirect
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_4));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_0));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_A));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_L));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_A));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_L));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_A));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_L));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));

        //Place Resource
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_5));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.OEM_MINUS));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_J));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_J));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_J));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));

        //Place Blocking
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_A));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_L));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_1));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_7));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test3.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
        //Test 4 - Testing Building Destruction
                //Place Base
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));

        // Place Laser
            // Place Laser
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));



            //Place blocking on weakside
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_J));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_A));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_L));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));

            //Placing reflecting
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_2));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_8));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_J));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_A));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_L));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));

            //Placing Resource
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_5));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.OEM_MINUS));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_J));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_J));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_J));

            //Placing Redirecting
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_2));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_0));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_L));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_L));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test4.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));



            //Test 5 - Remove test
                //Place Base
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));

                // Place Laser
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                
                // Place blocks
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_J));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_J));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_2));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_8));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_A));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_L));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));

                 //Remove
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_R));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_P));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_R));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_P));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_R));
                test5.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_P));

        //Test 6 - Move test
            //Place Base
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));

            // Place Laser
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));

            // Place blocks
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_J));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_J));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_2));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_8));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_A));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_L));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));

            // Move Blocks
                //Filler
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I));
                //Actual move
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_U));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_Q));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_Q));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_U));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W));
            test6.Add(() => InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K));




    }

    // Update is called once per frame
    void Update () {
        getTest();
        if (Input.GetKeyDown("space")) { Application.CaptureScreenshot("testScreenshots/test.png"); }
        
    }

	IEnumerator doTest(List<Action> keyPressList) {
		foreach(var keypress in keyPressList) {
            keypress();
            yield return new WaitForSeconds(sec);
        }
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
            print("Starting special building interaction test");
            StartCoroutine(doTest(test3));
        }
        if (Input.GetKeyDown("[4]")) {
            print("Starting building destruction test");
            StartCoroutine(doTest(test4));
        }
        if (Input.GetKeyDown("[5]")) {
            print("Starting building remove test");
            StartCoroutine(doTest(test5));
        }
        if (Input.GetKeyDown("[6]")) {
            print("Starting building move test");
            StartCoroutine(doTest(test6));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindowsInput;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (test1 ());
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}

	IEnumerator test1() {
		InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W);
		yield return new WaitForSeconds(1);
		InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S);
		yield return new WaitForSeconds(1);
		InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W);
		yield return new WaitForSeconds(1);
		InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E);
		yield return new WaitForSeconds(1);
		InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S);
		yield return new WaitForSeconds(1);
		InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W);
		yield return new WaitForSeconds(1);
		InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W);
		yield return new WaitForSeconds(1);
		InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W);
		yield return new WaitForSeconds(1);
		InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E);
		yield return new WaitForSeconds(1);
		InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S);
		yield break;

	}
}

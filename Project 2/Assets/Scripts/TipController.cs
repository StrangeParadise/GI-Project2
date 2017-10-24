using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TipController : MonoBehaviour {

	public GameObject ball;
	public GameObject tipCanvas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		showTip (generateTip (ball.transform.position));
	}
	public string generateTip(Vector3 current) {
		if (SceneManager.GetActiveScene ().name == "Level1") {
			if (inRange(current, new Vector3 (0.0f , 0.0f, 0.0f), 3)) {
				return("Press arrow keys or WASD to move the ball");
			}
			if (inRange(current, new Vector3 (0.0f , 0.0f, 16.0f), 3)) {
				return("Press Q and E to change the orintation of the camera");
			}
			if (inRange(current, new Vector3 (8.0f , 0.5f, 19.0f), 3)) {
				return("Press Left SHIFT to accelerate");
			}
			if (inRange(current, new Vector3 (48.0f , 0.5f, -30.0f), 3)) {
				return("Look! The Teleporter is ahead! Get in there!");
			}
		}
		return null;
	}

	private bool inRange(Vector3 current, Vector3 target, float offset) {
		if (current.x < target.x + offset && current.x > target.x - offset 
			&& current.z < target.z + offset && current.z > target.z - offset) {
			return true;
		}
		return false;
	}

	public void showTip(string tip) {
		tipCanvas.SetActive (true);
		tipCanvas.GetComponentInChildren<Text> ().text = tip;
	}
}

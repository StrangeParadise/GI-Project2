﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour {

    // Variables
    public GameObject player;

	// The orintation of the camera
    public int orin;

	public float rotation_speed;

    private Transform view;
    private enum CamPos { Back, Right, Front, Left };

	private Vector3 offset;
	private bool isClockwise;
	private bool isAntiClockwise;
	private float deltaCounter;

    // Use this for initialization
    void Start () {
        view = this.transform;
        orin = (int)CamPos.Back;
		offset = player.transform.position - view.position;
		isClockwise = false;
		isAntiClockwise = false;
		deltaCounter = 0.0f;
		if (SceneManager.GetActiveScene ().name == "Level2") {
			orin = 3;
			view.RotateAround(player.transform.position, Vector3.up, 90.0f);
		}
    }
	
	// Update is called once per frame
	void Update () {
		view.position = player.transform.position - offset;

		if (!isClockwise && !isAntiClockwise) {

			if (Input.GetKeyDown (KeyCode.Q)) {
				if (orin < 3) {
					orin++;
				} else {
					orin = 0;
				}

				isAntiClockwise = true;
				deltaCounter = 0.0f;
			}

			if (Input.GetKeyDown (KeyCode.E)) {
				if (orin > 0) {
					orin--;
				} else {
					orin = 3;
				}

				isClockwise = true;
				deltaCounter = 0.0f;
			}
		}

		if (isClockwise)
		{
			view.RotateAround(player.transform.position, Vector3.up, Time.deltaTime * rotation_speed);
			deltaCounter += Time.deltaTime * rotation_speed;
		}
		if (isAntiClockwise) {
			view.RotateAround(player.transform.position, Vector3.up, -Time.deltaTime * rotation_speed);
			deltaCounter += Time.deltaTime * rotation_speed;
		}

		if (deltaCounter >= 90.0f) {
			view.RotateAround(player.transform.position, Vector3.up, isClockwise?(90.0f - deltaCounter):(deltaCounter - 90.0f));
			isClockwise = false;
			isAntiClockwise = false;
			deltaCounter = 0.0f;
		}

		offset = player.transform.position - view.position;
    }
}

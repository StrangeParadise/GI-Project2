﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour {

    // Variables
    public float acceleration;
    public CameraController script;

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
        move(script.orin);
	}

    private void move(int orin) {
        switch (orin) {
            case 0:
                if (Input.GetKey(KeyCode.W))
                {
                    moveFwd();
                }
                if (Input.GetKey(KeyCode.S))
                {
                    moveBack();
                }
                if (Input.GetKey(KeyCode.A))
                {
                    moveLeft();
                }
                if (Input.GetKey(KeyCode.D))
                {
                    moveRight();
                }
                break;
            case 1:
                if (Input.GetKey(KeyCode.W))
                {
                    moveLeft();
                }
                if (Input.GetKey(KeyCode.S))
                {
                    moveRight();
                }
                if (Input.GetKey(KeyCode.A))
                {
                    moveBack();
                }
                if (Input.GetKey(KeyCode.D))
                {
                    moveFwd();
                }
                break;
            case 2:
                if (Input.GetKey(KeyCode.W))
                {
                    moveBack();
                }
                if (Input.GetKey(KeyCode.S))
                {
                    moveFwd();
                }
                if (Input.GetKey(KeyCode.A))
                {
                    moveRight();
                }
                if (Input.GetKey(KeyCode.D))
                {
                    moveLeft();
                }
                break;
            case 3:
                if (Input.GetKey(KeyCode.W))
                {
                    moveRight();
                }
                if (Input.GetKey(KeyCode.S))
                {
                    moveLeft();
                }
                if (Input.GetKey(KeyCode.A))
                {
                    moveFwd();
                }
                if (Input.GetKey(KeyCode.D))
                {
                    moveBack();
                }
                break;
            default:
                break;
        }
    }

    private void moveFwd() {
		rb.AddForce (Vector3.forward * acceleration);
    }
    private void moveBack()
    {
		rb.AddForce (Vector3.back * acceleration);
    }
    private void moveLeft()
    {
		rb.AddForce (Vector3.left * acceleration);
    }
    private void moveRight()
    {
		rb.AddForce (Vector3.right * acceleration);
    }

}

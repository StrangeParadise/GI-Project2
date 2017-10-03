using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    // Variables
    public float acceleration;
    public CameraController script;
    public float accConstant;

    private Rigidbody rb;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody>();
        respawn();
    }

    // Update is called once per frame
    void Update() {
        if (transform.position.y < -10) {
            respawn();
        }
        if (Input.GetKey(KeyCode.LeftShift)) {
            accelerate();
        }
        else {
            move(script.orin);
        }
    }

    private void move(int orin) {
        switch (orin) {
            case 0:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                    moveFwd();
                }
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                    moveBack();
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                    moveLeft();
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                    moveRight();
                }
                break;
            case 1:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                    moveLeft();
                }
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                    moveRight();
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                    moveBack();
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                    moveFwd();
                }
                break;
            case 2:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                    moveBack();
                }
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                    moveFwd();
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                    moveRight();
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                    moveLeft();
                }
                break;
            case 3:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                    moveRight();
                }
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                    moveLeft();
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                    moveFwd();
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                    moveBack();
                }
                break;
            default:
                break;
        }
    }

    private void moveFwd() {
        rb.AddForce(Vector3.forward * acceleration);
    }
    private void moveBack() {
        rb.AddForce(Vector3.back * acceleration);
    }
    private void moveLeft() {
        rb.AddForce(Vector3.left * acceleration);
    }
    private void moveRight() {
        rb.AddForce(Vector3.right * acceleration);
    }
    private void respawn() {
        transform.position = new Vector3(0, 2, 0);
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
    }
    private void accelerate() {
        acceleration *= accConstant;
        move(script.orin);
        acceleration /= accConstant;
    }
    private void changeBall() {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour {

    // Variables
    public float speed;
    public CameraController script;

    private Transform playerTrans;

	// Use this for initialization
	void Start () {
        playerTrans = this.transform;
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
        playerTrans.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    private void moveBack()
    {
        playerTrans.Translate(Vector3.back * speed * Time.deltaTime);
    }
    private void moveLeft()
    {
        playerTrans.Translate(Vector3.left * speed * Time.deltaTime);
    }
    private void moveRight()
    {
        playerTrans.Translate(Vector3.right * speed * Time.deltaTime);
    }

}

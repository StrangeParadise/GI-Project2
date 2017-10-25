using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour {

    // Variables
    public float acceleration;
    public float accConstant;

    public Camera view;

    public GameObject woodBall;
    public GameObject stoneBall;
    public GameObject metalBall;
	public GameObject fireBall;

	// The life that the player have
    public int lifeCount;

    public GameProcessController gameProcess;

    // The parameter to multiply to the force for different types of ball
    private float Ka = 1.0f;
    private Rigidbody rb;
    private CameraController script;

    private Vector3 destination;
    private float offset;

	private int ballType = 0;

    // Use this for initialization
    void Start() {
        script = view.GetComponent<CameraController>();
        rb = GetComponent<Rigidbody>();
        lifeCount = 3;

		// Set the destination for each level
		if (SceneManager.GetActiveScene ().name == "Level1") {
			destination = new Vector3 (16.8f, -0.15f, -28.0f);
		} else if (SceneManager.GetActiveScene ().name == "Level2") {
			destination = new Vector3(59.2f, 0.85f, 0.0f);
		}
        
        offset = 1.0f;
    }

    // Update is called once per frame
    void Update() {

		// Always check if the player is win
		if (checkWin (destination, offset)) {
			stablize ();
			return;
		}

		// Press ESC to quit the game
        if (Input.GetKey(KeyCode.Escape)) {
            gameProcess.quit();
        }

		// Automatically respawn if a player has life
        if (transform.position.y < -10 && lifeCount > 0)
        {
            respawn();
            lifeCount--;
        }

		// If the player is dead and there is no life, then game over
        else if (lifeCount == 0 && transform.position.y < -10) {
            gameProcess.gameOver();
        }

		// Press C to change the ball to other type
        if (Input.GetKeyDown(KeyCode.C) && isStable()) {
			ballType++;
			if (ballType > 3) {
				ballType = 0;
			}
            changeBall(ballType);
        }

		// Press Left Shift to accelerate the ball
        if (Input.GetKey(KeyCode.LeftShift)) {
            accelerate();
        }
        else {
            move(script.orin);
        }
    }

    // Set the parameter of the force
    public void setKa(float Ka) {
        this.Ka = Ka;
    }

	// Move the ball
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

	// Add force to the ball
    private void moveFwd() {
        rb.AddForce(Vector3.forward * acceleration * Ka);
    }
    private void moveBack() {
        rb.AddForce(Vector3.back * acceleration * Ka);
    }
    private void moveLeft() {
        rb.AddForce(Vector3.left * acceleration * Ka);
    }
    private void moveRight() {
        rb.AddForce(Vector3.right * acceleration * Ka);
    }
    private void respawn() {
        transform.position = new Vector3(0, 2, 0);
        stablize();
    }

	// Acceleration of the ball
    private void accelerate() {
        acceleration *= accConstant;
        move(script.orin);
        acceleration /= accConstant;
    }

	// Change ball function
    private void changeBall(int ballType) {
        GameObject ball;
		if (ballType == 0) {
			Ka = 1.0f;
			ball = woodBall;
		} else if (ballType == 1) {
			Ka = 5.0f;
			ball = stoneBall;
		} else if (ballType == 2) {
			Ka = 3.0f;
			ball = metalBall;
		} else {
			ball = fireBall;
		}

        GameObject ballClone = (GameObject)Instantiate(ball, transform.position, transform.rotation);
		ballClone.tag = "Player";

        ballClone.GetComponent<BallController>().script = script;
        ballClone.GetComponent<BallController>().woodBall = woodBall;
        ballClone.GetComponent<BallController>().stoneBall = stoneBall;
        ballClone.GetComponent<BallController>().metalBall = metalBall;
		ballClone.GetComponent<BallController>().fireBall = fireBall;
        ballClone.GetComponent<BallController>().view = view;
        ballClone.GetComponent<BallController>().acceleration = acceleration;
        ballClone.GetComponent<BallController>().accConstant = accConstant;
        ballClone.GetComponent<BallController>().gameProcess = gameProcess;
        ballClone.GetComponent<BallController>().lifeCount = lifeCount;
        ballClone.GetComponent<BallController>().setKa(this.Ka);
		ballClone.GetComponent<BallController>().ballType = ballType;
        print("lifeCount == " + lifeCount);

        Destroy(gameObject);
        script.player = ballClone;
    }

	// Stablize the bal
    private void stablize() {
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
    }

	// Check if the ball is stable
    private bool isStable() {
        return rb.angularVelocity.magnitude <= 2 && rb.velocity.magnitude <= 2;
    }

	// Check if the ball is in the area of the destination
    private bool checkWin(Vector3 destination, float offset) {
        if (transform.position.x < destination.x + offset && transform.position.x > destination.x - offset 
            && transform.position.z < destination.z + offset && transform.position.z > destination.z - offset && transform.position.y > destination.y + 0.4) {
            gameProcess.gameWin();
			return true;
        }
		return false;
    }

	// Setters
    public void setDestination(Vector3 destination)
    {
        this.destination = destination;
    }
    public void setOffset(float offset)
    {
        this.offset = offset;
    }
}

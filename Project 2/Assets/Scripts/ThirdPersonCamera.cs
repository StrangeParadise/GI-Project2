using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    public float sensitivity;

    public float distance;

    public GameObject player;

    private float minDistance;
    private float maxDistance;

    private Transform playerTrans;
    private Transform viewTrans;

    private float mouseX;
    private float mouseY;
    private Vector3 currentRotation;
    private float rotationSmoonthTime;
    private Vector3 rotationSmoonthVelocity;

    // Use this for initialization
    void Start () {
        sensitivity = 1f;

        playerTrans = player.transform;
        viewTrans = this.transform;

        minDistance = 2;
        maxDistance = 20;

        rotationSmoonthTime = 0.2f;
	}
	
	// Update is called once per frame
	void Update () {

        // View control by mouse
        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * sensitivity;
        Vector2 mouseDir = new Vector2(mouseX, mouseY).normalized;

        mouseY = Mathf.Clamp(mouseY, 2, 40);
        if (mouseDir != Vector2.zero) {
            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(mouseY, mouseX, 0), ref rotationSmoonthVelocity, rotationSmoonthTime);
            viewTrans.eulerAngles = currentRotation;
            viewTrans.position = playerTrans.position + new Vector3(0, 3, 0) - viewTrans.forward * distance;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            if (distance > minDistance) {
                distance -= 0.5f;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            if (distance < maxDistance)
            {
                distance += 0.5f;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Variables
    public float speed;

    private Transform playerTrans;
    private Transform viewTrans;

    private float moveLR;
    private float moveFB;

    private Vector3 currentRotation;
    private float rotationSmoonthTime;
    private float rotationSmoonthVelocity;

    private Animator animator;

    // Functions
    void Start () {
        speed = 4f;
        playerTrans = this.transform;
        viewTrans = Camera.main.transform;
        rotationSmoonthTime = 0.1f;

        animator = GetComponent<Animator>();

    }

	void Update () {
        moveLR = Input.GetAxis("Horizontal") * speed;
        moveFB = Input.GetAxis("Vertical") * speed;
        Vector2 inputDir = new Vector2(moveLR, moveFB).normalized;
        if (inputDir != Vector2.zero)
        {
            float tagetRotationDegree = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + viewTrans.eulerAngles.y;
            currentRotation = Vector3.up * Mathf.SmoothDampAngle(playerTrans.eulerAngles.y, tagetRotationDegree, ref rotationSmoonthVelocity, rotationSmoonthTime);
            playerTrans.eulerAngles = currentRotation;
            playerTrans.localPosition += speed * playerTrans.forward * Time.deltaTime;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                playerTrans.localPosition += speed * playerTrans.forward * Time.deltaTime;
                animator.SetBool("run", true);
            }
            else
            {
                animator.SetBool("run", false);
                animator.SetBool("walk", true);
            }
        }
        else {
            animator.SetBool("walk", false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("jump", true);
        }
        else {
            animator.SetBool("jump", false);
        }
        if (Input.GetKeyDown(KeyCode.C)) {
            animator.SetBool("slide", true);
        }
        else
        {
            animator.SetBool("slide", false);
        }
    }
}

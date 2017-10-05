using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // Variables
    public GameObject player;
    public int orin;

    private Transform view;
    private Transform playerTrans;
    private enum CamPos { Back, Right, Front, Left };

	private Vector3 offset;

    // Use this for initialization
    void Start () {
        view = this.transform;
        playerTrans = player.transform;
        orin = (int)CamPos.Back;
		offset = playerTrans.position - view.position;
    }
	
	// Update is called once per frame
	void Update () {
		view.position = playerTrans.position - offset;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (orin < 3)
            {
                orin++;
            }
            else
            {
                orin = 0;
            }
            camRotate(false);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (orin > 0)
            {
                orin--;
            }
            else
            {
                orin = 3;
            }
            camRotate(true);
        }
    }

    private void camRotate(bool isClockwise) {
        if (isClockwise)
        {
			view.RotateAround(playerTrans.position, Vector3.up, 90);
        }
        else {
			view.RotateAround(playerTrans.position, Vector3.up, -90);
        }
		offset = playerTrans.position - view.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // Variables
    public GameObject player;
    public int orin;

    private Transform view;
    private enum CamPos { Back, Right, Front, Left };

	private Vector3 offset;

    // Use this for initialization
    void Start () {
        view = this.transform;
        orin = (int)CamPos.Back;
		offset = player.transform.position - view.position;
    }
	
	// Update is called once per frame
	void Update () {
		view.position = player.transform.position - offset;
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
			view.RotateAround(player.transform.position, Vector3.up, 90);
        }
        else {
			view.RotateAround(player.transform.position, Vector3.up, -90);
        }
		offset = player.transform.position - view.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // Variables
    public GameObject player;
    public int orin;
	public float rotation_speed;

    private Transform view;
    private enum CamPos { Back, Right, Front, Left };

	private Vector3 offset;
	private bool isClockwise;
	private bool isAntiClockwise;
	private float delta_counter;

    // Use this for initialization
    void Start () {
        view = this.transform;
        orin = (int)CamPos.Back;
		offset = player.transform.position - view.position;
		isClockwise = false;
		isAntiClockwise = false;
		delta_counter = 0.0f;
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

			isAntiClockwise = true;
			delta_counter = 0.0f;
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

			isClockwise = true;
			delta_counter = 0.0f;
        }

		if (isClockwise)
		{
			view.RotateAround(player.transform.position, Vector3.up, Time.deltaTime * rotation_speed);
			delta_counter += Time.deltaTime * rotation_speed;
		}
		if (isAntiClockwise) {
			view.RotateAround(player.transform.position, Vector3.up, -Time.deltaTime * rotation_speed);
			delta_counter += Time.deltaTime * rotation_speed;
		}

		if (delta_counter >= 90.0f) {
			view.RotateAround(player.transform.position, Vector3.up, isClockwise?(90.0f - delta_counter):(delta_counter - 90.0f));
			isClockwise = false;
			isAntiClockwise = false;
			delta_counter = 0.0f;
		}

		offset = player.transform.position - view.position;


    }
		
}

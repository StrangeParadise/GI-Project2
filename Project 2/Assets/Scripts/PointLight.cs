using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLight : MonoBehaviour {

    // The color of the point light
    public Color color;
    // Height of the lighr source
    public float height;

    private float centreX, centreZ, centreY;

    // Use this for initialization
    void Start() {

        this.transform.position = new Vector3(0, height, 0);
    }

    // Update is called once per frame
    void Update() {
            
    }

    // Get the world position of the light
    public Vector3 GetWorldPosition() {
        return this.transform.position;
    }
}



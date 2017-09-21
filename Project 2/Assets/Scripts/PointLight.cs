using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLight : MonoBehaviour {

    // The color of the point light
    public Color color;
    // Rotation speed
    public float rotateSpeed;
    // Rotation radius
    public float radius;

    private float centreX, centreZ, centreY;

    // Use this for initialization
    void Start() {

        centreX = 0;
        centreY = 0;
        centreZ = 0;

        //this.transform.position = new Vector3(centreX + Mathf.Sqrt(2) * radius, centreY, centreZ + Mathf.Sqrt(2) * radius);
    }

    // Update is called once per frame
    void Update() {
            
    }

    // Get the world position of the light
    public Vector3 GetWorldPosition() {
        return this.transform.position;
    }
}



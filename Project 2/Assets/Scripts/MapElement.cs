using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapElement : MonoBehaviour {

    public float textureZoom;

	// Use this for initialization
	void Start () {

        MeshFilter meshFilter = this.gameObject.GetComponent<MeshFilter>();
        Vector2[] uv = meshFilter.mesh.uv;

        for (int i = 0; i < uv.Length; i++) {
            uv[i] = uv[i] * textureZoom;
        }

        meshFilter.mesh.uv = uv;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;using System.Collections.Generic;using UnityEngine;public class GameMapGenerator : MonoBehaviour {


    // The shader for the game map
    public Shader shader;
    // The light object
    public PointLight pointLight;

    // Heightmap <temporarily> storing the height information of the game map.
    private float[,] heightmap;
    // Use this for initialization
    void Start() {        CreateHeightMap(1, 0,0);        
        // Add a MeshRenderer component. This component actually renders the mesh that
        // is defined by the MeshFilter component.
        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();        renderer.material.shader = shader;    }

    // Update is called once per frame
    void Update() {        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

        // Pass updated light positions to shader.
        renderer.material.SetColor("_PointLightColor", this.pointLight.color);        renderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());    }

    // Generate the vector of a vertex.
    public Vector3 GetVertex(int i, int j) {        return new Vector3((float)i, 0.0f, (float)j);    }


    // Calculate and generate the vertex normal for a vertex.
    public Vector3 GetVertexNormal(int i, int j) {        Vector3 norm = new Vector3(0.0f, 1.0f, 0.0f);

        // Ignore vertex normals for those points on the edges,
        //  assign them to be (0, 1, 0) (upright).
        // Calculatate normals vector for 4 adjacent surfaces, 
        //  and normalize the sum of those four vectors.
        if (i != 0 && j != 0) {            Vector3 norm1 = Vector3.Cross((GetVertex(i, j - 1) - GetVertex(i, j)),                (GetVertex(i - 1, j) - GetVertex(i, j))).normalized;            Vector3 norm2 = Vector3.Cross((GetVertex(i + 1, j) - GetVertex(i, j)),                (GetVertex(i, j - 1) - GetVertex(i, j))).normalized;            Vector3 norm3 = Vector3.Cross((GetVertex(i, j + 1) - GetVertex(i, j)),                (GetVertex(i + 1, j) - GetVertex(i, j))).normalized;            Vector3 norm4 = Vector3.Cross((GetVertex(i - 1, j) - GetVertex(i, j)),                (GetVertex(i, j + 1) - GetVertex(i, j))).normalized;            norm = (norm1 + norm2 + norm3 + norm4).normalized;        }        return norm;    }    void CreateHeightMap(int n, int x, int z) {
        switch (n) {
            case 1:
                heightmap = new float[20, 20];
                break;
            default:
                heightmap = null;
                break;
        }
        
        // Create the terrain mesh from the generated heightmap.
        CreateMapMesh(1, heightmap);
    }

    // Create the terrain mesh from the generated heightmap.
    Mesh CreateMapMesh(int index, float[,] heightmap) {

        // Add MeshFilter and MeshCollider components to every element of the game map.
        MeshFilter terrainMesh = this.gameObject.AddComponent<MeshFilter>();
        MeshCollider col = this.gameObject.AddComponent<MeshCollider>();

        // Create the new mesh for the terrain.
        Mesh m = new Mesh();        m.name = "GameMapElement" + index.ToString();        m.Clear();

        Debug.Log("!!!!");

        Vector3[] vertices = new Vector3[6 * (heightmap.GetLength(0) - 1) * (heightmap.GetLength(1) - 1)];        Color[] colors = new Color[vertices.Length];        int[] triangles = new int[vertices.Length];        Vector3[] normals = new Vector3[vertices.Length];        int i, j, k = 0;

        // Make triangles for the terrain, and assign vertex normals.
        // For each square, there are two triangles to be drawn.
        //
        //          i + 0     i + 1
        //
        // j + 0      X---------X
        //            |         |
        //            |         |
        // j + 1      X---------X
        //
        //
        for (i = 0; i < heightmap.GetLength(0) - 1; i++) {            for (j = 0; j < heightmap.GetLength(1) - 1; j++) {

                // First triangle
                vertices[k + 0] = GetVertex(i, j);                normals[k + 0] = GetVertexNormal(i, j);                vertices[k + 1] = GetVertex(i, j + 1);                normals[k + 1] = GetVertexNormal(i, j + 1);                vertices[k + 2] = GetVertex(i + 1, j + 1);                normals[k + 2] = GetVertexNormal(i + 1, j + 1);

                // Second Triangle
                vertices[k + 3] = GetVertex(i, j);                normals[k + 3] = GetVertexNormal(i, j);                vertices[k + 4] = GetVertex(i + 1, j + 1);                normals[k + 4] = GetVertexNormal(i + 1, j + 1);                vertices[k + 5] = GetVertex(i + 1, j);                normals[k + 5] = GetVertexNormal(i + 1, j);                k += 6;            }        }

        // Assign colors according to the height of the terrain.
        for (k = 0; k < vertices.Length; k++) {            colors[k] = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);        }


        // Assign triangle indices.
        for (k = 0; k < vertices.Length; k++) {            triangles[k] = k;        }


        // Assign the calculated values to the mesh.
        m.vertices = vertices;        m.colors = colors;        m.triangles = triangles;        m.normals = normals;        terrainMesh.mesh = m;        col.sharedMesh = terrainMesh.mesh;        return m;    }}
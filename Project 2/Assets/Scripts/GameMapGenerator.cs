using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMapGenerator : MonoBehaviour {


    // Size of the terrain (length and width are set to be the same)
    public int terrainSize;
    // Random range of the generated terrain
    public float divRange;
    // Random range of the four corners
    public float initRange;
    // Base height of the generated terrain
    public float baseHeight;
    // The threshold height of the first and the second classes.
    public float thresholdOne;
    // The threshold height of the second and the third classes.
    public float thresholdTwo;
    // Smoothness of the generated terrain
    public float smoothness;
    // The shader for the terrain
    public Shader shader;
    // The light object
    //public PointLight pointLight;

    private float[,] heightmap;
    private float maxHeight, minHeight;

    // Use this for initialization
    void Start() {

        // Initialize the max and min height.
        maxHeight = -divRange;
        minHeight = divRange;

        // Add a MeshFilter component to this entity. This essentially comprises of a
        // mesh definition, which in this example is a collection of vertices, colours 
        // and triangles (groups of three vertices). 
        MeshFilter terrainMesh = this.gameObject.AddComponent<MeshFilter>();

        // Heightmap storing the height information of the terrain.
        heightmap = new float[terrainSize, terrainSize];

        // Initialize the random seed so that the terrain would not be the same.
        this.SetSeed(Random.Range(int.MinValue, int.MaxValue));

        // Run Diamond Square Algorithm to generate heightmap.
        this.DiamondSquare();

        // Create the terrain mesh from the generated heightmap.
        terrainMesh.mesh = this.CreateTerrainMesh();

        var col = GetComponent<MeshCollider>();
        col.sharedMesh = terrainMesh.mesh;

        // Add a MeshRenderer component. This component actually renders the mesh that
        // is defined by the MeshFilter component.
        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        renderer.material.shader = shader;
    }

    // Update is called once per frame
    void Update() {
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

        // Pass updated light positions to shader.
        //renderer.material.SetColor("_PointLightColor", this.pointLight.color);
        //renderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());
    }

    // Set the random seed.
    public void SetSeed(int seed) {

        Random.InitState(seed);
    }

    // Get the value of terrain size.
    public int GetTerrainSize() {

        return this.terrainSize;
    }

    // Get the height of a particular point.
    public float GetTerrainHeight(int i, int j) {

        if (i >= terrainSize || j >= terrainSize || i < 0 || j < 0)
            return 0.0f;
        else
            return heightmap[i, j];
    }

    // Generate the vector of a vertex.
    public Vector3 GetVertex(int i, int j) {

        return new Vector3((float)i, GetTerrainHeight(i, j), (float)j);
    }


    // Calculate and generate the vertex normal for a vertex.
    public Vector3 GetVertexNormal(int i, int j) {

        Vector3 norm = new Vector3(0.0f, 1.0f, 0.0f);

        // Ignore vertex normals for those points on the edges,
        //  assign them to be (0, 1, 0) (upright).
        // Calculatate normals vector for 4 adjacent surfaces, 
        //  and normalize the sum of those four vectors.
        if (i != 0 && j != 0) {
            Vector3 norm1 = Vector3.Cross((GetVertex(i, j - 1) - GetVertex(i, j)),
                (GetVertex(i - 1, j) - GetVertex(i, j))).normalized;
            Vector3 norm2 = Vector3.Cross((GetVertex(i + 1, j) - GetVertex(i, j)),
                (GetVertex(i, j - 1) - GetVertex(i, j))).normalized;
            Vector3 norm3 = Vector3.Cross((GetVertex(i, j + 1) - GetVertex(i, j)),
                (GetVertex(i + 1, j) - GetVertex(i, j))).normalized;
            Vector3 norm4 = Vector3.Cross((GetVertex(i - 1, j) - GetVertex(i, j)),
                (GetVertex(i, j + 1) - GetVertex(i, j))).normalized;

            norm = (norm1 + norm2 + norm3 + norm4).normalized;
        }

        return norm;
    }

    // Generate random heightmap using Diamond Square Algorithm.
    public void DiamondSquare() {

        float average;
        int oneStep, halfStep, i, j;

        // Initialize the corner values.
        heightmap[0, 0] = Random.value * initRange * 2.0f - initRange;
        heightmap[0, terrainSize - 1] = Random.value * initRange * 2.0f - initRange;
        heightmap[terrainSize - 1, 0] = Random.value * initRange * 2.0f - initRange;
        heightmap[terrainSize - 1, terrainSize - 1] = Random.value * initRange * 2.0f - initRange;

        // In each iteration, run Diamond Step first, then Square Step.
        for (oneStep = terrainSize - 1; oneStep > 1; oneStep /= 2) {
            halfStep = oneStep / 2;

            // Diamond Step
            for (i = 0; i < terrainSize - 1; i += oneStep) {
                for (j = 0; j < terrainSize - 1; j += oneStep) {

                    // Get the average height of four corners.
                    average = heightmap[i, j];
                    average += heightmap[i + oneStep, j];
                    average += heightmap[i, j + oneStep];
                    average += heightmap[i + oneStep, j + oneStep];
                    average /= 4.0f;

                    // Offset by a random value
                    average += (Random.value * (divRange * 2.0f)) - divRange;

                    // Set the diamond center's value
                    heightmap[i + halfStep, j + halfStep] = average;
                }
            }

            // Square Step
            for (i = 0; i < terrainSize - 1; i += halfStep) {
                for (j = (i + halfStep) % oneStep; j < terrainSize - 1; j += oneStep) {

                    // Get the average height of four corners
                    average = heightmap[(i - halfStep + terrainSize - 1) % (terrainSize - 1), j];
                    average += heightmap[(i + halfStep) % (terrainSize), j];
                    average += heightmap[i, (j + halfStep) % (terrainSize)];
                    average += heightmap[i, (j - halfStep + terrainSize - 1) % (terrainSize - 1)];
                    average /= 4.0f;


                    // Offset by a random value
                    average += (Random.value * (divRange * 2.0f)) - divRange;

                    // Set the height value 
                    heightmap[i, j] = average;

                    // Set the height on the opposite edge if this is an edge piece
                    if (i == 0) {
                        heightmap[terrainSize - 1, j] = average;
                    }

                    if (j == 0) {
                        heightmap[i, terrainSize - 1] = average;
                    }
                }
            }

            // Lower the random value range (divRange) by the factor of smoothness.
            divRange -= divRange * 0.5f * smoothness;
        }

        // Apply base height to the heightmap, and determine the max and min height.
        for (i = 0; i < terrainSize; i++) {
            for (j = 0; j < terrainSize; j++) {

                heightmap[i, j] += baseHeight;

                // Determine the max and min height
                if (heightmap[i, j] > maxHeight)
                    maxHeight = heightmap[i, j];

                if (heightmap[i, j] < minHeight)
                    minHeight = heightmap[i, j];
            }
        }

    }

    // Create the terrain mesh from the generated heightmap.
    Mesh CreateTerrainMesh() {

        // Create the new mesh for the terrain.
        Mesh m = new Mesh();
        m.name = "GameMap";
        m.Clear();

        Vector3[] vertices = new Vector3[6 * (terrainSize - 1) * (terrainSize - 1)];
        Color[] colors = new Color[vertices.Length];
        int[] triangles = new int[vertices.Length];
        Vector3[] normals = new Vector3[vertices.Length];

        int i, j, k = 0;

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
        for (i = 0; i < terrainSize - 1; i++) {
            for (j = 0; j < terrainSize - 1; j++) {

                // First triangle
                vertices[k + 0] = GetVertex(i, j);
                normals[k + 0] = GetVertexNormal(i, j);

                vertices[k + 1] = GetVertex(i, j + 1);
                normals[k + 1] = GetVertexNormal(i, j + 1);

                vertices[k + 2] = GetVertex(i + 1, j + 1);
                normals[k + 2] = GetVertexNormal(i + 1, j + 1);

                // Second Triangle
                vertices[k + 3] = GetVertex(i, j);
                normals[k + 3] = GetVertexNormal(i, j);

                vertices[k + 4] = GetVertex(i + 1, j + 1);
                normals[k + 4] = GetVertexNormal(i + 1, j + 1);

                vertices[k + 5] = GetVertex(i + 1, j);
                normals[k + 5] = GetVertexNormal(i + 1, j);

                k += 6;
            }
        }

        // Assign colors according to the height of the terrain.
        for (k = 0; k < vertices.Length; k++) {
            if (vertices[k].y > maxHeight - thresholdTwo * (maxHeight - minHeight))
                // White for the snow
                colors[k] = new Vector4(0.9f, 0.9f, 1.0f, 1.0f);
            else if (vertices[k].y > thresholdOne * (maxHeight - minHeight) + minHeight)
                // Brown for the mountain
                colors[k] = new Vector4(0.7f, 0.4f, 0.1f, 1.0f);
            else
                // Green for the grasses
                colors[k] = new Vector4(0.25f, 0.7f, 0.1f, 1.0f);

        }


        // Assign triangle indices.
        for (k = 0; k < vertices.Length; k++) {
            triangles[k] = k;
        }


        // Assign the calculated values to the mesh.
        m.vertices = vertices;
        m.colors = colors;
        m.triangles = triangles;
        m.normals = normals;

        return m;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameMapGenerator : MonoBehaviour {

    // Filename of the map
    public string mapFilename;
    // The size of each element in the game map
    public float unitSize;
    // The zoom of the texture on the surface
    public float textureZoom;
    // The shader for the game map
    public Shader shader;
    // The light object
    public PointLight pointLight;

    // The raw data of the game map (explained in README located at /Asset/GameMap/)
    private List<int[]> rawMap;

    // Vertices and normals of the game terrain.
    List<Vector3[,]> vertices, normals;
    List<Vector2[,]> uvs;


    // Use this for initialization
    void Start() {
        // Read map information from file.
        rawMap = ReadMapFile();


        // Initialize and generate vertices for the game.
        vertices = new List<Vector3[,]>();
        normals = new List<Vector3[,]>();
        uvs = new List<Vector2[,]>();
        CreateVertices();

        // Create map mesh from the raw map data.
        Mesh m = CreateMapMesh();

        // Add MeshFilter and MeshCollider components to the game map, and assign the mesh to them.
        MeshFilter terrainMesh = this.gameObject.AddComponent<MeshFilter>();
        MeshCollider col = this.gameObject.AddComponent<MeshCollider>();

        terrainMesh.mesh = m;
        col.sharedMesh = m;

        // Add a MeshRenderer component. This component actually renders the mesh that
        // is defined by the MeshFilter component.
        MeshRenderer renderer = this.gameObject.gameObject.GetComponent<MeshRenderer>();
        //renderer.material.shader = shader;
    }


    // Update is called once per frame
    void Update() {
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

        // Pass updated light positions to shader.
        renderer.material.SetColor("_PointLightColor", this.pointLight.color);
        renderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());
    }


    // Generate the vector of a vertex.
    public Vector3 GetVertex(float x, float height, float z) {

        return new Vector3(x, height, z);
    }


    // Generate the vector of a vertex when the parameters are integers.
    public Vector3 GetIntVertex(int x, int height, int z) {

        return new Vector3((float)x, (float)height, (float)z);
    }


    // Calculate and generate the vertex normal for a vertex.
    public Vector3 GetVertexNormal() {

        Vector3 norm = new Vector3(0.0f, 1.0f, 0.0f);

        return norm;
    }


    // Read map information from file, and load it to List of int[].
    private List<int[]> ReadMapFile() {
        string filePath = Application.dataPath + "/GameMap/" + mapFilename;
        Debug.Log(filePath);

        // The game map file is a csv file, containing groups of 5 parameters:
        //      X Location    Z Location    Terrain Type    Rotation     Height
        //
        // For more information, please read the README in /Asset/GameMap/.

        StreamReader sr = new StreamReader(filePath);
        var rawMap = new List<int[]>();

        while (!sr.EndOfStream) {
            string[] line = sr.ReadLine().Split(',');
            int[] values = new int[5];
            for (int i = 0; i < 5; i++)
                values[i] = int.Parse(line[i]);
            rawMap.Add(new int[] { values[0], values[1], values[2], values[3], values[4] });
        }

        Debug.Log("Map Elements Count: " + rawMap.Count);

        return rawMap;
    }

    // Generate the heightmap from the raw map data.
    void CreateVertices() {
        float x, z;
        int type, rotation, height;
        Vector3[,] vertex, normal;
        Vector2[,] uv;

        for (int index = 0; index < rawMap.Count; index++) {

            x = rawMap[index][0] * unitSize - unitSize / 2.0f;
            z = rawMap[index][1] * unitSize - unitSize / 2.0f;
            type = rawMap[index][2];
            rotation = rawMap[index][3];
            height = rawMap[index][4];

            switch (type) {
                case 1:
                    vertex = new Vector3[2, 2];
                    vertex[0, 0] = GetVertex(x, height, z);
                    vertex[0, 1] = GetVertex(x, height, z + unitSize);
                    vertex[1, 0] = GetVertex(x + unitSize, height, z);
                    vertex[1, 1] = GetVertex(x + unitSize, height, z + unitSize);

                    normal = new Vector3[2, 2];
                    normal[0, 0] = new Vector3(0.0f, 1.0f, 0.0f);
                    normal[0, 1] = new Vector3(0.0f, 1.0f, 0.0f);
                    normal[1, 0] = new Vector3(0.0f, 1.0f, 0.0f);
                    normal[1, 1] = new Vector3(0.0f, 1.0f, 0.0f);

                    uv = new Vector2[2, 2];
                    uv[0, 0] = new Vector2(0.0f * textureZoom, 0.0f * textureZoom);
                    uv[0, 1] = new Vector2(0.0f * textureZoom, 1.0f * textureZoom);
                    uv[1, 0] = new Vector2(1.0f * textureZoom, 0.0f * textureZoom);
                    uv[1, 1] = new Vector2(1.0f * textureZoom, 1.0f * textureZoom);
                    break;
                default:
                    vertex = null;
                    normal = null;
                    uv = null;
                    break;
            }

            vertices.Add(vertex);
            normals.Add(normal);
            uvs.Add(uv);

        }

    }

    // Create the terrain mesh from the generated heightmap.
    Mesh CreateMapMesh() {

        // Create the new mesh for the terrain.
        Mesh m = new Mesh();
        m.name = "GameMapMesh";
        m.Clear();

        // Initialize mesh parameters, storing them in List.
        List<Vector3> new_vertices = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<int> triangles = new List<int>();
        List<Vector3> new_normals = new List<Vector3>();
        List<Vector2> new_uvs = new List<Vector2>();

        // Process all elements of the game map, generating mesh for each of them.
        for (int index = 0; index < rawMap.Count; index++) {

            // Make triangles for the terrain, and assign vertex normals.
            //                  x        x + 1
            //
            //        z         X==========X
            //                  |          |
            //                  |          |
            //      z + 1       X==========X
            //
            //
            for (int i = 0; i < vertices[index].GetLength(0) - 1; i++) {
                for (int j = 0; j < vertices[index].GetLength(1) - 1; j++) {

                    // First triangle
                    new_vertices.Add(vertices[index][i, j]);
                    new_normals.Add(normals[index][i, j]);
                    new_uvs.Add(uvs[index][i, j]);

                    new_vertices.Add(vertices[index][i, j + 1]);
                    new_normals.Add(normals[index][i, j + 1]);
                    new_uvs.Add(uvs[index][i, j + 1]);

                    new_vertices.Add(vertices[index][i + 1, j + 1]);
                    new_normals.Add(normals[index][i + 1, j + 1]);
                    new_uvs.Add(uvs[index][i + 1, j + 1]);

                    // Second Triangle
                    new_vertices.Add(vertices[index][i + 0, j + 0]);
                    new_normals.Add(normals[index][i + 0, j + 0]);
                    new_uvs.Add(uvs[index][i, j]);

                    new_vertices.Add(vertices[index][i + 1, j + 1]);
                    new_normals.Add(normals[index][i + 1, j + 1]);
                    new_uvs.Add(uvs[index][i + 1, j + 1]);

                    new_vertices.Add(vertices[index][i + 1, j + 0]);
                    new_normals.Add(normals[index][i + 1, j + 0]);
                    new_uvs.Add(uvs[index][i + 1, j]);

                }
            }


        }

        // Assign colors according to the height of the terrain.
        for (int k = 0; k < new_vertices.Count; k++) {
            colors.Add(new Vector4(0.5f, 0.5f, 0.5f, 1.0f));
        }


        // Assign triangle indices.
        for (int k = 0; k < new_vertices.Count; k++) {
            triangles.Add(k);
        }


        // Assign the calculated values to the mesh.
        m.vertices = new_vertices.ToArray();
        m.colors = colors.ToArray();
        m.triangles = triangles.ToArray();
        m.normals = new_normals.ToArray();
        m.uv = new_uvs.ToArray();

        return m;
    }

}

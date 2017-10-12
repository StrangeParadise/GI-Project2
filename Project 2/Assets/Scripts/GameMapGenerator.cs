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
    // The thickness of the terrain
    public float terrainThickness;
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
    public Vector3 GetVertexNormal(Vector3 self, Vector3 v1, Vector3 v2) {

        Vector3 norm = Vector3.Cross((v1 - self), (v2 - self)).normalized;

        return norm;
    }


    // Read map information from file, and load it to List of int[].
    private List<int[]> ReadMapFile() {
        string filePath = Application.dataPath + "/GameMap/" + mapFilename;
        Debug.Log(filePath);

        // The game map file is a csv file, containing groups of 5 parameters:
        //  X[0,0]   Z[0,0]   X[0,1]   Z[0,1]   X[1,0]   Z[1,0]   X[1,1]   Z[1,1]
        //  Terrain Type     Height From     Height To
        //
        // For more information, please read the README in /Asset/GameMap/.

        StreamReader sr = new StreamReader(filePath);
        var rawMap = new List<int[]>();

        while (!sr.EndOfStream) {
            string[] line = sr.ReadLine().Split(',');
            int[] values = new int[13];
            for (int i = 0; i < 13; i++)
                values[i] = int.Parse(line[i]);
            rawMap.Add(new int[] { values[0], values[1], values[2], values[3], values[4], values[5], values[6],
                values[7], values[8], values[9], values[10], values[11], values[12] });
        }

        Debug.Log("Map Elements Count: " + rawMap.Count);

        return rawMap;
    }

    // Generate the heightmap from the raw map data.
    void CreateVertices() {
        int type;
        Vector3[,] vertex, normal;
        Vector2[,] uv;

        for (int index = 0; index < rawMap.Count; index++) {

            type = rawMap[index][8];

            switch (type) {
                case 1:
                    vertex = new Vector3[2, 2];
                    vertex[0, 0] = GetVertex(rawMap[index][0] * unitSize - unitSize / 2.0f,
                        rawMap[index][9], rawMap[index][1] * unitSize - unitSize / 2.0f);
                    vertex[0, 1] = GetVertex(rawMap[index][2] * unitSize - unitSize / 2.0f,
                        rawMap[index][10], rawMap[index][3] * unitSize - unitSize / 2.0f);
                    vertex[1, 0] = GetVertex(rawMap[index][4] * unitSize - unitSize / 2.0f,
                        rawMap[index][11], rawMap[index][5] * unitSize - unitSize / 2.0f);
                    vertex[1, 1] = GetVertex(rawMap[index][6] * unitSize - unitSize / 2.0f,
                        rawMap[index][12], rawMap[index][7] * unitSize - unitSize / 2.0f);

                    normal = new Vector3[2, 2];
                    normal[0, 0] = GetVertexNormal(vertex[0, 0], vertex[0, 1], vertex[1, 0]);
                    normal[0, 1] = GetVertexNormal(vertex[0, 1], vertex[1, 1], vertex[0, 0]);
                    normal[1, 0] = GetVertexNormal(vertex[1, 0], vertex[0, 0], vertex[1, 1]);
                    normal[1, 1] = GetVertexNormal(vertex[1, 1], vertex[1, 0], vertex[0, 1]);

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

            CreateButtom(vertex);
            CreateEdges(vertex);

        }

    }

    // Create the buttom of the map terrain.
    private void CreateButtom(Vector3[,] vertex) {

        Vector3[,] new_vertex, normal;
        Vector2[,] uv;


    }

    // Create 4 edges of the map terrain.
    private void CreateEdges(Vector3[,] vertex) {
        
        Vector3[,] new_vertex, normal;
        Vector2[,] uv;
        int x1, x2, z1, z2;

        new_vertex = new Vector3[2, 2];
        new_vertex[1, 0] = new Vector3(vertex[0, 0].x, vertex[0, 0].y - terrainThickness, vertex[0, 0].z);
        new_vertex[1, 1] = new Vector3(vertex[0, 1].x, vertex[0, 1].y - terrainThickness, vertex[0, 1].z);
        new_vertex[0, 0] = new Vector3(vertex[1, 0].x, vertex[1, 0].y - terrainThickness, vertex[1, 0].z);
        new_vertex[0, 1] = new Vector3(vertex[1, 1].x, vertex[1, 1].y - terrainThickness, vertex[1, 1].z);


        normal = new Vector3[2, 2];
        normal[0, 0] = GetVertexNormal(new_vertex[0, 0], new_vertex[0, 1], new_vertex[1, 0]);
        normal[0, 1] = GetVertexNormal(new_vertex[0, 1], new_vertex[1, 1], new_vertex[0, 0]);
        normal[1, 0] = GetVertexNormal(new_vertex[1, 0], new_vertex[0, 0], new_vertex[1, 1]);
        normal[1, 1] = GetVertexNormal(new_vertex[1, 1], new_vertex[1, 0], new_vertex[0, 1]);

        uv = new Vector2[2, 2];
        uv[0, 0] = new Vector2(0.0f * textureZoom, 0.0f * textureZoom);
        uv[0, 1] = new Vector2(0.0f * textureZoom, 1.0f * textureZoom);
        uv[1, 0] = new Vector2(1.0f * textureZoom, 0.0f * textureZoom);
        uv[1, 1] = new Vector2(1.0f * textureZoom, 1.0f * textureZoom);

        vertices.Add(new_vertex);
        normals.Add(normal);
        uvs.Add(uv);

        for (int i = 0; i < 4; i++) {
            switch (i) {
                case 0:
                    x1 = 0;
                    z1 = 0;
                    x2 = 0;
                    z2 = 1;
                    break;
                case 1:
                    x1 = 0;
                    z1 = 1;
                    x2 = 1;
                    z2 = 1;
                    break;
                case 2:
                    x1 = 1;
                    z1 = 1;
                    x2 = 1;
                    z2 = 0;
                    break;
                case 3:
                    x1 = 1;
                    z1 = 0;
                    x2 = 0;
                    z2 = 0;
                    break;
                default:
                    x1 = 0;
                    z1 = 0;
                    x2 = 0;
                    z2 = 0;
                    break;
            }

            new_vertex = new Vector3[2, 2];
            new_vertex[1, 0] = new Vector3(vertex[x1, z1].x, vertex[x1, z1].y, vertex[x1, z1].z);
            new_vertex[1, 1] = new Vector3(vertex[x2, z2].x, vertex[x2, z2].y, vertex[x2, z2].z);
            new_vertex[0, 0] = new Vector3(vertex[x1, z1].x, vertex[x1, z1].y - terrainThickness, vertex[x1, z1].z);
            new_vertex[0, 1] = new Vector3(vertex[x2, z2].x, vertex[x2, z2].y - terrainThickness, vertex[x2, z2].z);


            normal = new Vector3[2, 2];
            normal[0, 0] = GetVertexNormal(new_vertex[0, 0], new_vertex[0, 1], new_vertex[1, 0]);
            normal[0, 1] = GetVertexNormal(new_vertex[0, 1], new_vertex[1, 1], new_vertex[0, 0]);
            normal[1, 0] = GetVertexNormal(new_vertex[1, 0], new_vertex[0, 0], new_vertex[1, 1]);
            normal[1, 1] = GetVertexNormal(new_vertex[1, 1], new_vertex[1, 0], new_vertex[0, 1]);

            uv = new Vector2[2, 2];
            uv[0, 0] = new Vector2(0.0f * textureZoom, 0.0f * textureZoom);
            uv[0, 1] = new Vector2(0.0f * textureZoom, 1.0f * textureZoom);
            uv[1, 0] = new Vector2(1.0f * textureZoom, 0.0f * textureZoom);
            uv[1, 1] = new Vector2(1.0f * textureZoom, 1.0f * textureZoom);

            vertices.Add(new_vertex);
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
        for (int index = 0; index < vertices.Count; index++) {

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

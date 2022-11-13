using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMaker : MonoBehaviour
{
    [SerializeField] private Material material;
    private int numVertices = 0;
    private LineHandler _lineHandler;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    void Start()
    {
        _lineHandler = FindObjectOfType<LineHandler>();
        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    public void MakePolygonMesh()
    {
        Mesh mesh = new Mesh();

        meshRenderer.material = material;
        meshFilter.mesh = mesh;
        numVertices = _lineHandler.GetNumPoints();

        List<Vector3> VertexList = _lineHandler.GetCopyOfPoints();
        VertexList.Add(_lineHandler.GetCentroid());
        mesh.vertices = VertexList.ToArray();

        int[] triangles = new int[3 * numVertices];

        Debug.Log($"Number of vertices: {numVertices}");

        for (int i = 0; i < numVertices; i++)
        {
            triangles[i * 3] = i;
            triangles[i * 3 + 1] = (i + 1) % numVertices;
            triangles[i * 3 + 2] = VertexList.Count - 1;

        }

        mesh.triangles = triangles;
    }

    
}


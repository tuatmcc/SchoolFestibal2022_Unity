using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GeneratePathMesh : MonoBehaviour
{
    [SerializeField] private bool GenerateMesh = false;
    [SerializeField] private CinemachineSmoothPath Path;
    [SerializeField] private float Width = 5;
    [SerializeField] private Material MeshMaterial;

    private CinemachinePathBase.PositionUnits Units = CinemachinePathBase.PositionUnits.PathUnits;

    private void OnValidate()
    {
        if (!GenerateMesh) return;
        Generate();
        GenerateMesh = false;
    }

    private void Generate()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = CalculateAllVerticles();
        mesh.triangles = CalculateTriangles(mesh.vertices.Length);
        mesh.uv = CaluculateUV(mesh.vertices);

        mesh.RecalculateNormals();
        MeshFilter filter;
        MeshRenderer renderer;
        if (TryGetComponent(out filter) && TryGetComponent(out renderer))
        {
            filter.mesh = mesh;
            renderer.material = MeshMaterial;
        }

    }

    private Vector3[] CalculateAllVerticles()
    {
        List<Vector3> allVertices = new List<Vector3>();

        for (int i = 0; i < Path.m_Waypoints.Length; i++)
        {
            allVertices = allVertices.Concat(CalculateVerticlesInPart(i)).ToList();
        }

        return allVertices.ToArray();
    }

    private IEnumerable<Vector3> CalculateVerticlesInPart(int part)
    {
        List<Vector3> vertices = new List<Vector3>();

        for (float i = 0; i < Path.DistanceCacheSampleStepsPerSegment; i++)
        {
            float posOnPath = part + (i / Path.DistanceCacheSampleStepsPerSegment);
            Vector3 worldPos = Path.EvaluatePositionAtUnit(posOnPath, Units);
            Vector3 localPos = transform.InverseTransformPoint(worldPos);
            Quaternion rot = Path.EvaluateOrientationAtUnit(posOnPath, Units);
            Vector3 r = (rot * Vector3.right) * Width * 0.5f;
            vertices.Add(localPos + r);
            vertices.Add(localPos - r);
        }

        return vertices;
    }

    private int[] CalculateTriangles(int verticesLength)
    {
        List<int> triangles = new List<int>();

        for (int i = 0; i < verticesLength - 2; i += 2)
        {
            triangles.Add(i);
            triangles.Add(i + 2);
            triangles.Add(i + 1);
            triangles.Add(i + 1);
            triangles.Add(i + 2);
            triangles.Add(i + 3);
        }

        return triangles.ToArray();
    }

    private Vector2[] CaluculateUV(Vector3[] vertices)
    {
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        return uvs;
    }

}

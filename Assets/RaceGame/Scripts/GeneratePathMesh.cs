using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GeneratePathMesh : MonoBehaviour
{
    [SerializeField] private bool generateMesh = false;
    [SerializeField] private CinemachineSmoothPath path;
    [SerializeField] private float width = 5;
    [SerializeField] private Material meshMaterial;

    private readonly CinemachinePathBase.PositionUnits _units = CinemachinePathBase.PositionUnits.PathUnits;

    private void OnValidate()
    {
        if (!generateMesh) return;
        Generate();
        generateMesh = false;
    }

    private void Generate()
    {
        var mesh = new Mesh();

        mesh.vertices = CalculateAllVerticles();
        mesh.triangles = CalculateTriangles(mesh.vertices.Length);
        mesh.uv = CalculateUV(mesh.vertices);

        mesh.RecalculateNormals();
        MeshFilter filter;
        MeshRenderer renderer;
        if (TryGetComponent(out filter) && TryGetComponent(out renderer))
        {
            filter.mesh = mesh;
            renderer.material = meshMaterial;
        }

    }

    private Vector3[] CalculateAllVerticles()
    {
        var allVertices = new List<Vector3>();

        for (var i = 0; i < path.m_Waypoints.Length; i++)
        {
            allVertices = allVertices.Concat(CalculateVerticlesInPart(i)).ToList();
        }

        return allVertices.ToArray();
    }

    private IEnumerable<Vector3> CalculateVerticlesInPart(int part)
    {
        var vertices = new List<Vector3>();

        for (float i = 0; i < path.DistanceCacheSampleStepsPerSegment; i++)
        {
            var posOnPath = part + (i / path.DistanceCacheSampleStepsPerSegment);
            var worldPos = path.EvaluatePositionAtUnit(posOnPath, _units);
            var localPos = transform.InverseTransformPoint(worldPos);
            var rot = path.EvaluateOrientationAtUnit(posOnPath, _units);
            var r = (rot * Vector3.right) * width * 0.5f;
            vertices.Add(localPos + r);
            vertices.Add(localPos - r);
        }

        return vertices;
    }

    private int[] CalculateTriangles(int verticesLength)
    {
        var triangles = new List<int>();

        for (var i = 0; i < verticesLength - 2; i += 2)
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

    private Vector2[] CalculateUV(Vector3[] vertices)
    {
        var uvs = new Vector2[vertices.Length];
        for (var i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        return uvs;
    }

}

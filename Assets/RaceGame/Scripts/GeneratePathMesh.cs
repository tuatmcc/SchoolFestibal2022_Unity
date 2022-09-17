using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace RaceGame.Scripts
{
    [RequireComponent(typeof(CinemachineSmoothPath))]
    [RequireComponent(typeof(LineRenderer))]
    public class GeneratePathMesh : MonoBehaviour
    {
        // trigger to start generating mesh
        [SerializeField] private bool generateMesh = false;
        
        [SerializeField] private float width = 5;
        [SerializeField] private Material meshMaterial;

        private CinemachineSmoothPath _path;
        private LineRenderer _lineRenderer;

        private readonly CinemachinePathBase.PositionUnits _units = CinemachinePathBase.PositionUnits.PathUnits;

        private void OnValidate()
        {
            if (!generateMesh) return;
            if (meshMaterial == null)
            {
                Debug.LogError(typeof(Material) + "がありません!!");
                return;
            }

            _path = GetComponent<CinemachineSmoothPath>();
            _lineRenderer = GetComponent<LineRenderer>();
            // Generate();
            GenerateLine();
            
            generateMesh = false;
        }

        private Vector3[] CalculateAllVertex()
        {
            var allVertices = new List<Vector3>();

            for (var i = 0; i < _path.m_Waypoints.Length; i++)
            {
                allVertices = allVertices.Concat(CalculateVertexInPart(i)).ToList();
            }

            return allVertices.ToArray();
        }

        private IEnumerable<Vector3> CalculateVertexInPart(int part)
        {
            var vertices = new List<Vector3>();

            for (float i = 0; i < _path.DistanceCacheSampleStepsPerSegment; i++)
            {
                var posOnPath = part + (i / _path.DistanceCacheSampleStepsPerSegment);
                var worldPos = _path.EvaluatePositionAtUnit(posOnPath, _units);
                var localPos = transform.InverseTransformPoint(worldPos);
                var height = new Vector3(0, width * 0.5f, 0);
                vertices.Add(localPos + height);
            }

            return vertices;
        }

        private void GenerateLine()
        {
            var positions = CalculateAllVertex();

            _lineRenderer.positionCount = positions.Length;
            _lineRenderer.SetPositions(positions);

            _lineRenderer.startWidth = width;
            _lineRenderer.endWidth = width;

            _lineRenderer.material = meshMaterial;
        }
    }
}
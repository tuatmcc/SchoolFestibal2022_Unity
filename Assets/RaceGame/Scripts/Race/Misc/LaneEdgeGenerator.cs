using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

namespace RaceGame.Race.Misc
{
	public class LaneEdgeGenerator : MonoBehaviour
	{
		[SerializeField] private CinemachineSmoothPath smoothPath;
		[SerializeField] private float width;
		[SerializeField] private float thickness;
		[SerializeField] private float offset;
		[SerializeField] private Material meshMaterial;

		[SerializeField] private bool onValidate;

		[SerializeField] private GameObject lane1;
		[SerializeField] private GameObject lane2;
		
		private const CinemachinePathBase.PositionUnits Units
			= CinemachinePathBase.PositionUnits.PathUnits;

		private MeshFilter _lane1Filter;
		private MeshFilter _lane2Filter;

		private MeshRenderer _lane1Renderer;
		private MeshRenderer _lane2Renderer;
		
		private void OnValidate()
		{
			if(onValidate)
				Generate();
		}

		private void Start()
		{
			Generate();
		}

		[ContextMenu(nameof(Generate))]
		private void Generate()
		{
			_lane1Filter = lane1.GetComponent<MeshFilter>();
			_lane1Renderer = lane1.GetComponent<MeshRenderer>();
			_lane2Filter = lane2.GetComponent<MeshFilter>();
			_lane2Renderer = lane2.GetComponent<MeshRenderer>();

			lane1.transform.parent = gameObject.transform;
			lane2.transform.parent = gameObject.transform;
			
			lane1.transform.localPosition = Vector3.zero;
			lane2.transform.localPosition = Vector3.zero;

			var lane1Mesh = new Mesh();
			var lane2Mesh = new Mesh();

			(lane1Mesh.vertices, lane2Mesh.vertices) = CalcAllVertices();
			lane1Mesh.triangles = CalcTriangles(lane1Mesh.vertices.Length);
			lane2Mesh.triangles = CalcTriangles(lane2Mesh.vertices.Length);
			lane1Mesh.uv = CaleUvs(lane1Mesh.vertices);
			lane2Mesh.uv = CaleUvs(lane2Mesh.vertices);
			lane1Mesh.RecalculateNormals();
			lane2Mesh.RecalculateNormals();

			_lane1Filter.mesh = lane1Mesh;
			_lane2Filter.mesh = lane2Mesh;

			_lane1Renderer.material = meshMaterial;
			_lane2Renderer.material = meshMaterial;
		}

		private (Vector3[], Vector3[]) CalcAllVertices()
		{
			var lane1AllVertices = new List<Vector3>();
			var lane2AllVertices = new List<Vector3>();

			for (var part = 0; part < smoothPath.m_Waypoints.Length; part++)
			{
				(var v1,var v2) = CalcVerticesPart(part);
				lane1AllVertices = lane1AllVertices.Concat(v1).ToList();
				lane2AllVertices = lane2AllVertices.Concat(v2).ToList();
			}

			return (lane1AllVertices.ToArray(), lane2AllVertices.ToArray());
		}

		private (IEnumerable<Vector3>, IEnumerable<Vector3>) CalcVerticesPart(int part)
		{
			var lane1Vertices = new List<Vector3>();
			var lane2Vertices = new List<Vector3>();

			for (var i = 0; i < smoothPath.DistanceCacheSampleStepsPerSegment; i++)
			{
				var pos = part + (float) i / smoothPath.DistanceCacheSampleStepsPerSegment;
				var point = smoothPath.EvaluatePositionAtUnit(pos, Units);
				var loaclPoint = transform.InverseTransformPoint(point);
				var rot = smoothPath.EvaluateOrientationAtUnit(pos, Units);
				var r = (rot * Vector3.right) * width / 2;
				
				lane1Vertices.Add(loaclPoint + (width / 2 - thickness + offset) * r);
				lane1Vertices.Add(loaclPoint + (width / 2 + thickness + offset) * r);
				
				lane2Vertices.Add(loaclPoint - (width / 2 + thickness - offset) * r);
				lane2Vertices.Add(loaclPoint - (width / 2 - thickness - offset) * r);
			}

			return (lane1Vertices, lane2Vertices);
		}

		private int[] CalcTriangles(int verticesLength)
		{
			var triangles = new List<int>();

			for (var pointNum = 0; pointNum < verticesLength - 2; pointNum += 2)
			{
				triangles.Add(pointNum);
				triangles.Add(pointNum + 2);
				triangles.Add(pointNum + 1);
				triangles.Add(pointNum + 1);
				triangles.Add(pointNum + 2);
				triangles.Add(pointNum + 3);
			}

			return triangles.ToArray();
		}

		private Vector2[] CaleUvs(Vector3[] vertices)
		{
			var uvs = new Vector2[vertices.Length];
			for(var i = 0; i < uvs.Length; i++)
			{
				uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
			}

			return uvs;
		}
	}
}
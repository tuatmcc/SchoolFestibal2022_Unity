using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

namespace RaceGame.Misc
{
	public class LaneEdgeGenerator : MonoBehaviour
	{
		[SerializeField] private CinemachineSmoothPath smoothPath;
		[SerializeField] private float width;
		[SerializeField] private float thickness;
		[SerializeField] private float offset;
		[SerializeField] private Material meshMaterial;

		[SerializeField] private bool onValidate;
		
		private const CinemachinePathBase.PositionUnits Units
			= CinemachinePathBase.PositionUnits.PathUnits;

		private GameObject lane1;
		private GameObject lane2;

		private MeshFilter lane1Filter;
		private MeshFilter lane2Filter;

		private MeshRenderer lane1Renderer;
		private MeshRenderer lane2Renderer;
		
		private void OnValidate()
		{
			if(onValidate)
				Generate();
		}

		[ContextMenu(nameof(Generate))]
		private void Generate()
		{
			if(lane1 != null && lane2 != null && !onValidate)
				return;

			if (lane1 == null)
			{
				lane1 = new GameObject("lane1");
				lane1Filter = lane1.AddComponent<MeshFilter>();
				lane1Renderer = lane1.AddComponent<MeshRenderer>();
			}

			if (lane2 == null)
			{
				lane2 = new GameObject("lane2");
				lane2Filter = lane2.AddComponent<MeshFilter>();
				lane2Renderer = lane2.AddComponent<MeshRenderer>();
			}

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

			lane1Filter.mesh = lane1Mesh;
			lane2Filter.mesh = lane2Mesh;

			lane1Renderer.material = meshMaterial;
			lane2Renderer.material = meshMaterial;
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

			for (int i = 0; i < smoothPath.DistanceCacheSampleStepsPerSegment; i++)
			{
				float pos = part + (float) i / smoothPath.DistanceCacheSampleStepsPerSegment;
				Vector3 point = smoothPath.EvaluatePositionAtUnit(pos, Units);
				Vector3 loaclPoint = transform.InverseTransformPoint(point);
				Quaternion rot = smoothPath.EvaluateOrientationAtUnit(pos, Units);
				Vector3 r = (rot * Vector3.right) * width / 2;
				
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

			for (int pointNum = 0; pointNum < verticesLength - 2; pointNum += 2)
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
			for(int i = 0; i < uvs.Length; i++)
			{
				uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
			}

			return uvs;
		}
	}
}
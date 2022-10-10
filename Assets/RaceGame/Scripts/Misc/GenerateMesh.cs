using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using PositionUnits = Cinemachine.CinemachinePathBase.PositionUnits;

namespace RaceGame.Misc
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class GenerateMesh : MonoBehaviour
    {
        [SerializeField] private CinemachineSmoothPath smoothPath;
        [SerializeField] private float width;
        [SerializeField] private float thickness;
        [SerializeField] private Material meshMaterial;
        [SerializeField] private bool test;
        [SerializeField] private float foo = 3.54f;
        [SerializeField] private float bar = 5.71f;

        private const PositionUnits Units = PositionUnits.PathUnits;

        private void OnValidate()
        {
            Generate();
        }

        [ContextMenu(nameof(Generate))]
        private void Generate()
        {
            var mesh = new Mesh();

            mesh.vertices = CalcAllVertices(); // 頂点座標
            mesh.triangles = CalcTriangles(mesh.vertices.Length); // 頂点を結ぶ順番
            mesh.uv = CalcUvs(mesh.vertices); // UVの計算
            mesh.RecalculateNormals();

            // メッシュの適用
            var filter = GetComponent<MeshFilter>();
            filter.mesh = mesh;

            // マテリアルの適用
            var renderer = GetComponent<Renderer>();
            renderer.material = meshMaterial;
        }

        /// <summary>
        /// 頂点座標を返す
        /// </summary>
        /// <returns>頂点座標のリスト</returns>
        private Vector3[] CalcAllVertices()
        {
            var allVertices = new List<Vector3>();
            for (var part = 0; part < smoothPath.m_Waypoints.Length; part++)
            {
                // 2点のWaypoint間の頂点を計算して帰ってきた配列を結合
                allVertices = allVertices.Concat(CalcVerticesPart(part)).ToList();
            }

            return allVertices.ToArray();
        }

        /// <summary>
        /// 2点のWaypoint間の頂点座標を計算して返す
        /// </summary>
        /// <param name="part">Waypoint</param>
        /// <returns>2点のWaypoint間の頂点座標のリスト</returns>
        private IEnumerable<Vector3> CalcVerticesPart(int part)
        {
            var vertices = new List<Vector3>();

            for (var i = 0; i < smoothPath.DistanceCacheSampleStepsPerSegment; i++)
            {
                var pos = part + (float) i / smoothPath.DistanceCacheSampleStepsPerSegment;
                var point = smoothPath.EvaluatePositionAtUnit(pos, Units);
                var localPoint = transform.InverseTransformPoint(point);
                var rot = smoothPath.EvaluateOrientationAtUnit(pos, Units);
                var r = (rot * Vector3.right) * width / 2;

                if (test)
                {
                    vertices.Add(localPoint + foo * r);
                    vertices.Add(localPoint + bar * r);
                }
                else
                {
                    vertices.Add(localPoint - bar * r);
                    vertices.Add(localPoint - foo * r);
                }
            }

            return vertices;
        }

        /// <summary>
        /// 頂点を結ぶ順番を計算して返す
        /// </summary>
        /// <param name="verticesLength">頂点の数</param>
        /// <returns>頂点を結ぶ順番の配列</returns>
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

        /// <summary>
        /// UVを計算して返す
        /// </summary>
        /// <param name="vertices">メッシュの頂点座標</param>
        /// <returns>UV座標の配列</returns>
        private Vector2[] CalcUvs(Vector3[] vertices)
        {
            var uvs = new Vector2[vertices.Length];
            for (var i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
            }
            return uvs;
        }
    }
}
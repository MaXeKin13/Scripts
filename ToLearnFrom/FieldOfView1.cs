using System;
using System.Collections.Generic;
using System.Linq;
using BatuDev.Utils.Attributes;
using UnityEditor;
using UnityEngine;

namespace BatuDev.General
{
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class FieldOfView : MonoBehaviour
	{
		[SerializeField] private float radius = 5f;
		[SerializeField] private float degree = 90f;
		[SerializeField] private int segments = 1;
		[SerializeField] private Material material;
		[SerializeField] private bool updateOnEachFrame = true;

		public Action<Collider> onCollision;

		private MeshFilter _mf;
		private MeshRenderer _mr;

		private void Start() {
			_mf = GetComponent<MeshFilter>();
			_mr = GetComponent<MeshRenderer>();
		}

		private void Update() {
			if (updateOnEachFrame)
				UpdateFOV();
		}

		public void UpdateFOV() {
			CreateFOV();
		}

#if UNITY_EDITOR
		[Button]
#endif
		public void CreateFOV() {
			Start();

			var mesh = new Mesh();
			var verts = new Vector3[segments + 2];
			var tris = new int[segments * 3];

			var tr = transform;
			var deg = 0f;
			verts[0] = tr.position;
			var hits = new List<Collider>();
			for (var i = 1; i < verts.Length; i++) {
				var pos = tr.position
				          + tr.forward * radius * Mathf.Cos(deg * Mathf.Deg2Rad)
				          + tr.right * radius * Mathf.Sin(deg * Mathf.Deg2Rad);
				if (Physics.Raycast(tr.position, (pos - tr.position).normalized, out var hit,
					    radius, -1)) {
					verts[i] = hit.point;
					onCollision?.Invoke(hit.collider);
					if (!hits.Select(h => h.name).ToArray().Contains(hit.collider.name)) {
						hits.Add(hit.collider);
					}
				} else {
					verts[i] = pos;
				}


				deg += degree / segments;
			}

			var k = 0;
			for (var i = 0; i < segments; i++) {
				tris[k] = 0;
				tris[k + 1] = i + 1;
				tris[k + 2] = i + 2;

				k += 3;
			}

			mesh.vertices = verts.Select(v => transform.InverseTransformPoint(v)).ToArray();
			mesh.triangles = tris;
#if UNITY_EDITOR
			MeshUtility.Optimize(mesh);
#endif

			_mf.mesh = mesh;
			_mr.material = material;
		}
	}
}
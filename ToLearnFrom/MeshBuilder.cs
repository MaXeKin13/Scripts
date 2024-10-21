using BatuDev.Utils;
using UnityEngine;

namespace BatuDev.MeshCreator
{
	/// <summary>
	/// WIP
	/// </summary>
	public class MeshBuilder
	{
		private float _bezier;
		
		public Mesh BuildMesh(Transform tr, Vector3[] positions, Vector3[] controlPoints,
			float length, int segments = 10) {
			if (segments < 10)
				segments = 10;

			var mesh = new Mesh();

			var vertexCount = 2 * (positions.Length + (positions.Length - 1) * (segments - 1));
			var verts = new Vector3[vertexCount];
			var tris = new int[(vertexCount - 2) * 3];

			var point = positions[0];
			var r = (positions[1] - positions[0]).normalized;
			r =  Vector3.Cross(r, Vector3.down).normalized;
			for (int i = 0, k = 0, j = 0; i < positions.Length + (positions.Length - 1) * (segments - 1); i++, k += 2) {
				var right = point + r * length / 2;
				var left = point - r * length / 2;

				Debug.DrawLine(right, right + Vector3.up * 5f, Color.blue, 5f);
				Debug.DrawLine(left, left + Vector3.up * 5f, Color.blue, 5f);
				
				verts[k] = right;
				verts[k + 1] = left;

				_bezier += 1f / segments;

				point = MathUtils.QuadraticLerp(positions[j],
					controlPoints[j],
					controlPoints[j + 1],
					positions[j + 1],
					_bezier);

				if (i != 0 && i % segments == 0) {
					_bezier = 0f;
					j++;
				}
			}
			
			//Set tris
			for (int k = 0, j = 0; k < tris.Length; k += 6, j += 2) {
				if (k >= tris.Length) break;
				
				tris[k] = j;
				tris[k + 1] = j + 1;
				tris[k + 2] = j + 2;
				tris[k + 3] = j + 1;
				tris[k + 4] = j + 3;
				tris[k + 5] = j + 2;
			}

			// for (var t = 1; t < positions.Length; t++) {
			// 	_bezier = 0f;
			// 	
			// 	var p1 = positions[t - 1];
			// 	var p2 = positions[t];
			// 	var c1 = controlPoints[t - 1];
			// 	var c2 = controlPoints[t];
			//
			// 	// Set vertex
			// 	var point = MathUtils.QuadraticLerp(p1, c1,
			// 		c2, p2,
			// 		_bezier);
			// 	var r = (p2 - p1).normalized;
			// 	r =  Vector3.Cross(r, Vector3.down).normalized;
			// 	for (int i = 0, k = (segments * 3 - (segments - 2)) * (t - 1); i < segments; i++, k += 2) {
			// 		var right = point + r * length / 2;
			// 		var left = point - r * length / 2;
			//
			// 		if (i == segments - 1) {
			// 			verts[k] = tr.InverseTransformPoint(right);
			// 			verts[k + 1] = tr.InverseTransformPoint(left);
			// 		
			// 			_bezier += 1f / segments;
			// 			point = MathUtils.QuadraticLerp(p1, c1,
			// 				c2, p2,
			// 				_bezier);
			// 			right = point + r * length / 2;
			// 			left = point - r * length / 2;
			// 		
			// 			verts[k + 2] = tr.InverseTransformPoint(right);
			// 			verts[k + 3] = tr.InverseTransformPoint(left);
			// 		} else {
			// 			verts[k] = tr.InverseTransformPoint(right);
			// 			verts[k + 1] = tr.InverseTransformPoint(left);
			// 		}
			//
			// 		_bezier += 1f / segments;
			// 		point = MathUtils.QuadraticLerp(p1, c1,
			// 			c2, p2,
			// 			_bezier);
			// 	}
			//
			// 	foreach (var v in verts) {
			// 		Debug.DrawLine(tr.TransformPoint(v), tr.TransformPoint(v + Vector3.up * 10f), Color.blue, 2f);
			// 	}
			//
			// 	// Set tris
			// 	for (int i = 0, k = segments * 6 * (t - 1), j = (t - 1) * 2; i < segments; i++, k += 6, j += 2) {
			// 		tris[k] = j;
			// 		tris[k + 1] = j + 1;
			// 		tris[k + 2] = j + 2;
			// 		tris[k + 3] = j + 1;
			// 		tris[k + 4] = j + 3;
			// 		tris[k + 5] = j + 2;
			// 	}
			// }
			//
			// foreach (var t in tris) {
			// 	Debug.Log($"{t}");
			// }
			
			// Debug.Log($"{verts.Length}");
			mesh.vertices = verts;
			mesh.triangles = tris;
			
			return mesh;
		}
	}
}
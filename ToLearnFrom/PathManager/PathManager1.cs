using System;
using System.Collections.Generic;
using System.Linq;
using BatuDev.MeshCreator;
using BatuDev.Utils.Attributes;
using BatuDev.Utils.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace BatuDev.General.Path
{
	public class PathManager : MonoBehaviour
	{
		public List<PathPoint> path = new List<PathPoint>();
		public float pointPlacementDistance = 15f;
		public float pointSphereSize = .5f;
		public float textDistance = 1f;

		[Space]
		[Header("Mesh Generation")]
		public float length = 10f;
		public int segments = 5;

		[Button]
		public void BuildMesh() {
			var go = new GameObject("mesh");
		
			var mb = new MeshBuilder();
			var mf = go.GetComponent<MeshFilter>();
			var mr = go.GetComponent<MeshRenderer>();
			if(mf == null)
				mf = go.AddComponent<MeshFilter>();
			if(mr == null)
				mr = go.AddComponent<MeshRenderer>();
			
			mf.mesh = mb.BuildMesh(go.transform,
				path.Select(p => p.position).ToArray(),
				path.Select(p => p.leftControlPoint).ToArray(),
				length, segments);
		}

		[Button]
		public void ClearMesh() {
			GameObject.Find("mesh").DestroySelf();
		}

		[Serializable]
		public class PathPoint
		{
			public Vector3 position;
			public Vector3 leftControlPoint;
			public Vector3 rightControlPoint;
			public bool useBezierMovement;
			public Vector3 lookDirection;
			public bool setRotationAsNextPointDirection;
			public float waitBeforeArrival;
			public UnityEvent onArrival;
			public float waitAfterCompletion;
			public UnityEvent onCompletion;
			public WaitUntilTrue waitUntilTrue;

			public static implicit operator PathPoint(Vector3 v) {
				return new PathPoint {
					position = v
				};
			}
			
			[Serializable]
			public class WaitUntilTrue
			{
				public MonoBehaviour mb;
				public string variableName;
			}
		}
	}
}
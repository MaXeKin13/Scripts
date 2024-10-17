using System;
using System.Collections.Generic;
using System.Linq;
using BatuDev.Utils;
using BatuDev.Utils.Extensions;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace BatuDev.General.Path
{
	[CustomEditor(typeof(PathManager))]
	public class EPathManager : Editor
	{
		private PathManager _pathManager;

		private Vector3 _selectedPoint;
		private int _selectedIndex;

		public override void OnInspectorGUI() {
			DrawDefaultInspector();

            if (GUILayout.Button("Buildmesh"))
            {
				_pathManager.BuildMesh();
                SceneView.RepaintAll();
            }
            if (GUILayout.Button("Duplicate Last Point")) {
				AddNewPoint(_pathManager.path.Last().position);
				SceneView.RepaintAll();
			}

			if (GUILayout.Button("Align All Control Points")) {
				foreach (var pathPoint in _pathManager.path) {
					pathPoint.leftControlPoint = pathPoint.rightControlPoint = pathPoint.position;
				}

				SceneView.RepaintAll();
			}

			if (GUILayout.Button("Increase Control Points + 1 Up")) {
				foreach (var pathPoint in _pathManager.path) {
					pathPoint.leftControlPoint = pathPoint.rightControlPoint += Vector3.up;
				}

				SceneView.RepaintAll();
			}

			if (GUILayout.Button("Increase Control Points - 1 Down")) {
				foreach (var pathPoint in _pathManager.path) {
					pathPoint.leftControlPoint = pathPoint.rightControlPoint += Vector3.down;
				}

				SceneView.RepaintAll();
			}

			if (GUILayout.Button("Clear Points")) {
				_pathManager.path.Clear();
				SceneView.RepaintAll();
			}

			if (GUILayout.Button("Remove Last Point")) {
				if (_pathManager.path.Count == 1) {
					_pathManager.path.Clear();

					_selectedPoint = Vector3.zero;
				} else {
					if (_selectedIndex == _pathManager.path.Count - 1) {
						_selectedIndex--;
						_selectedPoint = _pathManager.path[_selectedIndex].position;
					}

					_pathManager.path.RemoveAt(_pathManager.path.Count - 1);
				}

				SceneView.RepaintAll();
			}

			if (GUILayout.Button("Remove First Point")) {
				if (_pathManager.path.Count == 1) {
					_pathManager.path.Clear();
				} else {
					_pathManager.path.RemoveAt(0);
				}

				_selectedIndex = 0;

				SceneView.RepaintAll();
			}

			if (GUILayout.Button("Shift Each Point +1")) {
				_pathManager.path = _pathManager.path.Shift();
				SceneView.RepaintAll();
			}

			if (GUILayout.Button("Shift Each Point -1")) {
				_pathManager.path = _pathManager.path.Shift(-1);
				SceneView.RepaintAll();
			}

			if (GUILayout.Button("Reverse Path")) {
				_pathManager.path.Reverse();
				SceneView.RepaintAll();
			}

			if (GUILayout.Button("Clear Selection")) {
				_selectedIndex = 0;
				_selectedPoint = Vector3.zero;
				SceneView.RepaintAll();
			}

			if (GUILayout.Button("Next Point Selection")) {
				if (_selectedIndex + 1 < _pathManager.path.Count) {
					_selectedIndex++;
					SelectPoint(_selectedIndex);
					SceneView.RepaintAll();
				}
			}

			if (GUILayout.Button("Previous Point Selection")) {
				if (_selectedIndex > 0) {
					_selectedIndex--;
					SelectPoint(_selectedIndex);
					SceneView.RepaintAll();
				}
			}
		}

		private void OnEnable() {
			if (!_pathManager) {
				_pathManager = target as PathManager;

				if (_pathManager && _pathManager.path == null) {
					_pathManager.path = new List<PathManager.PathPoint>();
				}
			}
		}

		private void OnSceneGUI() {
			Tools.current = Tool.None;

			var e = Event.current;
			var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

			// point adding
			if (e.control && e.button == 0 && e.type == EventType.MouseDown) {
				var point = ray.GetPoint(_pathManager.pointPlacementDistance);
				AddNewPoint(point);
			}

			// select point
			if (e.shift && e.button == 0 && e.type == EventType.MouseDown) {
				var point = ray.GetPoint(_pathManager.pointPlacementDistance);
				_selectedPoint = point.ClosestPointTo(_pathManager.path.Select(p => p.position).ToArray(),
					out _selectedIndex);
			}

			// visualize points & controls
			var k = 0;
			foreach (var point in _pathManager.path) {
				Handles.color = Color.green;
				Handles.SphereHandleCap(-1500, point.position, Quaternion.identity, _pathManager.pointSphereSize,
					EventType.Repaint);

				// controls
				Handles.color = Color.yellow;
				Handles.SphereHandleCap(_pathManager.path.IndexOf(point) + 1, point.leftControlPoint, Quaternion.identity, _pathManager.pointSphereSize / 2,
					EventType.Repaint);
				Handles.SphereHandleCap(_pathManager.path.IndexOf(point) + 1, point.rightControlPoint, Quaternion.identity, _pathManager.pointSphereSize / 2,
					EventType.Repaint);
				Handles.color = Color.grey;
				Handles.DrawLine(point.position, point.leftControlPoint - (point.leftControlPoint - point.position).normalized * _pathManager.pointSphereSize / 4);
				Handles.DrawLine(point.position, point.rightControlPoint - (point.rightControlPoint - point.position).normalized * _pathManager.pointSphereSize / 4);
				HandleControlPoints(e, _pathManager.path.IndexOf(point) + 1);

				// point number
				Handles.Label(point.position + Vector3.up * _pathManager.textDistance, (k++).ToString(),
					new GUIStyle {
						fontStyle = FontStyle.Bold,
						fontSize = 17
					});

				Handles.color = Color.blue;

				// duplicate button
				if (Handles.Button(point.position + Vector3.up * (_pathManager.textDistance * 1.5f),
					    Quaternion.LookRotation(
						    (Camera.current.transform.position - point.position).normalized, Vector3.up),
					    1f, 1f, Handles.RectangleHandleCap)) {
					AddNewPoint(point.position);
					SelectPoint(_pathManager.path.Count - 1);
					break;
				}
			}

			// visualize lines
			if (_pathManager.path.Count > 1) {
				Handles.color = Color.red;
				for (var i = 1; i < _pathManager.path.Count; i++) {
					Handles.DrawLine(_pathManager.path[i - 1].position, _pathManager.path[i].position);
				}
			}

			// visualize rotation
			Handles.color = Color.blue;
			foreach (var point in _pathManager.path) {
				var rot = point.lookDirection == Vector3.zero
					? Quaternion.identity
					: Quaternion.LookRotation(point.lookDirection, point.position + Vector3.up);
				Handles.ArrowHandleCap(-1500, point.position, rot, _pathManager.pointSphereSize, EventType.Repaint);
			}

			// alter point
			if (_selectedPoint != Vector3.zero) {
				_pathManager.path[_selectedIndex].position =
					Handles.PositionHandle(_pathManager.path[_selectedIndex].position, Quaternion.identity);
			}

			// bezier
			for (var i = 0; i < _pathManager.path.Count - 1; i++) {
				var point = _pathManager.path[i];
				var nextPoint = _pathManager.path[i + 1];

				Handles.DrawBezier(point.position, nextPoint.position,
					point.leftControlPoint,
					nextPoint.rightControlPoint,
					Color.green,
					Texture2D.normalTexture,
					1f);
			}
		}

		private void HandleControlPoints(Event @e, int index) {
			var type = @e.GetTypeForControl(index);
		}

		private void AddNewPoint(Vector3 point) {
			_pathManager.path.Add(new PathManager.PathPoint {
				position = point,
				leftControlPoint = point,
				rightControlPoint = point
			});
		}

		private void SelectPoint(int pointIndex) {
			if (_pathManager.path.Count <= pointIndex || pointIndex < 0)
				return;

			_selectedIndex = pointIndex;
			_selectedPoint = _pathManager.path[pointIndex].position;
		}
	}
}
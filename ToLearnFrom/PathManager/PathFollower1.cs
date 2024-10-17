using System.Collections;
using BatuDev.Utils;
using BatuDev.Utils.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace BatuDev.General.Path
{
	[SelectionBase]
	public class PathFollower : MonoBehaviour
	{
		[Header("Path Following")]
		[SerializeField] private bool startAtInitialPoint;
		[SerializeField] private bool loop;
		[SerializeField, Range(0, 1)] private float rotationLerp = .2f;
		[SerializeField] private bool followAtStart;
		[Range(0f, 10f)] public float speedMultiplier = .5f;

		[Inject] public PathManager pathManager;

		private const float DistanceCheck = .4f;
		private float _bezierLerp;
		private float _regularLerp;

		public UnityAction OnPathComplete;
		
		private int _currentPointIndex;
		private bool _arrivalCoroutine;
		private bool _arrivalDone = true;

		private Quaternion _targetRotation;

		private bool _following;

		private void Start() {
			if (pathManager.path.Count > 0) {
				var initialPoint = pathManager.path[0];
				_targetRotation = initialPoint.setRotationAsNextPointDirection
					? Quaternion.LookRotation(initialPoint.lookDirection, Vector3.up)
					: transform.rotation;
				StartCoroutine(Rotator());
			
				if (startAtInitialPoint) {
					transform.position = pathManager.path[0].position;
					_currentPointIndex = 1;
				}
			}

			if (followAtStart) {
				BeginFollowing();
			}
		}
#if UNITY_EDITOR
		[Button]
#endif
		public void BeginFollowing() {
			_following = true;
			
			StopAllCoroutines();
			StartCoroutine(Rotator());
		}
#if UNITY_EDITOR
		[Button]
#endif
		public void StopFollowing() {
			_following = false;
			
			StopAllCoroutines();
		}

		private void LateUpdate() {
			if(!_following) return;
			
			var pos = Vector3.Lerp(transform.position, pathManager.path[_currentPointIndex].position, _regularLerp += Time.deltaTime * speedMultiplier);
			if (_arrivalDone) {
				if (pathManager.path[_currentPointIndex].useBezierMovement &&
				    _currentPointIndex > 0 &&
				    _currentPointIndex <= pathManager.path.Count - 1) {
					pos = MathUtils.QuadraticLerp(pathManager.path[_currentPointIndex - 1].position,
						pathManager.path[_currentPointIndex - 1].leftControlPoint,
						pathManager.path[_currentPointIndex].leftControlPoint,
						pathManager.path[_currentPointIndex].position,
						_bezierLerp += Time.deltaTime * speedMultiplier);
				}
				transform.position = pos;
			}

			if (Vector3.Distance(pathManager.path[_currentPointIndex].position, transform.position) < DistanceCheck) {
				_bezierLerp = 0f;
				_regularLerp = 0f;
				
				OnArrival();
			}
		}

		private void OnArrival() {
			if (!_arrivalCoroutine) {
				_arrivalCoroutine = true;
				StartCoroutine(Arrival());
			}
		}

		private IEnumerator Arrival() {
			_arrivalDone = false;
			
			var pathPoint = pathManager.path[_currentPointIndex];
			if (pathPoint.lookDirection != Vector3.zero)
				_targetRotation = Quaternion.LookRotation(pathPoint.lookDirection, Vector3.up);

			StartCoroutine(InvokeArrival(pathPoint));
			
			if (pathPoint.waitUntilTrue != null) {
				var mb = pathPoint.waitUntilTrue.mb;
				if (mb != null) {
					var v = mb.GetType().GetField(pathPoint.waitUntilTrue.variableName);
					if (v != null) {
						var variableInMb = (bool) v.GetValue(mb);
						while (!variableInMb) {
							variableInMb = (bool) v.GetValue(mb);
							
							yield return null;
						}
					}
				}
			}

			pathPoint.onCompletion?.Invoke();

			yield return new WaitForSeconds(pathPoint.waitAfterCompletion);
			
			_currentPointIndex++;
			if (_currentPointIndex >= pathManager.path.Count) {
				_arrivalDone = loop;
				_arrivalCoroutine = !loop;
				_currentPointIndex = loop ? 0 : pathManager.path.Count - 1;

				OnPathComplete?.Invoke();
			} else {
				var point = pathManager.path[_currentPointIndex];
				var rot = Quaternion.Euler(point.lookDirection);
				if (point.lookDirection.magnitude > Mathf.Epsilon) {
					rot = Quaternion.LookRotation(point.lookDirection, Vector3.up);
				}
				_targetRotation = point.setRotationAsNextPointDirection ? rot : transform.rotation;
				
				_arrivalCoroutine = false;
				_arrivalDone = true;
			}
		}

		private IEnumerator InvokeArrival(PathManager.PathPoint pathPoint) {
			yield return new WaitForSeconds(pathPoint.waitBeforeArrival);

			pathPoint.onArrival?.Invoke();
		}

		private IEnumerator Rotator() {
			while (true) {
				transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotationLerp);
		
				yield return null;
			}
		}
	}
}
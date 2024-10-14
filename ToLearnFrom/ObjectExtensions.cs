using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BatuDev.Utils.Extensions
{
	public static class ObjectExtensions
	{
		public static void DestroySelf(this Component comp, bool forceDestroy = false) {
			if (forceDestroy) {
				Object.Destroy(comp);
				return;
			}
			
#if UNITY_EDITOR
			Object.DestroyImmediate(comp);
#else
			Object.Destroy(comp);
#endif
		}

		public static void DestroySelf(this GameObject go, bool forceDestroy = false) {
			if (forceDestroy) {
				Object.Destroy(go);
				return;
			}
			
#if UNITY_EDITOR
			Object.DestroyImmediate(go);
#else
			Object.Destroy(go);
#endif
		}

		public static void DestroySelf(this Transform tr, bool force = false) {
#if UNITY_EDITOR
			if (force) {
				Object.Destroy(tr.gameObject);
				return;
			}
			Object.DestroyImmediate(tr.gameObject);
#else
			Object.Destroy(tr.gameObject);
#endif
		}

		public static void DisableSelf(this GameObject go) {
			go.SetActive(false);
		}

		public static void DisableSelf(this GameObject go, float disableIn, GameObject coroutineOwner,
			Action onDisabled = null) {
			disableIn = Mathf.Clamp(disableIn, 0f, disableIn);
			if (disableIn == 0f) {
				go.DisableSelf();
				return;
			}
			
			IEnumerator DelayedDisable() {
				yield return new WaitForSeconds(disableIn);
				
				go.DisableSelf();
				onDisabled?.Invoke();
			}

			coroutineOwner.GetComponent<MonoBehaviour>().StartCoroutine(DelayedDisable());
		}

		public static void EnableSelf(this GameObject go) {
			go.SetActive(true);
		}

		public static void EnableSelf(this GameObject go, float enableIn, GameObject coroutineOwner) {
			enableIn = Mathf.Clamp(enableIn, 0f, enableIn);
			
			IEnumerator DelayedEnable() {
				yield return new WaitForSeconds(enableIn);
				
				go.EnableSelf();
			}

			coroutineOwner.GetComponent<MonoBehaviour>().StartCoroutine(DelayedEnable());
		}

		public static Component[] GetAllComponents(this GameObject go) {
			var toReturn = new List<Component>();
			toReturn.AddRange(go.GetComponents<Component>().Union(go.transform.GetComponentsInChildren<Component>(true)));
			return toReturn.ToArray();
		}

		public static void MoveTowards(this Transform tr, Vector3 dir, float speed) {
			IEnumerator Move() {
				while (tr.gameObject.activeInHierarchy) {
					tr.Translate(dir * (speed * Time.deltaTime), Space.World);

					yield return null;
				}
			}

			tr.GetComponent<MonoBehaviour>().StartCoroutine(Move());
		}

		public static void MoveTowards(this Transform tr, float speed, Vector3 position) {
			IEnumerator Move() {
				var dir = (position - tr.position).normalized;
				while (tr.gameObject.activeInHierarchy) {
					tr.Translate(dir * (speed * Time.deltaTime), Space.World);

					yield return null;
				}
			}

			tr.GetComponent<MonoBehaviour>().StartCoroutine(Move());
		}

#if UNITY_EDITOR
		public static void Select(this GameObject obj) {
			Selection.activeGameObject = obj;
		}

		public static void Select(this Transform tr) {
			Selection.activeGameObject = tr.gameObject;
		}
#endif

		public static Transform[] Children(this Transform tr) {
			if (tr.childCount == 0) return null;

			var toReturn = new Transform[tr.childCount];
			for (var i = 0; i < tr.childCount; i++) {
				toReturn[i] = tr.GetChild(i);
			}

			return toReturn;
		}

		public static Transform[] Parents(this Transform tr) {
			if (tr.parent == null) return null;

			var ks = 0;
			var parent = tr.parent;
			var toReturn = new List<Transform>();
			while (ks < 1000 && parent != null) {
				ks++;

				toReturn.Add(parent);
				
				parent = parent.parent;
			}

			return toReturn.ToArray();
		}

		public static Transform[] Children(this Transform tr, bool includeInactive) {
			if (tr.childCount == 0) return null;

			var toReturn = tr.GetComponentsInChildren<Transform>(includeInactive);

			return toReturn;
		}

		public static Transform[] AllChildren(this Transform tr, bool includeInactive) {
			if (tr.Children() == null) return null;

			return tr.GetComponentsInChildren<Transform>(includeInactive);
		}

		public static void ClearChildren(this Transform tr) {
			foreach (var transform in tr.Children()) {
				transform.DestroySelf();
			}
		}

		public static Transform LastChild(this Transform tr) {
			if (tr.childCount == 0) return null;
			return tr.GetChild(tr.childCount - 1);
		}

		public static T GetComponentExtended<T>(this Transform tr) {
			var p = tr.GetComponent<T>();
			if (p != null) {
				return p;
			}

			var c = tr.GetComponentInChildren<T>();
			if (c != null) {
				return c;
			}
			
			return tr.GetComponentInParent<T>();
		}

		public static T[] GetComponentsExtended<T>(this Transform tr, bool includeInactive = false) {
			List<T> comps = new List<T>();
			var p = tr.GetComponentsInParent<T>(includeInactive);
			if (p != null) {
				comps.AddRange(p);
			}

			var c = tr.GetComponentsInChildren<T>(includeInactive);
			if (c != null) {
				comps.AddRange(c);
			}

			if (tr.GetComponents<T>() != null) {
				comps.AddRange(tr.GetComponents<T>());
			}
			
			return comps.ToArray();
		}

		public static T GetCopyOf<T>(this Component comp, T other) where T : Component
		{
			var type = comp.GetType();
			if (type != other.GetType()) return null;
			var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
			var pinfos = type.GetProperties(flags);
			foreach (var pinfo in pinfos) {
				if (pinfo.CanWrite) {
					try {
						pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
					}
					catch { }
				}
			}
			var finfos = type.GetFields(flags);
			foreach (var finfo in finfos) {
				finfo.SetValue(comp, finfo.GetValue(other));
			}
			return comp as T;
		}
	}
}
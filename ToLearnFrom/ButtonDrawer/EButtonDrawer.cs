using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BatuDev.Utils.Attributes
{
	[CustomEditor(typeof(ButtonDrawer))]
	public class EButtonDrawer : Editor
	{
		private ButtonDrawer _buttonDrawer;

		private readonly List<string> _buttonNames = new List<string>();
		private readonly List<MethodInfo> _methods = new List<MethodInfo>();
		private readonly List<object> _methodTypes = new List<object>();
		
		private void OnEnable() {
			_buttonDrawer = target as ButtonDrawer;

			hideFlags = HideFlags.NotEditable;

			if (_buttonDrawer != null) {
				var comps = _buttonDrawer.gameObject.GetComponents<Component>();
				foreach (var component in comps) {
					var methods = component.GetType().GetMethods();
					foreach (var method in methods) {
						if (Attribute.GetCustomAttribute(method, typeof(ButtonAttribute)) != null) {
							_methodTypes.Add(component);
							_buttonNames.Add(method.Name);
							_methods.Add(method);
						}
					}
				}
			}
		}

		public override void OnInspectorGUI() {
			for (var i = 0; i < _methods.Count; i++) {
				var m = _methods[i];
				if (GUILayout.Button(_buttonNames[i])) {
					m.Invoke(_methodTypes[i], null);
				}
			}
		}
	}
}
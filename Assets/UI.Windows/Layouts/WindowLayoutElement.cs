﻿using System;
using UnityEngine;
using System.Collections.Generic;

namespace UnityEngine.UI.Windows {

	public class WindowLayoutElement : WindowLayoutBase {

		[ReadOnly("Layout Info")]
		new public LayoutTag tag = LayoutTag.None;
		
		[ReadOnly]
		public bool containsLayouts = false;

		#if UNITY_EDITOR
		[Header("Editor-Only Parameters")]
		public string comment;
		
		public bool autoStretchX = false;
		public bool autoStretchY = false;

		[HideInInspector]
		public WindowComponent tempEditorComponent;
		[HideInInspector]
		private bool randomColorSetup;
		[HideInInspector]
		public Color randomColor;
		[HideInInspector]
		public Rect editorRect;
		[HideInInspector]
		public Vector2 editorAnchorMin;
		[HideInInspector]
		public Vector2 editorAnchorMax;
		
		[HideInInspector]
		public float editorMinWidth = 0f;
		[HideInInspector]
		public float editorMinHeight = 0f;

		[HideInInspector]
		public Rect tempEditorRect;

		[HideInInspector]
		public int editorDrawDepth;

		[HideInInspector]
		public bool editorHovered;

		public void OnDrawGizmos() {
			
			// Hack to draw handles always

			if (this.randomColorSetup == false) {

				this.randomColor = UnityEngine.UI.Windows.Plugins.ColorHSV.GetDistinctColor();
				this.randomColorSetup = true;

			}

			this.OnDrawGUI_EDITOR(false, false);
			
		}
		
		public void OnDrawGizmosSelected() {

			var selected = (UnityEditor.Selection.activeGameObject == this.gameObject);
			this.OnDrawGUI_EDITOR(selected, true);
			
		}

		public void OnEnable() {

			if (Application.isPlaying == true) return;

			this.Reset();

		}

		[ContextMenu("Reset")]
		public void Reset() {

			this.tag = LayoutTag.None;

		}
		
		public override void OnValidate() {
			
			base.OnValidate();

			var layoutComponent = this.GetComponent<LayoutElement>();
			if (layoutComponent != null) {

				this.editorMinWidth = layoutComponent.minWidth;
				this.editorMinHeight = layoutComponent.minHeight;

			}

			this.containsLayouts = this.GetComponentsInChildren<WindowLayoutElement>(true).Length > 1;

			var layouts = this.GetComponentsInParent<WindowLayout>();
			if (layouts != null && layouts.Length > 0) {

				var layout = layouts[0];
				layout.OnValidate();

			}

		}

		VerticalLayoutGroup lay1;
		LayoutElement lay;
		private void OnDrawGUI_EDITOR(bool selected, bool selectedHierarchy) {

			var scale = 1f;
			var canvas = this.GetComponentInParent<Canvas>();
			if (canvas != null) scale = canvas.transform.localScale.x;

			var textStyle = new GUIStyle(GUI.skin.label);
			textStyle.fontStyle = FontStyle.Normal;
			textStyle.stretchWidth = true;
			textStyle.fontSize = 12;
			textStyle.richText = true;
			textStyle.alignment = TextAnchor.MiddleCenter;
			textStyle.wordWrap = false;

			var points = new Vector3[4];
			var rect = this.transform as RectTransform;
			rect.GetWorldCorners(points);

			var descr = "<b>" + this.tag.ToString() + "</b>\n" + this.comment + "\n" + "(Animation: " + (this.animation != null ? this.animation.name : "None") + ")" +
				(this.tempEditorComponent != null ? ("\n(Component: " + this.tempEditorComponent.name + ")") : "");

			var center = Vector3.zero;
			for (int i = 0; i < 4; ++i) center += points[i];
			center /= 4f;
			
			textStyle.fixedWidth = rect.rect.width;
			textStyle.fixedHeight = rect.rect.height;

			var face = this.randomColor;
			face.a = 0.3f;
			UnityEditor.Handles.DrawSolidRectangleWithOutline(points, face, this.randomColor);

			var shadowOffset = Vector3.one * 1f;
			shadowOffset.z = 0f;
			shadowOffset *= scale;

			var color = Color.black;
			color.a = 0.5f;
			textStyle.normal.textColor = color;
			UnityEditor.Handles.Label(center + shadowOffset, descr, textStyle);
			
			color = Color.white;
			color.a = (selected == true) ? 1f : 0.7f;
			textStyle.normal.textColor = color;
			UnityEditor.Handles.Label(center, descr, textStyle);
			
			color = Color.white;
			color.a = selected ? 1f : 0f;
			UnityEditor.Handles.color = color;

			var offset = 50f * scale;

			this.DrawLineWithOffset(offset, center, points[0], scale);
			this.DrawLineWithOffset(offset, center, points[1], scale);
			this.DrawLineWithOffset(offset, center, points[2], scale);
			this.DrawLineWithOffset(offset, center, points[3], scale);

			color = Color.white;
			color.a = 0.5f;
			UnityEditor.Handles.color = color;

		}

		private void DrawLineWithOffset(float offset, Vector3 pos1, Vector3 pos2, float scale) {

			var oldColor = UnityEditor.Handles.color;

			var shadowColor = new Color(0f, 0f, 0f, oldColor.a > 0f ? 0.5f : 0f);
			var shadowOffset = Vector3.one * 1f;
			shadowOffset.z = 0f;
			shadowOffset *= scale;

			var ray = new Ray(pos1, (pos2 - pos1).normalized);
			pos1 = ray.GetPoint(offset);

			UnityEditor.Handles.color = shadowColor;
			UnityEditor.Handles.DrawDottedLine(pos1 + shadowOffset, pos2 + shadowOffset, 10f);
			UnityEditor.Handles.color = oldColor;
			UnityEditor.Handles.DrawDottedLine(pos1, pos2, 10f);
			//UnityEditor.Handles.DrawLine(pos1, pos2);
			
			UnityEditor.Handles.color = oldColor;

		}
		#endif
		
	}
	
}


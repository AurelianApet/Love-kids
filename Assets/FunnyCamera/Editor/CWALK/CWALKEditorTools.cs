//----------------------------------------------
// Panel Mesh Editor
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------


using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;


public class CWALKEditorTools  {

	static Texture2D mBackdropTex;
	static Texture2D mContrastTex;


	static public T[] FindActive<T> () where T : Component
	{
		#if UNITY_3_5 || UNITY_4_0
		return GameObject.FindSceneObjectsOfType(typeof(T)) as T[];
		#else
		return GameObject.FindObjectsOfType(typeof(T)) as T[];
		#endif
	}


	/// <summary>
	/// Begin drawing the content area.
	/// </summary>
	static public void BeginContents ()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Space(4f);
		EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(10f));
		GUILayout.BeginVertical();
		GUILayout.Space(2f);
	}

	/// <summary>
	/// End drawing the content area.
	/// </summary>
	
	static public void EndContents ()
	{
		GUILayout.Space(3f);
		GUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(3f);
		GUILayout.EndHorizontal();
		GUILayout.Space(3f);
	}

	static public void DrawHeader (string text)
	{
		GUILayout.Space(3f);
		GUI.color = new Color(0f, 1f, 0f);
		GUILayout.BeginHorizontal();
		GUI.backgroundColor = Color.red;
		GUILayout.Space(3f);
		GUILayout.Label ("<b><size=11>" + text + "</size></b>", "dragtab");
		GUILayout.Space(2f);
		GUILayout.EndHorizontal();
	}

	/// <summary>
	/// Integer vector field.
	/// </summary>
	
	static public IntVector IntPair (string prefix, string leftCaption, string rightCaption, int x, int y)
	{
		GUILayout.BeginHorizontal();
		
		if (string.IsNullOrEmpty(prefix))
		{
			GUILayout.Space(82f);
		}
		else
		{
			GUILayout.Label(prefix, GUILayout.Width(74f));
		}
		
		CWALKEditorTools.SetLabelWidth(48f);
		IntVector vec2 = new IntVector();
		vec2.x = EditorGUILayout.IntField(leftCaption, x, GUILayout.MinWidth(30f));
		vec2.y = EditorGUILayout.IntField(rightCaption, y, GUILayout.MinWidth(30f));
		
		CWALKEditorTools.SetLabelWidth(80f);
		
		GUILayout.EndHorizontal();
		return vec2;
	}
	static public bool DrawPrefixButton (string text)
	{
		return GUILayout.Button(text, "DropDownButton", GUILayout.Width(100f));
	}

	/// <summary>
	///set the label height;
	/// </summary>
	
	static public void SetLabelWidth (float width)
	{
		EditorGUIUtility.labelWidth = width;
	}
/*
	static public void DrawSprite (Texture2D tex, Rect rect,StickerBorderData borderData ,Color color)
	{
		DrawSprite(tex, rect, borderData, color, null);
	}
*/
	/// <summary>
	/// Struct type for the integer vector field below.
	/// </summary>
	
	public struct IntVector
	{
		public int x;
		public int y;
	}

	static public void RegisterUndo (string name, params Object[] objects)
	{
		if (objects != null && objects.Length > 0)
		{
			UnityEditor.Undo.RecordObjects(objects, name);
			foreach (Object obj in objects)
			{
				if (obj == null) continue;
				EditorUtility.SetDirty(obj);
			}
		}
	}

	static public void DrawSeparator ()
	{
		GUILayout.Space(12f);
		
		if (Event.current.type == EventType.Repaint)
		{
			Texture2D tex = blankTexture;
			Rect rect = GUILayoutUtility.GetLastRect();
			GUI.color = new Color(0f, 0f, 0f, 0.25f);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 4f), tex);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 1f), tex);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 9f, Screen.width, 1f), tex);
			GUI.color = Color.white;
		}
	}

	static public Texture2D blankTexture
	{
		get
		{
			return EditorGUIUtility.whiteTexture;
		}
	}

	static public void DrawOutline (Rect rect, Color color)
	{
		if (Event.current.type == EventType.Repaint)
		{
			Texture2D tex = blankTexture;
			GUI.color = color;
			GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, 1f, rect.height), tex);
			GUI.DrawTexture(new Rect(rect.xMax, rect.yMin, 1f, rect.height), tex);
			GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, rect.width, 1f), tex);
			GUI.DrawTexture(new Rect(rect.xMin, rect.yMax, rect.width, 1f), tex);
			GUI.color = Color.white;
		}
	}

	/// <summary>
	/// Draw a sprite preview.
	/// </summary>
	/*
	static public void DrawSprite (Texture2D tex, Rect drawRect, StickerBorderData borderData,Color color, Material mat)
	{
		if (tex == null) return;

		// Create the texture rectangle that is centered inside rect.
		Rect outerRect = drawRect;
		outerRect.width = tex.width;
		outerRect.height = tex.height;
		
		if (tex.width > 0)
		{
			float f = drawRect.width / outerRect.width;
			outerRect.width *= f;
			outerRect.height *= f;
		}
		
		if (drawRect.height > outerRect.height)
		{
			outerRect.y += (drawRect.height - outerRect.height) * 0.5f;
		}
		else if (outerRect.height > drawRect.height)
		{
			float f = drawRect.height / outerRect.height;
			outerRect.width *= f;
			outerRect.height *= f;
		}
		
		if (drawRect.width > outerRect.width) outerRect.x += (drawRect.width - outerRect.width) * 0.5f;
		
		// Draw the background
		CWALKEditorTools.DrawTiledTexture(outerRect, CWALKEditorTools.backdropTexture);
		
		// Draw the sprite
		GUI.color = color;
		
		if (mat == null)
		{
			Rect uv = new Rect(0,0, tex.width, tex.height);
			uv = NGUIMath.ConvertToTexCoords(uv, tex.width, tex.height);
			GUI.DrawTextureWithTexCoords(outerRect, tex, uv, true);
		}
		else
		{
			// NOTE: There is an issue in Unity that prevents it from clipping the drawn preview
			// using BeginGroup/EndGroup, and there is no way to specify a UV rect... le'suq.
			UnityEditor.EditorGUI.DrawPreviewTexture(outerRect, tex, mat);
		}
		
		// Draw the border indicator lines
		GUI.BeginGroup(outerRect);
		{
			tex = CWALKEditorTools.contrastTexture;
			GUI.color = Color.white;
			
			if (borderData.borderLeft > 0)
			{
				float x0 = (float)borderData.borderLeft / borderData.width * outerRect.width - 1;
				CWALKEditorTools.DrawTiledTexture(new Rect(x0, 0f, 1f, outerRect.height), tex);
			}
			
			if (borderData.borderRight > 0)
			{
				float x1 = (float)(borderData.width - borderData.borderRight) / borderData.width * outerRect.width - 1;
				CWALKEditorTools.DrawTiledTexture(new Rect(x1, 0f, 1f, outerRect.height), tex);
			}
			
			if (borderData.borderBottom > 0)
			{
				float y0 = (float)(borderData.height - borderData.borderBottom) / borderData.height * outerRect.height - 1;
				CWALKEditorTools.DrawTiledTexture(new Rect(0f, y0, outerRect.width, 1f), tex);
			}
			
			if (borderData.borderTop > 0)
			{
				float y1 = (float)borderData.borderTop / borderData.height * outerRect.height - 1;
				CWALKEditorTools.DrawTiledTexture(new Rect(0f, y1, outerRect.width, 1f), tex);
			}
		}
		GUI.EndGroup();
		
		// Draw the lines around the sprite
		Handles.color = Color.black;
		Handles.DrawLine(new Vector3(outerRect.xMin, outerRect.yMin), new Vector3(outerRect.xMin, outerRect.yMax));
		Handles.DrawLine(new Vector3(outerRect.xMax, outerRect.yMin), new Vector3(outerRect.xMax, outerRect.yMax));
		Handles.DrawLine(new Vector3(outerRect.xMin, outerRect.yMin), new Vector3(outerRect.xMax, outerRect.yMin));
		Handles.DrawLine(new Vector3(outerRect.xMin, outerRect.yMax), new Vector3(outerRect.xMax, outerRect.yMax));
		
		// Sprite size label
		string text = string.Format("Sticker Boder Size: {0}x{1}", Mathf.RoundToInt(borderData.width), Mathf.RoundToInt(borderData.height));
		EditorGUI.DropShadowLabel(GUILayoutUtility.GetRect(Screen.width, 18f), text);
	}
	*/

	/// <summary>
	/// draw a bg texture
	/// </summary>
	
	static public void DrawTiledTexture (Rect rect, Texture tex)
	{
		GUI.BeginGroup(rect);
		{
			int width  = Mathf.RoundToInt(rect.width);
			int height = Mathf.RoundToInt(rect.height);
			
			for (int y = 0; y < height; y += tex.height)
			{
				for (int x = 0; x < width; x += tex.width)
				{
					GUI.DrawTexture(new Rect(x, y, tex.width, tex.height), tex);
				}
			}
		}
		GUI.EndGroup();
	}

	/// <summary>
	/// Returns a dark bg
	/// </summary>
	
	static public Texture2D backdropTexture
	{
		get
		{
			if (mBackdropTex == null) mBackdropTex = CreateCheckerTex(
				new Color(0.1f, 0.1f, 0.1f, 0.5f),
				new Color(0.2f, 0.2f, 0.2f, 0.5f));
			return mBackdropTex;
		}
	}

	static Texture2D CreateCheckerTex (Color c0, Color c1)
	{
		Texture2D tex = new Texture2D(16, 16);
		tex.name = "[52Cwalk] background Texture";
		tex.hideFlags = HideFlags.DontSave;
		
		for (int y = 0; y < 8; ++y) for (int x = 0; x < 8; ++x) tex.SetPixel(x, y, c1);
		for (int y = 8; y < 16; ++y) for (int x = 0; x < 8; ++x) tex.SetPixel(x, y, c0);
		for (int y = 0; y < 8; ++y) for (int x = 8; x < 16; ++x) tex.SetPixel(x, y, c0);
		for (int y = 8; y < 16; ++y) for (int x = 8; x < 16; ++x) tex.SetPixel(x, y, c1);
		
		tex.Apply();
		tex.filterMode = FilterMode.Point;
		return tex;
	}



	static public Rect ConvertToTexCoords (Rect rect, int width, int height)
	{
		Rect final = rect;
		
		if (width != 0f && height != 0f)
		{
			final.xMin = rect.xMin / width;
			final.xMax = rect.xMax / width;
			final.yMin = 1f - rect.yMax / height;
			final.yMax = 1f - rect.yMin / height;
		}
		return final;
	}

	/// <summary>
	/// Returns a usable texture that looks like a high-contrast checker board.
	/// </summary>
	
	static public Texture2D contrastTexture
	{
		get
		{
			if (mContrastTex == null) mContrastTex = CreateCheckerTex(
				new Color(0f, 0.0f, 0f, 0.5f),
				new Color(1f, 1f, 1f, 0.5f));
			return mContrastTex;
		}
	}




	/// <summary>
	/// Load the asset at the specified path.
	/// </summary>

	static public Object LoadAsset (string path)
	{
		if (string.IsNullOrEmpty(path)) return null;
		return AssetDatabase.LoadMainAssetAtPath(path);
	}


	/// <summary>
	/// Convenience function to load an asset of specified type, given the full path to it.
	/// </summary>
	
	static public T LoadAsset<T> (string path) where T: Object
	{
		Object obj = LoadAsset(path);
		if (obj == null) return null;
		
		T val = obj as T;
		if (val != null) return val;
		
		if (typeof(T).IsSubclassOf(typeof(Component)))
		{
			if (obj.GetType() == typeof(GameObject))
			{
				GameObject go = obj as GameObject;
				return go.GetComponent(typeof(T)) as T;
			}
		}
		return null;
	}

}

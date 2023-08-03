//----------------------------------------------
// Panel Mesh Editor
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Editor component used to display a list of sprites.
/// </summary>

public class MeshSpriteSelector : ScriptableWizard
{
	static public MeshSpriteSelector instance;
	
	void OnEnable () { instance = this; }
	void OnDisable () { instance = null; }
	
	public delegate void Callback (Texture sprite);
	
	SerializedObject mObject;
	SerializedProperty mProperty;
	//MeshSprite
	MeshSprite mSprite;
	Vector2 mPos = Vector2.zero;
	Callback mCallback;
	float mClickTime = 0f;

	/// <summary>
	/// Draw the custom wizard.
	/// </summary>
	
	void OnGUI ()
	{
		CWALKEditorTools.SetLabelWidth(80f);
		
		if (CWALKSettingTools.S_MeshSpriteMaker == null)
		{
			GUILayout.Label("No config selected.", "LODLevelNotifyText");
		}
		else
		{
			bool close = false;
			CWALKEditorTools.DrawSeparator();
			
			List<Texture2D> texturelist = CWALKSettingTools.S_MeshSpriteMaker.textureList;
			
			if(texturelist == null)
			{
				return ;
			}
			float size = 80f;
			float padded = size + 10f;
			int columns = Mathf.FloorToInt(Screen.width / padded);
			if (columns < 1) columns = 1;
			
			int offset = 0;
			Rect rect = new Rect(10f, 0, size, size);
			
			GUILayout.Space(10f);
			mPos = GUILayout.BeginScrollView(mPos);
			int rows = 1;
			
			while (offset < texturelist.Count)
			{
				GUILayout.BeginHorizontal();
				{
					int col = 0;
					rect.x = 10f;
					
					for (; offset < texturelist.Count; ++offset)
					{
						Texture	tex =texturelist[offset];
						
						if (tex == null) continue;
						
						// Button comes first
						if (GUI.Button(rect, ""))
						{
							if (Event.current.button == 0)
							{
								float delta = Time.realtimeSinceStartup - mClickTime;
								mClickTime = Time.realtimeSinceStartup;
								
								if (CWALKSettingTools.S_SelectedSprite != tex.name)
								{
									if (mSprite != null)
									{
										mSprite.mainTexture = tex;
										EditorUtility.SetDirty(mSprite.gameObject);
									}
									
									CWALKSettingTools.S_SelectedSprite = tex.name;
									if (mCallback != null) mCallback(tex);
								}
								else if (delta < 0.5f) close = true;
							}
							else
							{
								
							}
						}
						
						if (Event.current.type == EventType.Repaint)
						{
							
							// Calculate the texture's scale that's needed to display the sprite in the clipped area
							float scaleX = rect.width / tex.width;
							float scaleY = rect.height / tex.height;
							
							// Stretch the sprite so that it will appear proper
							float aspect = (scaleY / scaleX) / ((float)tex.height / tex.width);
							Rect clipRect = rect;
							
							if (aspect != 1f)
							{
								if (aspect < 1f)
								{
									// The sprite is taller than it is wider
									float padding = size * (1f - aspect) * 0.5f;
									clipRect.xMin += padding;
									clipRect.xMax -= padding;
								}
								else
								{
									// The sprite is wider than it is taller
									float padding = size * (1f - 1f / aspect) * 0.5f;
									clipRect.yMin += padding;
									clipRect.yMax -= padding;
								}
							}
							
							GUI.DrawTexture(clipRect, tex);
							
							// Draw the selection
							if (CWALKSettingTools.S_SelectedSprite == tex.name)
							{
								CWALKEditorTools.DrawOutline(rect, new Color(0.4f, 1f, 0f, 1f));
							}
						}
						
						GUI.backgroundColor = new Color(1f, 1f, 1f, 0.5f);
						GUI.contentColor = new Color(1f, 1f, 1f, 0.7f);
						GUI.Label(new Rect(rect.x, rect.y + rect.height, rect.width, 32f), tex.name, "ProgressBarBack");
						GUI.contentColor = Color.white;
						GUI.backgroundColor = Color.white;
						
						if (++col >= columns)
						{
							++offset;
							break;
						}
						rect.x += padded;
					}
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(padded);
				rect.y += padded + 26;
				++rows;
			}
			GUILayout.Space(rows * 26);
			GUILayout.EndScrollView();
			
			if (close) Close();
		}
	}
	
	/// <summary>
	/// Show the selection wizard.
	/// </summary>
	
	static public void Show (Callback callback)
	{
		if (instance != null)
		{
			instance.Close();
			instance = null;
		}
		
		MeshSpriteSelector comp = ScriptableWizard.DisplayWizard<MeshSpriteSelector>("Select a Mesh Texture");
		comp.mSprite = null;
		comp.mCallback = callback;
	}
}

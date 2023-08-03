//----------------------------------------------
// Panel Mesh Editor
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using CWALK;

[CustomEditor(typeof(MeshSpriteMaker))]

public class MeshSpriteMakerInspector : Editor {

	private int maxLength = 128;
	private Vector2 m_scrollPosition;
	void OnEnable()
	{
		var meshspritemaker = target as MeshSpriteMaker;
		if (meshspritemaker != null) {
			CWALKSettingTools.S_MeshSpriteMaker  = meshspritemaker;
		}
	}

	void MarkSpriteAsDirty ()
	{
		var meshspritemaker = target as MeshSpriteMaker;

		MeshSprite[] sprites = CWALKEditorTools.FindActive<MeshSprite>();
		
		foreach (MeshSprite ms in sprites)
		{
			if (MeshSpriteMaker.CheckIfRelated(ms.meshSpriteMaker, meshspritemaker))
			{
				ms.UpdateMesh();
				EditorUtility.SetDirty(ms);
			}
		}
	}



	public override void OnInspectorGUI ()
	{
		var meshspritemaker = target as MeshSpriteMaker;

		base.OnInspectorGUI ();
		GUILayout.Space (6f);

		if (meshspritemaker != null) {

			Color tempBgcolor = GUI.backgroundColor;

			GUI.backgroundColor = Color.green;
			//First lets create a button that will save all of the item data
			if (GUILayout.Button (new GUIContent ("Save Config"))) {
				if(meshspritemaker != null)
				{
					EditorUtility.SetDirty(meshspritemaker.gameObject);
				}
			}
			CWALKEditorTools.RegisterUndo ("Sprite Maker Change", meshspritemaker);
			//Then lets create a button that will add a brand new MeshSpriteData to the list and add a toggle to the foldout list
			if (GUILayout.Button (new GUIContent ("Add New Item"))) {
				meshspritemaker.AddItem();
			}

			for (int i = 0; meshspritemaker.items != null && i < meshspritemaker.items.Count; i++) {
				GUI.backgroundColor = tempBgcolor;
				meshspritemaker.itemFoldouts [i] = EditorGUILayout.Foldout (meshspritemaker.itemFoldouts [i], new GUIContent (meshspritemaker.items [i].name + " Info"));
				if (meshspritemaker.itemFoldouts[i] == true) {
					
					meshspritemaker.items [i].name = EditorGUILayout.TextField (new GUIContent ("name"), meshspritemaker.items [i].name);
					GUILayout.Space (5f);
					meshspritemaker.items[i].Mode = (MeshSpriteMode)EditorGUILayout.EnumPopup ("Mesh Mode :",meshspritemaker.items[i].Mode);
					
					GUILayout.Space (5f);
					CWALKEditorTools.IntVector dimensionVec = CWALKEditorTools.IntPair ("Dimension", "Width:", "Height:", 
					                                                                      meshspritemaker.items [i].texWidth, 
					                                                                      meshspritemaker.items [i].texHeight);



					GUI.backgroundColor = Color.blue;
					GUILayout.Space (6f);
					CWALKEditorTools.IntVector leftRightVec = CWALKEditorTools.IntPair ("Set Border", "Left:", "Right:", 
					                                                                      meshspritemaker.items [i].LeftBorder, 
					                                                                      meshspritemaker.items [i].RightBorder);
					
					CWALKEditorTools.IntVector topBottomVec = CWALKEditorTools.IntPair (" ", "Top:", "Bottom:", 
					                                                                    meshspritemaker.items [i].BottomBorder, 
					                                                                      meshspritemaker.items [i].TopBorder);



					meshspritemaker.items [i].texWidth = dimensionVec.x;
					meshspritemaker.items [i].texHeight = dimensionVec.y;
					
					meshspritemaker.items [i].LeftBorder = leftRightVec.x;
					meshspritemaker.items [i].RightBorder = leftRightVec.y;
					meshspritemaker.items [i].TopBorder =topBottomVec.y;
					meshspritemaker.items [i].BottomBorder = topBottomVec.x;

					Texture2D t = EditorGUILayout.ObjectField("Sprite Tex", meshspritemaker.textureList[i], typeof(Texture2D), true) as Texture2D;

					meshspritemaker.textureList[i] = t;

					if(t != null)
					{
						if(t != meshspritemaker.textureList[i])
						{
							meshspritemaker.textureList[i] = t;
						}
						meshspritemaker.items[i].name = t.name;
						meshspritemaker.items[i].texWidth = t.width;
						meshspritemaker.items[i].texHeight = t.height;	
					}

					if(t != null)
					{
						float changeFator =1.0f;
						if(maxLength < t.width ||
						   maxLength < t.height)
						{
							if(t.width > t.height)
							{
								changeFator = maxLength*1.0f/t.width;
							}
							else
							{
								changeFator = maxLength*1.0f/t.height;
							}
						}

						int width = Mathf.CeilToInt( t.width * changeFator);
						int height =Mathf.CeilToInt( t.height * changeFator);
						
						Rect rect = GUILayoutUtility.GetRect(width, height);
						GUI.BeginGroup(rect);//clipping
						
						Texture2D linetex = CWALKEditorTools.contrastTexture;
						//Texture2D bgtex = CWALKEditorTools.backdropTexture;
						GUI.color = Color.white;
						
						GUI.Box(new Rect(0,0,width,height),"");
						GUI.DrawTexture(new Rect(0,0,width,height),t);
						
						if (meshspritemaker.items [i].LeftBorder > 0)
						{
							float x0 = (float)meshspritemaker.items [i].LeftBorder / meshspritemaker.items [i].texWidth * width - 1;
							CWALKEditorTools.DrawTiledTexture(new Rect(x0, 0f, 1f, height), linetex);
						}
						
						if (meshspritemaker.items [i].RightBorder > 0)
						{
							float x1 = (float)(meshspritemaker.items[i].texWidth - meshspritemaker.items[i].RightBorder) / meshspritemaker.items[i].texWidth*width - 1;
							CWALKEditorTools.DrawTiledTexture(new Rect(x1, 0f, 1f, height), linetex);
						}
						
						if (meshspritemaker.items [i].TopBorder > 0)
						{
							float y0 = (float)(meshspritemaker.items [i].texHeight - meshspritemaker.items [i].TopBorder) / meshspritemaker.items [i].texHeight *height - 1;
							CWALKEditorTools.DrawTiledTexture(new Rect(0f, y0, width, 1f), linetex);
						}
						
						if (meshspritemaker.items [i].BottomBorder >0)
						{
							float y1 = (float)meshspritemaker.items [i].BottomBorder / meshspritemaker.items [i].texHeight * height- 1;
							CWALKEditorTools.DrawTiledTexture(new Rect(0f, y1, width, 1f), linetex);
						}
						
						GUI.EndGroup();              
						
					}
					else
					{
						Color tempcolor1 = GUI.color;
						GUI.color = Color.red;
						GUILayout.Box( "Perview Texture is null ",GUILayout.Width(maxLength),GUILayout.Height(maxLength));
						GUI.color = tempcolor1;
					}


					GUI.backgroundColor = Color.red;
					//We should also allow the user to remove the selected item so lets give them that option here
					if (GUILayout.Button (new GUIContent ("Remove Item"), GUILayout.Width (Screen.width*0.87f))) {
						meshspritemaker.items .RemoveAt (i);
						meshspritemaker.itemFoldouts .RemoveAt (i);
						i--;
					}
					GUI.backgroundColor = tempBgcolor;

				}

				CWALKEditorTools.RegisterUndo ("Sprite Maker Change", meshspritemaker);
				
			}

		}

		if (GUI.changed) {
			MarkSpriteAsDirty ();
		}

	}
	
	
}

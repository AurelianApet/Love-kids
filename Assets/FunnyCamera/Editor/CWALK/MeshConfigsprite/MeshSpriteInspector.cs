//----------------------------------------------
// Panel Mesh Editor
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEditor;
using CWALK;

[CustomEditor(typeof(MeshSprite))]

public class MeshSpriteInspector : Editor {

	private static MeshSpriteMaker s_meshSpriteMaker = null;
	
	void OnSelectAtlas (Texture tex)
	{
		var meshsprite = target as MeshSprite;
		if (meshsprite != null) {
			meshsprite.SpriteName = tex.name;
			EditorUtility.SetDirty(meshsprite.gameObject);
		}
	}
	
	void OnEnable()
	{
		var meshsprite = target as MeshSprite;
		if (meshsprite != null) {
			meshsprite.Init ();
		}
	}
	
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		GUILayout.Space(6f);
		var meshsprite = target as MeshSprite;
		
		if (meshsprite != null) {
			
			s_meshSpriteMaker = meshsprite.meshSpriteMaker;
			
			CWALKEditorTools.BeginContents ();
			CWALKEditorTools.RegisterUndo ("Sprite Change", meshsprite);
			MeshSpriteMaker SpriteAssemble = EditorGUILayout.ObjectField("Mesh Sprite Maker:", s_meshSpriteMaker, typeof(MeshSpriteMaker), false) as MeshSpriteMaker;

			if(SpriteAssemble == null)
			{
				meshsprite.meshSpriteMaker = null;
				Color tempBgcolor = GUI.backgroundColor;
				GUI.backgroundColor = Color.red;
				EditorGUILayout.HelpBox("No MeshSpriteMaker file found,Please Check On", MessageType.Warning);
				GUI.backgroundColor = tempBgcolor;
				return;
			}
			else
			{
				if(SpriteAssemble != meshsprite.meshSpriteMaker)
				{
					meshsprite.meshSpriteMaker = SpriteAssemble;
				}
			}
	
			if(meshsprite.material ==null)
			{	
				GUILayout.Space(6f);
				Color tempBgcolor = GUI.backgroundColor;
				GUI.backgroundColor = Color.red;
				EditorGUILayout.HelpBox("No Material On Render", MessageType.Warning);
				GUI.backgroundColor = tempBgcolor;
				return;
			}
			else
			{
				if(meshsprite.material.mainTexture == null)
				{
					meshsprite.selectDefaultTex();
				}
			}

			CWALKEditorTools.RegisterUndo ("Sprite Change", meshsprite);
			GUILayout.BeginHorizontal();
			if (CWALKEditorTools.DrawPrefixButton("Mesh Sprite"))
			{
				CWALKSettingTools.S_MeshSpriteMaker = meshsprite.meshSpriteMaker ;
				if( meshsprite.mainTexture !=null)
				{
					CWALKSettingTools.S_SelectedSprite = meshsprite.mainTexture.name;
				}
				MeshSpriteSelector.Show(OnSelectAtlas);
			}

			GUILayout.Space(14f);
			if(meshsprite.mainTexture !=null)
			{
				GUILayout.Label(meshsprite.mainTexture.name, "helpbox");
			}
			else
			{
				
				Color tempBgcolor = GUI.backgroundColor;
				GUI.backgroundColor = Color.red;
				GUILayout.Label("material texture is null", "helpbox");
				GUI.backgroundColor = tempBgcolor;
				//
			}
			
			GUILayout.EndHorizontal();

			//MeshSpriteMode meshmode = (MeshSpriteMode) EditorGUILayout.EnumPopup("Mesh Mode :", meshsprite.meshMode);
			CWALKEditorTools.RegisterUndo ("Sprite Change", meshsprite);
			Color tempBgColor = GUI.backgroundColor;
			
			GUI.backgroundColor = Color.green;
			GUILayout.Space(5f);
			MeshSpriteMode meshmode = (MeshSpriteMode) EditorGUILayout.EnumPopup("Mesh Mode:", meshsprite.meshMode);
			GUILayout.Space(5f);
			CWALKEditorTools.RegisterUndo ("Sprite Change", meshsprite);
			float spritewidth = EditorGUILayout.FloatField ("Sprite Width:",meshsprite.Width);
			spritewidth = Mathf.Clamp(spritewidth,1f,1000f);
			
			float spriteheight = EditorGUILayout.FloatField ("Sprite Height:",meshsprite.Height);
			spriteheight = Mathf.Clamp(spriteheight,1f,1000f);

			if(GUI.changed)
			{
				meshsprite.Width = spritewidth;
				meshsprite.Height = spriteheight;
				meshsprite.meshMode = meshmode;
				//		meshsprite.borderColor = _borderColor;
				EditorUtility.SetDirty(meshsprite);
			}
			
			GUI.backgroundColor = tempBgColor;
			CWALKEditorTools.EndContents();
		}
	}
	
	
	void OnSelectAtlas (Object obj)
	{
		serializedObject.Update();
		SerializedProperty sp = serializedObject.FindProperty("Config");
		sp.objectReferenceValue = obj;
		serializedObject.ApplyModifiedProperties();
	}



}

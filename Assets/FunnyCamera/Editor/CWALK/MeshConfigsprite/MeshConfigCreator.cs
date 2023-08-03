//----------------------------------------------
// Panel Mesh Editor
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------

using UnityEngine;

//This is going to be an editor window script, so we need to include UnityEditor
using UnityEditor;

//We are going to be using a generic list to store the item data so we need to include the System.Collections.Generic library
using System.Collections.Generic;

using CWALK;

//This is an editor window class so we are going to want to make sure that the class extends from EditorWindow
public class MeshAtlasCreator : EditorWindow
{ 
	string atlasName = "New MeshAtlas";
	[MenuItem("MeshSprite/Create MeshAtlas")]
	static void init ()
	{
		EditorWindow.GetWindow<MeshAtlasCreator> ();
	}

	//Now we need to display all of the item editing stuff, so we need to create an OnGUI function
	private void OnGUI ()
	{
		GUILayout.Space(10f);
		GUILayout.Space(6f);
		GUILayout.BeginHorizontal();
		bool create = false;
		GUI.backgroundColor = Color.green;
		create = GUILayout.Button("Create", GUILayout.Width(76f));

		GUI.backgroundColor = Color.white;
		atlasName = GUILayout.TextField(atlasName);
		GUILayout.EndHorizontal();

		GUILayout.Space(10f);
		if (create) {
			// Create a new prefab for the atlas

			string prefabPath =	"Assets/" + atlasName + ".prefab";

			Object prefab =  PrefabUtility.CreateEmptyPrefab(prefabPath);
			
			// Create a new game object for the atlas
			GameObject go = new GameObject(atlasName);
			go.AddComponent<MeshSpriteMaker>();
			// Update the prefab
			PrefabUtility.ReplacePrefab(go, prefab);
			DestroyImmediate(go);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

		}
		EditorGUILayout.HelpBox("Please input the MeshSpriteAtlas's name you want,and clicked the 'create' button,then it will create a new perfab of MeshSpriteAtlas at Assets/xxx.perfab", MessageType.Info);

 	
	}

}

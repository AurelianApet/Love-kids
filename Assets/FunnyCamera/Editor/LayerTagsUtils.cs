#if !(UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7)
#define UNITY_5_OR_LATER
#endif

using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[InitializeOnLoad]
public static class LayerTagsUtils
{
	private static string[] tags = { "StickerItem", "TextItem","StickerCloseBtn", "TextItemCloseBtn","TextItemRotateScaleBtn","StickerRotateScaleBtn","itemPlane" };

	private static string[] Layers = { "TextItem", "StickerItem" };

	static LayerTagsUtils()
	{
		CheckLayers (Layers);
		CheckTags (tags);
        
	}

	// will check if the specified layer names are present and add any missing ones
	// it will simply add them from the first open slots so do not depend on any
	// specific order but rather grab layers from the layer names at runtime
	public static void CheckLayers(string[] layerNames)
	{
		SerializedObject manager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
		#if !UNITY_4
		SerializedProperty layersProp = manager.FindProperty("layers");
		#endif
		
		foreach (string name in layerNames)
		{
			// check if layer is present
			bool found = false;
			for (int i = 0; i <= 31; i++)
			{
				#if UNITY_4
				string nm = "User Layer " + i;
				SerializedProperty sp = manager.FindProperty(nm);
				#else
				SerializedProperty sp = layersProp.GetArrayElementAtIndex(i);
				#endif
				if (sp != null && name.Equals(sp.stringValue))
				{
					found = true;
					break;
				}
			}
			
			// not found, add into 1st open slot
			if (!found)
			{
				SerializedProperty slot = null;
				for (int i = 8; i <= 31; i++)
				{
					#if UNITY_4
					string nm = "User Layer " + i;
					SerializedProperty sp = manager.FindProperty(nm);
					#else
					SerializedProperty sp = layersProp.GetArrayElementAtIndex(i);
					#endif
					if (sp != null && string.IsNullOrEmpty(sp.stringValue))
					{
						slot = sp;
						break;
					}
				}
				
				if (slot != null)
				{
					slot.stringValue = name;
				}
				else
				{
					Debug.LogError("Could not find an open Layer Slot for: " + name);
				}
			}
		}
		
		// save
		manager.ApplyModifiedProperties();
	}
	
	public static void CheckTags(string[] tagNames)
	{
		SerializedObject manager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
		SerializedProperty tagsProp = manager.FindProperty("tags");
		
		List<string> DefaultTags = new List<string>(){ "Untagged", "Respawn", "Finish", "EditorOnly", "MainCamera", "Player", "GameController" };
		
		foreach (string name in tagNames)
		{
			if (DefaultTags.Contains(name)) continue;
			
			// check if tag is present
			bool found = false;
			for (int i = 0; i < tagsProp.arraySize; i++)
			{
				SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
				if (t.stringValue.Equals(name)) { found = true; break; }
			}
			
			// if not found, add it
			if (!found)
			{
				tagsProp.InsertArrayElementAtIndex(0);
				SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
				n.stringValue = name;
			}
		}
		
		// save
		manager.ApplyModifiedProperties();
	}
	
	public static void CheckSortLayers(string[] tagNames)
	{
		SerializedObject manager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
		SerializedProperty sortLayersProp = manager.FindProperty("m_SortingLayers");
		
		//for (int i = 0; i < sortLayersProp.arraySize; i++)
		//{ // used to figure out how all of this works and what properties values look like
		//    SerializedProperty entry = sortLayersProp.GetArrayElementAtIndex(i);
		//    SerializedProperty name = entry.FindPropertyRelative("name");
		//    SerializedProperty unique = entry.FindPropertyRelative("uniqueID");
		//    SerializedProperty locked = entry.FindPropertyRelative("locked");
		//    Debug.Log(name.stringValue + " => " + unique.intValue + " => " + locked.boolValue);
		//}
		
		foreach (string name in tagNames)
		{
			// check if tag is present
			bool found = false;
			for (int i = 0; i < sortLayersProp.arraySize; i++)
			{
				SerializedProperty entry = sortLayersProp.GetArrayElementAtIndex(i);
				SerializedProperty t = entry.FindPropertyRelative("name");
				if (t.stringValue.Equals(name)) { found = true; break; }
			}
			
			// if not found, add it
			if (!found)
			{
				manager.ApplyModifiedProperties();
				AddSortingLayer();
				manager.Update();
				
				int idx = sortLayersProp.arraySize - 1;
				SerializedProperty entry = sortLayersProp.GetArrayElementAtIndex(idx);
				SerializedProperty t = entry.FindPropertyRelative("name");
				t.stringValue = name;
			}
		}
		
		// save
		manager.ApplyModifiedProperties();
	}
	
	// you need 'using System.Reflection;' for these
	private static Assembly editorAsm;
	private static MethodInfo AddSortingLayer_Method;
	
	/// <summary> add a new sorting layer with default name </summary>
	public static void AddSortingLayer()
	{
		if (AddSortingLayer_Method == null)
		{
			if (editorAsm == null) editorAsm = Assembly.GetAssembly(typeof(Editor));
			System.Type t = editorAsm.GetType("UnityEditorInternal.InternalEditorUtility");
			AddSortingLayer_Method = t.GetMethod("AddSortingLayer", (BindingFlags.Static | BindingFlags.NonPublic), null, new System.Type[0], null);
		}
		AddSortingLayer_Method.Invoke(null, null);
	}


}

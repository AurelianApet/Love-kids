//----------------------------------------------
// Panel Mesh Editor
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CWALKSettingTools {


	static public string S_SelectedSprite
	{
		get { return GetString("Mesh Sprite", null); }
		set { SetString("Mesh Sprite", value); }
	}

	static public TextAsset S_Configxml
	{
		get { return Get<TextAsset>("Mesh Config", null); }
		set { Set("Mesh Config", value); }
	}


	static public MeshSpriteMaker S_MeshSpriteMaker
	{
		get { return Get<MeshSpriteMaker>("sprite Assemble", null); }
		set { Set("sprite Assemble", value); }
	}
	
	/// <summary>
	/// Get the previously saved boolean value.
	/// </summary>
	
	static public bool GetBool (string name, bool defaultValue) { return EditorPrefs.GetBool(name, defaultValue); }
	
	/// <summary>
	/// Get the previously saved integer value.
	/// </summary>
	
	static public int GetInt (string name, int defaultValue) { return EditorPrefs.GetInt(name, defaultValue); }
	
	/// <summary>
	/// Get the previously saved float value.
	/// </summary>
	
	static public float GetFloat (string name, float defaultValue) { return EditorPrefs.GetFloat(name, defaultValue); }
	
	/// <summary>
	/// Get the previously saved string value.
	/// </summary>
	
	static public string GetString (string name, string defaultValue) { return EditorPrefs.GetString(name, defaultValue); }
	/// <summary>
	/// Save the specified string value in settings.
	/// </summary>
	
	static public void SetString (string name, string val) { EditorPrefs.SetString(name, val); }


	/// <summary>
	/// Get a previously saved object from settings.
	/// </summary>
	
	static public T Get<T> (string name, T defaultValue) where T : Object
	{
		string path = EditorPrefs.GetString(name);
		if (string.IsNullOrEmpty(path)) return null;
		T retVal = CWALKEditorTools.LoadAsset<T>(path);
		
		if (retVal == null)
		{
			int id;
			if (int.TryParse(path, out id))
				return EditorUtility.InstanceIDToObject(id) as T;
		}
		return retVal;

	}

	/// <summary>
	/// Save the specified object in settings.
	/// </summary>
	
	static public void Set (string name, Object obj)
	{
		if (obj == null)
		{
			EditorPrefs.DeleteKey(name);
		}
		else
		{
			if (obj != null)
			{
				string path = AssetDatabase.GetAssetPath(obj);
				
				if (!string.IsNullOrEmpty(path))
				{
					EditorPrefs.SetString(name, path);
				}
				else
				{
					EditorPrefs.SetString(name, obj.GetInstanceID().ToString());
				}
			}
			else EditorPrefs.DeleteKey(name);
		}
	}





}

//----------------------------------------------
// Panel Mesh Editor
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

static public class MenuController
{
#region Create

	[MenuItem("MeshSprite/Create MeshSprite", false, 6)]
	static public void AddSprite ()
	{
		//Instantiate
		GameObject meshspriteobj =  new GameObject();
		meshspriteobj.name = "meshspriteobj";
		MeshSprite spriteobj = meshspriteobj.AddComponent<MeshSprite> ();
		meshspriteobj.GetComponent<Renderer> ().material = new Material (Shader.Find("Unlit/Transparent"));

		spriteobj.meshSpriteMaker = CWALKSettingTools.S_MeshSpriteMaker;

	}




#endregion

}

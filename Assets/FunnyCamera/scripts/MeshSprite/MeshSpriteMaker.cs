//----------------------------------------------
// Panel Mesh Editor
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CWALK;

public class MeshSpriteMaker : MonoBehaviour {

	[HideInInspector][SerializeField]
	private  List<bool> s_itemFoldouts;
	public List<bool> itemFoldouts
	{
		get
		{
			return s_itemFoldouts;
		}
	}

	[HideInInspector][SerializeField]
	private  List<MeshSpriteData> s_items;
	public List<MeshSpriteData> items
	{
		get
		{
			return s_items;
		}
	}

	[HideInInspector][SerializeField]
	private  List<Texture2D> s_TexList;
	public List<Texture2D> textureList
	{
		get
		{
			return s_TexList;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddItem()
	{
		s_items.Add (new MeshSpriteData ());
		s_itemFoldouts.Add (false);
		if (s_TexList != null) {
			s_TexList.Add (null);
		} else {
			s_TexList = new List<Texture2D>();
			s_TexList.Add (null);
		}
	}

	static public bool CheckIfRelated (MeshSpriteMaker a, MeshSpriteMaker b)
	{
		if (a == null || b == null) return false;
		return a == b || a.References(b) || b.References(a);
	}

	bool References (MeshSpriteMaker atlas)
	{
		if (atlas == null) return false;
		if (atlas == this) return true;
		return false;
	}


}

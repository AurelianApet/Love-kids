//----------------------------------------------
// FunnyCamera - realtime camera effects
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------

using UnityEngine;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using CWALK;

public class FrameController : MonoBehaviour {

	public MeshSprite e_MeshSprite;

	TextAsset itemXMLFile;
	//And lets make the list that will store all of the items
	List<MeshSpriteData> m_items;
	bool m_isNeedApply = false;
	public bool isNeedApply
	{
		get 
		{
			return m_isNeedApply;
		}
	}

	// Use this for initialization
	void Start () {

		init ();
	}

	/// <summary>
	/// Updates the mesh sprite by name.
	/// </summary>
	/// <param name="name">Name.</param>
	public void updateMeshSprite(string name)
	{
		Texture tex = Resources.Load ("Frame/" + name) as Texture;
		if (tex != null) {
			e_MeshSprite.setMaterialMainTex(tex);
			e_MeshSprite.UpdateMesh(name);
			m_isNeedApply = true;
		}
	}

	/// <summary>
	/// Clears all frame effect
	/// </summary>
	public void ClearAll()
	{
		if (e_MeshSprite != null) {
			e_MeshSprite.Clear ();
		}
		m_isNeedApply = false;
	}

	/// <summary>
	/// Init this frame effect
	/// </summary>
	void init()
	{
		float Screenheight = (float)Camera.main.orthographicSize* 2.0f; 
		float Screenwidth = Screenheight * Screen.width / Screen.height;
		//make the frame mesh filled the screen.
		float frameHeight = Screenheight ;
		float frameWidth = Screenwidth;

		if (e_MeshSprite != null) {
			e_MeshSprite.Init(frameWidth,frameHeight,MeshSpriteMode.DEFAULT,( createEmptyTex()));
		}
	}

	Texture2D createEmptyTex ()
	{
		Texture2D tex = new Texture2D(4, 4);
		
		tex.hideFlags = HideFlags.DontSave;

		for (int i=0; i!=4; i++) {
			for(int j=0;j!=4;j++)
			{
				tex.SetPixel(i,j,Color.clear);
			}
		}
		tex.Apply();
		tex.filterMode = FilterMode.Point;
		return tex;
	}
}


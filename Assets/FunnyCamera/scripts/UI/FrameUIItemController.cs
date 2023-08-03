//----------------------------------------------
// FunnyCamera - realtime camera effects
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;
using CWALK;
using UnityEngine.UI;

public class FrameUIItemController : MonoBehaviour {

	public string framename;
	// Use this for initialization
	void Start () {
		if (this.GetComponent<Button> () != null) {
			this.GetComponent<Button> ().onClick.AddListener (clicked);// add the listtener for clicked
		}
	}

	/// <summary>
	/// Clicked will call this method.
	/// </summary>
	void clicked()
	{
		GameObject.Find ("FrameManager").GetComponent<FrameController> ().updateMeshSprite (framename);
	}
}

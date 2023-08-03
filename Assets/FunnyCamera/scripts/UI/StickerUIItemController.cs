//----------------------------------------------
// FunnyCamera - realtime camera effects
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;
using CWALK;
using UnityEngine.UI;
public class StickerUIItemController : MonoBehaviour {

	public string e_StickerName;
	// Use this for initialization
	void Start () {
		if (this.GetComponent<Button> () != null) {
			this.GetComponent<Button> ().onClick.AddListener (clicked);
		}
	}


	void clicked()
	{
		GameObject.Find ("StickerManager").GetComponent<StickerManager> ().CreateItem (e_StickerName);
	}

}

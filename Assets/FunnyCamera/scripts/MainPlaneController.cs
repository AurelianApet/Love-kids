//----------------------------------------------
// FunnyCamera - realtime camera effects
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------


using UnityEngine;
using System.Collections;

public class MainPlaneController : MonoBehaviour {

	// Use this for initialization
	void Awake () {
	
		float Screenheight = (float)Camera.main.orthographicSize* 2.0f; 
		float Screenwidth = Screenheight * Screen.width / Screen.height;
		//	Debug.Log ("current is height " + Screenheight + " width " + Screenwidth);
		float height = Screenheight ;
		float width = Screenwidth;

	//	this.transform.localPosition = new Vector3(0,0,91.6f);
		
		transform.localScale = new Vector3(width/10, 1.0f, height/10);
		//	Debug.Log (transform.renderer.bounds.size);

	}
	
	// Update is called once per frame
	void Update () {
	}
}

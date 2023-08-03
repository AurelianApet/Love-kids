using UnityEngine;
using System.Collections;

public class MyMainPlaneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.localScale = new Vector3(-1f, this.transform.localScale.y, -0.8f);
	}
}

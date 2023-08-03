using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControlManager : MonoBehaviour {
	public GameObject chatBtnObj, funnyBtnObj, storyBtnObj;
	//public GameObject ObjLTPlane;
	// Use this for initialization
	void Start () {
		//ObjLTPlane.transform.localScale = new Vector3 (-0.6462f, 1.0f, 0.84f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChatBtnClicked(){
		Debug.Log ("chat btn clicked");
		UnityEngine.SceneManagement.SceneManager.LoadScene("ChatRoom");
	}

	public void FunnyBtnClicked(){
		Debug.Log ("funny btn clicked");
		UnityEngine.SceneManagement.SceneManager.LoadScene("FunnyCamera_Project");
		//ObjLTPlane.transform.localScale = new Vector3 (-0.5f, 1.0f, 0.65f);
	} 

	public void storyBtnClicked(){
		Debug.Log ("story btn clicked");
	}
}

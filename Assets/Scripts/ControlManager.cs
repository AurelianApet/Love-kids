using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour {
	public GameObject contactPanel;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void chatBtnClicked(){
		GameObject[] oldChilds = GameObject.FindGameObjectsWithTag("PannelTag");
		foreach (GameObject child in oldChilds) {
			child.SetActive (false);
		}
		contactPanel.SetActive (true);
	}

	public void FunnyBtnClicked(){
		GameObject[] oldChilds = GameObject.FindGameObjectsWithTag("PannelTag");
		foreach (GameObject child in oldChilds) {
			Destroy(child);
		}
		UnityEngine.SceneManagement.SceneManager.LoadScene("FunnyCamera_Project");
	}
}

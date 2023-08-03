using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapManager : MonoBehaviour {
	public string filepath, person;
	public chatManager chatmanager;
	public GameObject detailsPanel;
	public string time;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowSnap(){
		chatmanager.panel_num = 6;
		detailsPanel.transform.FindChild("NameLabel").GetComponent<UILabel>().text = this.person;
		detailsPanel.transform.FindChild("TimeLabel").GetComponent<UILabel>().text = this.time;
		GameObject texture = detailsPanel.transform.FindChild("Texture").gameObject;
		chatmanager.LoadImageHandler(true, this.filepath, texture, 396, 600);
		detailsPanel.SetActive(true);
	}
}

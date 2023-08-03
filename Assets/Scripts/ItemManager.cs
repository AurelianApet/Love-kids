using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {
	public string user_id = "", user_name = "", filePath = "";
	public chatManager chatmanager;
	public GameObject contactPopupPanel;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SnapSendClicked(){
		chatmanager.UploadPhoto(this.user_id, this.filePath);
		contactPopupPanel.SetActive (false);
	}
}

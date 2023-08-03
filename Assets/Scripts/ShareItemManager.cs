using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareItemManager : MonoBehaviour {
    public GameObject detailsPanel, sharePanel;
    public string user_id = "", time = "", download_url = "";
    public chatManager chatManager;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ItemClicked() {
        chatManager.panel_num = 6;
        detailsPanel.transform.FindChild("NameLabel").GetComponent<UILabel>().text = this.user_id;
        detailsPanel.transform.FindChild("TimeLabel").GetComponent<UILabel>().text = this.time;
        GameObject texture = detailsPanel.transform.FindChild("Texture").gameObject;
        chatManager.LoadImageHandler(true, this.download_url, texture, 396, 600);
        sharePanel.SetActive(false);
        detailsPanel.SetActive(true);
    }
}

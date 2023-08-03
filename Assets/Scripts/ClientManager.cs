using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour {

    public string user_id, user_name;
    public GameObject homePanel, chatPanel;
    public GameObject clientLabel, clientIdLabel, controlPanel;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onItemClicked() {
        Global.chat_type = 1;
		clientLabel.transform.GetComponent<UILabel>().text = user_name;
		clientIdLabel.transform.GetComponent<UILabel>().text = user_id;
        NGUITools.SetActive(homePanel, false);
        NGUITools.SetActive(chatPanel, true);        
		controlPanel.SetActive (false);
	}
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImageManager : MonoBehaviour {

    // Use this for initialization
    public string filePath;
    public GameObject imagePanel, chatPanel, parentItem;
	public GameObject sendBtn, deleteBtn;
    public chatManager chatManager;
	public GameObject contactPopupPanel, parentObj, cloneItem;
	public GameObject alertPanel;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onItemClicked() {   		
        Debug.Log("-- image item clicked ");
        if (filePath.Length > 2) {                      
			// to show the contact popup
			int cnt = 0;
			cloneItem.SetActive (true);
			if (Global.contactList.Count > 0) {
				foreach (var item in Global.contactList) {
					GameObject newSharedItem = Instantiate (cloneItem) as GameObject;
					newSharedItem.transform.SetParent (parentObj.transform);
					newSharedItem.transform.localPosition = new Vector3 (0, -75 * cnt, 0);
					newSharedItem.transform.localScale = new Vector3 (1, 1, 1);
					newSharedItem.transform.FindChild ("Label").GetComponent<UILabel> ().text = item.username;
					newSharedItem.transform.FindChild ("ItemManager").GetComponent<ItemManager> ().user_id = item.user_id;
					newSharedItem.transform.FindChild ("ItemManager").GetComponent<ItemManager> ().user_name = item.username;
					newSharedItem.transform.FindChild ("ItemManager").GetComponent<ItemManager> ().filePath = this.filePath;
				}
				cloneItem.SetActive (false);
				contactPopupPanel.SetActive (true);	
			} else {
				Debug.Log ("you have no contact for sending snap");
				alertPanel.SetActive (true);
				alertPanel.transform.FindChild ("MsgLabel").GetComponent<UILabel> ().text = "you have no contact for sending snap";
			}
        }        
    }

    public void onDeleteBtnClicked() {
        Debug.Log(string.Format("file path {0}", this.filePath));
        if (string.IsNullOrEmpty(this.filePath)) {
            Debug.Log("path is null or empty");
            return;
        }

        if (!File.Exists(this.filePath)){
            Debug.Log("file not exist");
            return;
        }
		Debug.Log ("image delete " + this.filePath);
        File.Delete(this.filePath);
		DestroyObject (parentItem);
		chatManager.RefreshItem ();
        return;        
    }
}

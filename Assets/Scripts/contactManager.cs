using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class contactManager : MonoBehaviour
{
    // firebase database reference
    DatabaseReference mRootDataRef, contactsRef;
    // Use this for initialization
    public GameObject homePanel, chatPanel;
    public GameObject clientItem, parentItem;    // prefab instance for Cloning.
    public GameObject contactCntLabel;    
    public int panelNum;       // if panelNum is 1, homePanel, if 2, chatPanel;
    private bool isFirstLoading;
    void Start()
    {
        isFirstLoading = true;
        this.panelNum = 1;   
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://snapchat-4d7ed.firebaseio.com/");
        mRootDataRef = FirebaseDatabase.DefaultInstance.RootReference;        
        contactsRef = mRootDataRef.Child("Contacts").Child(PlayerPrefs.GetString("user_id"));
        Global.contactList = new List<Contact>();
        contactsRef.ValueChanged += ContactsRef_ValueChanged;
    }

    private void ContactsRef_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            Debug.Log("---- contactsRef change event " + e.DatabaseError.ToString());
            return;       
        }
        else {
            /*
            // to initialize children items in ScrollView 
            //Transform[] childlist = parentItem.transform.GetComponentsInChildren<Transform>(true);
            if (childlist != null) {
                for (int i = 1; i < childlist.Length; i++) {
                    if (childlist[i] != transform) {
                        DestroyObject(childlist[i].gameObject);
                    }
                }
            }*/
            if (isFirstLoading) {
                isFirstLoading = false;
                Debug.Log("-- get contacts from firebase");
                var list = e.Snapshot.Value as Dictionary<string, object>;
                if (list == null)
                {
                    Debug.Log("--- contact of user is empty");
                    NGUITools.SetActive(contactCntLabel, true);
                    contactCntLabel.transform.GetComponent<UILabel>().text = "YOU HAVE NO CONTACTS YET";
                    return;
                }
                int i = 0;
                foreach (var item in list)
                {
                    contactCntLabel.transform.GetComponent<UILabel>().text = "";
                    NGUITools.SetActive(contactCntLabel, false);
                    var values = item.Value as Dictionary<string, object>;
                    string tmpValue = "", user_id = "", username = "";                    
                    foreach (var v in values)
                    {
                        switch (v.Key)
                        {
                            case "user_id":                                
                                user_id = v.Value.ToString();
                                break;                            
                            case "user_name":                                
								username = v.Value.ToString();
                                break;
                        }
                    }
					AddObjectToPanel(i, user_id, username);
                }
            }            
        }
        //throw new System.NotImplementedException();
    }

    public void AddObjectToPanel(int index, string user_id, string username) {
        NGUITools.SetActive(contactCntLabel, false);
        contactCntLabel.transform.GetComponent<UILabel>().text = "";
        Contact newContact;
        NGUITools.SetActive(clientItem, true);
        GameObject newObj = Instantiate(clientItem) as GameObject;
        newObj.name = "ClientItem" + index.ToString();
        newObj.transform.SetParent(parentItem.transform);
        newObj.transform.localScale = new Vector3(1, 1, 1);
        newObj.transform.localPosition = new Vector3(0, (-150) * (Global.contactList.Count), 1);
		newObj.transform.FindChild("NameLabel").GetComponent<UILabel>().text = username;
        newObj.transform.FindChild("ClientManager").GetComponent<ClientManager>().user_id = user_id;        
		newObj.transform.FindChild("ClientManager").GetComponent<ClientManager>().user_name = username;        
		newObj.transform.FindChild("IdLabel").GetComponent<UILabel>().text = user_id;        

		newContact = new Contact(user_id, username);
        Global.contactList.Add(newContact);
        NGUITools.SetActive(clientItem, false);
    }

    public System.DateTime UnixtimeToDate(double unixTimeStamp)
    {
        System.DateTime dtDateTime = new System.DateTime(1970, 1, 1);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
        Debug.Log("---------unix time stamp in function " + unixTimeStamp + ", converted time : " + dtDateTime.Hour.ToString() + ":" + dtDateTime.Minute.ToString());
        return dtDateTime;
    }
    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                switch (this.panelNum)
                {
                    case 1:
                        UnityEngine.SceneManagement.SceneManager.LoadScene("FunnyCamera_Project");
                        break;
                    case 2:
                        this.panelNum = 1;
                        NGUITools.SetActive(chatPanel, false);
                        NGUITools.SetActive(homePanel, true);
                        break;
                }
            }            
        }
    }

    public bool LoadContacts()
    {
        return false;
    }      

    public void PublicChatBtnClicked()
    {
        Debug.Log("public chat btn clicked");
        Global.chat_type = 2;
        this.panelNum = 2;
        chatPanel.transform.FindChild("Upper").FindChild("ClientLabel").GetComponent<UILabel>().text = "Public Channel";
        NGUITools.SetActive(homePanel, false);
        NGUITools.SetActive(chatPanel, true);
    }
}

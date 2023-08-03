using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections.Generic;

public class connectManager : MonoBehaviour {
    public GameObject mainPanel, alertPanel;
    public GameObject m_IdObj, m_UserNameObj;
    public GameObject backBtnObj, connectBtnObj;
    public string[] ChannelsToJoinOnConnect;
    bool isExist = false;
    // firebase reference
    DatabaseReference mRootDataRef, usersRef;

    // Use this for initialization
    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://snapchat-4d7ed.firebaseio.com/");
        mRootDataRef = FirebaseDatabase.DefaultInstance.RootReference;
        usersRef = mRootDataRef.Child("Users");               

        Debug.Log("--- login or not " + PlayerPrefs.GetInt("isLogin"));        
        if (PlayerPrefs.GetInt("isLogin") == 1)
        {
			UnityEngine.SceneManagement.SceneManager.LoadScene("FunnyCamera_Project");
        }
    }     
    
    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android) {
            if (Input.GetKeyDown(KeyCode.Escape)) {                
                UnityEngine.SceneManagement.SceneManager.LoadScene("FunnyCamera_Project");
            }
        }
    }

    public void onConnectBtnClicked()
    {
        string user_id = "", user_name = "";
        user_id = m_IdObj.transform.FindChild("Label").GetComponent<UILabel>().text;
        user_name = m_UserNameObj.transform.FindChild("Label").GetComponent<UILabel>().text;
        m_IdObj.transform.FindChild("Label").GetComponent<UILabel>().text = "";
        Debug.Log("-- user id " + user_id);

		if (user_id != "Type your Id here" && !string.IsNullOrEmpty(user_id) && !string.IsNullOrEmpty(user_name))
        {
            // validation in firebase      
            connectBtnObj.transform.GetComponent<UIButton>().enabled = false;
            usersRef.GetValueAsync().ContinueWith(x =>
            {
                if (x.IsFaulted || x.IsCanceled)
                {
                    Debug.Log(" usersRef is faulted");
                    alertPanel.transform.FindChild("Label").GetComponent<UILabel>().text = "Please Type another Id";
                    alertPanel.SetActive(true);
                    mainPanel.SetActive(false);
                }
                else
                {
                    var lists = x.Result.Value as Dictionary<string, object>;
                    int index = 0;
                    foreach (var list in lists)
                    {
                        var values = list.Value as Dictionary<string, object>;
                        object tempObj;
                        values.TryGetValue("user_id", out tempObj);
						if(tempObj == null)
							break;
                        if (tempObj.ToString() == user_id)
                        {
                            Debug.Log("this id already exists");
                            this.isExist = true;

                            break;
                        }
                    }
                    if (this.isExist)
                    {
                        alertPanel.SetActive(true);
                        mainPanel.SetActive(false);
                    }
                    else
                    {
                        string key = usersRef.Push().Key;
                        usersRef.Child(key).Child("user_id").SetValueAsync(user_id);      // to add user to firebase user field        
                        usersRef.Child(key).Child("status").SetValueAsync("on");
						usersRef.Child(key).Child("user_name").SetValueAsync(user_name);
                        // to save user_id to PlayerPrefab
                        PlayerPrefs.SetInt("isLogin", 1);
                        PlayerPrefs.SetString("user_id", user_id);
						PlayerPrefs.SetString("useritem_key", key);
						PlayerPrefs.SetString("username", user_name);
                        UnityEngine.SceneManagement.SceneManager.LoadScene("ChatRoom");
                    }
                }
            });
        }
		else if(string.IsNullOrEmpty(user_name)){
			alertPanel.transform.FindChild("Label").GetComponent<UILabel>().text = "Please Type your nickname for chatting";
			alertPanel.SetActive(true);
			mainPanel.SetActive(false);
		}
        else {
            alertPanel.transform.FindChild("Label").GetComponent<UILabel>().text = "Please Type another Id";
            alertPanel.SetActive(true);
            mainPanel.SetActive(false);
        }

    }

    public void onAlertBtnClicked() {
        this.isExist = false;
        connectBtnObj.transform.GetComponent<UIButton>().enabled = true;
        m_IdObj.transform.GetComponent<UIInput>().value = "";
		m_UserNameObj.transform.GetComponent<UIInput>().value = "";
        alertPanel.transform.FindChild("Label").GetComponent<UILabel>().text = "Please Type Id correctly";
        alertPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void onBackBtnClicked() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("FunnyCamera_Project");
        backBtnObj.transform.GetComponent<UIButton>().enabled = false;
        connectBtnObj.transform.GetComponent<UIButton>().enabled = false;
    }
}

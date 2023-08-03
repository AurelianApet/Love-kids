using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ExitGames.Client.Photon.Chat;
using Firebase;
using Firebase.Database;
using Firebase.Storage;
using Firebase.Unity.Editor;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

/*
public class tmpUser {
public string user_id;
public string status;
public tmpUser(string id, string status) {
this.user_id = id;
this.status = status;
}
}*/

public class chatManager : MonoBehaviour, IChatClientListener
{

    // Use this for initialization
    public GameObject homePanel, popUpPanel, chatPanel, alertPanel, imagePanel, sharePanel, detailPanel;
    public GameObject chatBox, idInputObj, clientLabel, clientIdLabel;
    public GameObject fromMeItem, fromCliItem, parentItem, imageItem, parentImageItem, shareItem, parentShareItem;
    public GameObject stateLabel, uploadStatusLabel;
    public GameObject backToCameraBtn;
    public GameObject publicChatBtn;
	public GameObject controlPanel, menuPopupPanel;
	public GameObject nameLabel, idLabel;
    public string selectedChannelName;
    public string channels;
    public ChatClient chatClient;
    public int item_pos;
    public contactManager contactManager;
    public int texWidth = 150, texHeight = 280;
    public string my_id, my_name;
    // variables  for chatting
    public int HistoryLengthToFetch; // set in inspector. Up to a certain degree, previously sent messages can be fetched for context       
    public int panel_num;
    // emoji part
    public TextAsset textAsset;        // to load the text of emoji info
    public GameObject emojiTexture;    // prefab for emoticon 
    private Dictionary<string, string> emojiRects = new Dictionary<string, string>();
    private static char emSpace = '\u2001';
    // variables for Firebase
    // realtime data base
    DatabaseReference mRootDataRef, usersRef, contactsRef, sharesPhotoRef, snapRef;   
    // storage 
    FirebaseStorage storage;
    StorageReference photoStorageRef; 
    
    // Create a root reference    

    //struct for emoji
    private struct PosStringTuple
    {
        public int pos;      // position of emoji in input string
        public string emoji; // emoji 

        public PosStringTuple(int p, string s)
        {
            this.pos = p;
            this.emoji = s;
        }
    }

    void Start()
    {		
        my_id = PlayerPrefs.GetString("user_id");        
		my_name = PlayerPrefs.GetString ("username");
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://snapchat-4d7ed.firebaseio.com/");
        mRootDataRef = FirebaseDatabase.DefaultInstance.RootReference;
        usersRef = mRootDataRef.Child("Users");
        contactsRef = mRootDataRef.Child("Contacts").Child(this.my_id);
        sharesPhotoRef = mRootDataRef.Child("SharePhotos");
		snapRef = mRootDataRef.Child ("Snap").Child(this.my_id);
        usersRef.ValueChanged += UsersRef_ValueChanged;

        // Get a reference to the storage service, using the default Firebase App
        storage = FirebaseStorage.DefaultInstance;
        photoStorageRef = storage.GetReferenceFromUrl("gs://snapchat-4d7ed.appspot.com/Photos");      
        
        item_pos = -200;
        
        Global.userList = new List<User>();
        this.ParseEmojiInfo(this.textAsset.text);
        Connect();     
		nameLabel.transform.GetComponent<UILabel> ().text = this.my_name;
		idLabel.transform.GetComponent<UILabel> ().text = this.my_id;
    }

    void Update()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Service();            
        }
    }

    private void UsersRef_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            Debug.Log(" -- database error ");
            return;
        }
        else {
            var list = e.Snapshot.Value as Dictionary<string, object>;
            if (list == null)
            {
                Debug.Log("--- users ref is empty");
                return;
            }
            else {
                string user_id = "", status = "", user_name = "";
                User newUser;             
                int i = 0;
                foreach (var item in list) {
                    var values = item.Value as Dictionary<string, object>;
                    foreach (var v in values) {
                        switch (v.Key) {
                            case "user_id":
                                user_id = v.Value.ToString();
                                break;
                            case "status":
                                status = v.Value.ToString();
                                break;
							case "user_name":
								user_name = v.Value.ToString ();
								break;
                        }                        
                    }
					newUser = new User(user_id, status, user_name);                    
                    Global.userList.Add(newUser);
                    Debug.Log("--- user list " + Global.userList[i].user_id + ", " + Global.userList[i].status);
                    i++;                   
                }
            }
        }
        //throw new System.NotImplementedException();
    }

    // Update is called once per frame
    

    private void OnApplicationQuit()
    {
        Global.isQuit = true;
        if (this.chatClient != null)
        {
            this.chatClient.Disconnect();
        }
    }

    public void onBackBtnCliked() {
        backToCameraBtn.transform.GetComponent<UIButton>().enabled = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("FunnyCamera_Project");
    }

    // ................ customize .......................

    // emoji part
    private static string GetConvertedString(string inputString)
    {
        string[] converted = inputString.Split('-');
        for (int j = 0; j < converted.Length; j++)
        {
            converted[j] = char.ConvertFromUtf32(Convert.ToInt32(converted[j], 16));
        }
        return string.Join(string.Empty, converted);
    }

    private void ParseEmojiInfo(string inputString)
    {
        using (StringReader reader = new StringReader(inputString))
        {
            string line = reader.ReadLine();
            while (line != null && line.Length > 1)
            {
                // We add each emoji to emojiRects 
                string[] split = line.Split(' ');
                this.emojiRects[GetConvertedString(split[0])] = split[0];
                line = reader.ReadLine();
            }
        }
    }

    public IEnumerator SetUITextThatHasEmoji(GameObject textToEdit, string inputString)
    {
        List<PosStringTuple> emojiReplacements = new List<PosStringTuple>();
        StringBuilder sb = new StringBuilder();

        int i = 0;
        while (i < inputString.Length)
        {
            string singleChar = inputString.Substring(i, 1);
            string doubleChar = "";
            string fourChar = "";

            if (i < (inputString.Length - 1))
            {
                doubleChar = inputString.Substring(i, 2);
                Debug.Log(" double char" + doubleChar);
            }

            if (i < (inputString.Length - 3))
            {
                fourChar = inputString.Substring(i, 4);
                Debug.Log(" fourchar char" + fourChar);
            }

            if (this.emojiRects.ContainsKey(fourChar))
            {
                // Check 64 bit emojis first
                Debug.Log(" find the fouchar emoji");
                sb.Append(emSpace);
                emojiReplacements.Add(new PosStringTuple(sb.Length - 1, fourChar));
                i += 4;
            }
            else if (this.emojiRects.ContainsKey(doubleChar))
            {
                // Then check 32 bit emojis
                Debug.Log(" find the twochar emoji");
                sb.Append(emSpace);
                emojiReplacements.Add(new PosStringTuple(sb.Length - 1, doubleChar));
                i += 2;
            }
            else if (this.emojiRects.ContainsKey(singleChar))
            {
                // Finally check 16 bit emojis
                sb.Append(emSpace);
                Debug.Log(" find the single char emoji");
                emojiReplacements.Add(new PosStringTuple(sb.Length - 1, singleChar));
                i++;
            }
            else
            {
                sb.Append(inputString[i]);
                i++;
            }
        }

        // Set text
        textToEdit.transform.GetComponent<UILabel>().text = sb.ToString();

        // And spawn RawImages as emojis
        //TextGenerator textGen = textToEdit.transform.GetComponent<UILabel>().cachedGameObject();

        // One rawimage per emoji        
        Debug.Log(" emoji replace count " + emojiReplacements.Count);
        for (int j = 0; j < emojiReplacements.Count; j++) {
            int emojiIndex = emojiReplacements[j].pos;
            Debug.Log("emoji index " + emojiIndex);
            NGUITools.SetActive(this.emojiTexture, true);
            GameObject newTexture = Instantiate(this.emojiTexture) as GameObject;
            newTexture.transform.SetParent(textToEdit.transform);
            Vector3 imagePos = new Vector3(-225 + 42 * emojiIndex, 0, 0);
            newTexture.transform.localPosition = imagePos;
            newTexture.transform.localScale = new Vector3(1, 1, 1);
            string texPath = string.Format("Emoji/png/{0}", this.emojiRects[emojiReplacements[j].emoji]);
            newTexture.transform.GetComponent<UITexture>().mainTexture = Resources.Load(texPath) as Texture;
            NGUITools.SetActive(this.emojiTexture, false);            
        }
        
        yield return null;
    }

    // photon connect part
    public void Connect()
    {
        string user_id = this.my_id;        
        if (!string.IsNullOrEmpty(user_id))
        {
            this.chatClient = new ChatClient(this);
            Debug.Log(" -- app id " + ChatSettings.Instance.AppId);
            this.chatClient.Connect(ChatSettings.Instance.AppId, "1.0", new ExitGames.Client.Photon.Chat.AuthenticationValues(user_id));
            Debug.Log("Connecting as: " + user_id);
            StartCoroutine(SubscribeRoutine());
        }
    }

    IEnumerator SubscribeRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        this.chatClient.Subscribe(new string[] { this.selectedChannelName });
        Debug.Log("--- subscribe  is called");
    }

    public void SendChatMessage()
    {
        string input_line = "";                
        input_line = chatBox.transform.FindChild("Label").GetComponent<UILabel>().text;        
        Debug.Log("--- typing value " + input_line);
        
        if (string.IsNullOrEmpty(input_line) || input_line == "Type a message here")
        {
            Debug.Log("-- you didn't enter anything");
            return;
        }
                
        switch (Global.chat_type)
        {
            case 1:    // private chat
				string client_id = clientIdLabel.transform.GetComponent<UILabel>().text;
                this.chatClient.SendPrivateMessage(client_id, input_line);                
                break;
            case 2:   // public chat
                this.chatClient.PublishMessage(this.selectedChannelName, input_line);
                break;
            default:
                break;
        }
		AddMsgToPanel(1, "", input_line, false, null);
        chatBox.transform.GetComponent<UIInput>().value = "";
    }

    public void SendNotify(string receiver,string downloadUrl) {        
        switch (Global.chat_type)
        {
            case 1:    // private chat
				//string client_id = clientIdLabel.transform.GetComponent<UILabel>().text;
				this.chatClient.SendPrivateMessage(receiver, string.Format("a0a0a0 {0}", downloadUrl));
                break;
            case 2:   // public chat
                this.chatClient.PublishMessage(this.selectedChannelName, string.Format("a0a0a0 {0}", downloadUrl));
                break;
            default:
                break;
        }
		string notifyString = string.Format("you sent the photo to {0}", receiver);
		AddMsgToPanel(1, receiver, notifyString, true, downloadUrl);
    }

    public void StoryBtnClicked() {
        this.panel_num = 5;
        GameObject[] items = GameObject.FindGameObjectsWithTag("SharedItem");
        foreach (GameObject item in items) {
            Destroy(item);
        }

        sharePanel.SetActive(true);
        imagePanel.SetActive(false);
        StartCoroutine(ShowShareRoutine());
    }

    IEnumerator ShowShareRoutine() {
		snapRef.GetValueAsync().ContinueWith(x => {
            if (x.IsFaulted || x.IsCanceled)
            {
                Debug.Log("share ref is faulted ");
            }
            else {
                var lists = x.Result.Value as Dictionary<string, object>;
				if(lists != null){
					int index = 0;
					foreach (var list in lists) {
						string user_id = "", download_url = "", uploadtime = "";
						var values = list.Value as Dictionary<string, object>;
						foreach (var v in values) {
							switch (v.Key) {
							case "sender":
								user_id = v.Value.ToString();
								break;
							case "localpath":
								download_url = v.Value.ToString();
								break;
							case "time":
								uploadtime = v.Value.ToString();
								break;
							}
						}
						StartCoroutine(LoadingShareItem(index, user_id, download_url, uploadtime));
						index++;                    
					}	
				}
				else{
					Debug.Log("you have no snap from your friends");
				}
            }
        });
        yield return null;
    }

    IEnumerator LoadingShareItem(int index, string user_id, string download_url, string time) {
        GameObject newSharedItem = Instantiate(shareItem) as GameObject;
        newSharedItem.transform.SetParent(parentShareItem.transform);
        newSharedItem.transform.localPosition = new Vector3(0, -200 * index, 0);
        newSharedItem.transform.localScale = new Vector3(1, 1, 1);
        newSharedItem.transform.FindChild("Label").GetComponent<UILabel>().text = string.Format("{0}_{1}", user_id, time);
        newSharedItem.transform.FindChild("ShareItemManager").GetComponent<ShareItemManager>().user_id = user_id;
        newSharedItem.transform.FindChild("ShareItemManager").GetComponent<ShareItemManager>().download_url = download_url;
        newSharedItem.transform.FindChild("ShareItemManager").GetComponent<ShareItemManager>().time = time;
        GameObject texture = newSharedItem.transform.FindChild("Texture").gameObject;
		StartCoroutine(LoadImage(false, download_url, texture, 130, 150));
        yield return null;
    }

	public void UploadPhoto(string receiver, string localpath)
    {
        string filename = string.Format("{0}_{1}", this.my_id, GetFileName(localpath));
        //string receiver = clientLabel.transform.GetComponent<UILabel>().text;
        if (receiver != "PUBLIC CHANNEL" || string.IsNullOrEmpty(receiver))
            StartCoroutine(UploadRoutine(receiver, filename, localpath));
        else
            Debug.Log("you can not send photo in Public Channel");
    }

	IEnumerator DisplayPercent(float percent){
		uploadStatusLabel.transform.GetComponent<UILabel> ().text = string.Format("{0} %", percent);
		yield return null;
	}
    IEnumerator UploadRoutine(string receiver, string filename, string localpath) {                // Upload the file to the path "Photos/<filename>.jpg"                

        StorageReference imageRef = photoStorageRef.Child(filename);
		float upload_percent = 0.0f;
        uploadStatusLabel.transform.GetComponent<UILabel>().text = "SENDING";
		uploadStatusLabel.SetActive(true);
        IProgress<UploadState> progressHandler = new Firebase.Storage.StorageProgress<UploadState>(state =>
        {
            Debug.Log(String.Format("Progress: {0} of {1} bytes transferred.",					
                      state.BytesTransferred, state.TotalByteCount));
				
			upload_percent = state.BytesTransferred / state.TotalByteCount;
			upload_percent = 100 * upload_percent;
			//StartCoroutine(DisplayPercent(upload_percent));
        });

        var handler = imageRef.PutFileAsync(localpath, null, progressHandler, CancellationToken.None, null);

        handler.ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
                uploadStatusLabel.transform.GetComponent<UILabel>().text = "SENDING FAILED";
            }
            else
            {
                // Metadata contains file metadata such as size, content-type, and download URL.                                
				uploadStatusLabel.SetActive(false);
                StorageMetadata metadata = task.Result;

                string download_url = metadata.DownloadUrl.ToString();                

                uploadStatusLabel.transform.GetComponent<UILabel>().text = "SENDING FINISHED";
                string key = sharesPhotoRef.Child(receiver).Push().Key;
                sharesPhotoRef.Child(receiver).Child(key).Child("user_id").SetValueAsync(this.my_id);
                sharesPhotoRef.Child(receiver).Child(key).Child("download_url").SetValueAsync(download_url);
                string uploadTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                sharesPhotoRef.Child(receiver).Child(key).Child("time").SetValueAsync(uploadTime);

                Debug.Log("download url = " + download_url);
                // to send the link to friend
                SendNotify(receiver, download_url);
            }
        });
        yield return null;
    }       

	public void RefreshItem(){
		GameObject[] items = GameObject.FindGameObjectsWithTag("MyPhotoShare");
		int i = 0;
		foreach (GameObject item in items) {
			item.transform.localPosition = new Vector3(0, (-170) * i, 0);
			i++;
		}
	}    

	public void ChatBoxRefresh(){
		GameObject[] items = GameObject.FindGameObjectsWithTag("MyChatBox");
		int i = 0;
		foreach (GameObject item in items) {
			//string objectName = item.trans

		}
	}

    public void LoadImageHandler(bool isOnline, string url, GameObject obj, int width, int height) {
        StartCoroutine(LoadImage(isOnline, url, obj, width, height));
    }

    IEnumerator LoadImage(bool isOnline, string url, GameObject obj, int width, int height)
    {
        Debug.Log(" load image  " + url);
        string streamUrl = "";
        if (!isOnline)
            streamUrl = string.Format("file:///{0}", url);
        else
            streamUrl = url;
        WWW www = new WWW(streamUrl);
        yield return www;
        if (www.error == null)
        {
            Debug.Log(" to success in loadimage ");
            Texture2D texTmp = new Texture2D(width, height);
            www.LoadImageIntoTexture(texTmp);
            obj.transform.GetComponent<UITexture>().mainTexture = texTmp;
        }
        else
        {
            Debug.Log("please check the image url, there is no image on this url");
            yield return null;
        }
    }
    
	IEnumerator DownloadAndSave(string sender, string url) {
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null)
		{
			Debug.Log(" to success downloading ");

			if (www.bytes.Length > 10) {
				Debug.Log(" byteLength is short");
				yield return null;                     
			}

			string fieName = string.Format("snap_{0}_{1}", sender, System.DateTime.Now.ToString("HH-mm-ss"));
			string filepath = Path.Combine(GetDirPath(), fieName);
			Debug.Log ("download path " + filepath);
			File.WriteAllBytes(filepath, www.bytes);
			string key = snapRef.Push ().Key;
			snapRef.Child (key).Child("sender").SetValueAsync (sender);
			snapRef.Child (key).Child ("localpath").SetValueAsync (filepath);
			snapRef.Child (key).Child ("time").SetValueAsync (System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
			StartCoroutine(LoadingShareItem(4, sender, url, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")));
		}
		else
		{
			Debug.Log("please check the image url, there is no image on this url");
			yield return null;
		}
	}

	private void AddMsgToPanel(int direction, string client_id, string msg, bool isFile, string downloadUrl) {
        if (client_id != this.my_id) {
            // to rearrange items
            GameObject[] oldItems = GameObject.FindGameObjectsWithTag("MyChatBox");
            foreach (GameObject item in oldItems) {
                float currentPosY = item.transform.localPosition.y;
                item.transform.localPosition = new Vector3(0, currentPosY + 160, 0);
            }
            GameObject newItem = new GameObject();
            switch (direction)
            {
                case 1:   // outgoing
                    NGUITools.SetActive(fromMeItem, true);
                    newItem = Instantiate(fromMeItem) as GameObject;
                    break;
                case 2:   // incoming
                    NGUITools.SetActive(fromCliItem, true);
                    newItem = Instantiate(fromCliItem) as GameObject;
                    break;
            }
            newItem.transform.SetParent(parentItem.transform);
            newItem.transform.localScale = new Vector3(1, 1, 1);
            newItem.transform.localPosition = new Vector3(0, 0, 1);            

            if (direction == 2)
            {
                newItem.transform.FindChild("NameLabel").GetComponent<UILabel>().text = client_id;
            }

            GameObject msgLabel = newItem.transform.FindChild("MsgLabel").gameObject;

            StartCoroutine(SetUITextThatHasEmoji(msgLabel, msg));
            newItem.transform.FindChild("TimeLabel").GetComponent<UILabel>().text = System.DateTime.Now.Hour.ToString() + ":" + System.DateTime.Now.Minute.ToString();
            newItem.transform.FindChild("DateLabel").GetComponent<UILabel>().text = GetMonth(System.DateTime.Now.Month) + " " + System.DateTime.Now.Day.ToString() + "," + System.DateTime.Now.Year.ToString();
            double timestamp = (System.DateTime.Now.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
            Debug.Log("--- send message time stamp " + timestamp);
            NGUITools.SetActive(fromMeItem, false);
            NGUITools.SetActive(fromCliItem, false);

			if (isFile && !string.IsNullOrEmpty(downloadUrl)) {

				// to enable the showSnap button
				newItem.transform.FindChild("ShowSnapBtn").gameObject.SetActive(true);
				newItem.transform.FindChild ("SnapManager").GetComponent<SnapManager> ().filepath = downloadUrl;
				newItem.transform.FindChild ("SnapManager").GetComponent<SnapManager> ().person = client_id;
				newItem.transform.FindChild ("SnapManager").GetComponent<SnapManager> ().time = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
				// download snap and save to phone
				if(direction == 2)
					StartCoroutine(DownloadAndSave(client_id, downloadUrl));
			}
        }        
    }

    public void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName))
        {
            return;
        }

        ChatChannel channel = null;
        bool found = this.chatClient.TryGetChannel(channelName, out channel);
        if (!found)
        {
            Debug.Log("ShowChannel failed to find channel: " + channelName);
            return;
        }
    }

    public void onAddPanelBtnClicked() {
        idInputObj.transform.GetComponent<UIInput>().value = "";
		NGUITools.SetActive(homePanel, false);
        NGUITools.SetActive(popUpPanel, true);
    }

    public void onAddFriendBtnClicked()
    {
        string friendId = "", friends_name = "", errorMsg = "";
        friendId = idInputObj.transform.FindChild("Label").GetComponent<UILabel>().text;

        if (this.my_id == friendId)
        {
            errorMsg = "Sorry, you are trying to add yourself.";
            alertPanel.transform.FindChild("MsgLabel").GetComponent<UILabel>().text = errorMsg;
            NGUITools.SetActive(alertPanel, true);            
            return;
        }                
        int index = Global.userList.FindIndex(x => x.user_id == friendId);        
        if(index < 0)
        {
            //errorMsg = "This id is not existed. \n Make sure friend's Id is correct";
            errorMsg = "The person is not found in channel";
            alertPanel.transform.FindChild("MsgLabel").GetComponent<UILabel>().text = errorMsg;
            NGUITools.SetActive(alertPanel, true);
            return;
        }

        int index2 = Global.contactList.FindIndex(x => x.user_id == friendId);  // to check whether this is already added in your contact.
        if (index2 > -1) {
            errorMsg = "The person is already existed in your contact";
            alertPanel.transform.FindChild("MsgLabel").GetComponent<UILabel>().text = errorMsg;
            NGUITools.SetActive(alertPanel, true);
            return;
        }

		friends_name = Global.userList [index].user_name;
        string key = contactsRef.Push().Key;
        contactsRef.Child(key).Child("user_id").SetValueAsync(friendId);
		contactsRef.Child(key).Child("user_name").SetValueAsync(friends_name);
		contactManager.AddObjectToPanel(Global.contactList.Count, friendId, friends_name);
        idInputObj.transform.GetComponent<UIInput>().value = "";
        NGUITools.SetActive(popUpPanel, false);
		NGUITools.SetActive(homePanel, true);
    }

    public void onBackToChannel() {
        popUpPanel.SetActive(false);
		homePanel.SetActive(true);
    }

    public void onAlertBtnClicked() {
        idInputObj.transform.GetComponent<UIInput>().value = "";
		alertPanel.transform.FindChild ("MsgLabel").GetComponent<UILabel> ().text = "";
        NGUITools.SetActive(alertPanel, false);
    }

    public string GetDirPath() {
        string dirpath = "";
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Debug.Log("iPhone platform");
            dirpath = Application.persistentDataPath;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            dirpath = Application.persistentDataPath;
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            dirpath = string.Format("{0}/ScreenShots", Application.persistentDataPath);            
        }
        return dirpath;
    }

    public string GetFileName(string filePath) {
        string filename = "";
        string[] splits = filePath.Split(new char[] { '/' });        
        filename = splits[splits.Length - 1];
        Debug.Log("in editor file name " + filename);
        return filename;
    }
    
	// to show the image panel in order to send the snap
    public void onSendSnapBtnClicked() {  
        // to initialize Game Object children                
		sharePanel.SetActive(false);
        this.panel_num = 4;
        GameObject[] oldChilds = GameObject.FindGameObjectsWithTag("MyPhotoShare");

        foreach (GameObject child in oldChilds) {
            Destroy(child);
        }

        imagePanel.SetActive(true);
        string dirPath = GetDirPath();
        DirectoryInfo dir = new DirectoryInfo(dirPath);
        if (!dir.Exists)
        {
            Debug.Log("Screen shot folder create is success");
            dir.Create();
        }
        
        string[] files = Directory.GetFiles(dirPath);
        
        for (int j = 0; j < files.Length; j++) {
            string[] splits = files[j].Split(new char[] { '\\' });
            if (splits == null || splits.Length < 2) {
                Debug.Log(" '\' not exists");
                continue;
            }
            files[j] = dirPath + "/" + splits[1];
            Debug.Log("--changed path " + files[j]);
        }
        int i = 0;
        imageItem.SetActive(true);
        foreach (string filepath in files) {
            GameObject newImageItem = Instantiate(imageItem) as GameObject;            
            newImageItem.transform.SetParent(parentImageItem.transform);
            newImageItem.transform.localScale = new Vector3(1, 1, 1);
            newImageItem.transform.localPosition = new Vector3(0, -170 * i , 0);
            string fileName = filepath.Substring(filepath.LastIndexOf('/') + 1, filepath.Length - filepath.LastIndexOf('/') - 1);
            fileName = fileName.Substring(0, fileName.Length - 5);
            newImageItem.transform.FindChild("Label").GetComponent<UILabel>().text = fileName;
            newImageItem.transform.FindChild("ImageManager").GetComponent<ImageManager>().filePath = filepath;
            GameObject texture = newImageItem.transform.FindChild("Texture").gameObject;
            StartCoroutine(LoadImage(false, filepath, texture, 110, 160));
            i++;
        }
        imageItem.SetActive(false);
		menuPopupPanel.SetActive (false);
        NGUITools.SetActive(chatPanel, false);        
    }

    string GetMonth(int month)
    {
        string letter = string.Empty;
        switch (month)
        {
            case 1:
                letter = "Jan";
                break;
            case 2:
                letter = "Feb";
                break;
            case 3:
                letter = "Mar";
                break;
            case 4:
                letter = "Apr";
                break;
            case 5:
                letter = "May";
                break;
            case 6:
                letter = "June";
                break;
            case 7:
                letter = "July";
                break;
            case 8:
                letter = "Aug";
                break;
            case 9:
                letter = "Sep";
                break;
            case 10:
                letter = "Oct";
                break;
            case 11:
                letter = "Nov";
                break;
            case 12:
                letter = "Dec";
                break;
        }
        return letter;
    }
   
    //......................................defalt part ..................................
    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
        {
            UnityEngine.Debug.LogError(message);
        }
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
        {
            UnityEngine.Debug.LogWarning(message);
        }
        else
        {
            UnityEngine.Debug.Log(message);
        }
    }

    public void OnConnected()
    {
        
        
        if (this.selectedChannelName != null)
        {
           this.chatClient.Subscribe(new string[] {this.selectedChannelName}, this.HistoryLengthToFetch);
        }
        List<string> userIdList = new List<string>();
        if (Global.contactList != null && Global.contactList.Count > 0) {
            foreach (var item in Global.contactList) {
                userIdList.Add(item.user_id);
            }
            this.chatClient.AddFriends(userIdList.ToArray());       
        }

        this.chatClient.SetOnlineStatus(ChatUserStatus.Online); 
        
    }

    public void OnDisconnected()
    {        
        Debug.Log("--- disconnected from onDisconnected");
    }


    public void OnChatStateChange(ChatState state)
    {
        // use OnConnected() and OnDisconnected()
        // this method might become more useful in the future, when more complex states are being used.
        
        Global.connectionStatus = state.ToString();
        if (Global.connectionStatus == "ConnectedToFrontEnd")
        {
            publicChatBtn.transform.GetComponent<UIButton>().enabled = true;
        }
        else
        {
            publicChatBtn.transform.GetComponent<UIButton>().enabled = false;
        } 

        stateLabel.transform.GetComponent<UILabel>().text = Global.connectionStatus;
        Debug.Log("-- status change " + state.ToString());
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("OnSubscribed: " + string.Join(", ", channels));        
        // Switch to the first newly created channel
        ShowChannel(channels[0]);
    }

    public void OnUnsubscribed(string[] channels)
    {
        
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string msgs = "";
        for (int i = 0; i < senders.Length; i++)
        {
            msgs = string.Format("{0}{1}={2}, ", msgs, senders[i], messages[i]);
            //pickMessage(senders[i], messages[i], channelName);         
        }
        Debug.Log("OnGetMessages: " + channelName + senders.Length + msgs);

    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log("OnPrivateMessage:  " + channelName + ", " + sender + ", " + message);

        pickMessage(sender, message, channelName);
    }

    private void pickMessage(string sender, object message, string channelName)
    {
        Debug.Log(" to get message from " + sender);
        string msg = message.ToString();
        string downloadUrl = "";
        if (msg.Length > 15)
        {
            if (msg.Substring(0, 6) == "a0a0a0")
            {
                string[] splits = msg.Split(' ');
                if (splits.Length != 2) {
                    Debug.Log(" this is not shared link");
                    return;
                }
                    
                if (!splits[1].StartsWith("https://firebasestorage"))
                {
                    Debug.Log("this is not shared link");
                    return;
                }
                downloadUrl = splits[1];
                // to download from link
				AddMsgToPanel(2, sender, string.Format("{0} sent you snap", sender), true, downloadUrl);
            }
            else {
				AddMsgToPanel(2, sender, msg, false, null);
            }
        }
        else {
			AddMsgToPanel(2, sender, msg, false, null);
        }        
    }
    
    byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
            return null;
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }    

    /// <summary>
    /// New status of another user (you get updates for users set in your friends list).
    /// </summary>
    /// <param name="user">Name of the user.</param>
    /// <param name="status">New status of that user.</param>
    /// <param name="gotMessage">True if the status contains a message you should cache locally. False: This status update does not include a
    /// message (keep any you have).</param>
    /// <param name="message">Message that user set.</param>
    /// 

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {        
        Debug.LogWarning("status: " + string.Format("{0} is {1}. Msg:{2}", user, status, message));      
    }

    //........... action part.....

    public void onCancelBtnClicked() {
        switch (this.panel_num) {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:    // Image panel
                imagePanel.SetActive(false);
				homePanel.SetActive(true);
                this.panel_num = 3;
                break;
			case 5:    // share panel
				sharePanel.SetActive (false);
				imagePanel.SetActive (true);
				controlPanel.SetActive (true);
                this.panel_num = 4;
                break;
            case 6:    // Detail panel
                detailPanel.SetActive(false);
                sharePanel.SetActive(true);
				controlPanel.SetActive (true);
                this.panel_num = 5;
                break;
        }        
    }

    public void onBackToHomeClicked() {
        chatPanel.SetActive(false);
        homePanel.SetActive(true);
		controlPanel.SetActive (true);
    }

	public void onLogoutBtnClicked(){
		Debug.Log ("log out btn clicked");
		string key = PlayerPrefs.GetString ("useritem_key");

		usersRef.Child (key).RemoveValueAsync ().ContinueWith (task => {
			if(task.IsFaulted){
				Debug.Log("database is faulted");
			}
			else if(task.IsCompleted){
				Debug.Log(" deleting the user field");
			}
			else{
				Debug.Log("exception");
			}
		});

		contactsRef.RemoveValueAsync ().ContinueWith (target => {
			if(target.IsFaulted){
				Debug.Log("database is faulted");
			}
			else if(target.IsCompleted){
				Debug.Log(" deleting the user field");
			}
			else{
				Debug.Log("exception");
			}
		});

		PlayerPrefs.SetInt ("isLogin", 0);
		UnityEngine.SceneManagement.SceneManager.LoadScene ("ChatLogin");
	}

	public void onMenuBtnClicked(){
		menuPopupPanel.SetActive (true);
	}
}


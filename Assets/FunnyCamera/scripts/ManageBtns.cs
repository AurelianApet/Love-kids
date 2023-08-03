using UnityEngine;
using System.Collections;
using System.IO;
using common;

public class ManageBtns : MonoBehaviour {

    public UnityEngine.UI.Button shareBtn;
    public UnityEngine.UI.Button backBtn;        
    public int width = 720, height = 1280;
    public GameObject uiLayer;
    public UIButton chatBtnObj;    
    // Use this for initialization
    void Start () {
        EventTriggerListener.Get(shareBtn.gameObject).onDown += ShareBtnDown;
        EventTriggerListener.Get(shareBtn.gameObject).onUp += ShareBtnUp;        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public string GetFilePath() {
        string path = "";
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Debug.Log("iPhone platform");
            path = string.Format("{0}.png", System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            path = string.Format("{0}.png", System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));            
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor) {
            path = string.Format("{0}/ScreenShots/{1}.png",
                             Application.persistentDataPath,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
            DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + "/ScreenShots/");
            if (!dir.Exists)
            {
                dir.Create();
            }
        }
        return path;
    }   

    void ShareBtnDown(GameObject obj)
    {
        Debug.Log("share btn down action");        
        Utils.btn_clicked = true;

    }

    IEnumerator delayRoutine() {
        yield return new WaitForSeconds(0.05f);
        uiLayer.SetActive(true);
    }

    void ShareBtnUp(GameObject obj)
    {
        Debug.Log("share btn up action");
        uiLayer.SetActive(false);
        Application.CaptureScreenshot(GetFilePath());
        Debug.Log("--- screen shot");
        StartCoroutine(delayRoutine());
        Utils.btn_clicked = false;
    }

    public void ChatMsgBtnClicked() {
        Debug.Log("--- chat scene load");
        //chatBtnObj.GetComponent<UIButton>().is
        UnityEngine.SceneManagement.SceneManager.LoadScene("ChatLogin");
    }
}

//----------------------------------------------
// FunnyCamera - realtime camera effects
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if !UNITY_WEBPLAYER || !UNITY_WEBGL
using System.IO;
#endif
using UnityEngine.UI;
using DG.Tweening;

public class PhotoController : MonoBehaviour {

	[SerializeField]
	Camera _TargetCamera; // drag the security camera onto this field in the inspector
	
	Texture2D mTexture;
	public delegate void CaptureFinished(Texture2D e);  
	public static event CaptureFinished ScreenCaptureFinished;  

	#if UNITY_IOS
	const string STATISTIC_SCREENSHOT = "funnycamera.png";
	#elif UNITY_EDITOR_WIN || UNITY_WEBPLAYER ||UNITY_EDITOR||UNITY_WEBGL
	const string STATISTIC_SCREENSHOT = "funnycamera.png";
	#elif UNITY_ANDROID
	const string STATISTIC_SCREENSHOT = "funnycamera.png";
	#endif
	public static PhotoController instance;

	public RawImage e_previewImage;
	public MaskUIController e_maskcontroller;
	public GameObject m_contentObj;
	public InputField e_descriptionInputField;
	public Text logtext;
	public Texture2D e_watermark;

    CaptureAndSave snapShot;

    public NativeShare e_shareTool;
	// Use this for initialization
	void Start () {
		instance = this;
        snapShot = GameObject.FindObjectOfType<CaptureAndSave>();
	}

    void OnEnable()
    {
        CaptureAndSaveEventListener.onError += OnError;
        CaptureAndSaveEventListener.onSuccess += OnSuccess;
    }

    void OnSuccess(string msg)
    {
        Debug.Log("Success : " + msg);
    }

    void OnError(string error)
    {
        Debug.Log("Error : " + error);
    }

	// Update is called once per frame
	void Update () {
		
	}

	public void startRenderCameraForEdit()
	{
		StartCoroutine(RenderCameraTex(false));
       
	}

	public void StartRenderCameraForShare()
	{
		RenderCameraForShare ();
	}

	void GetRenderTexture(Texture2D tex)
	{
		e_previewImage.texture = tex;
	}

	void RenderCameraForShare()
	{
		StartCoroutine(RenderCameraTex(true));
		ShowSharePanel();
	}
	
	// return file name
	string fileName(int width, int height)
	{
		return string.Format("screen_{0}x{1}_{2}.png",
		                     width, height,
		                     System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
	}
	
	IEnumerator  RenderCameraTex(bool isAddWaterMark = false)
	{

		if (_TargetCamera != null) {
			Rect mRect;
			
			yield return new WaitForEndOfFrame();
			mRect = new Rect (0, 0, Screen.width, Screen.height);
			//init RenderTexture
			RenderTexture mRender=new RenderTexture((int)mRect.width,(int)mRect.height,10);
			//set the camera targettexture
			_TargetCamera.targetTexture=mRender;
			
			//start render
			_TargetCamera.Render();
			
			//active the texture
			RenderTexture.active=mRender;
			
			if (mTexture == null) {
				mTexture=new Texture2D((int)mRect.width,(int)mRect.height,TextureFormat.RGB24,false);
			}
			
			//read pixel
			mTexture.ReadPixels(mRect,0,0);
			/*
			if(isAddWaterMark)
			{
				int startX = mTexture.width - e_watermark.width;
				int startY =e_watermark.height;// 0;//background.height - TodrawLogo.height;
				
				for (int x = startX; x < mTexture.width; x++)
				{
					for (int y = 0; y <startY; y++)
					{
						Color bgColor = mTexture.GetPixel(x, y);
						Color wmColor = e_watermark.GetPixel(x - startX, y);
						
						Color final_color = Color.Lerp(bgColor, wmColor, wmColor.a / 1.0f);
						
						mTexture.SetPixel(x, y, final_color);
					}
				}
			}
			*/

			mTexture.Apply();
			yield return new WaitForEndOfFrame();
			if(IsFileExist(STATISTIC_SCREENSHOT))
			{
				DeleteFile(Application.persistentDataPath , STATISTIC_SCREENSHOT);  
			}
			_TargetCamera.targetTexture = null;   
			RenderTexture.active = null; 
			GameObject.Destroy(mRender);
			if(ScreenCaptureFinished != null)
			{
				ScreenCaptureFinished(mTexture);
			}

		}
	}

	void ShowSharePanel()
	{
		if (e_maskcontroller != null) {
			e_maskcontroller.Show();
		}
		m_contentObj.GetComponent<RectTransform> ().DOLocalMove (new Vector3(0,0,0),0.6f).OnComplete(sharePanelShowTweenEnd);
	}

	void sharePanelShowTweenEnd()
	{
		e_previewImage.texture = mTexture;
		e_previewImage.GetComponent<RectTransform>().DOScale(new Vector3(1,1,1),0.8f).SetEase(Ease.OutBounce);
	}
	
	void sharePanelHideTweenEnd()
	{
		if (e_maskcontroller != null) {
			e_maskcontroller.Hide();
		}
		e_previewImage.GetComponent<RectTransform>().DOScale(new Vector3(0,0,0),0.2f).SetEase(Ease.Linear);

		System.GC.Collect();
		Resources.UnloadUnusedAssets();
	}

	public void HideSharePanel()
	{
		m_contentObj.GetComponent<RectTransform> ().DOLocalMove (new Vector3(250,0,0),0.6f).OnComplete(sharePanelHideTweenEnd);
	}
    /*
    string log01 ="waiting01";
    string log02 = "waiting02";
    void OnGUI()
    {
        GUILayout.Button(log01);
        GUILayout.Button(log02);
        if (mTexture != null)
        {
            GUILayout.Button("Width : " + mTexture.width + " height is " + mTexture.height);
        }
    }
    */
	public void savetogallay()
	{
		StartCoroutine (SaveToAlbum ());
	}

	IEnumerator SaveToAlbum()
	{
		if(!IsFileExist(STATISTIC_SCREENSHOT))
		{
      //      log01 = "file is writed to device!";
#if !UNITY_WEBPLAYER 
			File.WriteAllBytes(GetDefaultFilePath() +  STATISTIC_SCREENSHOT, mTexture.EncodeToPNG());
#endif
		}

		while(!IsFileExist(STATISTIC_SCREENSHOT)){
			yield return new WaitForSeconds(0.05f);
		}

     //   log02 = "file is existed!";
        #if UNITY_EDITOR
        
		#elif UNITY_IPHONE
         snapShot.SaveTextureToGallery(mTexture);
        
#elif UNITY_ANDROID
         snapShot.SaveTextureToGallery(mTexture);
       
#endif

    }
	
	void DeleteFile(string path,string name)
	{
		#if !UNITY_WEBPLAYER 
		File.Delete(path+"//"+ name);
		#endif
	}
	
	//get default save/read file path on device
	public static string GetDefaultFilePath(){
		return Application.persistentDataPath + "/";
	}
	
	//Is file exist in default path
	public static bool IsFileExist(string name){
		return IsFileExistByPath(GetDefaultFilePath() + name);
	}
	
	//is file exist in custom path and name
	public static bool IsFileExistByPath(string path){
		#if !UNITY_WEBPLAYER ||!UNITY_WEBGL
		FileInfo info = new FileInfo(path);
		if (info == null || info.Exists == false) {
			return false;
		};
		return true;
        #else
		return false;
        #endif
	}

	public void moreShareClicked()
	{
		StartCoroutine (moreshare ());
	}

	IEnumerator moreshare()
	{
		if(!IsFileExist(STATISTIC_SCREENSHOT))
		{
			//      log01 = "file is writed to device!";
			#if !UNITY_WEBPLAYER 
			File.WriteAllBytes(GetDefaultFilePath() +  STATISTIC_SCREENSHOT, mTexture.EncodeToPNG());
			#endif
		}
		
		while(!IsFileExist(STATISTIC_SCREENSHOT)){
			yield return new WaitForSeconds(0.05f);
		}

		#if UNITY_EDITOR
		#elif UNITY_IPHONE
		e_shareTool.ShareImage("FunnyCamera...", GetDefaultFilePath() + STATISTIC_SCREENSHOT);
#elif UNITY_ANDROID
		e_shareTool.ShareImage("FunnyCamera...", GetDefaultFilePath() + STATISTIC_SCREENSHOT);
#endif
    }
}

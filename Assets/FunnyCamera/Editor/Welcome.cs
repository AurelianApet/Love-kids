//----------------------------------------------
// FunnyCamera - realtime camera effects
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System;

[InitializeOnLoad]
public class Welcome : EditorWindow 
{
	/******* TO MODIFY **********/
	/******* TO MODIFY **********/
	/******* TO MODIFY **********/
	/******* TO MODIFY **********/
	/******* TO MODIFY **********/
	private const string EVERYPLAYURL = "https://www.assetstore.unity3d.com/en/#!/content/16005";
	private const string RATEUS_URL = "https://www.assetstore.unity3d.com/en/#!/content/66184";
	private const string ONLINE_DOC_URL = "https://www.dropbox.com/s/x2h9rb3lu0mvwzy/FunnyCamera_Develop.pdf?dl=0";
	
	private const string NAME_OF_THE_GAME = "FunnyCamera-realtime webcam/facial filter effects";
	/******* TO MODIFY **********/
	/******* TO MODIFY **********/
	/******* TO MODIFY **********/
	/******* TO MODIFY **********/
	private const string EmailURL = "mailto:lycwalk@gmail.com";
	
	private const float width = 500;
	private const float height = 500;
	
	private const string PREFSHOWATSTARTUP = "52CWALK.FunnyCamera";
	
	private static bool showAtStartup;
	private static GUIStyle imgHeader;
	private static bool interfaceInitialized;
	private static Texture onlineDocIcon;
	private static Texture FunnyCameraIcon;
	private static Texture bgHeaderIcon;
	private static Texture rateIcon;
	private static Texture questionIcon;
	private static Texture everyplayIcon;
	private static Texture supportIcon;
	
	[MenuItem("Tools/FunnyCamera/Welcome Screen", false, 0)]
	[MenuItem("Window/FunnyCamera/Welcome Screen", false, 0)]
	
	public static void OpenWelcomeWindow(){
		GetWindow<Welcome>(true);
	}

	static Welcome(){
		EditorApplication.playmodeStateChanged -= OnPlayModeChanged;
		EditorApplication.playmodeStateChanged += OnPlayModeChanged;
		
		showAtStartup = PlayerPrefs.GetInt(PREFSHOWATSTARTUP, 1) == 1;

		if (!showAtStartup) {
			OpenWelcomeWindow();
			PlayerPrefs.SetInt(PREFSHOWATSTARTUP, 1);
			PlayerPrefs.Save();
		}

		/*
		if (showAtStartup){
			EditorApplication.update -= OpenAtStartup;
			EditorApplication.update += OpenAtStartup;
		}
		*/
	}
	
	static void OpenAtStartup(){
		OpenWelcomeWindow();
		EditorApplication.update -= OpenAtStartup;
	}
	
	static void OnPlayModeChanged(){
		EditorApplication.update -= OpenAtStartup;
		EditorApplication.playmodeStateChanged -= OnPlayModeChanged;
	}
	
	void OnEnable(){
		#if UNITY_5_3_OR_NEWER
		titleContent = new GUIContent("Welcome To " + NAME_OF_THE_GAME); 
		#endif
		
		maxSize = new Vector2(width, height);
		minSize = maxSize;
	}	
	
	public void OnGUI(){
		
		InitInterface();
		GUI.Box (new Rect (0, 0, width, 70), bgHeaderIcon);
		GUI.Box (new Rect (0, 0, 68, 68), FunnyCameraIcon);
	
		GUILayoutUtility.GetRect(position.width, 72);

		GUILayout.Space(20);
		GUILayout.BeginVertical();
		Color orgColor = GUI.color;
		GUI.color = Color.red;
		if (Button(everyplayIcon,"Improt Everyplay (Free)","Firstly, Import Everyplay from Assetstore,It provides Record Features,")){
			Application.OpenURL(EVERYPLAYURL);
		}
		GUI.color = orgColor;

		if (Button(rateIcon,"Rate this asset","Write us a review on the asset store.")){
			Application.OpenURL(RATEUS_URL);
		}
		
		
		if (Button(onlineDocIcon,"Online documentation","Read the full documentation.")){
			Application.OpenURL(ONLINE_DOC_URL);
		}

		if (Button(supportIcon,"Support","EMIAL:lycwalk@gmail.com;Contact us when you have any problem!  ")){
			Application.OpenURL(EmailURL);
		}
		
		GUILayout.Space(3);
		/*
		bool show = GUILayout.Toggle(showAtStartup, "Show at startup");
		if (show != showAtStartup){
			showAtStartup = show;
			int i = GetInt(showAtStartup);
			Debug.Log("toggle i = " + i);
			PlayerPrefs.SetInt(PREFSHOWATSTARTUP, i);
			PlayerPrefs.Save();
		}
		*/
		GUILayout.EndVertical();
		
	}
	
	int GetInt(bool b)
	{
		if(b)
			return 1;
		else
			return 0;
	}
	
	void InitInterface(){
		
		if (!interfaceInitialized){

			bgHeaderIcon =(Texture)Resources.Load("bgbanner") as Texture;
			FunnyCameraIcon = (Texture)Resources.Load("FunnycameraIcon") as Texture;
			onlineDocIcon = (Texture)Resources.Load("btn_onlinedoc") as Texture;
			rateIcon = (Texture)Resources.Load("btn_rate") as Texture;
			questionIcon = (Texture)Resources.Load("btn_question") as Texture;
			everyplayIcon = (Texture)Resources.Load("btn_everyplay") as Texture;
			supportIcon = (Texture)Resources.Load("btn_support") as Texture;
			interfaceInitialized = true;
		}
	}
	
	bool Button(Texture texture, string heading, string body, int space=10){
		
		GUILayout.BeginHorizontal();
		
		GUILayout.Space(54);
		GUILayout.Box(texture, GUIStyle.none, GUILayout.MaxWidth(48));
		GUILayout.Space(10);
		
		GUILayout.BeginVertical();
		GUILayout.Space(1);
		GUILayout.Label(heading, EditorStyles.boldLabel);
		GUILayout.Label(body);
		GUILayout.EndVertical();
		
		GUILayout.EndHorizontal();
		
		var rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
		
		bool returnValue = false;
		if (Event.current.type == EventType.mouseDown && rect.Contains(Event.current.mousePosition)){
			returnValue = true;
		}
		
		GUILayout.Space(space);
		
		return returnValue;
	}
}

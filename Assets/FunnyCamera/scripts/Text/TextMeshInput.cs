//----------------------------------------------
// FunnyCamera - realtime camera effects
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextMeshInput : MonoBehaviour {

	private bool inEditMode = false;
	private string storedString;
	public TextMesh textComponent;
	public GameObject backgroundObj;
	private string guiString;
	TouchScreenKeyboard keyboard;
	bool m_isStartWork = false;
	TextMesh m_StaticMesh;
	public bool isStartWork
	{
		get
		{
			return m_isStartWork;
		}
	}


	float orgHeight = 0;
	// Use this for initialization
	void Start () {
//		textComponent = textComponent.GetComponent<TextMesh>();
		storedString = textComponent.text;
		guiString = storedString;
		
	
		orgHeight = textComponent.GetComponent<Renderer> ().bounds.size.y *1.2f;

		checkChars();    //Check so that the 3d-text isn't empty
		fitCollider();    //Set the Collider to fit the 3d Text Size


		TextMesh mesh = GameObject.Find ("StaticMeshText")!=null?GameObject.Find ("StaticMeshText").GetComponent<TextMesh> ():null;

		if (mesh != null) {
			m_StaticMesh = mesh;
			m_StaticMesh.characterSize = textComponent.characterSize;
			m_StaticMesh.font = textComponent.font;
			m_StaticMesh.fontSize = textComponent.fontSize;
			m_StaticMesh.alignment = textComponent.alignment;
			m_StaticMesh.anchor = textComponent.anchor;
			m_StaticMesh.transform.position = new Vector3(-10000,0,0);
			m_StaticMesh.text = textComponent.text;
		} else {
			m_StaticMesh = null;
			GameObject obj = new GameObject();
			obj.name = "StaticMeshText";
			obj.AddComponent<MeshRenderer>();
			m_StaticMesh = obj.AddComponent<TextMesh>();
			m_StaticMesh.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

			m_StaticMesh.characterSize = textComponent.characterSize;
			m_StaticMesh.fontSize = textComponent.fontSize;
			m_StaticMesh.alignment = textComponent.alignment;
			m_StaticMesh.anchor = textComponent.anchor;
			m_StaticMesh.transform.position = new Vector3(-10000,0,0);
			m_StaticMesh.text = textComponent.text;
		}
	}

	void Update()
	{

	}


	void OnGUI()
	{
		if (inEditMode) {
			//Make a TextField which sends to the 3d-text GameObject

			if(isMobileRuntime)
			{
				foreach (char c in Input.inputString) {
					if (c == "\b"[0]) {
						if (keyboard.text.Length != 0) {
							keyboard.text = keyboard.text.Substring(0, keyboard.text.Length - 1);
						}
					}
				}

				textComponent.text = keyboard.text; 
				if(m_StaticMesh != null)
				{
					m_StaticMesh.text = keyboard.text;
				}

				if(keyboard.done)
				{
					inEditMode = false;
					keyboard.active = false;
					keyboard = null;
					this.m_isStartWork = true;
					if(keyboard.text.Length == 0)
					{
						textComponent.text = "Click And Edit Text";
						if(m_StaticMesh != null)
						{
							m_StaticMesh.text = "Click And Edit Text";
						}
					}
				}

				fitCollider ();     //Resize the Collider
			}
			else
			{
	//			GUILayout.Button("dddddddddddddd h");
		//		GUILayout.Label(GUI.GetNameOfFocusedControl());
				GUI.SetNextControlName ("hiddenTextField"); //Prepare a Control Name so we can focus the TextField
				GUI.FocusControl ("hiddenTextField");        //Focus the TextField
				guiString = GUI.TextField (new Rect (0, 10, 200, 25), guiString, 25);    //Display a TextField outside the Screen Rect
				//guiString = GUILayout.TextField(guiString,GUILayout.Width(200));
				//Listen for keys
				//     if (Input.anyKey) {
				textComponent.text = guiString;    //Set the 3d-text to the same as our hidden TextField
				if(m_StaticMesh != null)
				{
					m_StaticMesh.text = guiString;
				}
				fitCollider ();     //Resize the Collider
			}
		}
	}

	public void setFont(Font font)
	{
		m_StaticMesh.GetComponent<TextMesh> ().font = font;
		m_StaticMesh.GetComponent<Renderer> ().sharedMaterial = font.material;

		textComponent.GetComponent<TextMesh> ().font = font;
		textComponent.GetComponent<Renderer> ().sharedMaterial = font.material;

		fitCollider ();
	}

	//Set the Collider to fit the 3d Text Size
	void fitCollider () {
		Vector3 tempSize = Vector3.zero;

		if (m_StaticMesh != null) {
			tempSize = m_StaticMesh.GetComponent<Renderer> ().bounds.size;
			tempSize.Set(tempSize.x,orgHeight,tempSize.z);
		} else {
			tempSize = textComponent.GetComponent<Renderer> ().bounds.size;
			float matLength = Mathf.Max (tempSize.x, tempSize.y);
			tempSize.Set (matLength, orgHeight, 0);
		}

		if (tempSize.x < 1f) {
			tempSize.Set(1f,tempSize.y,tempSize.x);
		}

		GetComponent<BoxCollider> ().size = tempSize * 1.2f;
		backgroundObj.GetComponent<MeshSprite>().Width = (tempSize.x)*1.2f;
		backgroundObj.GetComponent<MeshSprite>().Height = ( tempSize.y )*1.5f;

		GetComponent<TextItemController> ().UpdateSubBtnPos ();
	}
	
	//Check the Size of the 3d-text
	void checkChars () {
		if(textComponent.text.ToCharArray().Length==0) {
			textComponent.text = "";
			fitCollider();
		}
	}

	bool isMobileRuntime {
		get {
			return (Application.platform == RuntimePlatform.Android) || (Application.platform == RuntimePlatform.IPhonePlayer);
		}
	}

	public void StartWork()
	{
		if (!m_isStartWork) {
			this.m_isStartWork = true;
			this.inEditMode = true;

			keyboard = TouchScreenKeyboard.Open(textComponent.text);	
			if( m_StaticMesh != null )
			{
				m_StaticMesh.text = textComponent.text;
			}
		}
	}

	public void updateTextInfo()
	{
		m_StaticMesh.text = textComponent.text;
		m_StaticMesh.characterSize = textComponent.characterSize;
		m_StaticMesh.font = textComponent.font;
		m_StaticMesh.fontSize = textComponent.fontSize;
		fitCollider ();
	}

	public void StopWork()
	{
		this.m_isStartWork = false;
		if( keyboard != null )
		{
			inEditMode = false;
			keyboard.active = false;
			keyboard = null;
			if(textComponent.text.ToCharArray().Length==0) {
				textComponent.text = "Click And Edit Text";
			}
			fitCollider();
		}
#if UNITY_WEBPLAYER
		inEditMode = false;
		fitCollider();
#endif
	}
}

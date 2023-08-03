//----------------------------------------------
// FunnyCamera - realtime camera effects
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------


using UnityEngine;
using System.Collections;
using CWALK;

public class TextItemController : MonoBehaviour {

	private TextManager m_textmanager;

	public bool isActive = false;
	private Vector3 m_activePos ;
	private Vector3 m_unactivePos;
	public GameObject e_BackgroundObj;
	public GameObject m_closeObj;
	public GameObject m_RotateScaleObj;
	public TextMesh e_TargetTextObj;

	int m_ClickedCount = 0;
	// Use this for initialization
	void Start () {
		m_activePos = this.transform.localPosition;
		m_unactivePos = new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 0.5f);

	}


	// Update is called once per frame
	void Update () {

	}



	public void DeleteSelfObj()
	{
		if (m_textmanager != null) {
			m_textmanager.RemoveStickerItem(this);
			Destroy(this.gameObject);
		}
	}

	public void initItem()
	{

		if(m_closeObj != null)
		{
			m_closeObj.transform.localPosition= new Vector3(-1f * e_BackgroundObj.GetComponent<MeshSprite>().Width/2,
			                                                e_BackgroundObj.GetComponent<MeshSprite>().Height/2,
			                                                -0.1f);
		}

		if (m_RotateScaleObj != null) {
			m_RotateScaleObj.transform.localPosition= new Vector3(e_BackgroundObj.GetComponent<MeshSprite>().Width/2,
			                                                      -1f * e_BackgroundObj.GetComponent<MeshSprite>().Height/2,
			                                                -0.1f);
		}

		m_ClickedCount = 1;
		isActive = true;
	}

	public void setActive(bool value)
	{
		isActive = value;
		if (value ) {
			this.transform.localPosition = new Vector3(this.transform.localPosition.x,
			                                           this.transform.localPosition.y,
			                                           m_activePos.z);

			if(e_BackgroundObj != null  && m_closeObj!=null && m_RotateScaleObj!= null)
			{
				e_BackgroundObj.SetActive(true);
				m_closeObj.SetActive(true);
				m_RotateScaleObj.SetActive(true);
				GetComponent<TextMeshInput>().updateTextInfo();
			}
			m_ClickedCount ++;

		} else {
			GetComponent<TextMeshInput>().StopWork();

			this.transform.localPosition = new Vector3(this.transform.localPosition.x,
			                                           this.transform.localPosition.y,
			                                           m_unactivePos.z);
			if(e_BackgroundObj != null && m_closeObj!=null)
			{
			
				e_BackgroundObj.SetActive(false);
				m_closeObj.SetActive(false);
				m_RotateScaleObj.SetActive(false);
			}
			m_ClickedCount = 0;
		}
	}

	public void setInputActive()
	{
		if (m_ClickedCount >1 && !GetComponent<TextMeshInput>().isStartWork) {
			GetComponent<TextMeshInput>().StartWork();
		}
	}


	public void UpdateSubBtnPos()
	{
		if(m_closeObj != null)
		{
			m_closeObj.transform.localPosition= new Vector3(-1f * e_BackgroundObj.GetComponent<MeshSprite>().Width/2,
			                                                e_BackgroundObj.GetComponent<MeshSprite>().Height/2,
			                                                -0.1f);

		}

		if (m_RotateScaleObj != null) {
			m_RotateScaleObj.transform.localPosition = new Vector3 (e_BackgroundObj.GetComponent<MeshSprite> ().Width / 2,
			                                                        -1f * e_BackgroundObj.GetComponent<MeshSprite> ().Height / 2,
			                                                      -0.1f);
		}
	}

	public void setTextManager(TextManager mgr)
	{
		m_textmanager = mgr;
	}

	public void setTextColor(Color clr)
	{
		Color oldColor = e_TargetTextObj.GetComponent<TextMesh> ().color;
		e_TargetTextObj.GetComponent<TextMesh> ().color = new Color(clr.r,clr.g,clr.b,oldColor.a);
        //e_TargetTextObj.GetComponent<TextMesh>().color = new Color(clr.r, clr.g, clr.b, clr.a);
	}
	
	public void setTextAlpha(float value)
	{
		Color tempcolor = e_TargetTextObj.GetComponent<TextMesh> ().color;
		e_TargetTextObj.GetComponent<TextMesh> ().color = new Color (tempcolor.r, tempcolor.g, tempcolor.b, value);
	}

	public void setTextText(Font font)
	{

		GetComponent<TextMeshInput> ().setFont(font);
	}

}

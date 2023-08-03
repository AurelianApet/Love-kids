//----------------------------------------------
// FunnyCamera - realtime camera effects
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;
using CWALK;

public class StickerItemController : MonoBehaviour {

	private StickerManager m_stickermanager;
	public bool isActive = false;
	private Vector3 m_activePos ;
	private Vector3 m_unactivePos ;
	private GameObject m_borderObj;
	private GameObject m_stickerObj;
	private GameObject m_closeObj;
	private GameObject m_RotateScaleObj;
	private GameObject m_newRotateObj;
	private float _targetSizeX = 0.4f;

	void Awake()
	{
		m_activePos = transform.localPosition;
		m_unactivePos = new Vector3(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z + 0.5f);
		m_borderObj = transform.FindChild ("StickerBorder").gameObject;
		m_stickerObj = transform.FindChild ("StickerObj").gameObject;
		m_closeObj = transform.FindChild ("CloseBtnObj").gameObject;
		m_RotateScaleObj = transform.FindChild ("RotateScaleBtn").gameObject;
		m_newRotateObj = transform.FindChild ("RotateScaleBtn (2)").gameObject;
		if (m_stickerObj == null) {
			Debug.Log("nullnull null null null ");
		}
	}

	void Update()
	{
		if (this.isActive) {
			if(Input.GetMouseButtonDown(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit = new RaycastHit();;
				if(this.m_closeObj.GetComponent<Collider>().Raycast(ray,out hit,1000))
				{
					Debug.Log("curreing is enter");
					DeleteSelfObj();
				}
			}
		}
	}

	public void setStickerManager(StickerManager mgr)
	{
		m_stickermanager = mgr;
	}

	public void setActive(bool value)
	{
		isActive = value;
		if (value) {
			this.transform.localPosition = new Vector3(this.transform.localPosition.x,
			                                           this.transform.localPosition.y,
			                                           m_activePos.z);
			if(m_borderObj != null)
			{
				m_borderObj.SetActive(true);
				m_closeObj.SetActive(true);
				m_RotateScaleObj.SetActive(true);
				m_newRotateObj.SetActive (true);
			}

		} else {
			this.transform.localPosition = new Vector3(this.transform.localPosition.x,
			                                           this.transform.localPosition.y,
			                                           m_unactivePos.z);
			//this.transform.localPosition = m_unactivePos;
			if(m_borderObj != null && m_closeObj!=null)
			{
				m_borderObj.SetActive(false);
				m_closeObj.SetActive(false);
				m_RotateScaleObj.SetActive(false);
				m_newRotateObj.SetActive (false);
			}
		}
	}

	public void DeleteSelfObj()
	{
		if (m_stickermanager != null) {
			m_stickermanager.RemoveStickerItem(this);
			Destroy(this.gameObject);
		}
	}

	public bool initItemByMode(string stickerName)
	{
		Texture2D	tex =  (Texture2D)(Resources.Load("Face/"+stickerName));
		if (tex != null) {
			if (m_stickerObj != null) {
				m_stickerObj.GetComponent<Renderer> ().material.mainTexture = tex;
				float aspect = tex.width * 1.0f / tex.height;
				float scalez = _targetSizeX / aspect;
				m_stickerObj.transform.localScale = new Vector3 (_targetSizeX, 0, scalez);
			
				if (m_borderObj != null) {
					m_borderObj.GetComponent<MeshSprite> ().Width = _targetSizeX * 11;
					m_borderObj.GetComponent<MeshSprite> ().Height = scalez * 11;
					this.GetComponent<BoxCollider> ().size = new Vector3 (m_borderObj.GetComponent<MeshSprite> ().Width,
					                                                      m_borderObj.GetComponent<MeshSprite> ().Height,
					                                                    0f);
				}


				if (m_closeObj != null) {
					m_closeObj.transform.localPosition = new Vector3 (-1f * m_borderObj.GetComponent<MeshSprite> ().Width / 2,
					                                                m_borderObj.GetComponent<MeshSprite> ().Height / 2,
					                                                -0.1f);
					
				}
				
				if (m_RotateScaleObj != null) {
					m_RotateScaleObj.transform.localPosition = new Vector3 (m_borderObj.GetComponent<MeshSprite> ().Width / 2,
					                                                        -1f * m_borderObj.GetComponent<MeshSprite> ().Height / 2,
					                                                        -0.1f);
				}//

				if (m_newRotateObj != null) {
					m_newRotateObj.transform.localPosition = new Vector3 (m_borderObj.GetComponent<MeshSprite> ().Width / 2,
						m_borderObj.GetComponent<MeshSprite> ().Height / 2,
						-0.1f);
				}

				isActive = true;
			}
			return true;
		} else {
			return false;
		}

	}

}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using DG.DemiLib;


public class MaskUIController : MonoBehaviour {

	public GameObject m_ShareBgSprite;
	public static MaskUIController instance;
	public bool isUGUI = false;
	// Use this for initialization
	void Start () {
		instance = this;
	

	}

	public void Hide()
	{
		if (m_ShareBgSprite != null) {
			m_ShareBgSprite.GetComponent<Image> ().DOFade(0,0.6f).OnComplete(DoFadeFinished);
		//	m_ShareBgSprite.GetComponent<Image> ().DOFade(0,0.6f).OnComplete(DoFadeFinished);
		}
	}

	public void Show()
	{
		m_ShareBgSprite.SetActive (true);
		if (m_ShareBgSprite != null) {
			m_ShareBgSprite.GetComponent<Image> ().DOFade(0.8f,0.4f);
		}
	}

	void DoFadeFinished()
	{
		m_ShareBgSprite.SetActive (false);
	}
}

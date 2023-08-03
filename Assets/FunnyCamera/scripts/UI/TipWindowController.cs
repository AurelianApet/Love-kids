//----------------------------------------------
// FunnyCamera - realtime camera effects
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TipWindowController : MonoBehaviour {

	public bool isUGUI = false;
	// Use this for initialization
	public void Show()
	{
		if(MaskUIController.instance != null)
		{
			MaskUIController.instance.Show();
		}
		GetComponent<RectTransform>().DOLocalMove(new Vector3(0,0,0),0.6f);
	}

	public void Hide()
	{
		if(MaskUIController.instance != null)
		{
			MaskUIController.instance.Hide();
		}
	
		GetComponent<RectTransform>().DOLocalMove(new Vector3(250,0,0),0.6f);
	}

	public void ApplyBtnClicked()
	{
		Hide ();
	}

	public void CancelBtnClicked()
	{
		Hide ();
	}
}

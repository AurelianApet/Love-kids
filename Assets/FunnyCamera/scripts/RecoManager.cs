//----------------------------------------------
// FunnyCamera - realtime camera effects
// Copyright © 2015-2016 52cwalk team
// Contact us (lycwalk@gmail.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class RecoManager : MonoBehaviour {

	public Text text;

	public Image e_timeLine;
	public Toggle e_ToggleObj;
	Tweener timebacktweener;

	static bool m_isSupportRec = false;

	// Use this for initialization
	void Start () {
		Everyplay.ReadyForRecording += OnReadyForRecording;
		Everyplay.RecordingStopped += OnStopRecordingEvent;
		Everyplay.RecordingStarted += StartRecordingEvent;
	}

	public static bool isSupportRec()
	{
		return m_isSupportRec;
	}
	public static bool IsRecording()
	{
		return Everyplay.IsRecording ();
	}

	// Update is called once per frame
	void Update () {

	}

	public void OnReadyForRecording(bool enabled) {
		if (enabled) {
			m_isSupportRec = true;
			text.text= "supporting rec";
		} else {
			text.text= "not supporting rec";
		}
	}

	public void StartRecordingEvent()
	{
		text.text = "start recording ";
	}

	public void OnStopRecordingEvent() {
		text.text = "stop recording ";
		Everyplay.SetMetadata ("level","FunnyCamera!");

		ShowLastVideo ();
	}

	public void StartReco1()
	{
		if (m_isSupportRec) {
			Everyplay.StartRecording ();
			e_ToggleObj.enabled = false;
		}
		startBackTime ();
	}

	public void StopReco()
	{
		Everyplay.StopRecording ();
		if (timebacktweener.IsPlaying()) {
			timebacktweener.Pause ();
		}

		if (m_isSupportRec) {
			e_ToggleObj.enabled = true;
		}
		e_timeLine.fillAmount = 0f;
	}

	public void ShowLastVideo()
	{
		Everyplay.PlayLastRecording();
	}

	public void openVideo()
	{

	}

	void startBackTime()
	{
		e_timeLine.fillAmount = 0f;
		timebacktweener = e_timeLine.DOFillAmount (1, 11).SetEase (Ease.Linear).OnComplete(fillAmountEnd);
	}

	void fillAmountEnd()
	{
		if (m_isSupportRec) {
			StopReco ();
		}
	}
}

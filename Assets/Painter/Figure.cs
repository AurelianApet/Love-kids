using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Figure : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler
{
	public GamController controller;
	public int perfectCount;
	public GameObject triggersParent;
	public List<int> triggers;

	void Start()
	{
		if(triggersParent)
		triggersParent.SetActive(false);
	}

	public void GameStart()
	{
		if(triggersParent)
		triggersParent.SetActive(true);
	}


	public void OnPointerEnter(PointerEventData data)
	{
		controller.SendMessage("PointerEnter",this);
	}
	public void OnPointerExit(PointerEventData data)
	{
		if(data.selectedObject)
		if(data.selectedObject.tag != "Trigger")
		controller.SendMessage("PointerExit");
	}
	public void OnPointerDown(PointerEventData data)
	{
		controller.SendMessage("PointerClickDown");
	}
	public void OnPointerUp(PointerEventData data)
	{
		controller.SendMessage("PointerClickUp");
	}

	void PointerEnter(int id)
	{
		if(!triggers.Contains(id))
	    	triggers.Add(id);
	}
}

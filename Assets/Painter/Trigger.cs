using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Trigger : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{

	public Figure parent;
	public int id;

	public void OnPointerEnter(PointerEventData data)
	{
		parent.SendMessage("PointerEnter",id);
	}

	public void OnPointerDown(PointerEventData data)
	{
		parent.controller.SendMessage("PointerClickDown");
	}
	public void OnPointerUp(PointerEventData data)
	{
		parent.controller.SendMessage("PointerClickUp");
	}
}

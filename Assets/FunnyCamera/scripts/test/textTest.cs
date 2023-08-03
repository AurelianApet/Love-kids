using UnityEngine;
using System.Collections;

public class textTest : MonoBehaviour {

	public TextManager e_textmanager;

	public void createNewTextItem()
	{
		e_textmanager.setWork (true);
		e_textmanager.CreateItem ();
	}
}

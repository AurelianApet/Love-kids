using UnityEngine;
using System.Collections;

public class stickerTest : MonoBehaviour {

	public StickerManager e_stickermanager;
	public string stickername;
	// Use this for initialization
	void Start () {
		if (e_stickermanager != null) {
			e_stickermanager.setWork(true);
		}
	}

	public void CreateStickerItem()
	{
		if (!e_stickermanager.CreateItem (stickername)) {
			Debug.LogError("The Sticke Texture or the Instantiate Obj is not existed,Please check it!");
		}
	}

}

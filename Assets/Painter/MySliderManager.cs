using UnityEngine;
using System.Collections;
using common;

public class MySliderManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // the object identified by hit.transform was clicked
                // do whatever you want
                //Debug.Log("slider clicked "+hit.collider.tag);
            }
        }
	}

    void OnMouseDown()
    {
        Debug.Log("slider action");
        Utils.can_draw = false;
    }

    void OnMouseUp()
    {
        Utils.can_draw = true;
    }
}

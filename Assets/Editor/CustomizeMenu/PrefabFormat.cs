using UnityEngine;
using UnityEditor;
using System.Collections;

public class PrefabFormat : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [MenuItem("PrefabControl/ClearAll")]
    static public void TestCode()
    {
        PlayerPrefs.DeleteAll();
    }    
}

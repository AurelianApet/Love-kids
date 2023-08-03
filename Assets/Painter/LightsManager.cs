using UnityEngine;
using System.Collections;

public class LightsManager : MonoBehaviour {

    public Material shader1;
    public Material shader2;
    public Material shader3;
    public Material shader4;
    public Material shader5;
    public Material shader6;
    public Material shader7;
    public Material shader8;
    public Material shader9;
    public Material shader10;
    public Material shader11;

    public GameObject main_obj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    
    public void changeLensMode(int lens_mode)
    {
        if(lens_mode == 1)
        {
            main_obj.GetComponent<Renderer>().material = shader1;
        }
        else if (lens_mode == 2)
        {
            main_obj.GetComponent<Renderer>().material = shader2;
        }
        else if (lens_mode == 3)
        {
            main_obj.GetComponent<Renderer>().material = shader3;
        }
        else if (lens_mode == 4)
        {
            main_obj.GetComponent<Renderer>().material = shader4;
        }
        else if (lens_mode == 5)
        {
            main_obj.GetComponent<Renderer>().material = shader5;
        }
        else if (lens_mode == 6)
        {
            main_obj.GetComponent<Renderer>().material = shader6;
        }
        else if (lens_mode == 7)
        {
            main_obj.GetComponent<Renderer>().material = shader7;
        }
        else if (lens_mode == 8)
        {
            main_obj.GetComponent<Renderer>().material = shader8;
        }
        else if (lens_mode == 9)
        {
            main_obj.GetComponent<Renderer>().material = shader9;
        }
        else if (lens_mode == 10)
        {
            main_obj.GetComponent<Renderer>().material = shader10;
        }
        else if (lens_mode == 11)
        {
            main_obj.GetComponent<Renderer>().material = shader11;
        }
    }
}

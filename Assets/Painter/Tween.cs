using UnityEngine;
using System.Collections;

public class Tween : MonoBehaviour {

	public float time;
	float _time;
	void OnEnable()
	{
		_time = 0;
	}

	void Update () 
	{
		_time += Time.deltaTime;
		if(_time > time)
		{
			gameObject.SetActive(false);
		}
	}
}

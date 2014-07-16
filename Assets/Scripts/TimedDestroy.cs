using UnityEngine;
using System.Collections;

// Set a game object to destroy after a certain amount of time
public class TimedDestroy : MonoBehaviour 
{
	public float _lifetime = 1; // The lift time of the gameobject

	private float _time_instantiated; // The time the gameobject was instantiated

	// Use this for initialization
	void Start () 
	{
		_time_instantiated = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.time - _time_instantiated  > _lifetime)
			Destroy(gameObject);
	}
}

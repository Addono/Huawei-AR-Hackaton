using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TracePlayer : MonoBehaviour
{
	public GameObject Door;
	
	private GameObject _player;
	private int _lineOfSightLayer = 1 << 9;
	
	public UnityEvent onTriggerEnter;
	public UnityEvent onTriggerExit;

	private bool previousState;

	// Use this for initialization
	void Start () {
		_player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 ourPosition = transform.position + Vector3.up / 2;
		Vector3 theirPoisition = _player.transform.position + Vector3.up / 2;
		float distance = Vector3.Distance(ourPosition, theirPoisition);
		RaycastHit hit;
		bool newState = Physics.Raycast(ourPosition, (theirPoisition - ourPosition).normalized, out hit, distance, _lineOfSightLayer);
		if (newState)
		{
			Debug.DrawLine(ourPosition, hit.point, Color.red);
		}
		else
		{
			Debug.DrawLine(ourPosition, theirPoisition, Color.green);
		}

		if (newState != previousState)
		{
			previousState = newState;
			if (newState)
			{
				onTriggerEnter.Invoke();
			}
			else
			{
				onTriggerExit.Invoke();
			}
		}
	}
}

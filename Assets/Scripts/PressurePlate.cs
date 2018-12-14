using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour {

	public UnityEvent onTriggerEnter;
	public UnityEvent onTriggerExit;

	void OnTriggerEnter (Collider c) {
		onTriggerEnter.Invoke();
	}

	void OnTriggerExit (Collider c) {
		onTriggerExit.Invoke();
	}
}

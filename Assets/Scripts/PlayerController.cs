using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

	public LayerMask mask;
	PlayerMover mover;
	EventSystem _eventSystem;
	void Start () {
		mover = GetComponent<PlayerMover>();
		_eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask) && !_eventSystem.IsPointerOverGameObject()) {
				mover.MoveToPoint(hit.point);
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour {

	public float movementSpeed;

	Vector3 moveToPoint;
	float dist;
	Vector3 dir;
	Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	void Update () {
		if (dist > 0) {
			rb.position += dir * movementSpeed * Time.deltaTime;
			dist -= movementSpeed * Time.deltaTime;
		}
	}

	public void MoveToPoint (Vector3 point) {
		moveToPoint = point;
		moveToPoint.y = 0;
		dist = Vector3.Distance(rb.position, moveToPoint);
		dir = (moveToPoint - rb.position).normalized;
	}

	void OnCollisionEnter (Collision c) {
		dist = 0;
	}
}

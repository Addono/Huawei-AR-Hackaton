using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform player;

	Vector3 startPos;
	bool followingPlayer;

	void Start () {
		startPos = transform.position;
	}
	
	void Update () {
		if (followingPlayer) {
			var newPos = player.position;
			newPos.y = startPos.y;
			transform.position = newPos;
		} else {
			transform.position = startPos;
		}
	}

	public void FollowPlayer () {
		followingPlayer = true;
	}

	public void CenterCamera () {
		followingPlayer = false;
	}
}

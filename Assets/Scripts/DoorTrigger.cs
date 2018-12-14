using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

	public SlidingDoor door;
    Collider player;

    void Start() {
        player = FindObjectOfType<PlayerController>().GetComponentInChildren<Collider>();
        if (!player)
            Debug.LogError("couldn't find an object in the scene with a PlayerController and collider");
    }

    void OnTriggerEnter (Collider c) {
		if (c == player) {
            door.Open();
		}
	}

	void OnTriggerExit (Collider c) {
		if (c == player) {
            door.Close();
        }
	}
}

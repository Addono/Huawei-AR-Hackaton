using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointGuard : MonoBehaviour {
    public float speed;
    public Transform[] waypoints;

    int nextWaypoint = 0;

	void Update () {
        var goalPos = waypoints[nextWaypoint].position;
        transform.rotation = Quaternion.LookRotation(goalPos - transform.position, Vector3.up);
        transform.position = Vector3.MoveTowards(transform.position, goalPos, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, goalPos) < 0.1f) {
            nextWaypoint = (nextWaypoint + 1) % waypoints.Length;
        }
	}
}

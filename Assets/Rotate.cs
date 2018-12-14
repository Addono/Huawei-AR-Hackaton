using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Rotate : MonoBehaviour {

    [FormerlySerializedAs("rotationSpeed")] public int RotationSpeed = 50;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, RotationSpeed, 0) * Time.deltaTime);
	}
}

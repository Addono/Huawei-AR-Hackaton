using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShootCannonBalls : MonoBehaviour
{

	public GameObject CannonBall;
	public Transform Origin;

	private float timer = 0f;
	
	// Use this for initialization
	void Start ()
	{
		ShootCannonball();
	}

	private void ShootCannonball()
	{
		GameObject cb = Instantiate(CannonBall, Origin);
		cb.GetComponent<Rigidbody>().AddRelativeForce(0, 0, 500);
	}

	// Update is called once per frame
	void Update ()
	{
		timer -= Time.deltaTime;
		while (timer < 0)
		{
			ShootCannonball();
			timer += 2;
		}
	}
}

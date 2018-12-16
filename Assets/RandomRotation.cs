using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{

    [Range(0f, 1f)] public float scaling = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * scaling;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNpc : MonoBehaviour
{

    public float SpawnInterval = 2f;
    public GameObject npc;
    
    private float _time;
    
    // Start is called before the first frame update
    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        while (_time > SpawnInterval)
        {
            _time -= SpawnInterval;
            Spawn();
        }
    }

    private void Spawn()
    {
        Instantiate(npc, transform.position + Vector3.up * 3f, Quaternion.identity);
    }
}

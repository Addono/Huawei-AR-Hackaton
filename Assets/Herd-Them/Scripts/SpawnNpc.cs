using System.Collections;
using System.Collections.Generic;
using Scripts;
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
    private void Spawn()
    {
       
        Instantiate(npc, transform.position + Vector3.up * 3f, Quaternion.identity);
    }

    void Update()
    {
        _time += Time.deltaTime;

        while (_time > SpawnInterval)
        {
            _time -= SpawnInterval;
            Spawn();
        }
    }

    public List<Transform> getPatrolPoints()
    {
        GameObject patrolPointsGameObject = gameObject.transform.Find("PatrolPoints").gameObject;
        int patrolPointsGameObjectCount = patrolPointsGameObject.transform.childCount;
        List<Transform> patrolPoints = new List<Transform>();
        for (int i = 0; i < patrolPointsGameObjectCount; ++i)
        {
            patrolPoints.Add(patrolPointsGameObject.transform.GetChild(i));
        }

        return patrolPoints;
    }
}

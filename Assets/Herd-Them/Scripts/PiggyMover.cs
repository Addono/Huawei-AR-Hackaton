using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts
{
    public class PiggyMover : MonoBehaviour
    {
        [SerializeField] public bool EnablePatrolWaiting;

        [SerializeField] public float TotalWaitTime = 2f;

        [SerializeField] public List<Transform> PatrolPoints;

        private NavMeshAgent _navMeshAgent;
        private Rigidbody _rigidbody;
        private SpawnNpc _spawnNpc;
        private int _currentRandomIndex;
        private bool _travelling;
        private bool _waiting;
        private float _waitTimer;
        private bool _isStart;

        private void Start()
        {
            GameObject world = GameObject.FindWithTag("GameArea");
            _spawnNpc = world.GetComponent<SpawnNpc>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _rigidbody = GetComponent<Rigidbody>();
            _isStart = true;
            PatrolPoints = _spawnNpc.getPatrolPoints();
            if (PatrolPoints != null && PatrolPoints.Count >= 2)
            {
                ChangePatrolPoint();
                SetDestination();
            }
            else
            {
                Debug.LogError("Not enough patrol objects.");
            }
        }

        private void FixedUpdate()
        {
            if (!IsOnMesh() && !_isStart)
            {
                _navMeshAgent.enabled = false;
                _rigidbody.isKinematic = false;
                _rigidbody.useGravity = true;
                Debug.Log("NOT on ground");
            }
            else
            {
                _isStart = false;
                _navMeshAgent.enabled = true;
                _rigidbody.isKinematic = true;
                _rigidbody.useGravity = false;
                Debug.Log("On ground");
                if (_travelling && _navMeshAgent.remainingDistance <= 1.0f)
                {
                    _travelling = false;
                    if (EnablePatrolWaiting)
                    {
                        _waiting = true;
                        _waitTimer = 0f;
                    }
                    else
                    {
                        ChangePatrolPoint();
                        SetDestination();
                    }
                }

                if (_waiting)
                {
                    _waitTimer += Time.fixedDeltaTime;
                    if (_waitTimer >= TotalWaitTime)
                    {
                        _waiting = false;
                        ChangePatrolPoint();
                        SetDestination();
                    }
                }
            }
        }


        private void ChangePatrolPoint()
        {
            _currentRandomIndex = Random.Range(0, PatrolPoints.Count);
        }

        private void SetDestination()
        {
            Vector3 targetVector = Vector3.Scale(Random.insideUnitSphere, new Vector3(1,0,1)) * 0.4f + PatrolPoints[_currentRandomIndex].transform.position;
            _navMeshAgent.SetDestination(targetVector);
            _travelling = true;
        }

        private bool IsOnMesh()
        {
            NavMeshHit hit;
            bool sampleResult =
                NavMesh.SamplePosition(transform.position, out hit, _navMeshAgent.radius, NavMesh.AllAreas);
            return sampleResult;
        }
    }
}
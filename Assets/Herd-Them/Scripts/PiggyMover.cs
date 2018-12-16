using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts
{
    public class PiggyMover : MonoBehaviour
    {
        [SerializeField] public bool EnablePatrolWaiting;

        [SerializeField] public float TotalWaitTime = 2f;

        [SerializeField] public List<Waypoint> PatrolPoints;

        private NavMeshAgent _navMeshAgent;
        private int _currentRandomIndex;
        private bool _travelling;
        private bool _waiting;
        private float _waitTimer;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
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
            if(_travelling && _navMeshAgent.remainingDistance <= 1.0f)
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
            
            if(_waiting)
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

        private void ChangePatrolPoint()
        {
            _currentRandomIndex = Random.Range(0, PatrolPoints.Count);
        }
        
        private void SetDestination()
        {
            Vector3 targetVector = PatrolPoints[_currentRandomIndex].transform.position;
            _navMeshAgent.SetDestination(targetVector);
            _travelling = true;
        }

//        private Vector3 _target;
//        private float _timeToChangeDirection = 2;
//
//        private NavMeshAgent _navMeshAgent;
//
//        private void Start()
//        {
//            GameObject gameArea = GameObject.FindWithTag("GameArea");
//            MeshCollider gameAreaMeshCollider = gameArea.GetComponent<MeshCollider>();
//            _navMeshAgent = GetComponent<NavMeshAgent>();
//            _target = GetNewTarget(gameAreaMeshCollider);
////            _timeToChangeDirection = WaitBeforeDirectionChange;
//            _navMeshAgent.SetDestination(_target);
//        }
//
//        // Update is called once per frame
//        private void FixedUpdate()
//        {
//            if (IsCloseEnough())
//            {
//                if (_timeToChangeDirection > 0)
//                {
//                    Debug.Log(_timeToChangeDirection);
//                    _timeToChangeDirection -= Time.deltaTime;
//                    return;
//                }
//
//                GameObject gameArea = GameObject.FindWithTag("GameArea");
//                MeshCollider gameAreaMeshCollider = gameArea.GetComponent<MeshCollider>();
//                _target = GetNewTarget(gameAreaMeshCollider);
//                _timeToChangeDirection = 2;
//                _navMeshAgent.SetDestination(_target);
//            }
//        }
//
//        private Vector3 GetNewTarget(MeshCollider gameAreaMeshCollider)
//        {
//            Vector3 min = gameAreaMeshCollider.bounds.min;
//            Vector3 max = gameAreaMeshCollider.bounds.max;
//            return new Vector3(Random.Range(min.x, max.x), max.y + 0.1f, Random.Range(min.z, max.z));
//        }
//
//        private bool IsCloseEnough()
//        {
//            if (_navMeshAgent.pathPending || _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance) return false;
//            return !_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude <= 0.1f;
//        }
    }
}
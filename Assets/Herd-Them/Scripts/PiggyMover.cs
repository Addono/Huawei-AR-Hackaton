using UnityEngine;
using UnityEngine.AI;

namespace Scripts
{
    public class PiggyMover : MonoBehaviour
    {
        public float MaxSpeed;
        private Vector3 _target;
        private float _timeToChangeDirection = 2;

        private NavMeshAgent _navMeshAgent;

        private void Start()
        {
            GameObject gameArea = GameObject.FindWithTag("GameArea");
            MeshCollider gameAreaMeshCollider = gameArea.GetComponent<MeshCollider>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _target = GetNewTarget(gameAreaMeshCollider);
//            _timeToChangeDirection = WaitBeforeDirectionChange;
            _navMeshAgent.SetDestination(_target);
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (IsCloseEnough())
            {
                if (_timeToChangeDirection > 0)
                {
                    Debug.Log(_timeToChangeDirection);
                    _timeToChangeDirection -= Time.deltaTime;
                    return;
                }

                GameObject gameArea = GameObject.FindWithTag("GameArea");
                MeshCollider gameAreaMeshCollider = gameArea.GetComponent<MeshCollider>();
                _target = GetNewTarget(gameAreaMeshCollider);
                _timeToChangeDirection = 2;
                _navMeshAgent.SetDestination(_target);
            }
        }

        private Vector3 GetNewTarget(MeshCollider gameAreaMeshCollider)
        {
            Vector3 min = gameAreaMeshCollider.bounds.min;
            Vector3 max = gameAreaMeshCollider.bounds.max;
            return new Vector3(Random.Range(min.x, max.x), max.y + 0.1f, Random.Range(min.z, max.z));
        }

        private bool IsCloseEnough()
        {
            if (_navMeshAgent.pathPending || _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance) return false;
            return !_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude <= 0.1f;
        }
    }
}
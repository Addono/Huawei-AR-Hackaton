using UnityEngine;

namespace Scripts
{
    public class PiggyMover : MonoBehaviour
    {
        public float MoveSpeed;
        public float RotateSpeed;
        public float WaitBeforeDirectionChange;
        
        private bool _isOnGround;
        private Vector3 _target;
        private float _timeToChangeDirection;

        private void Start()
        {
            GameObject gameArea = GameObject.FindWithTag("GameArea");
            MeshCollider gameAreaMeshCollider = gameArea.GetComponent<MeshCollider>();
            _target = GetNewTarget(gameAreaMeshCollider);
            _timeToChangeDirection = WaitBeforeDirectionChange;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (_isOnGround)
            {
                if (IsCloseEnough(transform.position, _target))
                {
                    if (_timeToChangeDirection > 0)
                    {
                        _timeToChangeDirection -= Time.deltaTime;
                        return;
                    }
                    GameObject gameArea = GameObject.FindWithTag("GameArea");
                    MeshCollider gameAreaMeshCollider = gameArea.GetComponent<MeshCollider>();
                    _target = GetNewTarget(gameAreaMeshCollider);
                    _timeToChangeDirection = WaitBeforeDirectionChange;
                }
                
                Vector3 targetDir = _target - transform.position; 
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, RotateSpeed * Time.deltaTime, 0.0f);
                Debug.DrawRay(transform.position, newDir, Color.red);
                transform.rotation = Quaternion.LookRotation(newDir);
                Vector3 newTarget = Vector3.MoveTowards(transform.position, _target, MoveSpeed * Time.deltaTime);
                GetComponent<Rigidbody>().MovePosition(newTarget);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("GameArea"))
            {
                _isOnGround = true;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("GameArea"))
            {
                _isOnGround = false;
            }
        }

        private Vector3 GetNewTarget(MeshCollider gameAreaMeshCollider)
        {
            Vector3 min = gameAreaMeshCollider.bounds.min;
            Vector3 max = gameAreaMeshCollider.bounds.max;
            return new Vector3(Random.Range(min.x, max.x), max.y + 0.1f, Random.Range(min.z, max.z));
        }

        private bool IsCloseEnough(Vector3 piggy, Vector3 target)
        {
            Debug.Log("Distance: " + Vector3.Distance(piggy, target));
            return Vector3.Distance(piggy, target) < 0.5;
        }
    }
}
using UnityEngine;

namespace Scripts
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField]
        public float DebugDrawRadius = 1.0f;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, DebugDrawRadius);
        }
    }
}

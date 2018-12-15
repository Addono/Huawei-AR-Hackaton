using UnityEngine;

namespace Scripts
{
    public class PiggyMover : MonoBehaviour
    {
        public float Speed;
        
        private void Start()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = transform.forward * Speed;
        }
    }
}

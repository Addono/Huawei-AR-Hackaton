using System;
using UnityEngine;

namespace Scripts
{
    public class Slingshot : MonoBehaviour
    {
        private GameObject projectileSource;
        
        private GameObject slingshot;
        private GameObject pendingProjectile;
        private bool released;
        private Plane _slingshotPlane;

        private Vector3 _projectileToScreenDirection = Vector3.forward;
        public Vector3 ProjectileToScreenDirection
        {
            set { _projectileToScreenDirection = value; }
        }

        public void Create(GameObject slingshotPrefab, GameObject projectilePrefab, GameObject projectileSource)
        {
            this.projectileSource = projectileSource;
            
            slingshot = Instantiate(
                slingshotPrefab, 
                _relativeToCameraPosition(0.3f), 
                projectileSource.transform.rotation
            );
            _slingshotPlane = new Plane(slingshot.transform.forward, slingshot.transform.position);
            
            pendingProjectile = Instantiate(projectilePrefab, projectileSource.transform);
        }

        public void Release()
        {
            released = true;
            
            float power = (float) Math.Sqrt(1000f * Vector3.Distance(slingshot.transform.position, pendingProjectile.transform.position));
            Vector3 force = power * (slingshot.transform.position - pendingProjectile.transform.position);

            Rigidbody rb = pendingProjectile.GetComponent<Rigidbody>();
            rb.AddForce(force, ForceMode.VelocityChange);
            
            Destroy(slingshot, 3);
            Destroy(pendingProjectile, 8);
        }

        public void Update()
        {
            if (slingshot && !released)
            {
                pendingProjectile.transform.SetPositionAndRotation(
                    _relativeToCameraPosition(0.3f, _projectileToScreenDirection), 
                    projectileSource.transform.rotation
                );
            } else if (slingshot && released)
            {
                Rigidbody rb = pendingProjectile.GetComponent<Rigidbody>();
                rb.useGravity = ProjectileHasPassedSlingshot(); // Enable gravity after the projectile has passed the slingshot.
            }
        }

        /// <summary>
        /// Checks if the projectile has passed the slingshot or not.
        /// Note: Assumes that the sling shot plane is up-to-date.
        /// </summary>
        /// <returns>True if the projectile has passed the slingshot, false otherwise.</returns>
        private bool ProjectileHasPassedSlingshot()
        {
            return !_slingshotPlane.GetSide(projectileSource.transform.position);
        }

        private Vector3 _relativeToCameraPosition(float distance, Vector3 direction)
        {
            return projectileSource.transform.position + distance * projectileSource.transform.TransformDirection(direction);
        }

        private Vector3 _relativeToCameraPosition(float distance)
        {
            return _relativeToCameraPosition(distance, Vector3.forward);
        }
    }
}
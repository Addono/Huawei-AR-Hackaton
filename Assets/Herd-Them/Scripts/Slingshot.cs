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
        private float _slingshotOpacity = 1f;
        private float _timeToFade = 1f;
        private float _fadeDuration = 1f;

        private Vector3 _projectileToScreenDirection = Vector3.forward;
        public Vector3 ProjectileToScreenDirection
        {
            set { _projectileToScreenDirection = value.normalized;}
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
                    projectileSource.transform.position + .3f * _projectileToScreenDirection,
                    projectileSource.transform.rotation
                );
            } else if (slingshot && released)
            {
                Rigidbody rb = pendingProjectile.GetComponent<Rigidbody>();
                rb.useGravity = ProjectileHasPassedSlingshot(); // Enable gravity after the projectile has passed the slingshot.

                // Update the opacity of the slingshot
                _timeToFade -= Time.deltaTime;
                _slingshotOpacity = Math.Max(0, -_timeToFade * 100f / _fadeDuration);
                
                Color c = slingshot.GetComponent<MeshRenderer>().material.color;
                c.a = _slingshotOpacity;
                slingshot.GetComponent<MeshRenderer>().material.color = c;
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

        private Vector3 _relativeToCameraPosition(float distance)
        {
            return _relativeToCameraPosition(distance, Vector3.forward);
        }

        private Vector3 _relativeToCameraPosition(float distance, Vector3 direction)
        {
            return projectileSource.transform.position + distance * projectileSource.transform.TransformDirection(direction);
        }
    }
}
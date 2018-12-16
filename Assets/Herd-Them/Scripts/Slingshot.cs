using System;
using UnityEngine;
using Random = System.Random;

namespace Scripts
{
    public class Slingshot : MonoBehaviour
    {
        private GameObject projectileSource;
        private GameObject indicator;
        
        private GameObject slingshot;
        private GameObject pendingProjectile;
        private bool released;
        private Plane _slingshotPlane;

        private Vector3 _projectileToScreenDirection = Vector3.forward;
        public Vector3 ProjectileToScreenDirection
        {
            set { _projectileToScreenDirection = value.normalized;}
        }

        public void Create(GameObject slingshotPrefab, GameObject projectilePrefab, GameObject projectileSource, GameObject indicator)
        {
            this.projectileSource = projectileSource;
            this.indicator = indicator;
            
            slingshot = Instantiate(
                slingshotPrefab, 
                _relativeToCameraPosition(0.3f, Vector3.forward), 
                projectileSource.transform.rotation
            );
            _slingshotPlane = new Plane(slingshot.transform.forward, slingshot.transform.position);
            
            pendingProjectile = Instantiate(projectilePrefab, projectileSource.transform.position, projectileSource.transform.rotation);
            Rigidbody rb = pendingProjectile.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
            
            pendingProjectile.GetComponent<Material>().color = Color.black;
        }

        public void Release()
        {
            released = true;
            
            var force = Force();

            Rigidbody rb = pendingProjectile.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(force, ForceMode.VelocityChange);

            iTween.FadeTo(slingshot, 0f, .5f);
            Destroy(slingshot, 3f);
            
            iTween.FadeTo(pendingProjectile, iTween.Hash("time", 0.5f, "delay", 7.5f, "alpha", 0));
            Destroy(pendingProjectile, 8);
        }

        private Vector3 Force()
        {
            float power = (float) Math.Sqrt(1000f * Vector3.Distance(slingshot.transform.position, pendingProjectile.transform.position));
            Vector3 force = power * (slingshot.transform.position - pendingProjectile.transform.position);
            return force;
        }

        public void Update()
        {
            if (slingshot && !released)
            {
                pendingProjectile.transform.SetPositionAndRotation(
                    projectileSource.transform.position + .3f * _projectileToScreenDirection,
                    projectileSource.transform.rotation
                );
                slingshot.transform.rotation = projectileSource.transform.rotation;

                LineRenderer lr = indicator.GetComponent<LineRenderer>();
                lr.positionCount = 50;
                lr.SetPosition(0, projectileSource.transform.position + .5f * _projectileToScreenDirection);

                Vector3 initial = Force();
                for (int i = 1; i < 50; i++)
                {
                    float t = (i - 2) / 50f;
                    Vector3 output = initial * t + 0.5f * Physics.gravity * t * t;
                    lr.SetPosition(i, pendingProjectile.transform.position + output);
                }

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
    }
}
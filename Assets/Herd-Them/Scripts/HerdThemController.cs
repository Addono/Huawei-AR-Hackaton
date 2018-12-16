using System;
using System.Collections.Generic;
using Common;
using HuaweiARInternal;
using HuaweiARUnitySDK;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class HerdThemController : MonoBehaviour
    {
        [Tooltip("plane visualizer")] public GameObject planePrefabs;
        
        [Tooltip("world")] public GameObject worldPrefab;        
        
        [Tooltip("projectile")] public GameObject projectilePrefab;
        
        [Tooltip("projectile source")] public GameObject projectileSource;
        
        [Tooltip("slingshot")] public GameObject slingshotPrefab;

        [Tooltip("indicator")] public GameObject indicator;

        private List<ARPlane> newPlanes = new List<ARPlane>();

        private GameObject world;
        private Slingshot slingshot;

        private int _score;
        
        public Text ScoreText;

        private void Start()
        {
            _score = 0;
            UpdateScore();
        }

        public void Update()
        {
            _DrawPlane();
            
            Touch touch;
            if (ARFrame.GetTrackingState() == ARTrackable.TrackingState.TRACKING) // Only check for touch if we are tracking our environment.
            {
                touch = Input.GetTouch(0);
                switch (Input.touchCount)
                {
                    case 1:
                        switch (touch.phase)
                        {
                            case TouchPhase.Began:
                                if (slingshot) // Cleanup in case we missed the touch phase end or release
                                {
                                    slingshot.Release();
                                }
                                
                                slingshot = world.AddComponent<Slingshot>();
                                slingshot.Create(slingshotPrefab, projectilePrefab, projectileSource, indicator);
                                
                                if (Camera.main != null)
                                    slingshot.ProjectileToScreenDirection = Camera.main.ScreenPointToRay(touch.position).direction * 0.5f;
                                break;
                            case TouchPhase.Ended:
                            case TouchPhase.Canceled:
                                slingshot.Release();
                                slingshot = null;
                                break;
                            default:
                                if (Camera.main != null)
                                    slingshot.ProjectileToScreenDirection = Camera.main.ScreenPointToRay(touch.position).direction * 0.5f;
                                break;
                        }
                        break;
                    default:
                        if (touch.phase == TouchPhase.Began)
                        {
                            _CreateWorld(touch);
                        }
                        break;
                }
            }
        }

        private void _DrawPlane()
        {
            newPlanes.Clear();
            ARFrame.GetTrackables<ARPlane>(newPlanes, ARTrackableQueryFilter.NEW);
            for (int i = 0; i < newPlanes.Count; i++)
            {
                GameObject planeObject = Instantiate(planePrefabs, Vector3.zero, Quaternion.identity, transform);
                planeObject.GetComponent<TrackedPlaneVisualizer>().Initialize(newPlanes[i]);
            }
        }

        private void _CreateWorld(Touch touch)
        {
            List<ARHitResult> hitResults = ARFrame.HitTest(touch);
            ARDebug.LogInfo("_DrawARLogo hitResults count {0}", hitResults.Count);
            foreach (ARHitResult singleHit in hitResults)
            {
                ARTrackable trackable = singleHit.GetTrackable();
                ARDebug.LogInfo("_DrawARLogo GetTrackable {0}", singleHit.GetTrackable());
                if (trackable is ARPlane && ((ARPlane) trackable).IsPoseInPolygon(singleHit.HitPose) 
                    || trackable is ARPoint)
                {
                    ARAnchor anchor = singleHit.CreateAnchor();
                    
                    Vector3 anchorPosition = anchor.GetPose().position;
                    anchorPosition.y += 0.5f;

                    if (world)
                    {
                        world.transform.position = anchorPosition;
                    }
                    else
                    {
                        world = Instantiate(worldPrefab, anchorPosition, Quaternion.AngleAxis(-90, Vector3.left));
                    }
                    break;
                }
            }
        }

        public void IncreaseScore()
        {
            _score += 1;
            UpdateScore();
        }

        public void DecreaseScore()
        {
            _score -= 1;
            UpdateScore();
        }
        
        private void UpdateScore()
        {
            ScoreText.text = "Score: " + _score;
        }
    }
}
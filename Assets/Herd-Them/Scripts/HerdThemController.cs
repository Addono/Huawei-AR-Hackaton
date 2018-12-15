﻿using System;
using System.Collections.Generic;
using Common;
using HuaweiARInternal;
using HuaweiARUnitySDK;
using UnityEngine;

namespace Scripts
{
    public class HerdThemController : MonoBehaviour
    {
        [Tooltip("plane visualizer")] public GameObject planePrefabs;
        
        [Tooltip("world")] public GameObject worldPrefab;        
        
        [Tooltip("projectile")] public GameObject projectilePrefab;
        
        [Tooltip("projectile source")] public GameObject projectileSource;
        
        [Tooltip("slingshot")] public GameObject slingshotPrefab;

        private List<ARPlane> newPlanes = new List<ARPlane>();

        private GameObject world;
        private Slingshot slingshot;

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
                                slingshot = world.AddComponent<Slingshot>();
                                slingshot.Create(slingshotPrefab, projectilePrefab, projectileSource);
                                
                                if (Camera.main != null)
                                    slingshot.ProjectileToScreenDirection = Camera.main.ScreenPointToRay(touch.position).direction * 0.5f;
                                break;
                            case TouchPhase.Moved:
                                if (Camera.main != null)
                                    slingshot.ProjectileToScreenDirection = Camera.main.ScreenPointToRay(touch.position).direction * 0.5f;
                                break;
                            case TouchPhase.Ended:
                            case TouchPhase.Canceled:
                                slingshot.Release();
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

                    if (world)
                    {
                        world.transform.position = anchor.GetPose().position;
                    }
                    else
                    {
                        world = Instantiate(worldPrefab, anchor.GetPose().position, Quaternion.identity);
                    }
                    break;
                }
            }
        }
    }
}
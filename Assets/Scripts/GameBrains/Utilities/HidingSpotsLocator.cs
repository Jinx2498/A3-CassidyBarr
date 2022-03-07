using System.Collections.Generic;
using GameBrains.Entities;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Utilities
{
    public class HidingSpotsLocator
    {
        float offset = 4f;
        public float Offset { get => offset; set => offset = value; }

        float searchRadius = 10f;
        public float SearchRadius { get => searchRadius; set => searchRadius = value; }

        int maxColliders = 10;

        public int MaxColliders { get => maxColliders; set => maxColliders = value; }

        Collider[] hitColliders;

        readonly SteerableAgent hider;

        public HidingSpotsLocator(SteerableAgent steerableEntity)
        {
            hider = steerableEntity;
        }

        public List<VectorXZ> GetVisibleHidingSpots(Entity threatEntity)
        {
            return GetVisibleHidingSpots(threatEntity.StaticData.Location);
        }

        public List<VectorXZ> GetVisibleHidingSpot(Transform threatTransform)
        {
            return GetVisibleHidingSpots((VectorXZ)threatTransform.position);
        }

        public List<VectorXZ> GetVisibleHidingSpots(VectorXZ threatPosition)
        {
            if (hitColliders == null || hitColliders.Length != MaxColliders)
            {
                hitColliders = new Collider[MaxColliders];
            }
            
            VectorXZ hiderPosition = (VectorXZ)hider.transform.position;

            // Find potential obstacles to hide behind
            int colliderCount
                = Physics.OverlapSphereNonAlloc(
                    (Vector3)hiderPosition,
                    searchRadius,
                    hitColliders,
                    hider.SteeringData.ObstacleLayerMask);

            List<VectorXZ> visibleHidingSpots = new List<VectorXZ>();
            for (var i = 0; i < colliderCount; i++)
            {
                Collider hitCollider = hitColliders[i];
                VectorXZ center = (VectorXZ)hitCollider.bounds.center;

                // closet point to threat on the collider
                VectorXZ closestPoint
                    = (VectorXZ)hitCollider.ClosestPoint((Vector3)threatPosition);

                // direction from the closest point to the center
                VectorXZ direction = center - closestPoint;

                // distance from closest point to the center
                float distance = direction.magnitude;

                // hiding spot is the point on the opposite side of the collider plus the offset
                VectorXZ possibleHidingSpot = center + direction.normalized * (distance + offset);

                if (hider.SteeringData.HasLineOfSight((VectorXYZ)possibleHidingSpot))
                {
                    visibleHidingSpots.Add(possibleHidingSpot);
                }
            }

            return visibleHidingSpots;
        }

        public VectorXZ GetClosestHidingSpot(Entity threatEntity)
        {
            return GetClosestHidingSpot(threatEntity.StaticData.Location);
        }

        public VectorXZ GetClosestHidingSpot(Transform threatTransform)
        {
            return GetClosestHidingSpot((VectorXZ)threatTransform.position);
        }

        public VectorXZ GetClosestHidingSpot(VectorXZ threatPosition)
        {
            if (hitColliders == null || hitColliders.Length != MaxColliders)
            {
                hitColliders = new Collider[MaxColliders];
            }
            
            VectorXZ hiderPosition = (VectorXZ)hider.transform.position;

            // Find potential obstacles to hide behind
            int colliderCount
                = Physics.OverlapSphereNonAlloc(
                    (Vector3)hiderPosition,
                    searchRadius,
                    hitColliders,
                    hider.SteeringData.ObstacleLayerMask);

            float closestHidingDistance = float.MaxValue;
            VectorXZ closestHidingSpot = hiderPosition;

            for (var i = 0; i < colliderCount; i++)
            {
                Collider hitCollider = hitColliders[i];
                VectorXZ center = (VectorXZ)hitCollider.bounds.center;

                // closet point to threat on the collider
                VectorXZ closestPoint
                    = (VectorXZ)hitCollider.ClosestPoint((Vector3)threatPosition);

                // direction from the closest point to the center
                VectorXZ direction = center - closestPoint;

                // distance from closest point to the center
                float distance = direction.magnitude;

                // hiding spot is the point on the opposite side of the collider plus the offset
                VectorXZ possibleHidingSpot = center + direction.normalized * (distance + offset);

                float distanceToHidingSpot
                    = VectorXZ.Distance(hiderPosition, possibleHidingSpot);

                if (hider.SteeringData.HasLineOfSight((VectorXYZ)possibleHidingSpot) &&
                    distanceToHidingSpot < closestHidingDistance)
                {
                    closestHidingDistance = distanceToHidingSpot;
                    closestHidingSpot = possibleHidingSpot;
                }
            }

            return closestHidingSpot;
        }
    }
}
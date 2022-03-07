using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;
using GameBrains.Utilities;
using UnityEngine;

namespace Testing
{
    public sealed class W20STestHidingSpot : ExtendedMonoBehaviour
    {
        public bool respawn;
        public bool testHidingSpot;
        public bool removeArrive;
        public bool resetHidingSpotLocator;
        public VectorXZ spawnLocation = new VectorXZ(10f, 0f);
        public Transform threatTransform;
        public Transform hidingSpotTransform;
        public SteerableAgent hider;
        Arrive arrive;
        HidingSpotsLocator hidingSpotsLocator;
        VectorXZ closestHidingSpot;
        public bool noStop;
        public bool noSlow;
        public bool neverCompletes;
        public float linearStopAtSpeed = 0.1f;
        public float slowEnoughLinearSpeed = 0.5f;
        public float linearDrag = 1.015f;
        public float closeEnoughDistance = 1.5f;
        public float brakingDistance = 5f;
        public float locatorOffset = 4f;
        public float locatorSearchRadius = 10f;
        public int locatorMaxColliders = 10;

        public override void Awake()
        {
            base.Awake();
            CreateHidingSpotLocator();
        }

        void CreateHidingSpotLocator()
        {
            hidingSpotsLocator =
                new HidingSpotsLocator(hider)
                {
                    Offset = locatorOffset,
                    SearchRadius = locatorSearchRadius,
                    MaxColliders = locatorMaxColliders
                };
        }

        public override void Update()
        {
            base.Update();
            
            if (hider == null) { return; }

            if (respawn)
            {
                respawn = false;
                hider.Spawn((VectorXYZ)spawnLocation);
            }

            if (resetHidingSpotLocator)
            {
                resetHidingSpotLocator = false;
                CreateHidingSpotLocator();
            }

            if (removeArrive)
            {
                removeArrive = false;
                RemoveAndDestroyArrive();
            }

            if (testHidingSpot)
            {
                testHidingSpot = false;

                if (threatTransform != null)
                {
                    closestHidingSpot = hidingSpotsLocator.GetClosestHidingSpot(threatTransform);

                    if (hidingSpotTransform != null)
                    {
                        hidingSpotTransform.position = (Vector3) closestHidingSpot;
                    }
                }

                RemoveAndDestroyArrive();

                arrive = hidingSpotTransform != null
                    ? Arrive.CreateInstance(hider.SteeringData, hidingSpotTransform)
                    : Arrive.CreateInstance(hider.SteeringData, closestHidingSpot);

                SetParameters(arrive);
                hider.SteeringData.AddSteeringBehaviour(arrive);
            }
        }
        
        void SetParameters(Arrive sb)
        {
            sb.NoStop = noStop;
            sb.NoSlow = noSlow;
            sb.NeverCompletes = neverCompletes;
            sb.LinearStopAtSpeed = linearStopAtSpeed;
            sb.SlowEnoughLinearSpeed = slowEnoughLinearSpeed;
            sb.LinearDrag = linearDrag;
            sb.CloseEnoughDistance = closeEnoughDistance;
            sb.BrakingDistance = brakingDistance;
        }

        void RemoveAndDestroyArrive()
        {
            if (arrive != null)
            {
                hider.SteeringData.RemoveSteeringBehaviour(arrive);
                Destroy(arrive);
                arrive = null;
            }
        }
    }
}
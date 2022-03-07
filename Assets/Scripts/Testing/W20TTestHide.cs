using GameBrains.Actuators.Motion.Steering.VelocityBased;
using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace Testing
{
    public sealed class W20TTestHide : ExtendedMonoBehaviour
    {
        public bool respawn;
        public bool testHide;
        public bool removeHide;
        public VectorXZ spawnLocation = new VectorXZ(10f, 0f);
        public Transform threatTransform;
        public SteerableAgent hider;
        Hide hide;
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

        public override void Update()
        {
            base.Update();
            
            if (hider == null) { return; }

            if (respawn)
            {
                respawn = false;
                hider.Spawn((VectorXYZ)spawnLocation);
            }

            if (removeHide)
            {
                removeHide = false;
                RemoveAndDestroyHide();
            }

            if (testHide)
            {
                testHide = false;

                if (threatTransform != null)
                {
                    RemoveAndDestroyHide();
                    hide = Hide.CreateInstance(hider.SteeringData, threatTransform);
                    SetParameters(hide);
                    hider.SteeringData.AddSteeringBehaviour(hide);
                }
            }
        }
        
        void SetParameters(Hide sb)
        {
            sb.NoStop = noStop;
            sb.NoSlow = noSlow;
            sb.NeverCompletes = neverCompletes;
            sb.LinearStopAtSpeed = linearStopAtSpeed;
            sb.SlowEnoughLinearSpeed = slowEnoughLinearSpeed;
            sb.LinearDrag = linearDrag;
            sb.CloseEnoughDistance = closeEnoughDistance;
            sb.BrakingDistance = brakingDistance;
            sb.LocatorOffset = locatorOffset;
            sb.LocatorSearchRadius = locatorSearchRadius;
            sb.LocatorMaxColliders = locatorMaxColliders;
            sb.CreateHidingSpotLocator();
        }
        
        void RemoveAndDestroyHide()
        {
            if (hide != null)
            {
                hider.SteeringData.RemoveSteeringBehaviour(hide);
                Destroy(hide);
                hide = null;
            }
        }
    }
}
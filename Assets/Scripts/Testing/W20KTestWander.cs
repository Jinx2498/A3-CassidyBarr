using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;
using UnityEngine;

namespace Testing
{
    public sealed class W20KTestWander : ExtendedMonoBehaviour
    {
        public bool respawn;
        public bool removeWanderFromSteeringBehaviours;
        public bool testWander;
        public VectorXZ spawnLocation = new VectorXZ(10f, 0f);
        public SteerableAgent steerableAgent;
        Wander wander;
        public float wanderCircleRadius = 20f;
        public float wanderCircleOffset = 100f;
        public float maximumSlideDegrees = 5f;
        public Vector2? wanderStopLocation;
        public float wanderCloseEnoughDistance = 1f;
        public bool neverCompletes;
        
        public bool moveNoStop = true;
        public bool moveNoSlow = true;
        public bool moveNeverCompletes = true;
        public float linearStopAtSpeed = 0.1f;
        public float slowEnoughLinearSpeed = 0.5f;
        public float linearDrag = 1.015f;
        public float closeEnoughDistance = 1.5f;
        
        public bool lookNoStop;
        public bool lookNoSlow;
        public bool lookNeverCompletes;
        public float angularStopAtSpeed = 0.1f;
        public float slowEnoughAngularVelocity = 5f;
        public float angularDrag = 1.1f;
        public float closeEnoughAngle = 5f;

        public override void Update()
        {
            base.Update();
            
            if (steerableAgent == null) { return; }

            if (respawn)
            {
                respawn = false;
                steerableAgent.Spawn((VectorXYZ)spawnLocation);
            }

            if (removeWanderFromSteeringBehaviours)
            {
                removeWanderFromSteeringBehaviours = false;
                RemoveAndDestroyWander();
            }

            if (testWander)
            {
                testWander = false;
                RemoveAndDestroyWander();
                wander = Wander.CreateInstance(steerableAgent.SteeringData);
                SetParameters(wander);
                steerableAgent.SteeringData.AddSteeringBehaviour(wander);
            }
        }
        
        void SetParameters(Wander sb)
        {
            sb.WanderCircleRadius = wanderCircleRadius;
            sb.WanderCircleOffset = wanderCircleOffset;
            sb.MaximumSlideDegrees = maximumSlideDegrees;
            sb.WanderStopLocation = wanderStopLocation;
            sb.WanderCloseEnoughDistance = wanderCloseEnoughDistance;
            sb.NeverCompletes = neverCompletes;

            LinearStop linearStop = sb.Move;

            if (linearStop != null)
            {
                linearStop.NoStop = moveNoStop;
                linearStop.NeverCompletes = moveNeverCompletes;
                linearStop.LinearStopAtSpeed = linearStopAtSpeed;
                
                var linearSlow = linearStop as LinearSlow;

                if (linearSlow != null)
                {
                    linearSlow.NoSlow = moveNoSlow;
                    linearSlow.SlowEnoughLinearSpeed = slowEnoughLinearSpeed;
                    linearSlow.LinearDrag = linearDrag;

                    var seek = linearStop as Seek;

                    if (seek != null)
                    {
                        seek.CloseEnoughDistance = closeEnoughDistance;
                    }
                }
            }

            AngularStop angularStop = sb.Look;

            if (angularStop != null)
            {
                angularStop.NoStop = lookNoStop;
                angularStop.NeverCompletes = lookNeverCompletes;
                angularStop.AngularStopAtSpeed = angularStopAtSpeed;
                
                var angularSlow = angularStop as AngularSlow;
                
                if (angularSlow != null)
                {
                    angularSlow.NoSlow = lookNoSlow;
                    angularSlow.SlowEnoughAngularVelocity = slowEnoughAngularVelocity;
                    angularSlow.AngularDrag = angularDrag;
                    
                    var align = angularSlow as Align;

                    if (align != null)
                    {
                        align.CloseEnoughAngle = closeEnoughAngle;
                    }
                }
            }
        }
        
        void RemoveAndDestroyWander()
        {
            if (wander != null)
            {
                steerableAgent.SteeringData.RemoveSteeringBehaviour(wander);
                Destroy(wander);
                wander = null;
            }
        }
    }
}
using GameBrains.Actuators.Motion.Movers.UsingControlledTransformWrapper.Movers;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.Controllers
{
    public class MoverController : Controller
    {
        protected Mover Mover;
        
        [SerializeField] VectorXZ velocity = VectorXZ.zero;
        [SerializeField] VectorXZ acceleration = VectorXZ.zero;

        public override void Awake()
        {
            base.Awake();
            
            Mover = GetComponent<Mover>();

            if (Mover == null)
            {
                Debug.LogError("Mover required");
            }
        }

        public override void Update()
        {
            base.Update();
            
            if (Mover == null || isPlayerControlled) return;

            Mover.Velocity = VectorXZ.ClampMagnitude(velocity, maximumSpeed);
            Mover.Acceleration = acceleration;
        }
    }
}
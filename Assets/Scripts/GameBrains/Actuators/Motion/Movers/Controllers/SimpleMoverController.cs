using GameBrains.Actuators.Motion.Movers.UsingControlledTransformWrapper.SimpleMovers;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.Controllers
{
    public class SimpleMoverController : Controller
    {
        protected SimpleMover SimpleMover;
        
        [SerializeField] float speed;
        [SerializeField] VectorXZ direction = VectorXZ.zero;

        public override void Awake()
        {
            base.Awake();
            
            SimpleMover = GetComponent<SimpleMover>();

            if (SimpleMover == null)
            {
                Debug.LogError("SimpleMover required");
            }
        }

        public override void Update()
        {
            base.Update();
            
            if (SimpleMover == null || isPlayerControlled) { return; }

            SimpleMover.Speed = Mathf.Clamp(speed, -maximumSpeed, maximumSpeed);
            SimpleMover.Direction = direction;
        }
    }
}
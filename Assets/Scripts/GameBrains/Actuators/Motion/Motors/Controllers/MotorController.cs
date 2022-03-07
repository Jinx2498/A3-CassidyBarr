using GameBrains.Entities.EntityData;
using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Motors.Controllers
{
    public class MotorController : ExtendedMonoBehaviour
    {
        public bool useFixedUpdate;

        [SerializeField] KinematicData kinematicData;
        public KinematicData KinematicData
        {
            get => kinematicData;
            set => kinematicData = value;
        }

        protected Motor motor;

        public override void Awake()
        {
            base.Awake();
            
            motor = GetComponentInChildren<Motor>();

            if (motor == null)
            {
                Debug.LogError("Motor required");
            }

            KinematicData = transform;
        }

        public override void Update()
        {
            base.Update();
            
            if (!useFixedUpdate)
            {
                DoUpdate(Time.deltaTime);
            }
        }

        protected virtual void DoUpdate(float deltaTime)
        {
            if (motor.enabled)
            {
                motor.CalculatePhysics(KinematicData, deltaTime);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (useFixedUpdate)
            {
                DoUpdate(Time.fixedDeltaTime);
            }
        }
    }
}
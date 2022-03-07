using System.Collections.Generic;
using GameBrains.Actions;
using GameBrains.Actuators.Motion.Movers.UsingVectorXZ.SimpleMovers;
using GameBrains.Extensions.MonoBehaviours;

namespace Testing
{
    public class W08TestChangeSpeedAction : ExtendedMonoBehaviour
    {
        SimpleMover[] simpleMovers;

        public float desiredSpeed = 2f;

        public override void OnEnable()
        {
            base.OnEnable();

            simpleMovers = FindObjectsOfType<SimpleMover>();

            var changeSpeedAction = new ChangeSpeedAction
            {
                desiredSpeed = desiredSpeed
            };

            var actions = new List<Action> { changeSpeedAction };

            foreach (SimpleMover simpleMover in simpleMovers)
            {
                simpleMover.Act(actions);
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();

            if (simpleMovers == null) { return; }
            
            for (int i = 0; i < simpleMovers.Length; i++)
            {
                simpleMovers[i] = null;
            }

            simpleMovers = null;
        }
    }
}
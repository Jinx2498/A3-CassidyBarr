using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;

namespace GameBrains.PerformanceMeasures
{
    public class PerformanceMeasure : ExtendedMonoBehaviour
    {
        #region Agent

        [SerializeField] protected Agent agent;
        protected virtual Agent Agent => agent;

        #endregion Agent

        [SerializeField] float performanceMeasure;
        [SerializeField] int updateInterval; // TODO: Use Regulator?
        float previousTime;

        public override void Awake()
        {
            base.Awake();

            // The Agent component should either be attached to the same
            // gameObject as the Actuator component or above it in the hierarchy.
            // This checks the gameObject first and then works its way upward.
            if (agent == null) { agent = GetComponentInParent<Agent>(); }
        }

        public override void Update()
        {
            base.Update();

            if (Time.time > (previousTime + updateInterval))
            {
                // Record the performance measure of this time interval
                //performanceMeasure = 0;

                // Reset, and prepare for the next time interval
                previousTime = Time.time;
            }
        }
    }
}
using System.Collections;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Messages;
using GameBrains.Visualization;
using UnityEngine;

namespace Testing
{
    public abstract class W06TestCastChecker : ExtendedMonoBehaviour
    {
        public abstract string CheckerName { get; }
        [SerializeField] protected Transform fromTransform;
        [SerializeField] protected MessageQueue messageQueue;
        [SerializeField] protected bool debug;

        CastChecker checker;

        public override void OnEnable()
        {
            base.OnEnable();
            DoCreateChecker();
            StartCoroutine(Visualize());
        }

        protected virtual void DoCreateChecker()
        {
            checker = CreateInstance();

            // Test setting color.
            checker.blockedColor = Color.yellow;
            checker.clearColor = Color.cyan;

            // Test altering default hideFlags.
            checker.visualizer.VisualizerObject.hideFlags &= ~HideFlags.HideInHierarchy;
        }

        protected abstract CastChecker CreateInstance();

        public override void OnDisable()
        {
            base.OnDisable();
            StopCoroutine(Visualize());
            checker = null;
        }

        protected virtual IEnumerator Visualize()
        {
            while (!Input.GetKey(KeyCode.Escape))
            {
                DoCheck();
                yield return new WaitForSeconds(1);
            }
        }

        protected virtual void DoCheck()
        {
            var fromPosition = fromTransform.position;

            Vector3 direction;
            float length;

            if (debug)
            {
                // This is for debugging the checker and visualizer using a known result.
                direction = new Vector3(0, 0, 1);
                length = 10;
            }
            else
            {
                var randomAngle = Random.Range(0f, Mathf.PI * 2f);
                direction // keep it parallel to the ground
                    = new Vector3(Mathf.Sin(randomAngle), 0, Mathf.Cos(randomAngle)).normalized;
                length = Random.Range(20f, 50f);
            }

            var hasLineOfSight
                = checker.HasLineOfSight(fromPosition, direction, length, out RaycastHit hitInfo);
            var hasLineOfSightMessage = hasLineOfSight ? "is clear" : "is blocked";
            var hitName = hitInfo.collider ? $"[{hitInfo.collider.name}]" : string.Empty;
            var toPosition = fromPosition + direction * length;
            var message
                = $"{CheckerName} {hasLineOfSightMessage}: {fromPosition}-->{toPosition} {hitName}";
            var messageColor = hasLineOfSight ? checker.clearColor : checker.blockedColor;

            if (messageQueue) { messageQueue.AddMessage(message, messageColor); }
        }
    }
}
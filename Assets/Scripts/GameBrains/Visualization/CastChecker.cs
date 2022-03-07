using GameBrains.Extensions.ScriptableObjects;
using UnityEngine;

namespace GameBrains.Visualization
{
    public abstract class CastChecker : ExtendedScriptableObject
    {
        public CastVisualizer visualizer;
        public bool showVisualizer = true;
        public bool showOnlyWhenBlocked;
        public Color clearColor = Color.cyan;
        public Color blockedColor = Color.yellow;
        public float heightOffset = 1.5f;
        public LayerMask obstacleLayerMask = 1 << 6;

        public override void OnDisable()
        {
            base.OnDisable();
            visualizer = null;
        }

        public virtual bool HasLineOfSight(
            Vector3 fromPosition,
            Vector3 toPosition,
            out RaycastHit hitInfo)
        {
            var directionVector = toPosition - fromPosition;
            var direction = directionVector.normalized;
            var length = directionVector.magnitude;
            return HasLineOfSight(fromPosition, direction, length, out hitInfo);
        }

        public abstract bool HasLineOfSight(
            Vector3 fromPosition,
            Vector3 direction,
            float length,
            out RaycastHit hitInfo);
    }
}
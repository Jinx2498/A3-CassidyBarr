using UnityEditor;
using UnityEngine;

namespace GameBrains.Visualization
{
    #if UNITY_EDITOR
    [FilePath(
        "ScriptableObjects/Visualizers/CapsuleCastChecker",
        FilePathAttribute.Location.ProjectFolder)]
    [CreateAssetMenu(
        fileName = "CapsuleCastChecker",
        menuName = "GameBrains/Visualization/CapsuleCastChecker")]
    #endif
    public class CapsuleCastChecker : CastChecker
    {
        public float castRadiusMultiplier = 1;
        public float radius = 0.5f;

        public override void OnEnable()
        {
            base.OnEnable();
            visualizer = CreateInstance<CapsuleCastVisualizer>();
            ((CapsuleCastVisualizer)visualizer).castRadiusMultiplier = castRadiusMultiplier;
        }

        public override bool HasLineOfSight(
            Vector3 fromPosition,
            Vector3 direction,
            float length,
            out RaycastHit hitInfo)
        {
            Vector3 capsuleBottomSphereCenterPosition
                = fromPosition + radius * Vector3.up;
            Vector3 capsuleTopSphereCenterPosition
                = fromPosition + heightOffset * Vector3.up;

            float castRadius = radius * castRadiusMultiplier;

            bool blocked = Physics.CapsuleCast(
                capsuleBottomSphereCenterPosition,
                capsuleTopSphereCenterPosition,
                castRadius,
                direction,
                out hitInfo,
                length,
                obstacleLayerMask);

            if (!visualizer) { return !blocked; }

            if (showVisualizer && (!showOnlyWhenBlocked || blocked))
            {
                visualizer.SetColor(blocked ? blockedColor : clearColor);
                var drawLength = blocked ? hitInfo.distance : length;
                visualizer.Draw(fromPosition, direction, drawLength);
            }
            else { visualizer.Hide(true); }

            return !blocked;
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace GameBrains.Visualization
{
    #if UNITY_EDITOR
    [FilePath(
        "ScriptableObjects/Visualizers/SphereCastChecker",
        FilePathAttribute.Location.ProjectFolder)]
    [CreateAssetMenu(
        fileName = "SphereCastChecker",
        menuName = "GameBrains/Visualization/SphereCastChecker")]
    #endif
    public class SphereCastChecker : CastChecker
    {
        public float castRadiusMultiplier = 1;
        public float radius = 0.5f;

        public override void OnEnable()
        {
            base.OnEnable();
            visualizer = CreateInstance<SphereCastVisualizer>();
            ((SphereCastVisualizer)visualizer).castRadiusMultiplier = castRadiusMultiplier;
        }

        public override bool HasLineOfSight(
            Vector3 fromPosition,
            Vector3 direction,
            float length,
            out RaycastHit hitInfo)
        {
            // Check from upper body position.
            var castFrom = fromPosition + Vector3.up * (heightOffset - radius);
            float castRadius = radius * castRadiusMultiplier;

            bool blocked = Physics.SphereCast(
                castFrom,
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
                visualizer.Draw(castFrom, direction, drawLength);
            }
            else { visualizer.Hide(true); }

            return !blocked;
        }
    }
}
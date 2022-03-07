using UnityEditor;
using UnityEngine;

namespace GameBrains.Visualization
{
    #if UNITY_EDITOR
    [FilePath(
        "ScriptableObjects/Visualizers/BoxCastChecker",
        FilePathAttribute.Location.ProjectFolder)]
    [CreateAssetMenu(
        fileName = "BoxCastChecker",
        menuName = "GameBrains/Visualization/BoxCastChecker")]
    #endif
    public class BoxCastChecker : CastChecker
    {
        public float castRadiusMultiplier = 1;
        public float radius = 0.5f;
        public Vector3 boxOrientation = Vector3.zero;

        public override void OnEnable()
        {
            base.OnEnable();
            visualizer = CreateInstance<BoxCastVisualizer>();
            ((BoxCastVisualizer)visualizer).castRadiusMultiplier = castRadiusMultiplier;
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
            Vector3 halfExtents = Vector3.one * castRadius;

            bool blocked = Physics.BoxCast(
                castFrom,
                halfExtents,
                direction,
                out hitInfo,
                Quaternion.Euler(boxOrientation),
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
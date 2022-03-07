using UnityEditor;
using UnityEngine;

namespace GameBrains.Visualization
{
    #if UNITY_EDITOR
    [FilePath(
        "ScriptableObjects/Visualizers/RayCastChecker",
        FilePathAttribute.Location.ProjectFolder)]
    [CreateAssetMenu(
        fileName = "RayCastChecker",
        menuName = "GameBrains/Visualization/RayCastChecker")]
    #endif
    public class RayCastChecker : CastChecker
    {
        public override void OnEnable()
        {
            base.OnEnable();
            visualizer = CreateInstance<RayCastVisualizer>();
        }

        public override bool HasLineOfSight(
            Vector3 fromPosition,
            Vector3 direction,
            float length,
            out RaycastHit hitInfo)
        {
            // Check from upper body position.
            var castFrom = fromPosition + Vector3.up * heightOffset;

            var blocked
                = Physics.Raycast(castFrom, direction, out hitInfo, length, obstacleLayerMask);

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
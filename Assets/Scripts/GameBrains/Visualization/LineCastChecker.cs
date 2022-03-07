using UnityEditor;
using UnityEngine;

namespace GameBrains.Visualization
{
    #if UNITY_EDITOR
    [FilePath(
        "ScriptableObjects/Visualizers/LineCastChecker",
        FilePathAttribute.Location.ProjectFolder)]
    [CreateAssetMenu(
        fileName = "LineCastChecker",
        menuName = "GameBrains/Visualization/LineCastChecker")]
    #endif
    public class LineCastChecker : CastChecker
    {
        public override void OnEnable()
        {
            base.OnEnable();
            visualizer = CreateInstance<LineCastVisualizer>();
        }

        public override bool HasLineOfSight(
            Vector3 fromPosition,
            Vector3 direction,
            float length,
            out RaycastHit hitInfo)
        {
            // Check from upper body position.
            var castFrom = fromPosition + Vector3.up * heightOffset;
            var castTo = castFrom + direction * length;

            var blocked = Physics.Linecast(castFrom, castTo, out hitInfo, obstacleLayerMask);

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
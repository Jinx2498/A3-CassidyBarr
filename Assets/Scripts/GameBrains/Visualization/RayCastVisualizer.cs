using UnityEditor;
using UnityEngine;

namespace GameBrains.Visualization
{
    #if UNITY_EDITOR
    [FilePath(
        "ScriptableObjects/Visualizers/RayCastVisualizer",
        FilePathAttribute.Location.ProjectFolder)]
    [CreateAssetMenu(
        fileName = "RayCastVisualizer",
        menuName = "GameBrains/Visualization/RayCastVisualizer")]
    #endif
    public class RayCastVisualizer : CastVisualizer
    {
        public LineRenderer LineRenderer => lineRenderer;
        LineRenderer lineRenderer;

        public override void OnDestroy()
        {
            base.OnDestroy();
            lineRenderer = null;
            visualizerObject = null;
        }

        public override void Hide(bool shouldHide)
        {
            if (lineRenderer) { lineRenderer.enabled = !shouldHide; }
        }

        public override void SetColor(Color color)
        {
            if (lineRenderer) { lineRenderer.material.color = color; }
        }

        public override void Draw(Vector3 startPosition, Vector3 direction, float length)
        {
            Vector3 endPosition = startPosition + direction * length;

            if (!lineRenderer) { return; }
            Hide(Vector3.Distance(startPosition, endPosition) < hideThreshold);
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, endPosition);
        }

        protected override void Create()
        {
            visualizerPrefab
                = Resources.Load<GameObject>("Prefabs/Visualization/RayCastVisualizerPrefab");
            visualizerObject = Instantiate(visualizerPrefab);
            visualizerObject.name = "RayCastVisualizer";
            visualizerObject.hideFlags = HideFlags.HideInHierarchy;
            lineRenderer = visualizerObject.GetComponent<LineRenderer>();
        }
    }
}
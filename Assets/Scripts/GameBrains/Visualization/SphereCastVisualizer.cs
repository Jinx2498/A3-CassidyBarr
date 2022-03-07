using UnityEditor;
using UnityEngine;

namespace GameBrains.Visualization
{
    #if UNITY_EDITOR
    [FilePath(
        "ScriptableObjects/Visualizers/SphereCastVisualizer",
        FilePathAttribute.Location.ProjectFolder)]
    [CreateAssetMenu(
        fileName = "SphereCastVisualizer",
        menuName = "GameBrains/Visualization/SphereCastVisualizer")]
    #endif
    public class SphereCastVisualizer : CastVisualizer
    {
        public float castRadiusMultiplier = 1f;

        Transform prefabMiddle;
        Transform prefabEnd;

        Transform middle;
        Transform end;

        Renderer middleRenderer;
        Renderer endRenderer;

        public override void OnDestroy()
        {
            base.OnDestroy();

            middleRenderer = null;
            endRenderer = null;

            middle = null;
            end = null;

            visualizerObject = null;
        }

        public override void Hide(bool shouldHide)
        {
            if (middleRenderer) { middleRenderer.enabled = !shouldHide; }
            if (endRenderer) { endRenderer.enabled = !shouldHide; }
        }

        public override void SetColor(Color color)
        {
            if (middleRenderer) { middleRenderer.material.color = color; }
            if (endRenderer) { endRenderer.material.color = color; }
        }

        public override void Draw(Vector3 startPosition, Vector3 direction, float length)
        {
            if (!visualizerObject) { return; }

            bool tooSmallToShow = length < hideThreshold;
            Hide(tooSmallToShow);
            if (tooSmallToShow) return;

            visualizerObject.transform.position = startPosition;
            Vector3 lengthDirection = length * direction;
            Vector3 halfLengthDirection = lengthDirection / 2f;

            middle.localPosition = halfLengthDirection + Vector3.up * prefabMiddle.localPosition.y;
            end.localPosition = lengthDirection + Vector3.up * prefabEnd.localPosition.y;

            Vector3 scale = prefabEnd.localScale;
            scale.x *= castRadiusMultiplier;
            scale.z *= castRadiusMultiplier;
            end.localScale = scale;

            scale = prefabMiddle.localScale;
            scale.x *= castRadiusMultiplier;
            scale.y *= length;
            middle.localScale = scale;

            middle.LookAt(middle.position + direction);
            middle.transform.Rotate(Vector3.right, 90f);

            end.LookAt(end.position + direction);
        }

        protected override void Create()
        {
            visualizerPrefab
                = Resources.Load<GameObject>("Prefabs/Visualization/SphereCastVisualizerPrefab");
            visualizerObject = Instantiate(visualizerPrefab);
            visualizerObject.name = "SphereCastVisualizer";
            visualizerObject.hideFlags = HideFlags.HideInHierarchy;

            prefabMiddle = visualizerPrefab.transform.Find("Middle");
            prefabEnd = visualizerPrefab.transform.Find("End");

            middle = visualizerObject.transform.Find("Middle");
            end = visualizerObject.transform.Find("End");

            middleRenderer = middle.GetComponent<Renderer>();
            endRenderer = end.GetComponent<Renderer>();
        }
    }
}
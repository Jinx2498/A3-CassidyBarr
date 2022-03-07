using GameBrains.Extensions;
using GameBrains.Extensions.ScriptableObjects;
using UnityEngine;

namespace GameBrains.Visualization
{
    public abstract class CastVisualizer : ExtendedScriptableObject
    {
        public GameObject visualizerPrefab;
        public GameObject VisualizerObject => visualizerObject;
        protected GameObject visualizerObject;

        public float hideThreshold = 0.1f;

        public override void OnEnable()
        {
            base.OnEnable();
            if (!visualizerObject) { Create(); }
            Hide(true);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            Hide(true);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            visualizerObject.CheckAndDestroy();
            visualizerObject = null;
        }

        public abstract void Hide(bool shouldHide);

        public abstract void SetColor(Color color);

        public virtual void Draw(Vector3 startPosition, Vector3 endPosition)
        {
            var directionVector = endPosition - startPosition;
            var length = directionVector.magnitude;
            var direction = directionVector.normalized;
            Draw(startPosition, direction, length);
        }

        public abstract void Draw(Vector3 startPosition, Vector3 direction, float length);

        protected abstract void Create();
    }
}
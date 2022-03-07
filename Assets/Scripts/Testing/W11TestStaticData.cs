using GameBrains.Entities.EntityData;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using GameBrains.Visualization;
using UnityEngine;

namespace Testing
{
    [AddComponentMenu("Scripts/Testing/W11 Test Static Data")]
    public class W11TestStaticData : ExtendedMonoBehaviour
    {
        public StaticData staticData;
        public Transform agentTransform;
        public VectorXYZ lookTargetPosition;
        public VectorXYZ moveTargetPosition;
        public bool checkHasLineOfSight;
        public bool checkIsAtPosition;

        public RayCastVisualizer rayCastVisualizer;
        public float closeEnoughDistance = 1.0f;

        public override void Awake()
        {
            base.Awake();
            
            staticData = agentTransform;
            rayCastVisualizer = ScriptableObject.CreateInstance<RayCastVisualizer>();
        }

        public override void Update()
        {
            base.Update();
            
            if (checkHasLineOfSight)
            {
                checkHasLineOfSight = false;

                Debug.Log(
                    staticData.HasLineOfSight(
                        lookTargetPosition,
                        rayCastVisualizer,
                        true));
            }

            if (checkIsAtPosition)
            {
                checkIsAtPosition = false;

                Debug.Log(
                    staticData.IsAtPosition(
                        staticData.Position + VectorXYZ.forward, 
                        closeEnoughDistance));
            }
        }
    }
}
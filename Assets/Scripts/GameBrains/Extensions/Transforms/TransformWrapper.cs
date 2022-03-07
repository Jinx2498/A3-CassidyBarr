using GameBrains.Extensions.MathExtensions;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Extensions.Transforms
{
    [System.Serializable]
    public class TransformWrapper
    {
        [SerializeField] Transform wrappedTransform;

        // This is used by TransformWrapperDrawer
        public Transform WrappedTransform => wrappedTransform;

        #region Constructor

        public TransformWrapper(Transform t)
        {
            wrappedTransform = t;
        }

        #endregion Constructor

        #region Copy Constructor

        public TransformWrapper(TransformWrapper sourceTransformWrapper)
        {
            wrappedTransform = sourceTransformWrapper.WrappedTransform;
        }

        #endregion Copy Constructor

        #region Position and Location

        public VectorXYZ Position
        {
            get => WrappedTransform != null ? (VectorXYZ) WrappedTransform.position : VectorXYZ.zero;
            set
            {
                if (WrappedTransform != null) WrappedTransform.position = value;
            }
        }

        public VectorXZ Location
        {
            get => WrappedTransform != null ? (VectorXZ) WrappedTransform.position : VectorXZ.zero;
            set
            {
                if (WrappedTransform != null)
                {
                    // Preserve Y
                    Vector3 position = (Vector3) value;
                    position.y = WrappedTransform.position.y;
                    WrappedTransform.position = position;
                }
            }
        }

        #endregion Position and Location

        #region Rotation and Orientation

        public Quaternion Rotation
        {
            get => WrappedTransform != null ?  WrappedTransform.rotation : Quaternion.identity;
            set
            {
                if (WrappedTransform != null) WrappedTransform.rotation = value;
            }
        }

        public float Orientation
        {
            get => Math.WrapAngle(Rotation.eulerAngles.y);

            set
            {
                Vector3 eulerAngles = Rotation.eulerAngles;
                eulerAngles.y = Math.WrapAngle(value);
                Rotation = Quaternion.Euler(eulerAngles);
            }
        }

        public VectorXYZ HeadingVectorXYZ
            => Quaternion.Euler(new Vector3(0, Orientation, 0)) * Vector3.forward;

        public VectorXZ HeadingVectorXZ
            => Math.DegreeAngleToVector2(Orientation);

        #endregion Rotation and Orientation

        public static implicit operator TransformWrapper(Transform t) { return new TransformWrapper(t); }
    }
}
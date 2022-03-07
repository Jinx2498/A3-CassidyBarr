using GameBrains.Extensions.MathExtensions;
using GameBrains.Extensions.ScriptableObjects;
using GameBrains.Extensions.Vectors;
using GameBrains.Visualization;
using UnityEngine;

namespace GameBrains.Entities.EntityData
{
    [System.Serializable]
    public class StaticData : ExtendedScriptableObject
    {
	    #region Creators

	    public static StaticData CreateStaticDataInstance(Transform t)
	    {
		    StaticData staticData = CreateInstance<StaticData>();

		    InitializeStaticData(t, staticData);

		    return staticData;
	    }

	    protected static void InitializeStaticData(Transform t, StaticData staticData)
	    {
		    staticData.ownerTransform = t;
		    staticData.owner = t.GetComponent<Entity>();

		    staticData.ResetStaticData();

		    staticData.SetupObstacleLayerMask();
	    }

	    public void ResetStaticData()
	    {
		    var meshFilter = ownerTransform.GetComponentInChildren<MeshFilter>();
		    var localScale = ownerTransform.localScale;
		    Size = meshFilter != null ? Vector3.Scale(localScale, meshFilter.mesh.bounds.size) : localScale;
	    
		    Radius = 0.5f;
		    Height = 2;
		    CenterOffset = VectorXYZ.up * (Height / 2);
	    }
	    
	    #endregion Creators
	    
	    #region Owner and Transform

	    public Entity Owner => owner;
	    protected Entity owner;

	    public Transform OwnerTransform => ownerTransform;
	    [SerializeField] protected Transform ownerTransform;

	    #endregion Owner and Transform

        #region Obstacles

        [SerializeField]
        LayerMask obstacleLayerMask;

        public LayerMask ObstacleLayerMask
        {
	        get => obstacleLayerMask;
	        set => obstacleLayerMask = value;
        }
        public int ObstacleLayer { get; protected set; } = 0;

        public bool IsObstacle
        {
	        get => OwnerTransform.gameObject.layer == ObstacleLayer;
	        set => OwnerTransform.gameObject.layer = value ? ObstacleLayer : 0;
        }
        
        void SetupObstacleLayerMask()
        {
	        ObstacleLayer = LayerMask.NameToLayer("Obstacle");
	        if (ObstacleLayer == -1)
	        {
		        Debug.LogError("Obstacle layer not defined. Using Default layer instead.");
		        ObstacleLayer = 0;
	        }

	        ObstacleLayerMask = 1 << ObstacleLayer;
        }

        #endregion Obstacles

        #region Casting

        public static implicit operator StaticData(Transform t)
        {
	        return CreateStaticDataInstance(t);
        }

        #endregion Casting
        
        #region Position and Location

        public VectorXYZ Position
        {
	        get => OwnerTransform ? (VectorXYZ) OwnerTransform.position : VectorXYZ.zero;
	        set
	        {
		        if (OwnerTransform) OwnerTransform.position = value;
	        }
        }

        public VectorXZ Location
        {
	        get => OwnerTransform ? (VectorXZ) OwnerTransform.position : VectorXZ.zero;
	        set
	        {
		        if (OwnerTransform)
		        {
			        // Preserve Y
			        Vector3 position = (Vector3) value;
			        position.y = OwnerTransform.position.y;
			        OwnerTransform.position = position;
		        }
	        }
        }

        #endregion Position and Location

        #region Rotation and Orientation

        public Quaternion Rotation
        {
	        get => OwnerTransform ? OwnerTransform.rotation : Quaternion.identity;
	        set
	        {
		        if (OwnerTransform) OwnerTransform.rotation = value;
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
	        => Math.DegreeAngleToVectorXZ(Orientation);

        #endregion Rotation and Orientation

        #region Dimensions
        
        public Vector3 Size { get; set; } // based on mesh or local scale

        // Default dimensions based on capsule with height 2 and radius 0.5

        public float Radius { get; set; }
        public float Height { get; set; }
        public VectorXYZ CenterOffset { get; set; }

        public VectorXYZ Top => Center + VectorXYZ.up * Height * Radius;

        public VectorXYZ Bottom => Center - VectorXYZ.up * Height * Radius;

        public VectorXYZ Center => CenterOffset + Position;

        #endregion Dimensions

        #region Utility methods

	    #region Close enough and far enough distances and angles

	    public float CloseEnoughDistance { get; set; } = 1;

	    public float FarEnoughDistance { get; set; } = 10;

	    public float CloseEnoughAngle { get; set; } = 5; // degrees

	    public float FarEnoughAngle { get; set; } = 22.5f; // degrees

	    #endregion Close enough and far enough distances and angles

	    #region Ray and Capsule cast colors

	    public Color ClearColor { get; set; } = Color.green;

	    public Color BlockedColor { get; set; } = Color.red;

	    #endregion Ray and Capsule cast colors

		#region IsAtPosition

		public bool IsAtPosition(VectorXYZ position)
		{
			return IsAtPosition(position, CloseEnoughDistance);
		}

		public bool IsAtPosition(
			VectorXYZ position,
			float closeEnoughDistance)
		{
			return (Position - position).magnitude <= closeEnoughDistance;
		}

		#endregion IsAtPosition

		#region HasLineOfSight

		public bool HasLineOfSight(
			VectorXYZ position,
			RayCastVisualizer rayCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			return HasLineOfSight(
				position,
				out RaycastHit hitInfo,
				rayCastVisualizer,
				showVisualizer,
				showOnlyWhenBlocked,
				clearColor,
				blockedColor);
		}

		public bool HasLineOfSight(
			VectorXYZ position,
			out RaycastHit hitInfo,
			RayCastVisualizer rayCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			if (clearColor == null) clearColor = ClearColor;
			if (blockedColor == null) blockedColor = BlockedColor;

			bool blocked = Physics.Raycast(
				Center,
				(position - Position).normalized,
				out hitInfo,
				(position - Position).magnitude,
				ObstacleLayerMask);

			if (rayCastVisualizer)
			{
				if (showVisualizer && (!showOnlyWhenBlocked || blocked))
				{
					rayCastVisualizer.SetColor(blocked ? blockedColor.Value : clearColor.Value);

					rayCastVisualizer.Draw(
					Center,
					(position - Position).normalized,
					blocked ? hitInfo.distance : (position - Position).magnitude);
				}
				else
				{
					rayCastVisualizer.Hide(true);
				}
			}

			return !blocked;
		}

		#endregion HasLineOfSight

		#endregion
    }
}
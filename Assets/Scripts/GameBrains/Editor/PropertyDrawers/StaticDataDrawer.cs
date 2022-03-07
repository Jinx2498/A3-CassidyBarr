using GameBrains.Actuators.Motion.Steering;
using GameBrains.Actuators.Motion.Steering.VelocityBased;
using GameBrains.Editor.Extensions;
using GameBrains.Editor.PropertyDrawers.Utilities;
using GameBrains.Entities.EntityData;
using GameBrains.Motion.Steering.VelocityBased;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(StaticData), true)]
    public class StaticDataDrawer : PropertyDrawer
    {
        StaticData staticData;
        bool showEntityData = true;
        bool showStaticInfo;
        bool showKinematicInfo;
        bool showSteeringInfo;
        bool showSteeringBehaviours = true;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            staticData =
                PropertyDrawerUtilities.GetActualObjectForSerializedProperty<StaticData>(
                    fieldInfo, property);

            if (staticData == null || !staticData.OwnerTransform)
            {
                return;
            }

            showEntityData = EditorGUILayout.Foldout(showEntityData, "Entity Data");

            if (!showEntityData) return;

            #region Header Foldouts

            EditorGUI.indentLevel += 1;

            showStaticInfo = EditorGUILayout.Foldout(showStaticInfo, nameof(StaticData));
            if (showStaticInfo) DrawStaticData(staticData);

            if (staticData is KinematicData kinematicData)
            {
                showKinematicInfo
                    = EditorGUILayout.Foldout(showKinematicInfo, nameof(KinematicData));
                if (showKinematicInfo) DrawKinematicData(kinematicData);
            }
            
            if (staticData is SteeringData steeringData)
            {
                showSteeringInfo
                    = EditorGUILayout.Foldout(showSteeringInfo, nameof(SteeringData));
                if (showSteeringInfo) DrawSteeringData(steeringData, showSteeringBehaviours);
            }

            EditorGUI.indentLevel -= 1;

            #endregion Header Foldouts

            // Constantly updates inspector but grinds
            // if (EditorApplication.isPlaying)
            // {
            //     EditorUtility.SetDirty(property.serializedObject.targetObject); 
            // }
        }

        #region Draw Static Data

        static void DrawStaticData(StaticData staticData)
        {
            EditorGUI.indentLevel += 1;

            staticData.ObstacleLayerMask
                = EditorGUILayoutExtensions.LayerMaskField(
                    "ObstacleLayerMask",
                    staticData.ObstacleLayerMask);

            staticData.Location =
                EditorGUILayoutExtensions.VectorXZField("Location", staticData.Location);

            staticData.Orientation
                = EditorGUILayout.FloatField("Orientation", staticData.Orientation);

            EditorGUILayoutExtensions.VectorXZField("Heading", staticData.HeadingVectorXZ);

            staticData.Radius = EditorGUILayout.FloatField("Radius", staticData.Radius);

            staticData.Height = EditorGUILayout.FloatField("Height", staticData.Height);

            staticData.CenterOffset
                = EditorGUILayoutExtensions.VectorXYZField(
                    "CenterOffset",
                    staticData.CenterOffset);

            EditorGUILayoutExtensions.VectorXYZField(
                "Top",
                staticData.Top);

            EditorGUILayoutExtensions.VectorXYZField(
                "Bottom",
                staticData.Bottom);

            EditorGUILayoutExtensions.VectorXYZField(
                "Center",
                staticData.Center);

            staticData.CloseEnoughDistance
                = EditorGUILayout.FloatField(
                    "CloseEnoughDistance",
                    staticData.CloseEnoughDistance);

            staticData.FarEnoughDistance
                = EditorGUILayout.FloatField(
                    "FarEnoughDistance",
                    staticData.FarEnoughDistance);

            staticData.CloseEnoughAngle
                = EditorGUILayout.FloatField(
                    "CloseEnoughAngle",
                    staticData.CloseEnoughAngle);

            staticData.FarEnoughAngle
                = EditorGUILayout.FloatField(
                    "FarEnoughAngle",
                    staticData.FarEnoughAngle);

            staticData.ClearColor
                = EditorGUILayout.ColorField("ClearColor", staticData.ClearColor);

            staticData.BlockedColor
                = EditorGUILayout.ColorField("BlockedColor", staticData.BlockedColor);

            EditorGUI.indentLevel -= 1;
        }

        #endregion Draw Static Data

        #region Draw Kinematic Data

        static void DrawKinematicData(KinematicData kinematicData)
        {
            EditorGUI.indentLevel += 1;

            kinematicData.Velocity =
                EditorGUILayoutExtensions.VectorXZField(new GUIContent("Velocity"),
                    kinematicData.Velocity);

            kinematicData.AngularVelocity
                = EditorGUILayout.FloatField(
                    "AngularVelocity",
                    kinematicData.AngularVelocity);

            kinematicData.Acceleration =
                EditorGUILayoutExtensions.VectorXZField(new GUIContent("Acceleration"),
                    kinematicData.Acceleration);

            kinematicData.AngularAcceleration
                = EditorGUILayout.FloatField(
                    "AngularAcceleration",
                    kinematicData.AngularAcceleration);

            EditorGUILayout.FloatField("Speed", kinematicData.Speed);

            kinematicData.MaximumSpeed
                = EditorGUILayout.FloatField("MaximumSpeed", kinematicData.MaximumSpeed);

            kinematicData.MaximumAngularSpeed
                = EditorGUILayout.FloatField(
                    "MaximumAngularSpeed",
                    kinematicData.MaximumAngularSpeed);

            kinematicData.MaximumAcceleration
                = EditorGUILayout.FloatField(
                    "MaximumAcceleration",
                    kinematicData.MaximumAcceleration);

            kinematicData.MaximumAngularAcceleration
                = EditorGUILayout.FloatField(
                    "MaximumAngularAcceleration",
                    kinematicData.MaximumAngularAcceleration);

            EditorGUI.indentLevel -= 1;
        }

        #endregion Draw Kinematic Data
        
        #region Draw Steering Info

        static void DrawSteeringData(SteeringData steeringData, bool showSteeringBehaviours)
        {
            EditorGUI.indentLevel += 1;

            steeringData.CombiningMethod
                = (SteeringData.CombiningMethods)EditorGUILayout.EnumPopup(
                    "Combining Method",
                    steeringData.CombiningMethod);

            showSteeringBehaviours
                = EditorGUILayout.Foldout(showSteeringBehaviours, "Steering Behaviours");

            if (showSteeringBehaviours && steeringData.SteeringBehaviours != null)
            {
                foreach (var keyValuePair in steeringData.SteeringBehaviours)
                {
                    GUIStyle windowStyle = new GUIStyle("window")
                    {
                        alignment = TextAnchor.UpperLeft
                    };

                    GUILayout.BeginHorizontal();
                    GUILayout.Space(EditorGUI.indentLevel * 15f);
                    GUILayout.BeginVertical(keyValuePair.Value.GetType().Name, windowStyle);

                    UnityEngine.GUI.enabled = false;

                    EditorGUILayout.TextField("ID", keyValuePair.Key.ToString());
                    
                    DrawSteeringBehaviour(keyValuePair.Value);

                    UnityEngine.GUI.enabled = true;

                    EditorGUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
            }

            EditorGUI.indentLevel -= 1;
        }
        
           #region Draw Steering Behaviours

        static void DrawSteeringBehaviour<T>(T steeringBehaviour) where T : SteeringBehaviour
        {
            if (steeringBehaviour is LinearStop stop) { DrawLinearStop(stop); }
            if (steeringBehaviour is LinearSlow slow) { DrawLinearSlow(slow); }
            if (steeringBehaviour is Seek seek) { DrawSeek(seek); }
            if (steeringBehaviour is Flee flee) { DrawFlee(flee); }
            if (steeringBehaviour is Arrive arrive) { DrawArrive(arrive); }
            if (steeringBehaviour is Interpose interpose) { DrawInterpose(interpose); }
            if (steeringBehaviour is Hide hide) { DrawHide(hide); }
            if (steeringBehaviour is Pursue pursue) { DrawPursue(pursue); }
            if (steeringBehaviour is Evade evade) { DrawEvade(evade); }
            if (steeringBehaviour is AvoidObstacles avoidObstacles)
            {
                DrawAvoidObstacles(avoidObstacles);
            }
            if (steeringBehaviour is AvoidWalls avoidWalls) { DrawAvoidWalls(avoidWalls); }

            if (steeringBehaviour is AngularStop stopTurning) { DrawAngularStop(stopTurning); }
            if (steeringBehaviour is AngularSlow slowTurning) { DrawAngularSlow(slowTurning); }
            if (steeringBehaviour is Align align) { DrawAlign(align); }
            if (steeringBehaviour is AngularArrive arriveOrientation)
            {
                DrawArriveOrientation(arriveOrientation);
            }
            if (steeringBehaviour is Face face) { DrawFace(face); }
            if (steeringBehaviour is FaceHeading faceHeading) { DrawFaceHeading(faceHeading); }

            if (steeringBehaviour is Wander wander) { DrawWander(wander); }
        }

        static void DrawLinearStop(LinearStop linearStop)
        {
            EditorGUILayout.ToggleLeft("Linear Stop Active", linearStop.LinearStopActive);
        }

        static void DrawLinearSlow(LinearSlow linearSlow)
        {
            EditorGUILayout.ToggleLeft("Linear Slow Active", linearSlow.LinearSlowActive);
            EditorGUILayout.FloatField("Slow Enough Linear Speed", linearSlow.SlowEnoughLinearSpeed);
            EditorGUILayout.FloatField("Linear Drag", linearSlow.LinearDrag);
        }

        static void DrawSeek(Seek seek)
        {
            EditorGUILayout.ToggleLeft("Seek Active", seek.SeekActive);
            EditorGUILayoutExtensions.VectorXZField("Target Location", seek.TargetLocation);
            EditorGUILayout.FloatField("Close Enough Distance", seek.CloseEnoughDistance);
        }

        static void DrawFlee(Flee flee)
        {
            EditorGUILayout.ToggleLeft("Flee Active", flee.FleeActive);
            EditorGUILayout.FloatField("Escape Distance", flee.EscapeDistance);
        }

        static void DrawArrive(Arrive arrive)
        {
            EditorGUILayout.ToggleLeft("Arrive Active", arrive.ArriveActive);
            EditorGUILayout.FloatField("Braking Distance", arrive.BrakingDistance);
        }

        static void DrawInterpose(Interpose interpose)
        {
            EditorGUILayoutExtensions.VectorXZField("First Location", interpose.FirstLocation);
            EditorGUILayoutExtensions.VectorXZField("Second Location", interpose.SecondLocation);
        }

        static void DrawHide(Hide hide)
        {
            EditorGUILayoutExtensions.VectorXZField("Threat Location", hide.OtherTargetLocation);
        }

        static void DrawPursue(Pursue pursue)
        {
            EditorGUILayout.ToggleLeft("Pursue Active", pursue.PursueActive);
            EditorGUILayoutExtensions.VectorXZField("Target Location", pursue.OtherTargetLocation);
        }

        static void DrawEvade(Evade evade)
        {
            EditorGUILayout.ToggleLeft("Evade Active", evade.EvadeActive);
            EditorGUILayout.FloatField("Escape Distance", evade.EscapeDistance);
        }

        static void DrawAvoidObstacles(AvoidObstacles avoidObstacles)
        {
            EditorGUILayout.FloatField("Force Multiplier", avoidObstacles.ForceMultiplier);
            EditorGUILayout.FloatField("Lookahead Multiplier", avoidObstacles.LookAheadMultiplier);
        }

        static void DrawAvoidWalls(AvoidWalls avoidWalls)
        {
            EditorGUILayout.IntField("Feeler Count", AvoidWalls.FeelerCount);
            EditorGUILayout.FloatField("Force Multiplier", avoidWalls.ForceMultiplier);
            EditorGUILayout.FloatField("Lookahead Multiplier", avoidWalls.LookAheadMultiplier);
        }

        static void DrawAngularStop(AngularStop angularStop)
        {
            EditorGUILayout.ToggleLeft("Angular Stop Active", angularStop.AngularStopActive);
        }

        static void DrawAngularSlow(AngularSlow angularSlow)
        {
            EditorGUILayout.ToggleLeft("Angular Slow Active", angularSlow.AngularSlowActive);
            EditorGUILayout.FloatField("Slow Enough Angular Velocity", angularSlow.SlowEnoughAngularVelocity);
            EditorGUILayout.FloatField("Angular Drag", angularSlow.AngularDrag);
        }

        static void DrawAlign(Align align)
        {
            EditorGUILayout.ToggleLeft("Align Active", align.AlignActive);
            EditorGUILayout.FloatField("Target Orientation", align.TargetOrientation);
            EditorGUILayout.FloatField("Close Enough Angle", align.CloseEnoughAngle);
        }

         static void DrawArriveOrientation(AngularArrive angularArrive)
         {
             EditorGUILayout.ToggleLeft(
                 "Arrive Orientation Active",
                 angularArrive.AngularArriveActive);
             EditorGUILayout.FloatField("Braking Angle", angularArrive.BrakingAngle);
         }

         static void DrawFace(Face face)
         {
             EditorGUILayout.ToggleLeft("Face Active", face.FaceActive);
             EditorGUILayoutExtensions.VectorXZField("Target Location", face.TargetLocation);
         }

         static void DrawFaceHeading(FaceHeading faceHeading)
         {
             EditorGUILayout.ToggleLeft("Face Heading Active", faceHeading.FaceHeadingActive);
         }

         static void DrawWander(Wander wander)
         {
             DrawSteeringBehaviour(wander.Move);
             DrawSteeringBehaviour(wander.Look);
             EditorGUILayout.FloatField("Wander Circle Radius", wander.WanderCircleRadius);
             EditorGUILayout.FloatField("Wander Circle Offset", wander.WanderCircleOffset);
             EditorGUILayout.FloatField("MaximumSlideDegrees", wander.MaximumSlideDegrees);
             EditorGUILayout.FloatField(
                 "Wander Close Enough Distance",
                 wander.WanderCloseEnoughDistance);
             if (wander.WanderStopLocation.HasValue)
             {
                 EditorGUILayoutExtensions.VectorXZField(
                 "Wander Stop Location",
                 wander.WanderStopLocation.Value);
             }
         }

        #endregion Draw Steering Behaviours

        #endregion

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0;
        }
    }
}
using GameBrains.Cameras;
using GameBrains.Entities;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;
using UnityEngine;

namespace GameBrains.GUI
{
    [AddComponentMenu("Scripts/GameBrains/GUI/Steerable Agent Console")]
    public class SteerableAgentConsole : MessageViewer
    {
        [SerializeField] GUISkin customSkin;
        [SerializeField] SteerableAgent steerableAgent;
        [SerializeField] VectorXZ spawnLocation = new VectorXZ(0f, 0f);
        [SerializeField] int commandColumnsPerRow = 3;
        [SerializeField] int targetRowsPerColumn = 3;
        [SerializeField] Seek seek;
        [SerializeField] GameObject[] targets;
        bool testMode;
        bool linesForButtonsAdded;
        GUIStyle centeredLabelStyle;

        public override void Awake()
        {
            base.Awake();

            if (!steerableAgent) { steerableAgent = GetComponent<SteerableAgent>(); }
        }

        public override void Start()
        {
            base.Start();

            if (steerableAgent)
            {
                var entityColor = ColorUtility.ToHtmlStringRGBA(steerableAgent.Color);
                windowTitle = $"<color=#{entityColor}>{steerableAgent.ShortName}'s Console</color>";
                seek = Seek.CreateInstance(steerableAgent.SteeringData);
                seek.TargetLocation = steerableAgent.SteeringData.Location;
                steerableAgent.SteeringData.AddSteeringBehaviour(seek);
            }
            else
            {
                windowTitle = $"{gameObject.name}'s Console";
            }
        }

        protected override void OnGUI()
        {
            UnityEngine.GUI.skin = customSkin;
            
            centeredLabelStyle ??= new GUIStyle(UnityEngine.GUI.skin.GetStyle("Label"))
            {
                alignment = TextAnchor.MiddleCenter
            };
            
            base.OnGUI();
        }

        protected override void SetMinimumWindowSize()
        {
            base.SetMinimumWindowSize();

            // Add lines for buttons.
            if (!linesForButtonsAdded && SelectableCamera.SelectableCameras != null)
            {
                linesForButtonsAdded = true;
                
                minimumWindowSize.y
                    += SelectableCamera.SelectableCameras.Count *
                       (UnityEngine.GUI.skin.button.lineHeight);
            }
        }

        // This creates the GUI inside the window.
        // It requires the id of the window it's currently making GUI for.
        protected override void WindowFunction(int windowID)
        {
            // Purposely not calling base.WindowFunction here.

            // Draw any Controls inside the window here.

            Color savedColor = UnityEngine.GUI.color;
            UnityEngine.GUI.color = defaultContentColor;

            GUILayout.BeginHorizontal();

            if (GUILayout.Button(steerableAgent.IsPlayerControlled ? " Is a Player" : " Is an AI"))
            {
                steerableAgent.IsPlayerControlled = !steerableAgent.IsPlayerControlled;
            }

            if (GUILayout.Button(testMode ? "Test Mode ON" : "Test Mode OFF"))
            {
                testMode = !testMode;
                if (steerableAgent != null)
                {
                    steerableAgent.SteeringData.SteeringBehaviours.Clear();
                    if (!testMode)
                    {
                        steerableAgent.SteeringData.AddSteeringBehaviour(seek);
                    }
                }
            }

            if (GUILayout.Button("Respawn"))
            {
                if (steerableAgent != null)
                {
                    steerableAgent.Spawn((VectorXYZ)spawnLocation);
                    if (!testMode)
                    {
                        steerableAgent.SteeringData.AddSteeringBehaviour(seek);
                        seek.TargetTransform = null;
                        seek.TargetLocation = steerableAgent.SteeringData.Location;
                        messageQueue.AddMessage($"Seeking current location {seek.TargetLocation}.");
                    }
                }
            }

            GUILayout.EndHorizontal();

            if (SelectableCamera.SelectableCameras != null &&
                SelectableCamera.SelectableCameras.Count > 0)
            {
                GUILayout.BeginVertical();

                int commandIndex = 0;

                while (commandIndex < SelectableCamera.SelectableCameras.Count)
                {
                    GUILayout.BeginHorizontal();

                    int commandColumn = 0;

                    while (commandColumn < commandColumnsPerRow &&
                           commandIndex < SelectableCamera.SelectableCameras.Count)
                    {
                        var selectableCamera = SelectableCamera.SelectableCameras[commandIndex];
                        if (GUILayout.Button(selectableCamera.DisplayName))
                        {
                            SelectableCamera.SetCurrent(selectableCamera);

                            if (SelectableCamera.CurrentCamera is TargetedCamera targetedCamera)
                            {
                                targetedCamera.Target = transform;
                            }
                        }

                        commandColumn++;
                        commandIndex++;
                    }

                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();
            }

            GUILayout.Label("Target Selector", centeredLabelStyle);
            
            #region Set Target

            int targetIndex = 0;

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();

            if (GUILayout.Button("None"))
            {
                if (testMode)
                {
                    messageQueue.AddMessage("Button disabled. Turn off test mode.");
                }
                else
                {
                    seek.TargetTransform = null;
                    seek.TargetLocation = steerableAgent.SteeringData.Location;
                    messageQueue.AddMessage($"Seeking current location {seek.TargetLocation}.");
                }
            }

            if (GUILayout.Button("Origin"))
            {
                if (testMode)
                {
                    messageQueue.AddMessage("Button disabled. Turn off test mode.");
                }
                else
                {
                    seek.TargetTransform = null;
                    seek.TargetLocation = VectorXZ.zero;
                    messageQueue.AddMessage($"Seeking location {seek.TargetLocation}.");
                }
            }

            if (GUILayout.Button("Random"))
            {
                if (testMode)
                {
                    messageQueue.AddMessage("Button disabled. Turn off test mode.");
                }
                else
                {
                    seek.TargetTransform = null;
                    seek.TargetLocation = (VectorXZ)Random.insideUnitSphere * 50f;
                    messageQueue.AddMessage($"Seeking location {seek.TargetLocation}.");
                }
            }

            var row = 3;

            // fill the rest of this column
            while (row < targetRowsPerColumn && targetIndex < targets.Length)
            {
                if (GUILayout.Button(targets[targetIndex].name))
                {
                    if (testMode)
                    {
                        messageQueue.AddMessage("Button disabled. Turn off test mode.");
                    }
                    else
                    {
                        seek.TargetTransform = targets[targetIndex].transform;
                        messageQueue.AddMessage($"Seeking transform {seek.TargetTransform.name}.");
                    }
                }

                row++;
                targetIndex++;
            }

            GUILayout.EndVertical();

            // rest of the columns
            while (targetIndex < targets.Length)
            {
                GUILayout.BeginVertical();

                row = 0;

                while (row < targetRowsPerColumn && targetIndex < targets.Length)
                {
                    if (GUILayout.Button(targets[targetIndex].name))
                    {
                        if (testMode)
                        {
                            messageQueue.AddMessage("Button disabled. Turn off test mode.");
                        }
                        else
                        {
                            seek.TargetTransform = targets[targetIndex].transform;
                            messageQueue.AddMessage(
                                $"Seeking transform {seek.TargetTransform.name}.");
                        }
                    }

                    row++;
                    targetIndex++;
                }

                GUILayout.EndVertical();
            }

            GUILayout.EndHorizontal();
            
            #endregion Set Target

            GUILayout.Label(messageQueue.GetMessages());

            UnityEngine.GUI.color = savedColor;

            // Make the windows be draggable.
            UnityEngine.GUI.DragWindow();
        }
    }
}
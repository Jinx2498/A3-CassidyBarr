using System.Collections.Generic;
using GameBrains.Actions;
using GameBrains.Actuators;
using GameBrains.Minds;
using GameBrains.Percepts;
using GameBrains.PerformanceMeasures;
using GameBrains.Sensors;
using UnityEngine;

namespace GameBrains.Entities
{
    // Agent with Sensors, Actuators, and Mind components
    public class SamAgent : Agent
    {
        #region Sensors

        [SerializeField] protected List<Sensor> sensors;

        // TODO: Make protected and add public accessors
        public virtual List<Sensor> Sensors
        {
            get => sensors;
            protected set => sensors = value;
        }

        #endregion Sensors

        #region Minds

        [SerializeField] protected Mind mind;

        // TODO: Make protected and add public accessors
        public virtual Mind Mind
        {
            get => mind;
            protected set => mind = value;
        }

        #region Think Types

        public enum ThinkTypes
        {
            Replace,
            Add,
            Merge
        }

        [SerializeField] ThinkTypes thinkTypes;
        public ThinkTypes ThinkType => thinkTypes;

        #endregion Think Types

        #endregion Minds

        #region Actuators

        [SerializeField] protected List<Actuator> actuators;

        // TODO: Make protected and add public accessors
        public virtual List<Actuator> Actuators
        {
            get => actuators;
            protected set => actuators = value;
        }

        #endregion Actuators

        #region Performance Measures

        [SerializeField] protected PerformanceMeasure performanceMeasure;

        // TODO: Make protected and add public accessors
        public virtual PerformanceMeasure PerformanceMeasure
        {
            get => performanceMeasure;
            set => performanceMeasure = value;
        }

        #endregion Performance Measures

        #region Percepts

        protected List<Percept> currentPercepts;

        #endregion Percepts

        #region Actions

        // TODO: Modify to continue or interrupt action currently in progress
        protected List<Action> currentActions = new List<Action>();

        #endregion

        #region Awake

        public override void Awake()
        {
            base.Awake();

            sensors = new List<Sensor>(GetComponentsInChildren<Sensor>());
            actuators = new List<Actuator>(GetComponentsInChildren<Actuator>());
            mind = GetComponentInChildren<Mind>();
            performanceMeasure = GetComponent<PerformanceMeasure>();
        }

        #endregion Awake

        #region Sense

        protected override void Sense(float deltaTime)
        {
            currentPercepts = new List<Percept>();

            foreach (Sensor sensor in Sensors)
            {
                var currentPercept = sensor.Sense();
                if (currentPercept != null)
                {
                    currentPercepts.Add(currentPercept);
                }
            }
        }

        #endregion Sense

        #region Act

        protected override void Act(float deltaTime)
        {
            foreach (Actuator actuator in Actuators)
            {
                actuator.Act(currentActions);

                CheckStatus();
            }
        }

        void CheckStatus()
        {
            for (int i = 0; i < currentActions.Count; i++)
            {
                if (currentActions[i].completionStatus == Action.CompletionsStates.Complete)
                {
                    if (VerbosityDebug)
                    {
                        Debug.Log("Action Completed: " + currentActions[i]);
                    }

                    currentActions.RemoveAt(i);
                    i--;
                }
                else
                {
                    currentActions[i].timeToLive -= Time.deltaTime;
                    if (currentActions[i].timeToLive <= 0)
                    {
                        if (VerbosityDebug)
                        {
                            Debug.Log("Action Timed Out: " + currentActions[i]);
                        }

                        currentActions[i].completionStatus = Action.CompletionsStates.Failed;
                        // Let failed remove this action
                    }

                    if (currentActions[i].completionStatus == Action.CompletionsStates.Failed)
                    {
                        if (VerbosityDebug)
                        {
                            Debug.Log("Action Failed: " + currentActions[i]);
                        }

                        currentActions.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        #endregion Act

        #region Mind

        protected override void Think(float deltaTime)
        {
            if (Mind != null && Mind.MindUpdateRegulator.IsReady)
            {
                // TODO: Should we deal with inprogress actions or just drop them
                ChooseThinkType(currentPercepts);

                if (VerbosityDebug)
                {
                    Debug.Log("Action count = " + currentActions.Count);
                }
            }
        }

        void ChooseThinkType(List<Percept> percepts)
        {
            switch (ThinkType)
            {
                case ThinkTypes.Replace:
                    currentActions = Mind.Think(percepts);
                    break;
                case ThinkTypes.Add:
                    currentActions.AddRange(Mind.Think(percepts));
                    break;
                case ThinkTypes.Merge:
                    MergeActions(Mind.Think(percepts));
                    break;
                default:
                    Debug.LogWarning("Unsupported ThinkType");
                    break;
            }
        }

        void MergeActions(List<Action> newActions)
        {
            foreach (Action action in newActions)
            {
                bool added = false;
                for (int i = 0; i < currentActions.Count; i++)
                {
                    // TODO: Can we have different actions of the same type??
                    if (currentActions[i].GetType() == action.GetType())
                    {
                        if (VerbosityDebug)
                        {
                            Debug.Log("Action Interrupted: " + currentActions[i]);
                        }

                        currentActions[i] = action; // replace
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    currentActions.Add(action);
                }
            }
        }

        #endregion Mind
    }
}
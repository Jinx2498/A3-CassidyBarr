using UnityEngine;

namespace GameBrains.Entities
{
    public class Agent : Entity
    {
        #region Player control

        [SerializeField] bool isPlayerControlled;

        public bool IsPlayerControlled
        {
            get => isPlayerControlled;
            set => isPlayerControlled = value;
        }

        #endregion

        #region Awake, Start, Update

        public override void Update()
        {
            base.Update();

            Sense(Time.deltaTime);

            Think(Time.deltaTime);

            Act(Time.deltaTime);
        }

        #endregion Awake, Start, Update
        
        #region Sense, think, and act

        protected virtual void Sense(float deltaTime) { }

        protected virtual void Think(float deltaTime) { }

        protected virtual void Act(float deltaTime) { }

        #endregion Sense, think, and act
    }
}
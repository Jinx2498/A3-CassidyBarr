using System.Collections;
using GameBrains.Entities;
using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;
using UnityEngine.UI;

namespace GameBrains.Weebles
{
    public class HealthBar : ExtendedMonoBehaviour
    {
        [SerializeField] Image foregroundImage;
        [SerializeField] float updateSpeedSeconds = 0.5f;

        public override void Awake()
        {
            base.Awake();
            //GetComponentInParent<Health>().OnHealthPercentChanged += HandleHealthChanged;
            GetComponentInParent<Entity>().OnHealthPercentChanged += HandleHealthChanged;
        }

        void HandleHealthChanged(float percent)
        {
            StartCoroutine(ChangeToPercent(percent));
        }

        IEnumerator ChangeToPercent(float percent)
        {
            var preChangePercent = foregroundImage.fillAmount;
            var elapsed = 0f;

            while (elapsed < updateSpeedSeconds)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            foregroundImage.fillAmount = percent;
        }
    }
}
using GameBrains.Extensions.MonoBehaviours;
using UnityEngine.UI;

namespace GameBrains.SceneManagement
{
    public class LoadProgress : ExtendedMonoBehaviour
    {
        public Slider slider;
        public Image fill;

        public override void Awake()
        {
            base.Awake();
            fill.fillAmount = slider.value = 0;
        }

        public override void Update()
        {
            base.Update();

            slider.value = Loader.GetLoadingProgress();
        }

        public void FillSlider() { fill.fillAmount = slider.value; }
    }
}
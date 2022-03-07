using GameBrains.Extensions.MonoBehaviours;

namespace GameBrains.SceneManagement
{
    public class LoaderCallback : ExtendedMonoBehaviour
    {
        bool firstUpdateDone;

        public override void Update()
        {
            base.Update();

            if (firstUpdateDone) { return; }

            firstUpdateDone = true;
            Loader.LoaderCallback();
        }
    }
}
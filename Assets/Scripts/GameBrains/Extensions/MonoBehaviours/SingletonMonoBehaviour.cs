using log4net;

namespace GameBrains.Extensions.MonoBehaviours
{
    public abstract class SingletonMonoBehaviour<TExtended>
        : ExtendedMonoBehaviour where TExtended : ExtendedMonoBehaviour
    {
        static SingletonMonoBehaviour<TExtended> instance;

        public static TExtended Instance
        {
            get
            {
                if (!instance)
                {
                    var log = LogManager.GetLogger(typeof(TExtended));
                    log.Debug("Instance is null. Was it called before Awake?");
                }

                return instance as TExtended;
            }
        }

        public override void Awake()
        {
            base.Awake();

            if (instance)
            {
                if (VerbosityDebugOrLog)
                {
                    Log.Debug($"{instance}: Duplicate singleton MonoBehaviour found and removed.");
                }
                instance.CheckAndDestroy();
                return;
            }

            instance = this;
        }
    }
}
using System.IO;
using log4net.Config;
using UnityEngine;

namespace GameBrains.DebuggingTools.Log4Net
{
    public static class LoggingConfiguration
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Configure()
        {
            XmlConfigurator.Configure(new FileInfo($"{Application.dataPath}/log4net.xml"));
        }
    }
}
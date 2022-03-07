using log4net.Appender;
using log4net.Core;
using UnityEngine;
using System.IO;

namespace GameBrains.DebuggingTools.Log4Net
{
    public class UnityDataPathAppender : AppenderSkeleton
    {
        readonly string fileName = Application.dataPath + "/LogFile.txt";

        protected override void Append(LoggingEvent loggingEvent)
        {
            var message = RenderLoggingEvent(loggingEvent);
            TextWriter tw = new StreamWriter(fileName, true);
            tw.WriteLine(message);
            tw.Close();
        }
    }
}
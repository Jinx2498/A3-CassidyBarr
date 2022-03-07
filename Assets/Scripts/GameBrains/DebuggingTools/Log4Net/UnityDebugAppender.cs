using log4net.Appender;
using log4net.Core;
using UnityEngine;

namespace GameBrains.DebuggingTools.Log4Net
{
    public class UnityDebugAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            var message = RenderLoggingEvent(loggingEvent);
            Debug.Log(message);
        }
    }
}
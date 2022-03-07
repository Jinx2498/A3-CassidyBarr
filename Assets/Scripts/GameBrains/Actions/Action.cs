namespace GameBrains.Actions
{
    public class Action
    {
        public enum CompletionsStates { Complete, InProgress, Failed }
        
        public CompletionsStates completionStatus;
        public float timeToLive;
    }
}
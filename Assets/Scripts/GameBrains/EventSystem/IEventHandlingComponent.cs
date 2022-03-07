namespace GameBrains.EventSystem
{
    public interface IEventHandlingComponent
    {
        bool HandleEvent<T>(Event<T> eventArguments);
    }
}
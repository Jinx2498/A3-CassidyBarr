namespace GameBrains.EventSystem
{
    // Called whenever the associated Event fires.
    public delegate void EventDelegate<T>(Event<T> eventT);

    public delegate bool MessageDelegate<T>(Event<T> eventT);
}
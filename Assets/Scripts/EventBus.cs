public sealed class EventBus
{
    static EventBus()
    {
    }

    private EventBus()
    {
    }

    public static EventBus Instance { get; } = new();

    public delegate void DayChanged(int day);
    public delegate void NightStarted();

    /// <summary>
    ///     Fired at the end of the night, and the beginning of the new day.
    /// </summary>
    public DayChanged OnDayChanged;
    
    /// <summary>
    ///     Fired at the beginning of the night, after the previous day.
    /// </summary>
    public NightStarted OnNightStarted;
}
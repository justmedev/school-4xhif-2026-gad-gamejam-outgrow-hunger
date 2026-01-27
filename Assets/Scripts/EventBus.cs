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

    public DayChanged OnDayChanged;
}
using Prism.Events;

namespace UnityCake.Events
{
    public class SetDisplayHeaderEvent : PubSubEvent<string>
    {
    }

    public class ExpandSecondMenuEvent : PubSubEvent
    {
    }

    public class ShrinkSecondMenuEvent : PubSubEvent
    {
    }
}

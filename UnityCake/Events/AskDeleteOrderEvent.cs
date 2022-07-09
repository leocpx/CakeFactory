using Core.Interfaces;
using Prism.Events;

namespace UnityCake.Events
{
    //public class AskDeleteOrderEvent : PubSubEvent<AskOrderParams>
    public class AskDeleteOrderEvent : PubSubEvent<IOrder>
    { }
}

using Core.Interfaces;
using Prism.Events;

namespace UnityCake.Events
{
    public class PackagingOrderClickedEvent : PubSubEvent<IOrder>
    { }
}

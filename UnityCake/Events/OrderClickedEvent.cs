using DBManager.Tables;
using Prism.Events;

namespace UnityCake.Events
{
    public class OrderClickedEvent : PubSubEvent<ProductionOrders>
    { }
}

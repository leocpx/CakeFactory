using DBManager.Tables;
using Prism.Events;

namespace UnityCake.Events
{
    public class CompleteOrderEvent : PubSubEvent<ProductionOrders>
    { }
}

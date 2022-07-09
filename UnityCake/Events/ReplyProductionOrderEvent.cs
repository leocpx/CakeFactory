using Core.Interfaces;
using DBManager.Tables;
using Prism.Events;

namespace UnityCake.Events
{
    //public class ReplyProductionOrderEvent : PubSubEvent<ProductionOrders>
    public class ReplyProductionOrderEvent : PubSubEvent<IOrder>
    { }
}

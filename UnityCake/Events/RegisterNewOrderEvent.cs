using Core.Interfaces;
using DBManager.Tables;
using Prism.Events;

namespace UnityCake.Events
{
    //public class RegisterNewProductionOrderEvent : PubSubEvent<ProductionOrders>
    public class RegisterNewOrderEvent : PubSubEvent<IOrder>
    { }
}

using DBManager.Tables;
using Prism.Events;
using System.Collections.Generic;

namespace UnityCake.Events
{
    public class OrderClickedEvent : PubSubEvent<ProductionOrders>
    { }
}

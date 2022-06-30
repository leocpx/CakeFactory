using DBManager.Tables;
using Prism.Events;
using System.Collections.Generic;

namespace UnityCake.Events
{
    public class RegisterFinishedGoodsDetailsEvent : PubSubEvent<List<FinishedGoodsDetails>>
    { }
}

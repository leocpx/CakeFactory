using DBManager.Tables;
using Prism.Events;
using System.Collections.Generic;

namespace UnityCake.Events
{
    public class ReplyUsersListEvent : PubSubEvent<List<Users>>
    { }
}

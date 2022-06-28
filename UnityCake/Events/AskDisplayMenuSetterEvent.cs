using DBManager.Tables;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace UnityCake.Events
{
    public class AskDisplayMenuSetterEvent : PubSubEvent
    { }

    public class AskUsersList : PubSubEvent
    { }

    public class ReplyUsersList : PubSubEvent<List<Users>>
    { }

    public class UserItemCLickedEvent : PubSubEvent<string>
    { }
}

using Prism.Events;
using System;
using System.Windows.Controls;

namespace UnityCake.Events
{
    public class ReplySecondMenuSetterEvent : PubSubEvent<Action<UserControl>>
    {
    }
}

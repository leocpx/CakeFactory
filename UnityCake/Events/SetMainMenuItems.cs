using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UnityCake.Events
{
    public class SetMainMenuItems : PubSubEvent<List<UserControl>>
    {
    }
}

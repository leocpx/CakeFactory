using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityCake
{
    public class Unity
    {
        public static EventAggregator EventAggregator { get; set; } = new EventAggregator();
    }
}

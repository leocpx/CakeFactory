using DBManager.Tables;
using Prism.Events;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityCake.Events
{
    public class RegisterNewUserEvent : PubSubEvent<Users>
    { }
}

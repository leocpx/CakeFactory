using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IScheduleItem
    {
        IOrder Order { get; set; }
    }

    public interface IOrder
    {
        long id { get; set; }
        long _finishedGoodId { get; set; }
        long _startTime { get; set; }
        long _workerId { get; set; }
        bool _completed { get; set; }
    }
}

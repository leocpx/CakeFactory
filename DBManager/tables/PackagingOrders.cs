using eLongMuSQL;
using System;

namespace DBManager.Tables
{
    [SQLTable("PackagingOrders")]
    public class PackagingOrders
    {
        [SQLColumn(SQLColumnType.BigInt, "id", true, true)]
        public Int64 id { get; set; }


        [SQLColumn(SQLColumnType.BigInt, "_finishedGoodId", true, false)]
        public Int64 _finishedGoodId { get; set; }


        [SQLColumn(SQLColumnType.BigInt, "_workerId", true, false)]
        public Int64 _workerId { get; set; }


        [SQLColumn(SQLColumnType.BigInt, "_startTime", true, false)]
        public Int64 _startTime { get; set; }


        [SQLColumn(SQLColumnType.Bit, "_completed", true, false)]
        public bool _completed { get; set; } = false;

        public PackagingOrders()
        {
            id = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
        }
    }
}

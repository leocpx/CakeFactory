using eLongMuSQL;
using System;

namespace DBManager.Tables
{


    [SQLTable("FinishedGoodsDetails")]
    public class FinishedGoodsDetails
    {
        [SQLColumn(SQLColumnType.BigInt, "id", true, true)]
        public Int64 id { get; set; }


        [SQLColumn(SQLColumnType.BigInt, "_finishedGoodId", true, false)]
        public Int64 _finishedGoodId { get; set; }


        [SQLColumn(SQLColumnType.BigInt, "_rawGoodId", true, false)]
        public Int64 _rawGoodId { get; set; }


        [SQLColumn(SQLColumnType.BigInt, "_quantity", true, false)]
        public Int64 _quantity { get; set; }


        [SQLColumn(SQLColumnType.Varchar, "_unit", true, false)]
        public string _unit { get; set; }


        public FinishedGoodsDetails()
        {
            id = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
        }
    }
}

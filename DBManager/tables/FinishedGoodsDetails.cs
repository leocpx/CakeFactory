using eLongMuSQL;
using System;
using System.Collections.Generic;

namespace DBManager.Tables
{


    [SQLTable(nameof(FinishedGoodsDetails))]
    public class FinishedGoodsDetails
    {
        [SQLColumn(SQLColumnType.BigInt, nameof(id), true, true)]
        public Int64 id { get; set; }


        [SQLColumn(SQLColumnType.BigInt, nameof(_finishedGoodId), true, false)]
        public Int64 _finishedGoodId { get; set; }


        [SQLColumn(SQLColumnType.BigInt, nameof(_rawGoodId), true, false)]
        public Int64 _rawGoodId { get; set; }


        [SQLColumn(SQLColumnType.BigInt, nameof(_quantity), true, false)]
        public Int64 _quantity { get; set; }


        [SQLColumn(SQLColumnType.Varchar, nameof(_unit), true, false)]
        public string _unit { get; set; }


        [SQLTableLink(typeof(RawGoodsInfo), nameof(_rawGoodId),nameof(RawGoodsInfo.id))]
        public List<RawGoodsInfo> _RawGoodsInfo { get; set; }

        public FinishedGoodsDetails()
        {
            id = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
        }
    }
}

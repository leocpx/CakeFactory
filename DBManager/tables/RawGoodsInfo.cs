using eLongMuSQL;
using System;

namespace DBManager.Tables
{
    [SQLTable("RawGoodsInfo")]
    public class RawGoodsInfo
    {
        [SQLColumn(SQLColumnType.BigInt, "id", true, true)]
        public Int64 id { get; set; }


        [SQLColumn(SQLColumnType.Varchar, "_rawgoodname", true, false)]
        public string _rawgoodname { get; set; }


        [SQLColumn(SQLColumnType.Varchar, "_barcode", true, false)]
        public string _barcode { get; set; }


        [SQLColumn(SQLColumnType.Varchar, "_priceperpiece", true, false)]
        public string _priceperpiece { get; set; }


        [SQLColumn(SQLColumnType.Varchar, "_orderfromconame", true, false)]
        public string _orderfromconame { get; set; }


        [SQLColumn(SQLColumnType.Varchar, "_phonenumber", true, false)]
        public string _phonenumber { get; set; }

        [SQLColumn(SQLColumnType.Varchar, "_units", true, false)]
        public string _units { get; set; }

        public RawGoodsInfo()
        {
            id = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
        }
    }
}

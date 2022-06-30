using eLongMuSQL;
using System;

namespace DBManager.Tables
{
    [SQLTable("FinishedGoodsInfo")]
    public class FinishedGoodsInfo
    {
        [SQLColumn(SQLColumnType.BigInt, "id", true, true)]
        public Int64 id { get; set; }


        [SQLColumn(SQLColumnType.Varchar, "_finishedGoodName", true, false)]
        public string _finishedGoodName { get; set; }


        [SQLColumn(SQLColumnType.Varchar, "_wholesaleprice", true, false)]
        public string _wholesaleprice { get; set; }


        [SQLColumn(SQLColumnType.Varchar, "_retailprice", true, false)]
        public string _retailprice { get; set; }


        [SQLColumn(SQLColumnType.BigInt, "_authorId", true, false)]
        public Int64 _authorId { get; set; }


        public FinishedGoodsInfo()
        {
            id = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
        }
    }
}

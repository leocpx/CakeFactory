using eLongMuSQL;
using System;

namespace DBManager.Tables
{
    [SQLTable(nameof(RawGoodsInventory))]
    public class RawGoodsInventory
    {
        [SQLColumn(SQLColumnType.BigInt, nameof(id), true, true)]
        public Int64 id { get; set; }


        [SQLColumn(SQLColumnType.Float, nameof(Quantity),true,false)]
        public float Quantity { get; set; }


        public RawGoodsInventory()
        {
        }
    }
}

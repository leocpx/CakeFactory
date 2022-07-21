using eLongMuSQL;
using System;

namespace DBManager.Tables
{
    [SQLTable(nameof(FinishedGoodsInventory))]
    public class FinishedGoodsInventory
    {
        
        [SQLColumn(SQLColumnType.BigInt, nameof(id), true, true)]
        public Int64 id { get; set; }


        [SQLColumn(SQLColumnType.Float, nameof(Quantity), true, false)]
        public float Quantity { get; set; }

        public FinishedGoodsInventory()
        {
        }
    }
}

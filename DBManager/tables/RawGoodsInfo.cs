using eLongMuSQL;
using System;
using System.Collections.Generic;

namespace DBManager.Tables
{
    [SQLTable(nameof(RawGoodsInfo),true)]
    public class RawGoodsInfo
    {
        #region -- COLUMNS --
        [SQLColumn(SQLColumnType.BigInt, nameof(id), true, true)]
        public Int64 id { get; set; }


        [SQLColumn(SQLColumnType.Varchar, nameof(_rawgoodname), true, false)]
        public string _rawgoodname { get; set; }


        [SQLColumn(SQLColumnType.Varchar, nameof(_barcode), true, false)]
        public string _barcode { get; set; }


        [SQLColumn(SQLColumnType.Varchar, nameof(_priceperpiece), true, false)]
        public string _priceperpiece { get; set; }


        [SQLColumn(SQLColumnType.Varchar, nameof(_orderfromconame), true, false)]
        public string _orderfromconame { get; set; }


        [SQLColumn(SQLColumnType.Varchar, nameof(_phonenumber), true, false)]
        public string _phonenumber { get; set; }

        [SQLColumn(SQLColumnType.Varchar, nameof(_units), true, false)]
        public string _units { get; set; } 
        #endregion

        #region -- LINKED TABLES --
        [SQLTableLink(typeof(RawGoodsInventory), nameof(id), nameof(RawGoodsInventory.id))]
        public List<RawGoodsInventory> _RawGoodsInventory { get; set; }
        #endregion

        #region -- CONSTRUCTOR --
        public RawGoodsInfo()
        {
            id = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
        } 
        #endregion
    }
}

using eLongMuSQL;
using System;
using System.Collections.Generic;

namespace DBManager.Tables
{
    [SQLTable(nameof(PackagingItemInfo))]
    public class PackagingItemInfo
    {
        #region -- COLUMNS --
        [SQLColumn(SQLColumnType.BigInt, nameof(id), true, true)]
        public Int64 id { get; set; }


        [SQLColumn(SQLColumnType.Varchar, nameof(_packagingItemName), true, false)]
        public string _packagingItemName { get; set; }


        [SQLColumn(SQLColumnType.Varchar, nameof(_barcode), true, false)]
        public string _barcode { get; set; }


        [SQLColumn(SQLColumnType.Varchar, nameof(_priceperpiece), true, false)]
        public string _priceperpiece { get; set; }


        [SQLColumn(SQLColumnType.Varchar, nameof(_orderfromconame), true, false)]
        public string _orderfromconame { get; set; }


        [SQLColumn(SQLColumnType.Varchar, nameof(_phonenumber), true, false)]
        public string _phonenumber { get; set; }
        #endregion

        #region -- LINKED TABLES --
        [SQLTableLink(typeof(PackagingItemInventory), nameof(id), nameof(PackagingItemInventory.id))]
        public List<PackagingItemInventory> _PackagingItemInventory { get; set; }

        #endregion

        public PackagingItemInfo()
        {
            id = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
        }
    }
}

using eLongMuSQL;
using System;
using System.Collections.Generic;

namespace DBManager.Tables
{
    [SQLTable(nameof(PackagingTypeDetails))]
    public class PackagingTypeDetails
    {
        #region -- COLUMNS --
        [SQLColumn(SQLColumnType.BigInt, nameof(id), true, true)]
        public Int64 id { get; set; }


        [SQLColumn(SQLColumnType.BigInt, nameof(_packagingTypeId), true, false)]
        public Int64 _packagingTypeId { get; set; }


        [SQLColumn(SQLColumnType.BigInt, nameof(_packagingItemId), true, false)]
        public Int64 _packagingItemId { get; set; }


        [SQLColumn(SQLColumnType.BigInt, nameof(_quantity), true, false)]
        public Int64 _quantity { get; set; }
        #endregion


        #region -- LINKED TABLES --
        [SQLTableLink(typeof(PackagingItemInfo),nameof(_packagingItemId),nameof(PackagingItemInfo.id))]
        public List<PackagingItemInfo> _PackagingItemInfo { get; set; }
        #endregion

        public PackagingTypeDetails()
        {
            id = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
        }
    }
}

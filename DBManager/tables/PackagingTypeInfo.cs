using eLongMuSQL;
using System;
using System.Collections.Generic;

namespace DBManager.Tables
{
    [SQLTable(nameof(PackagingTypeInfo), true)]
    public class PackagingTypeInfo
    {
        #region -- COLUMNS --
        [SQLColumn(SQLColumnType.BigInt, nameof(id), true, true)]
        public Int64 id { get; set; }


        [SQLColumn(SQLColumnType.Varchar, nameof(_packagingTypeName), true, false)]
        public string _packagingTypeName { get; set; }


        [SQLColumn(SQLColumnType.Varchar, nameof(_barcode), true, false)]
        public string _barcode { get; set; } 
        #endregion

        #region -- LINKED TABLES --
        [SQLTableLink(typeof(PackagingTypeDetails), nameof(id), nameof(PackagingTypeDetails._packagingTypeId))]
        public List<PackagingTypeDetails> _PackagingTypeDetails { get; set; }

        #endregion

        #region -- CONSTRUCTOR --
        public PackagingTypeInfo()
        {
            id = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
        } 
        #endregion
    }
}

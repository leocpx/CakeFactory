using eLongMuSQL;
using System;
using System.Collections.Generic;

namespace DBManager.Tables
{



    [SQLTable(nameof(FinishedGoodsInfo), true)]
    public class FinishedGoodsInfo
    {
        [SQLColumn(SQLColumnType.BigInt, nameof(id), true, true)]
        public Int64 id { get; set; }


        [SQLColumn(SQLColumnType.Varchar, nameof(_finishedGoodName), true, false)]
        public string _finishedGoodName { get; set; }


        [SQLColumn(SQLColumnType.Varchar, nameof(_wholesaleprice), true, false)]
        public string _wholesaleprice { get; set; }


        [SQLColumn(SQLColumnType.Varchar, nameof(_retailprice), true, false)]
        public string _retailprice { get; set; }


        [SQLColumn(SQLColumnType.Varchar,nameof(_category),true, false)]
        public string _category { get; set; }


        [SQLColumn(SQLColumnType.BigInt, nameof(_authorId), true, false)]
        public Int64 _authorId { get; set; }


        [SQLTableLink(typeof(FinishedGoodsDetails),nameof(id),nameof(FinishedGoodsDetails._finishedGoodId))]
        public List<FinishedGoodsDetails> _FinishedGoodsDetails { get; set; }


        [SQLTableLink(typeof(Users),nameof(_authorId),nameof(Users.id))]
        public List<Users> CreatedBy { get; set; }


        [SQLTableLink(typeof(FinishedGoodsInventory),nameof(id),nameof(FinishedGoodsInventory.id))]
        public List<FinishedGoodsInventory> _FinishedGoodsInventory { get; set; }


        public FinishedGoodsInfo()
        {
            id = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
        }
    }
}

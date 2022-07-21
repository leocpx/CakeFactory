﻿using Core.Interfaces;
using eLongMuSQL;
using System;
using System.Collections.Generic;

namespace DBManager.Tables
{

    [SQLTable(nameof(ProductionOrders), true)]
    public class ProductionOrders : IOrder
    {
        [SQLColumn(SQLColumnType.BigInt, nameof(id), true, true)]
        public Int64 id { get; set; }


        [SQLColumn(SQLColumnType.BigInt, nameof(_finishedGoodId), true, false)]
        public Int64 _finishedGoodId { get; set; }


        [SQLColumn(SQLColumnType.BigInt, nameof(_workerId), true, false)]
        public Int64 _workerId { get; set; }


        [SQLColumn(SQLColumnType.BigInt, nameof(_startTime), true, false)]
        public Int64 _startTime { get; set; }


        [SQLColumn(SQLColumnType.Bit, nameof(_completed), true, false)]
        public bool _completed { get; set; } = false;


        [SQLTableLink(typeof(FinishedGoodsInfo),nameof(_finishedGoodId),nameof(FinishedGoodsInfo.id))]
        public List<FinishedGoodsInfo> _FinishedGoodsInfo { get; set; }


        [SQLTableLink(typeof(Users),nameof(_workerId),nameof(Users.id))]
        public List<Users> _Worker { get; set; }


        public ProductionOrders()
        {
            id = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
        }
    }
}

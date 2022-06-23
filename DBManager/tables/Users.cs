using eLongMuSQL;
using System;

namespace DBManager.Tables
{
    [SQLTable("Users")]
    public class Users
    {
        [SQLColumn(SQLColumnType.BigInt,"id",true,true)]
        public Int64 id { get; set; }

        [SQLColumn(SQLColumnType.Varchar, "_user", true,false)]
        public string _user { get; set; }


        [SQLColumn(SQLColumnType.Varchar, "_pass", true, false)]
        public string _pass { get; set; }

        [SQLColumn(SQLColumnType.Int, "_level", true,false)]
        public int _level { get; set; }

        public Users() 
        { }
    }
}

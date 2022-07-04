using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLongMuSQL
{
    [SQLTable("DemoSQLTable")]
    public class DemoSQLTable
    {
        [SQLColumn(SQLColumnType.Varchar, "DemoPrivateKey", true, true)]
        public string DemoPrivateKey { get; set; }


        [SQLColumn(SQLColumnType.BigInt, "DemoColumn1Name", true, false)]
        public Int64 DemoColumn1Name { get; set; }


        [SQLColumn(SQLColumnType.BigInt, "DemoColumn2Name", true, false)]
        public Int64 DemoColumn2Name { get; set; }


        [SQLColumn(SQLColumnType.Int, "DemoColumn3Name", true, false)]
        public Int64 DemoColumn3Name { get; set; }
    }
}

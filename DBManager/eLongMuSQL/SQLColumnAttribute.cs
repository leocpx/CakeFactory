using System;

namespace eLongMuSQL
{
    public class SQLUsedOnlyInCreateTableAttribute : Attribute
    {
    }
    public class SQLColumnAttribute : Attribute
    {
        public SQLColumnType ColumnType { get; set; }
        public bool isPrimaryKey { get; set; } = false;
        public bool NotNULL { get; set; } = false;
        public string Name { get; set; }

        public SQLColumnAttribute(SQLColumnType ColumnType, string Name, bool NotNULL, bool isPrimaryKey)
        {
            this.ColumnType = ColumnType;
            this.NotNULL = NotNULL;
            this.Name = Name;
            this.isPrimaryKey = isPrimaryKey;
        }
    }


    public class SQLTableLinkAttribute : Attribute
    {
        public Type _TableType { get; set; }
        public string id1;
        public string id2; 

        public SQLTableLinkAttribute(Type _tableType,string id1, string id2)
        {
            this.id1 = id1;
            this.id2 = id2;
            _TableType = _tableType;
        }
    }

}

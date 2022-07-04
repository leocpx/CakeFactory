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

}

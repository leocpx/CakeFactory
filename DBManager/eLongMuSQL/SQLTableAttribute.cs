using System;

namespace eLongMuSQL
{
    public class SQLTableAttribute : Attribute
    {
        public string TableName { get; set; }
        public SQLTableAttribute()
        {
        }

        public SQLTableAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}
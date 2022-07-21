using System;

namespace eLongMuSQL
{
    public class SQLTableAttribute : Attribute
    {
        public string TableName { get; set; }
        public bool ShouldCreateIfMissing { get; set; }
        public SQLTableAttribute()
        {
        }

        public SQLTableAttribute(string tableName)
        {
            TableName = tableName;
        }

        public SQLTableAttribute(string tableName, bool shouldCreate)
        {
            TableName = tableName;
            ShouldCreateIfMissing = shouldCreate;
        }
    }
}
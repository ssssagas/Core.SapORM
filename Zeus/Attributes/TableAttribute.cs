using System;
using System.Collections.Generic;
using System.Text;

namespace Zeus.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false)]
    public class TableAttribute : Attribute
    {
        public TableAttribute()
        {
        }
        public TableAttribute(string tableName)
        {
            this.TableName = tableName;
        }
        public string TableName { get; set; }
    }
}

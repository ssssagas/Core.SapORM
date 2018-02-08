using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeus.Attributes
{
    public static class AttributeHelper
    {
        public static string GetTableName<T>(this T t)
        {
            var type = typeof(T);
            var attribute = type.GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();
            if (attribute == null)
                return null;
            return ((TableAttribute)attribute).TableName;
        }
        public static string GetPropertyName<T>(this T t)
        {
            var type = typeof(T);
            var attribute = type.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault();
            if (attribute == null)
                return null;
            return ((ColumnAttribute)attribute).Name;
        }
        public static bool IsPrimaryKey<T>(this T t)
        {
            var type = typeof(T);
            var attribute = type.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault();
            if (attribute == null)
                return false;
            return ((ColumnAttribute)attribute).IsPrimaryKey;
        }

    }
}

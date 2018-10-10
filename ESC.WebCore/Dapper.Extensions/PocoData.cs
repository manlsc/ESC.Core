using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Extensions
{
    /// <summary>
    /// 表信息
    /// </summary>
    public class PocoData
    {
        /// <summary>
        /// 表信息
        /// </summary>
        public TableInfo Table { set; get; }

        /// <summary>
        /// 列信息
        /// </summary>
        public List<ColumnInfo> Columns { set; get; }

        public PocoData(Type type)
        {
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Table = FromPoco(type, propertyInfos);
            Columns = GetColumnInfos(propertyInfos);
        }

        /// <summary>
        /// 获取表信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private TableInfo FromPoco(Type t, PropertyInfo[] propertyInfos)
        {
            TableInfo ti = new TableInfo();
            object[] a2 = t.GetCustomAttributes(typeof(TableNameAttribute), true);
            ti.TableName = ((a2.Length == 0) ? t.Name : (a2[0] as TableNameAttribute).Value);
            a2 = t.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
            ti.PrimaryKey = ((a2.Length == 0) ? null : (a2[0] as PrimaryKeyAttribute).Value);
            ti.SequenceName = ((a2.Length == 0) ? null : (a2[0] as PrimaryKeyAttribute).SequenceName);
            ti.AutoIncrement = (a2.Length != 0 && (a2[0] as PrimaryKeyAttribute).AutoIncrement);
            ti.KeyProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(ti.PrimaryKey, StringComparison.OrdinalIgnoreCase));
            if (ti.KeyProperty == null)
            {
                throw new Exception($"表{ti.TableName}缺失主键");
            }
            return ti;
        }


        /// <summary>
        /// 获取属性集合
        /// </summary>
        /// <param name="propertyInfos"></param>
        /// <returns></returns>
        private List<ColumnInfo> GetColumnInfos(PropertyInfo[] propertyInfos)
        {           
            List<ColumnInfo> columnInfos = new List<ColumnInfo>();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(ColumnAttribute), true);
                if (propertyInfo.GetCustomAttributes(typeof(IgnoreAttribute), true).Length != 0)
                {
                    continue;
                }
                ColumnInfo columnInfo = new ColumnInfo();
                if (customAttributes.Length != 0)
                {
                    ColumnAttribute columnAttribute = (ColumnAttribute)customAttributes[0];
                    columnInfo.ColumnName = ((columnAttribute.Name == null) ? propertyInfo.Name : columnAttribute.Name);
                    columnInfo.Width = columnAttribute.Width;
                    columnInfo.Disabled = columnAttribute.Disabled;
                    columnInfo.DisplayName = columnAttribute.DisplayColumn;
                    columnInfo.Visible = columnAttribute.Visible;
                    columnInfo.Required = columnAttribute.Required;
                    columnInfo.ColumnType = getPropertyType(propertyInfo);
                    if (columnAttribute is ResultColumnAttribute)
                    {
                        columnInfo.ResultColumn = true;
                    }
                    else
                    {
                        columnInfo.ResultColumn = false;
                    }
                }
                else
                {
                    columnInfo.ColumnName = propertyInfo.Name;
                    columnInfo.ResultColumn = false;
                }
                columnInfos.Add(columnInfo);
            }

            return columnInfos;
        }

        /// <summary>
        /// 泛型的原始类型
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        private static string getPropertyType(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.IsGenericType)
            {
                return propertyInfo.PropertyType.GenericTypeArguments[0].Name.ToLower();
            }
            return propertyInfo.PropertyType.Name.ToLower();
        }
    }
}

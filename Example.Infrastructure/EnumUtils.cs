using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Example.Infrastructure
{
    public class EnumUtils
    {
        public static void GenerateSqlByAssembly(string assemblyPath,string fileOutputPath)
        {
            var myAssembly = Assembly.LoadFile(assemblyPath);
            var types = myAssembly.GetTypes();
            var enums = new SortedDictionary<string, Type>();
            foreach (var type in types)
            {
                if (type.BaseType.Name.Equals("AuditableEntity") || type.BaseType.Name.Equals("BaseEntity"))
                {
                    var properties = type.GetProperties();
                    foreach (var prop in properties)
                    {
                        var propType = prop.PropertyType;
                        if (propType.IsEnum)
                        {
                            if (enums.ContainsValue(propType)) continue;
                            enums.Add(type.Name + "-" + propType.GetHashCode(), propType);
                        }
                    }
                }
            }
            StringBuilder sqlText = new StringBuilder();
            var sql = @"INSERT INTO dbo.SystemDictionary ( EnumType ,EnumTypeDesc , EnumField ,EnumFieldValue ,EnumFieldDesc ,BeEntityName ,Remark ,UpdateTime)
                        VALUES  ( N'{0}' , N'{1}' , N'{2}' , {3} ,  N'{4}' , N'{5}' , N'{6}' , '{7}')";
            foreach (var e in enums)
            {
                var typeAttribute = e.Value.GetCustomAttributes<DescriptionAttribute>();
                var enumType = e.Value.Name;
                var enumTypeDesc = string.Empty;
                var beEntityName = e.Key.Split('-')[0];
                if (typeAttribute.Any())
                {
                    enumTypeDesc = typeAttribute.First().Description;
                    //Console.WriteLine("类型说明：" + e.Value.Name + "--:" + typeAttribute.First().Description);
                }
                else
                {
                    //Console.WriteLine("类型说明：" + e.Value.Name);
                }
                //Console.WriteLine(".");
                foreach (var field in e.Value.GetFields())
                {
                    if (field.Name.Equals("value__")) continue;
                    var enumField = field.Name;
                    int value = 0;
                    var enumFieldValue = (int)field.GetValue(value);
                    var enumFieldDesc = string.Empty;
                    var filedAttribute = field.GetCustomAttributes<DescriptionAttribute>();
                    if (filedAttribute.Any())
                    {
                        enumFieldDesc = filedAttribute.First().Description;
                        //Console.WriteLine("字段说明：" + field.Name + "--:" + filedAttribute.First().Description);
                    }
                    else
                    {
                        Console.WriteLine("字段说明：" + field.Name);
                    }
                    sqlText.AppendFormat(sql, enumType, enumTypeDesc, enumField, enumFieldValue, enumFieldDesc, beEntityName, null, DateTime.Now);
                    sqlText.AppendLine("\r\n");
                }
            }
            Utils.WriteFile(fileOutputPath, sqlText.ToString(), null);
        }
    }
}

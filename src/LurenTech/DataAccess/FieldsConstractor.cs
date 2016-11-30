using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LurenTech.DataAccess
{
   public static class FieldsConstractor
    {
        public static TFields GetFields<TFields>(IDataReader reader)
        {
            TFields fields = (TFields)Activator.CreateInstance(typeof(TFields));
            
            var props = (typeof(TFields)).GetFields();

            foreach (var p in props)
            {
                PropertySet(fields, p.Name, reader.GetOrdinal(p.Name));
            }

            return fields;
        }

        private static void PropertySet(object p, string propName, object value)
        {
            Type t = p.GetType();
            FieldInfo info = t.GetField(propName);
            if (info == null)
                return;
            info.SetValue(p, value);
        }
    }
}

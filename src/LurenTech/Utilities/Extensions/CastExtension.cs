using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LurenTech.Utilities.Extensions
{
    public static class CastExtension
    {
        public static T Cast<T>(this Object obj)
        {
            Type objectType = obj.GetType();
            Type target = typeof(T);
            var x = Activator.CreateInstance(target, false);
            var z = from source in objectType.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            var d = from source in target.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name)
               .ToList().Contains(memberInfo.Name)).ToList();
            PropertyInfo propertyInfo;
            object value;
            foreach (var memberInfo in members)
            {
                propertyInfo = typeof(T).GetProperty(memberInfo.Name);
                
                try
                {
                    value = obj.GetType().GetProperty(memberInfo.Name).GetValue(obj, null);
                    propertyInfo.SetValue(x, value, null);
                }
                catch { }
            }
            return (T)x;
        }
    }
}

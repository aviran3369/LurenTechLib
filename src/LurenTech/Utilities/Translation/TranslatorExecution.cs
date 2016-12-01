using LurenTech.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LurenTech.Utilities.Translation
{
    public static class TranslatorExecution
    {
        private const string KEY_FORMAT = "{0}<>{1}";

        private static Dictionary<string, ITranslator> _translators = new Dictionary<string, ITranslator>();

        public static void RegisterTranslator<BE, Model>(ITranslator<BE, Model> translator)
        {
            string k1 = typeof(BE).Name;
            string k2 = typeof(Model).Name;
            string key = string.Format(KEY_FORMAT, k1, k2);

            if (!_translators.ContainsKey(key))
            {
                _translators.Add(key, translator);
            }
        }

        public static To Translate<To>(object item)
        {
            Type itemType = item.GetType();
            object translator = GetTranslator(typeof(To), itemType);

            MethodInfo mi = translator.GetType().GetMethod("Translate", new Type[] { itemType });
            return (To)mi.Invoke(translator, new object[] { item });
        }

        public static List<To> Translate<To>(IEnumerable<object> items)
        {
            List<To> objects = new List<To>();

            if (items.IsNullOrEmpty())
            {
                return objects;
            }

            Type itemsType = items.First().GetType();
            object translator = GetTranslator(typeof(To), itemsType);

            MethodInfo mi = translator.GetType().GetMethod("Translate", new Type[] { itemsType });

            foreach (var item in items)
            {
                objects.Add((To)mi.Invoke(translator, new object[] { item }));
            }

            return objects;
        }

        private static object GetTranslator(Type t1, Type t2)
        {
            string k1 = t1.Name;
            string k2 = t2.Name;
            string key1 = string.Format(KEY_FORMAT, k1, k2);
            string key2 = string.Format(KEY_FORMAT, k2, k1);

            if (_translators.ContainsKey(key1))
            {
                return _translators[key1];
            }
            else if (_translators.ContainsKey(key2))
            {
                return _translators[key2];
            }
            else
            {
                throw new Exception("There is no translator for These types.");
            }
        }
    }
}

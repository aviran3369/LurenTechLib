using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;
using System.IO;
using Microsoft.DotNet.InternalAbstractions;
using LurenTech.Configuration;
using LurenTech.Utilities.Caching;

namespace LurenTech.BusinessAction
{
    class RepositoryMapItem
    {
        public string Name { get; set; }
        public string MapTo { get; set; }
    }

    class RepositoryMap
    {
        public List<RepositoryMapItem> RepositoryMapItems { get; set; }

        public RepositoryMap()
        {
            RepositoryMapItems = new List<RepositoryMapItem>();
        }
    }

    public class RepositoryMapperException : Exception
    {
        public RepositoryMapperException(string message) : base(message)
        {
        }
    }

    public class RepositoryMapper
    {
        private const string CACHE_KEY = "RepositoryMapperItems";
        private readonly string MAP_FILE;

        private Dictionary<string, string> _repositoriesMap;
        private static RepositoryMapper _mapper;

        private RepositoryMapper()
        {
            MAP_FILE = ApplicationConfig.Configuration.GetSetting("RepositoryMapperFile");

            this._repositoriesMap = CacheHelper.Get(CACHE_KEY) as Dictionary<string, string>;

            if (this._repositoriesMap == null)
            {
                List<RepositoryMapItem> repositoryMapItems = ImportRepositoryMapItems();

                _repositoriesMap = new Dictionary<string, string>();

                foreach (var item in repositoryMapItems)
                {
                    this._repositoriesMap.Add(item.Name, item.MapTo);
                }

                CacheHelper.Set<Dictionary<string, string>>(CACHE_KEY, this._repositoriesMap);
            }
        }

        public static void LoadRepositoriesMap()
        {
            _mapper = new RepositoryMapper();
        }

        public static object GetRepositoryInstance<T>()
        {
            string type = typeof(T).Name;
            string repositoryType = _mapper._repositoriesMap[type];

            if (string.IsNullOrEmpty(repositoryType))
                throw new RepositoryMapperException("Repository Doesn't exists or doesn't mapped.");

            return _mapper.GetInstance(repositoryType);
        }

        private object GetInstance(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return Activator.CreateInstance(type);

            var runtimeId = RuntimeEnvironment.GetRuntimeIdentifier();
            var assemblies = DependencyContext.Default.GetRuntimeAssemblyNames(runtimeId);

            foreach (var asm in assemblies)
            {
                type = Assembly.Load(asm).GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }

            return null;
        }

        private List<RepositoryMapItem> ImportRepositoryMapItems()
        {
            RepositoryMap items = new RepositoryMap();
            using (StreamReader r = new StreamReader(new FileStream(MAP_FILE, FileMode.Open)))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<RepositoryMap>(json);
            }

            return items.RepositoryMapItems;
        }

    }
}

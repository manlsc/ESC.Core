using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Core
{
    /// <summary>
    /// 注入工厂
    /// </summary>
    public class DependencyResolverFactory : IDependencyResolverFactory
    {
        private readonly Type _resolverType;

        public DependencyResolverFactory(string resolverTypeName)
        {
            if (string.IsNullOrEmpty(resolverTypeName))
            {
                throw new ArgumentNullException("resolverTypeName");
            }

            _resolverType = Type.GetType(resolverTypeName, true, true);
        }

        public DependencyResolverFactory()
            : this(new ConfigurationManagerWrapper().AppSettings["dependencyResolverTypeName"])
        {
        }

        public IDependencyResolver CreateInstance()
        {
            return Activator.CreateInstance(_resolverType) as IDependencyResolver;
        }
    }
}

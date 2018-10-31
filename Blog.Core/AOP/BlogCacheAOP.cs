using Blog.Core.Common.Attributes;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Core.AOP
{
    public class BlogCacheAOP : IInterceptor
    {
        private ICaching _cache;

        public BlogCacheAOP(ICaching cache)
        {
            this._cache = cache;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;

            var qCacheingAttribute = method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute)) as CachingAttribute;

            if (qCacheingAttribute != null)
            {

                var expirationTime = qCacheingAttribute.AbsoluteExpiration;
                var cacheKey = CustomCacheKey(invocation);
                var cacheValue = _cache.Get(cacheKey);

                if (cacheValue != null)
                {
                    invocation.ReturnValue = cacheValue;
                    return;
                }

                invocation.Proceed();

                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    _cache.Set(cacheKey, invocation.ReturnValue);
                }
            }
            else
            {
                invocation.Proceed();
            }

        }

        //自定义缓存键
        private string CustomCacheKey(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            var methodArguments = invocation.Arguments.Select(GetArgumentValue).Take(3).ToList();//获取参数列表，最多三个

            string key = $"{typeName}:{methodName}:";
            foreach (var param in methodArguments)
            {
                key += $"{param}:";
            }

            return key.TrimEnd(':');
        }
        //object 转 string
        private string GetArgumentValue(object arg)
        {
            if (arg is int || arg is long || arg is string)
                return arg.ToString();

            if (arg is DateTime)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");

            return "";
        }
    }
}

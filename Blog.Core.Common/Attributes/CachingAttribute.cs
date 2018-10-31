using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CachingAttribute : Attribute
    {
        public int AbsoluteExpiration { get; set; } = 30;
    }
}

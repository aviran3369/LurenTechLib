using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LurenTech
{
    public sealed class NoResult
    {
        private NoResult(){}
        private readonly static NoResult _value = new NoResult();
        public static NoResult Value { get { return _value; } }
    }
}

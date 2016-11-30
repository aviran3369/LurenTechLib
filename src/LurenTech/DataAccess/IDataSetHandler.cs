using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LurenTech.DataAccess
{
    public interface IDataSetHandler<T>
    {
        T Handle(IDataReader reader);
    }
}

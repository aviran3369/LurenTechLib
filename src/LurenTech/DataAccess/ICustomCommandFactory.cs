using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace LurenTech.DataAccess
{
    public interface ICustomCommandFactory<T>
    {
        DbCommand GetCommand(SqlConnection connection, T commandData);
    }
}

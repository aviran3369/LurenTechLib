using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurenTech.DataAccess
{
    public interface IDeleteFactory<TModel>
    {
        DbCommand GetCommand(SqlConnection connection, TModel model);
    }
    public interface IDeleteFactory
    {
        DbCommand GetCommand(SqlConnection connection, Guid model);
    }
}

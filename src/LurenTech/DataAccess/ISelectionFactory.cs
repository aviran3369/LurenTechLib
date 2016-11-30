using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurenTech.DataAccess
{
    public interface ISelectionFactory<TModel, TFields, TCondition>
    {
        DbCommand GetCommand(SqlConnection connection, TCondition condition);
        TModel ConstructModel(IDataReader reader, TFields fields);
        void ContinueConstruct(IDataReader reader, List<TModel> results);
    }
}

using LurenTech.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using LurenTech.Utilities.Logging;
using Microsoft.SqlServer.Server;
using LurenTech.Utilities.Extensions;

namespace LurenTech.DataAccess
{
    public abstract class BaseRepository
    {
        #region Private Properties

        protected static string _connectionString;
        protected static BaseLogger _logger;

        #endregion

        #region Properties

        public static string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        public static BaseLogger Logger
        {
            get
            {
                return _logger;
            }
            set
            {
                _logger = value;
            }
        }

        #endregion

        #region Ctor

        public BaseRepository()
        {

        }

        #endregion

        public static DbCommand CreateCommand(SqlConnection connection, string storedProcedureName)
        {
            DbCommand cmd = connection.CreateCommand();
            cmd.CommandText = storedProcedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }

        public static void AddInParam(DbCommand command, DbType dbType, string parameterName, object value)
        {
            AddParam(command, ParameterDirection.Input, dbType, parameterName, value, 0);
        }
        
        public static void AddOutParam(DbCommand command, DbType dbType, string parameterName, int size)
        {
            AddParam(command, ParameterDirection.Output, dbType, parameterName, null, size);
        }

        private static void AddParam(DbCommand command, ParameterDirection diriction, DbType dbType, string parameterName, object value, int size)
        {
            var param = command.CreateParameter();
            
            param.DbType = dbType;
            param.Direction = diriction;
            param.ParameterName = string.Format("@{0}", parameterName);
            if (diriction == ParameterDirection.Input)
                param.Value = value;
            else
                param.Size = size;

            command.Parameters.Add(param);
        }
    }

    public abstract class BaseRepository<TModel> : BaseRepository
    {
        #region Data Access Methods

        protected Guid Insert(IInsertFactory<TModel> factory, TModel model)
        {
            Guid newId = Guid.NewGuid();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DbCommand command = factory.GetCommand(connection, model, newId);
                
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }

            return newId;
        }

        protected List<TModel> Select<TCondition, TFields>(ISelectionFactory<TModel, TFields, TCondition> factory, TCondition condition)
        {
            List<TModel> results = new List<TModel>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DbCommand command = factory.GetCommand(connection, condition);

                try
                {
                    connection.Open();
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        TFields fields = FieldsConstractor.GetFields<TFields>(reader);
                        while (reader.Read())
                        {
                            results.Add(factory.ConstructModel(reader, fields));
                        }

                        if (reader.NextResult())
                        {
                            factory.ContinueConstruct(reader, results);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }

            return results;
        }

        protected void Update(IUpdateFactory<TModel> factory, TModel item)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DbCommand command = factory.GetCommand(connection, item);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        protected void Delete(IDeleteFactory factory, Guid entityId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DbCommand command = factory.GetCommand(connection, entityId);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        protected void ExecuteCustomCommand<C>(ICustomCommandFactory<C> factory, C commandData)
        {
            ExecuteCustomCommand<NoCriteria, C>(factory, commandData, null);
        }

        protected T ExecuteCustomCommand<T, C>(ICustomCommandFactory<C> factory, C commandData, IDataSetHandler<T> datasetHandler)
        {
            T value = default(T);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DbCommand command = factory.GetCommand(connection, commandData);

                try
                {
                    connection.Open();
                    if (datasetHandler == null)
                    {
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            value = datasetHandler.Handle(reader);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }

            return value;
        }

        #endregion
    }
}

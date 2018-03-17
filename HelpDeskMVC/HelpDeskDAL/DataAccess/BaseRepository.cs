using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace HelpDeskDAL.DataAccess
{
    public abstract class BaseRepository
    {
        private static readonly Database db;
        private static readonly ConnectionStringSettings connectionSettings;
        private static readonly string ProviderName;

        static BaseRepository()
        {
            connectionSettings = ConfigurationManager.ConnectionStrings["HelpDeskDb"];
            ProviderName = connectionSettings.ProviderName;
            db = new SqlDatabase(connectionSettings.ConnectionString);
        }

        public int ExecuteNonQuery(string StoredProcName, params DbParameter[] parameters)
        {
            using (var command = db.GetStoredProcCommand(StoredProcName))
            {
                foreach (var parameter in parameters)
                {
                    db.AddParameter(
                        command,
                        parameter.ParameterName,
                        parameter.DbType,
                        parameter.Direction,
                        parameter.ParameterName,
                        DataRowVersion.Default,
                        parameter.Value);
                }
                return Convert.ToInt32(db.ExecuteNonQuery(command));
            }
        }

        public IDataReader GetReader(string StoredProcName, params DbParameter[] parameters)
        {
            using (var command = db.GetStoredProcCommand(StoredProcName))
            {
                foreach (var parameter in parameters)
                {
                    db.AddParameter(
                        command,
                        parameter.ParameterName,
                        parameter.DbType,
                        parameter.Direction,
                        parameter.ParameterName,
                        DataRowVersion.Default,
                        parameter.Value);
                }
                IDataReader reader = db.ExecuteReader(command);
                return reader;
            }
        }

        public static int ExecuteNonQueryWithReturnValue(string storedProcName, string returnParameter, params DbParameter[] parameters)
        {
            using (var command = db.GetStoredProcCommand(storedProcName))
            {
                foreach (var parameter in parameters)
                {
                    if (parameter != null)
                    {
                        db.AddParameter(
                        command,
                        parameter.ParameterName,
                        parameter.DbType,
                        parameter.Direction,
                        parameter.ParameterName,
                        DataRowVersion.Default,
                        parameter.Value);
                    }
                }
                db.ExecuteNonQuery(command);
                return Convert.ToInt32(db.GetParameterValue(command, returnParameter));
            }
        }

        public static Dictionary<string, object> ExecuteNonQueryWithMultipleReturnValues(string storedProcName, string[] returnParameter, params DbParameter[] parameters)
        {
            using (var command = db.GetStoredProcCommand(storedProcName))
            {
                foreach (var parameter in parameters)
                {
                    if (parameter != null)
                    {
                        db.AddParameter(
                       command,
                       parameter.ParameterName,
                       parameter.DbType,
                       parameter.Direction,
                       parameter.ParameterName,
                       DataRowVersion.Default,
                       parameter.Value);
                    }
                }
                db.ExecuteNonQuery(command);
                Dictionary<string, object> values = new Dictionary<string, object>();
                foreach (var item in returnParameter)
                {
                    values.Add(item, Convert.ToInt32(db.GetParameterValue(command, item)));
                }
                return values;
            }
        }

        public static string ExecuteNonQueryWithStringReturnValue(string storedProcName, string returnParameter, params DbParameter[] parameters)
        {
            using (var command = db.GetStoredProcCommand(storedProcName))
            {
                foreach (var parameter in parameters)
                {
                    if (parameter != null)
                    {
                        db.AddParameter(
                        command,
                        parameter.ParameterName,
                        parameter.DbType,
                        parameter.Direction,
                        parameter.ParameterName,
                        DataRowVersion.Default,
                        parameter.Value);
                    }
                }
                db.ExecuteNonQuery(command);
                return db.GetParameterValue(command, returnParameter).ToString();
            }
        }

        public static int ExecuteScalar(string storedProcName, params DbParameter[] parameters)
        {
            using (var command = db.GetStoredProcCommand(storedProcName))
            {
                foreach (var parameter in parameters)
                {
                    if (parameter != null)
                    {
                        db.AddParameter(
                        command,
                        parameter.ParameterName,
                        parameter.DbType,
                        parameter.Direction,
                        parameter.ParameterName,
                        DataRowVersion.Default,
                        parameter.Value);
                    }
                }
                return Convert.ToInt32(db.ExecuteScalar(command));
            }
        }
    }
}


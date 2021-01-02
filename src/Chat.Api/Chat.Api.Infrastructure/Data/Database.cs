using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Chat.Api.Core.Configs;
using Chat.Api.Core.Interfaces;
using Dapper;

namespace Chat.Api.Infrastructure.Data
{
    public class Database : IDatabase
    {
        private readonly DatabaseConfig _config;
        private readonly int _timeOut;
        public Database(DatabaseConfig config)
        {
            _config = config;
            _timeOut = 1200;
        }
        public async Task OperationalAsync(string sql)
        {
            using (var connection = new SqlConnection(_config.ConnectionStrings.ConnectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql);
                await connection.CloseAsync();
            }
        }

        public async Task<int> ExecuteAsync(string sql, object param = null)
        {
            using (var connection = new SqlConnection(_config.ConnectionStrings.ConnectionString))
            {
                await connection.OpenAsync();
                var result = await connection.ExecuteAsync(sql, param, commandTimeout: _timeOut);
                await connection.CloseAsync();
                return result;
            }
        }

        public async Task<int> ExecuteStoredProcedureAsync(string procedureName, object param = null)
        {
            using (var connection = new SqlConnection(_config.ConnectionStrings.ConnectionString))
            {
                await connection.OpenAsync();
                var result = await connection.ExecuteAsync(procedureName, param, commandType: CommandType.StoredProcedure, commandTimeout: _timeOut);
                await connection.CloseAsync();
                return result;
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
        {
            using (var connection = new SqlConnection(_config.ConnectionStrings.ConnectionString))
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<T>(sql, param, commandTimeout: _timeOut);
                await connection.CloseAsync();
                return result;
            }
        }

        public IEnumerable<T> Query<T>(string sql, object param = null)
        {
            using (var connection = new SqlConnection(_config.ConnectionStrings.ConnectionString))
            {
                connection.Open();
                var result = connection.Query<T>(sql, param, commandTimeout: _timeOut);
                connection.Close();
                return result;
            }
        }

        public async Task<IEnumerable<T>> QueryStoredProcedureAsync<T>(string procedureName, object param = null)
        {
            using (var connection = new SqlConnection(_config.ConnectionStrings.ConnectionString))
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<T>(procedureName, param, commandType: CommandType.StoredProcedure, commandTimeout: _timeOut);
                await connection.CloseAsync();
                return result;
            }
        }

        public (IEnumerable<T1>, IEnumerable<T2>) QueryStoredProcedureMultiple<T1, T2>(string procedureName,
            object param = null)
        {
            using (var connection = new SqlConnection(_config.ConnectionStrings.ConnectionString))
            {
                connection.Open();
                using (var multi = connection.QueryMultiple(procedureName, param, commandType: CommandType.StoredProcedure, commandTimeout: _timeOut))
                {
                    var result = (multi.Read<T1>().ToList(), multi.Read<T2>().ToList());
                    connection.Close();
                    return result;
                }
            }
        }

        public async Task<T> QueryStoredProcedureWithReturnStatementAsync<T>(string procedureName, object param = null)
        {
            using (var connection = new SqlConnection(_config.ConnectionStrings.ConnectionString))
            {
                await connection.OpenAsync();
                var parameters = new DynamicParameters(param);
                parameters.Add("@ReturnValue", dbType: DbType.String, direction: ParameterDirection.ReturnValue);
                await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _timeOut);
                var result = parameters.Get<T>("@ReturnValue");
                await connection.CloseAsync();
                return result;
            }
        }

        public IEnumerable<T> QueryStoredProcedure<T>(string procedureName, object param = null)
        {
            using (var connection = new SqlConnection(_config.ConnectionStrings.ConnectionString))
            {
                connection.Open();
                var result = connection.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure, commandTimeout: _timeOut);
                connection.Close();
                return result;
            }
        }
    }
}

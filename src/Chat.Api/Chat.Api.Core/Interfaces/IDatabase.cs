using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chat.Api.Core.Interfaces
{
    public interface IDatabase
    {
        Task OperationalAsync(string sql);
        Task<int> ExecuteAsync(string sql, object param = null);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null);
        IEnumerable<T> Query<T>(string sql, object param = null);
        Task<IEnumerable<T>> QueryStoredProcedureAsync<T>(string procedureName, object param = null);

        (IEnumerable<T1>, IEnumerable<T2>) QueryStoredProcedureMultiple<T1, T2>(string procedureName,
            object param = null);
        Task<T> QueryStoredProcedureWithReturnStatementAsync<T>(string procedureName, object param = null);
        IEnumerable<T> QueryStoredProcedure<T>(string procedureName, object param = null);
    }
}
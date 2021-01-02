using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Api.Core.Interfaces;

namespace Chat.Api.Core.DatabaseTests
{
    public class DatabaseTestService
    {
        private readonly IDatabase _database;

        public DatabaseTestService(IDatabase database)
        {
            _database = database;
        }
        public async Task<IEnumerable<string>> GetTestDataAsync()
        {
            var testData = await _database.QueryAsync<string>("SELECT [Name] FROM TestTable");
            return testData;
        }
    }
}

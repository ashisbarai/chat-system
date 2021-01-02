using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace Chat.Api.Core.Extensions
{
    public static class DapperExtension
    {
        public static SqlMapper.ICustomQueryParameter AsDapperTableValuedParamForId(this IEnumerable<int> ids)
        {
            return AsDapperTableValuedParamForId(ids, "ListIntType");
        }

        private static SqlMapper.ICustomQueryParameter AsDapperTableValuedParamForId<T>(IEnumerable<T> ids, string dbTypeName)
        {
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("Id", typeof(T)));
            ids.Visit(id => dt.Rows.Add(id));

            return dt.AsTableValuedParameter(dbTypeName);
        }
        public static SqlMapper.ICustomQueryParameter AsDapperTableValuedParamForId(this IEnumerable<string> ids)
        {
            return AsDapperTableValuedParamForId(ids, "ListNvarcharType");
        }
        public static SqlMapper.ICustomQueryParameter AsDapperTableValuedParamForId(this IEnumerable<Guid> ids)
        {
            return AsDapperTableValuedParamForId(ids, "ListUniqueIdentifierType");
        }
    }
}

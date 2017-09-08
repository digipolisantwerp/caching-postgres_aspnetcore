using Digipolis.Extensions.Caching.Postgres;
using Microsoft.Extensions.Options;

namespace Digipolis.Extensions.Caching.PostgreSql.Tests
{
    internal class TestPostgreSqlCacheOptions : IOptions<PostgreSqlCacheOptions>
    {
        private readonly PostgreSqlCacheOptions _innerOptions;

        public TestPostgreSqlCacheOptions(PostgreSqlCacheOptions innerOptions)
        {
            _innerOptions = innerOptions;
        }

        public PostgreSqlCacheOptions Value
        {
            get
            {
                return _innerOptions;
            }
        }
    }
}

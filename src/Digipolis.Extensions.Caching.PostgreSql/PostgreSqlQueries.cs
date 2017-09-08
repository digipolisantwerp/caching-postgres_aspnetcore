// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


namespace Digipolis.Extensions.Caching.Postgres
{
    internal class PostgreSqlQueries
    {
        private const string TableInfoFormat =
            "SELECT TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE " +
            "FROM INFORMATION_SCHEMA.TABLES " +
            "WHERE TABLE_SCHEMA = '{0}' " +
            "AND TABLE_NAME = '{1}'";

        private const string UpdateCacheItemFormat =
            "UPDATE {0} " +
            "SET \"ExpiresAtTime\" = " +
                "(CASE " +
                    "WHEN (DATE_PART('day', \"AbsoluteExpiration\" - @UtcNow) * 24 + " +
                          "DATE_PART('hour', \"AbsoluteExpiration\" - @UtcNow) * 60 + " +
                          "DATE_PART('minute', \"AbsoluteExpiration\" - @UtcNow) * 60 + " +
                          "DATE_PART('second', \"AbsoluteExpiration\" - @UtcNow)) <= \"SlidingExpirationInSeconds\" " +
                    "THEN \"AbsoluteExpiration\" " +
                    "ELSE " +
                    "@UtcNow + \"SlidingExpirationInSeconds\" * INTERVAL '1 second' " +
                "END) " +
            "WHERE \"Id\" = @Id " +
            "AND @UtcNow <= \"ExpiresAtTime\" " +
            "AND \"SlidingExpirationInSeconds\" IS NOT NULL " +
            "AND (\"AbsoluteExpiration\" IS NULL OR \"AbsoluteExpiration\" <> \"ExpiresAtTime\") ;";

        private const string GetCacheItemFormat =
            "SELECT \"Id\", \"ExpiresAtTime\", \"SlidingExpirationInSeconds\", \"AbsoluteExpiration\", \"Value\" " +
            "FROM {0} WHERE \"Id\" = @Id AND @UtcNow <= \"ExpiresAtTime\";";

        private const string SetCacheItemFormat =
            "INSERT INTO {0} " +
                  "(\"Id\", \"Value\", \"ExpiresAtTime\", \"SlidingExpirationInSeconds\", \"AbsoluteExpiration\") " +
                  "VALUES(@Id, @Value, @ExpiresAtTime, @SlidingExpirationInSeconds, @AbsoluteExpiration) " +
            "ON CONFLICT (\"Id\") " +
            "DO " +
            "UPDATE SET \"Value\" = @Value, \"ExpiresAtTime\" = @ExpiresAtTime, " +
              "\"SlidingExpirationInSeconds\" = @SlidingExpirationInSeconds, \"AbsoluteExpiration\" = @AbsoluteExpiration " +
            "WHERE {0}.\"Id\" = @Id";

        private const string DeleteCacheItemFormat = "DELETE FROM {0} WHERE \"Id\" = @Id";

        public const string DeleteExpiredCacheItemsFormat = "DELETE FROM {0} WHERE @UtcNow > \"ExpiresAtTime\"";

        public PostgreSqlQueries(string schemaName, string tableName)
        {
            var tableNameWithSchema = $"{schemaName}.\"{tableName}\"";

            // when retrieving an item, we do an UPDATE first and then a SELECT
            GetCacheItem = string.Format(UpdateCacheItemFormat + GetCacheItemFormat, tableNameWithSchema);
            GetCacheItemWithoutValue = string.Format(UpdateCacheItemFormat, tableNameWithSchema);
            DeleteCacheItem = string.Format(DeleteCacheItemFormat, tableNameWithSchema);
            DeleteExpiredCacheItems = string.Format(DeleteExpiredCacheItemsFormat, tableNameWithSchema);
            SetCacheItem = string.Format(SetCacheItemFormat, tableNameWithSchema);
            TableInfo = string.Format(TableInfoFormat, EscapeLiteral(schemaName), EscapeLiteral(tableName));
        }

        public string TableInfo { get; }

        public string GetCacheItem { get; }

        public string GetCacheItemWithoutValue { get; }

        public string SetCacheItem { get; }

        public string DeleteCacheItem { get; }

        public string DeleteExpiredCacheItems { get; }

        private string EscapeLiteral(string literal)
        {
            return literal.Replace("'", "''");
        }
    }
}

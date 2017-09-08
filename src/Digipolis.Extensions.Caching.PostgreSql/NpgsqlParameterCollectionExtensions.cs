// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using NpgsqlTypes;
using System;

namespace Digipolis.Extensions.Caching.Postgres
{
    internal static class NpgsqlParameterCollectionExtensions
    {
        // For all values where the length is less than the below value, try setting the size of the
        // parameter for better performance.
        public const int DefaultValueColumnWidth = 8000;

        // Maximum size of a primary key column is 900 bytes (898 bytes from the key + 2 additional bytes required by
        // the Sql Server).
        public const int CacheItemIdColumnWidth = 449;

        public static NpgsqlParameterCollection AddCacheItemId(this NpgsqlParameterCollection parameters, string value)
        {
            parameters.AddWithValue(Columns.Names.CacheItemId, NpgsqlDbType.Text, CacheItemIdColumnWidth, value);
            return parameters;
        }

        public static NpgsqlParameterCollection AddCacheItemValue(this NpgsqlParameterCollection parameters, byte[] value)
        {
            if (value != null && value.Length < DefaultValueColumnWidth)
            {
                parameters.AddWithValue(
                    Columns.Names.CacheItemValue,
                    NpgsqlDbType.Bytea,
                    DefaultValueColumnWidth,
                    value);
            }
            else
            {
                // do not mention the size
                parameters.AddWithValue(Columns.Names.CacheItemValue, NpgsqlDbType.Bytea, value);
            }

            return parameters;
        }

        public static NpgsqlParameterCollection AddSlidingExpirationInSeconds(
            this NpgsqlParameterCollection parameters,
            TimeSpan? value)
        {
            if (value.HasValue)
            {
                parameters.AddWithValue(
                    Columns.Names.SlidingExpirationInSeconds, NpgsqlDbType.Double, value.Value.TotalSeconds);
            }
            else
            {
                parameters.AddWithValue(Columns.Names.SlidingExpirationInSeconds, NpgsqlDbType.Double, DBNull.Value);
            }
            return parameters;
        }

        public static NpgsqlParameterCollection AddAbsoluteExpiration(
            this NpgsqlParameterCollection parameters,
            DateTimeOffset? utcTime)
        {
            if (utcTime.HasValue)
            {
                parameters.AddWithValue(
                    Columns.Names.AbsoluteExpiration, NpgsqlDbType.Timestamp, utcTime.Value);
            }
            else
            {
                parameters.AddWithValue(
                    Columns.Names.AbsoluteExpiration, NpgsqlDbType.Timestamp, DBNull.Value);
            }
            return parameters;
        }

        public static NpgsqlParameterCollection AddExpiresAtTime(
            this NpgsqlParameterCollection parameters,
            DateTimeOffset? utcTime)
        {
            if (utcTime.HasValue)
            {
                parameters.AddWithValue(
                    Columns.Names.ExpiresAtTime, NpgsqlDbType.Timestamp, utcTime.Value);
            }
            else
            {
                parameters.AddWithValue(
                    Columns.Names.ExpiresAtTime, NpgsqlDbType.Timestamp, DBNull.Value);
            }
            return parameters;
        }

        public static NpgsqlParameterCollection AddWithValue(
            this NpgsqlParameterCollection parameters,
            string parameterName,
            NpgsqlDbType dbType,
            object value)
        {
            var parameter = new NpgsqlParameter(parameterName, dbType);
            parameter.Value = value;
            parameters.Add(parameter);
            parameter.ResetDbType();
            return parameters;
        }

        public static NpgsqlParameterCollection AddWithValue(
            this NpgsqlParameterCollection parameters,
            string parameterName,
            NpgsqlDbType dbType,
            int size,
            object value)
        {
            var parameter = new NpgsqlParameter(parameterName, dbType, size);
            parameter.Value = value;
            parameters.Add(parameter);
            parameter.ResetDbType();
            return parameters;
        }
    }
}

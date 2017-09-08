# Digipolis.Extensions.Caching.PostgreSql Toolbox

Distributed cache implementation of Microsoft.Extensions.Caching.Distributed.IDistributedCache using PostgreSql.

## Table of Contents

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->

- [Installation](#installation)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Installation

To add the toolbox to a project, you add the package to the csproj-file :

```xml
  <ItemGroup>
    <PackageReference Include="Digipolis.Extensions.Caching.PostgreSql" Version="1.0.0" />
  </ItemGroup>
``` 

In Visual Studio you can also use the NuGet Package Manager to do this.

## Usage

In order to use the PostgreSql distributed cache you need to 

## Testing

All tests inside PostgreSqlCacheWithDatabaseTest.cs are tests that execute against a running database.

You can use this docker commeand to spin up a database:

```
	docker run --name pgsql_dist_caching_test_db -p 5432:5432/tcp -e POSTGRES_PASSWORD=postgres postgres
```



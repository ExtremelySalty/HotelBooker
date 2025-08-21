var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
                 .WithLifetime(ContainerLifetime.Session)
                 .WithDataVolume();

var db = sql.AddDatabase("ApiHotelBoooker");

builder.AddProject<Projects.HotelBooker_WebApi>("hotelbooker-webapi")
    .WithHttpHealthCheck("/health")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();

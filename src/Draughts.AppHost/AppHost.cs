var builder = DistributedApplication.CreateBuilder(args);

var API = builder.AddProject<Projects.Draughts_Api>("draughts-api");

var Web = builder.AddProject<Projects.Draughts_Web>("draughts-web")
    .WithReference(API)
    .WaitFor(API);

builder.Build().Run();

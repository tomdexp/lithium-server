using System.Reflection;
using Lithium.Server;
using Lithium.Server.Core;
using Lithium.Server.Core.Logging;
using Lithium.Server.Core.Networking;
using Lithium.Server.Core.Networking.Extensions;
using Lithium.Server.Core.Systems.Commands;
using Lithium.Server.Dashboard;
using Sentry.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

SentrySdk.Init(options =>
{
    options.Dsn = builder.Configuration["Sentry:Dsn"];
    options.Environment = builder.Configuration["Environment"];
    options.AttachStacktrace = true;
    options.SendDefaultPii = false;
    options.AttachStacktrace = true;
    options.SendDefaultPii = true;
    options.Debug = true;
    options.DiagnosticLevel = SentryLevel.Error;
    options.TracesSampleRate = 1.0;
});

builder.Services.Configure<SentryLoggingOptions>(options =>
{
    options.Dsn = builder.Configuration["Sentry:Dsn"];
    options.Environment = builder.Configuration["Environment"];
    options.InitializeSdk = true;
    options.AttachStacktrace = true;
    options.SendDefaultPii = false;
    options.AttachStacktrace = true;
    options.SendDefaultPii = true;
    options.MinimumBreadcrumbLevel = LogLevel.Debug;
    options.MinimumEventLevel = LogLevel.Debug;
    options.Debug = true;
    options.DiagnosticLevel = SentryLevel.Error;
    options.TracesSampleRate = 1.0;
});

builder.Logging.AddConfiguration(builder.Configuration);
builder.Logging.AddSentry();

builder.Logging.ClearProviders();

builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "[HH:mm:ss] ";
    options.IncludeScopes = false;
});

builder.Logging.AddFilter("Microsoft", LogLevel.Warning);
builder.Logging.AddFilter("System", LogLevel.Warning);

// SignalR
builder.Services.AddSignalR();

// Core services
builder.Services.AddSingleton<IServerConfigurationProvider, JsonServerConfigurationProvider>();
builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddSingleton<IClientManager, ClientManager>();
builder.Services.AddSingleton<IPluginRegistry, PluginRegistry>();
builder.Services.AddSingleton<IPluginManager, PluginManager>();

builder.Services.AddPacketHandlers(Assembly.GetExecutingAssembly());

// Networking
builder.Services.AddSingleton<QuicServer>();

// Lifetime
builder.Services.AddHostedService<ServerLifetime>();
builder.Services.AddHostedService<WorldService>();

// Console command service
builder.Services.AddConsoleCommands();

var app = builder.Build();

app.MapHub<ServerHub>("/hub/admin");

await app.RunAsync();
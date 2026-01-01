using System.Reflection;
using Lithium.Server;
using Lithium.Server.Core;
using Lithium.Server.Core.Logging;
using Lithium.Server.Core.Networking;
using Lithium.Server.Core.Networking.Extensions;
using Lithium.Server.Core.Systems.Commands;
using Lithium.Server.Dashboard;
using Sentry.Extensions.Logging;
using Sentry.Extensions.Logging.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

SentrySdk.Init(options =>
{
    options.Dsn = builder.Configuration["Sentry:Dsn"];
    options.Environment = builder.Configuration["Environment"];

    options.SetBeforeSendLog(static log =>
    {
        // Filter out all info logs
        return log.Level switch
        {
            SentryLogLevel.Error or SentryLogLevel.Fatal => log,
            _ => null
        };
    });
});

builder.Logging.ClearProviders();

builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "[HH:mm:ss] ";
    options.IncludeScopes = false;
});

builder.Services.Configure<SentryLoggingOptions>(options =>
{
    options.Dsn = builder.Configuration["Sentry:Dsn"];
    options.Environment = builder.Configuration["Environment"];
    options.InitializeSdk = true;

    options.SetBeforeSendLog(static log =>
    {
        var a = log.TryGetAttribute("force_send", out var a1);

        Console.WriteLine("BeforeSendLog: " + string.Join(", ", a, a1));

        if (log.TryGetAttribute("force_send", out var v) && v.ToString() is "true")
            return log;

        // Filter out all info logs
        return log.Level switch
        {
            SentryLogLevel.Error or SentryLogLevel.Fatal => log,
            _ => null
        };
    });
});

builder.Services.AddSentry<SentryLoggingOptions>();

builder.Logging.AddConfiguration(builder.Configuration);
builder.Logging.AddSentry();

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
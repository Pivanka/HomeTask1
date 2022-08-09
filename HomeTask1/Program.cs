using HomeTask1;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(System.Configuration.ConfigurationManager.AppSettings["LoadPath"] + "\\LogFile.txt")
    .CreateLogger();

try
{
    Log.Information("Starting up the service.");
    IHost host = Host.CreateDefaultBuilder(args)
        .UseWindowsService()
        .ConfigureServices(services =>
        {
            services.AddHostedService<PaymentTransactions>();
        })
        .UseSerilog()
        .Build();
    await host.RunAsync();
    return;
}
catch (Exception ex)
{
    Log.Fatal(ex, "There was a problem starting the service.");
    return;
}
finally
{
    Log.CloseAndFlush();
}

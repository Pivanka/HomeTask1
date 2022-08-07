using HomeTask1;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<PaymentTransactions>();
    })
    .Build();

await host.RunAsync();

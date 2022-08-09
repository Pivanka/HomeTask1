

namespace HomeTask1
{
    public class PaymentTransactions : BackgroundService
    {
        private readonly ILogger<PaymentTransactions> _logger;
        private string today;
        
        public PaymentTransactions(ILogger<PaymentTransactions> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            today = DateOnly.FromDateTime(DateTime.Now).ToString("MM/dd/yyyy");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The service has been stopped.");
            return base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                do 
                {
                    Client.FileProcess();
                    _logger.LogInformation("All files are processed.");
                } while (Client.HasNewFiles());
                if (today != DateOnly.FromDateTime(DateTime.Now).ToString("MM/dd/yyyy"))
                {
                    Client.CreateLogFile(today);
                    today = DateOnly.FromDateTime(DateTime.Now).ToString("MM/dd/yyyy");
                    _logger.LogInformation("Meta.log is created.");
                }
                await Task.Delay(15000, stoppingToken);
            }
        }
    }
}
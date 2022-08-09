

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
            today = DateOnly.FromDateTime(DateTime.Now).ToShortDateString();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                do 
                {
                    Facade.FileProcess();
                } while (Facade.HasNewFiles());
                if (today != DateOnly.FromDateTime(DateTime.Now).ToShortDateString())
                {
                    Facade.CreateLogFile(today);
                    today = DateOnly.FromDateTime(DateTime.Now).ToShortDateString();
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
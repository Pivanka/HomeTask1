namespace HomeTask1
{
    public class PaymentTransactions : BackgroundService
    {
        private readonly ILogger<PaymentTransactions> _logger;

        public PaymentTransactions(ILogger<PaymentTransactions> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
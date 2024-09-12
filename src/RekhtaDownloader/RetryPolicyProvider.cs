using System;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace RekhtaDownloader
{
    internal class RetryPolicyProvider
    {
        public RetryPolicyProvider(ILogger logger)
        {
            PageRetryPolicy = Policy.Handle<Exception>()
                                    .WaitAndRetryAsync(
                                        5,
                                        retryAttempt => TimeSpan.FromSeconds(retryAttempt),
                                        (exception, timeSpan, retryCount, context) =>
                                        {
                                            logger.LogInformation($"Failed to download page. Retrying #{retryCount}...");
                                            logger.LogInformation(exception.Message);
                                        });
        }

        public AsyncRetryPolicy PageRetryPolicy { get; }
    }
}
using System;
using Common;
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
                                            logger.Log($"Failed to download page. Retrying #{retryCount}...");
                                            logger.Log(exception.Message);
                                        });
        }

        public AsyncRetryPolicy PageRetryPolicy { get; }
    }
}
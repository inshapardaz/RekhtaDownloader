using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RekhtaDownloader
{
    internal class ConsumerStarter
    {
        public IEnumerable<Task> StartAsyncConsumers(int consumersCount, CancellationToken token, Action consumer)
        {
            return Enumerable.Range(0, consumersCount)
                             .Select(a => Task.Factory.StartNew(consumer, token, TaskCreationOptions.LongRunning, TaskScheduler.Default));
        }
    }
}

using System.Collections.Concurrent;
using System.Collections.Generic;

namespace signalr.server.Services
{
    public class CounterService : ICounterService
    {
        public ConcurrentDictionary<string, string> ConnectionIds { get; set; } = new ConcurrentDictionary<string, string>();
        public ConcurrentBag<KeyValuePair<string, string>> MessagesReplied { get; set; } = new ConcurrentBag<KeyValuePair<string, string>>();
        public void Reset()
        {
            MessagesReplied = new ConcurrentBag<KeyValuePair<string, string>>();
        }

        public void ResetAll()
        {
            MessagesReplied = new ConcurrentBag<KeyValuePair<string, string>>();
            MessagesReplied = new ConcurrentBag<KeyValuePair<string, string>>();
        }
    }
}

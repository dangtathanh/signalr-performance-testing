using System.Collections.Concurrent;
using System.Collections.Generic;

namespace signalr.server.Services
{
    public interface ICounterService
    {
        ConcurrentDictionary<string, string> ConnectionIds { set; get; }
        ConcurrentBag<KeyValuePair<string, string>> MessagesReplied { set; get; }
        void Reset();
        void ResetAll();
    }
}

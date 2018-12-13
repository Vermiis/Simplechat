using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTCPApp
{
    public class Messages
    {
        public ConcurrentQueue<string> messagesToSend = new ConcurrentQueue<string>();
        public ConcurrentQueue<string> messagesReceived = new ConcurrentQueue<string>();
    }
}

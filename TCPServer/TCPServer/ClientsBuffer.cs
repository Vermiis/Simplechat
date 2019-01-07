using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    class ClientsBuffer
    {
        public ConcurrentQueue<string> ConnectedUsers = new ConcurrentQueue<string>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientTCPApp
{
    static class Program
    {

        public static class ConnectionData
        {
            public static string IP { get; set; }
            public static int Port { get; set; }
            public static string Nick { get; set; }
            public static bool Connected { get; set; }
        }
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClientWindow());
            Thread ClientThread = new Thread(ClientCode.AsynchronousClient.Start);

        }
    }
}

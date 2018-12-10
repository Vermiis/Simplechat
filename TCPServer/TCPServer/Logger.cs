using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPServer
{
    public class Logger
    {

        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        private static string logpath = @"C:\\Users\\Ewelina\\Documents\\Simplechat\\TCPServer\\TCPServer\\bin\\Debug\\test.txt";

        public static void WriteToFileThreadSafe(string text, string path)
        {
            // Set Status to Locked
            _readWriteLock.EnterWriteLock();
            try
            {
                // Append text to the file
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(text);
                    sw.Close();
                }
            }
            finally
            {
                // Release lock
                _readWriteLock.ExitWriteLock();
            }
        }

        public static void SaveLog(string data)
        {
            WriteToFileThreadSafe(data, logpath);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientTCPApp
{
    public class Logger
    {
        


        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        private static string logpath = @"C:\Users\user\Documents\Simplechat2\ClientTCPApp\ClientTCPApp\bin\Debug\\test.txt";

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

﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


static class Program
{
    /// <summary>
    /// Główny punkt wejścia dla aplikacji.
    /// </summary>
    [STAThread]
    static void Main()
    {
        AsynchronousSocketListener.StartListening();


    }
}
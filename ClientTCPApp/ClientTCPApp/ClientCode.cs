using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using ClientTCPApp;

// State object for receiving data from remote device.  
namespace ClientTCPApp
{
   public class ClientCode
    {

       // private String[] messagequeue;

        public class StateObject
        {
            // Client socket.  
            public Socket workSocket = null;
            // Size of receive buffer.  
            public const int BufferSize = 256;
            // Receive buffer.  
            public byte[] buffer = new byte[BufferSize];
            // Received data string.  
            public StringBuilder sb = new StringBuilder();
            
        }

        public class AsynchronousClient
        {

            private static ManualResetEvent connectDone =
                new ManualResetEvent(false);
            private static ManualResetEvent sendDone =
                new ManualResetEvent(false);
            private static ManualResetEvent receiveDone =
                new ManualResetEvent(false);
          //  public Messages ClientMessages = new Messages();

            // The response from the remote device.  
            private static String response = String.Empty;
            // public Messages Msgs = new Messages();
            private static string messageToSend = String.Empty;
            

            public static void StartClient( string ipAddr, int port)
            {
                // Connect to a remote device.  
                try
                {
                    
                    //IPHostEntry ipHostInfo = Dns.GetHostEntry("");

                    IPAddress ipAddress = System.Net.IPAddress.Parse(ipAddr);
                    IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);


                    // Create a TCP/IP socket.  
                    Socket client = new Socket(ipAddress.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp);

                    // Connect to the remote endpoint.  
                    client.BeginConnect(remoteEP,
                        new AsyncCallback(ConnectCallback), client);
                    connectDone.WaitOne();

                    // Send test data to the remote device.  
                    Send(client, "NICK " + Program.ConnectionData.Nick + " MSG " +  " <EOF>");
                    sendDone.WaitOne();

                    // Receive the response from the remote device.  
                    Receive(client);
                    receiveDone.WaitOne();
                    

                    // Write the response to the console.  
                    
                   // Console.WriteLine("Response received : {0}", response);
                    Logger.SaveLog("Response received : " + response);
                    do
                    {
                        while (Messages.messagesToSend.TryDequeue(out messageToSend))
                        {
                            Send(client, "NICK " + Program.ConnectionData.Nick + " MSG " + messageToSend + " <EOF>");
                            sendDone.WaitOne();

                            // Receive the response from the remote device.  
                            Receive(client);
                            receiveDone.WaitOne();
                        }


                        if (Messages.InternalCommands.Contains("DC"))
                        {
                            Send(client, "NICK " + Program.ConnectionData.Nick + " QUIT " + " <EOF>");
                            client.Shutdown(SocketShutdown.Both);
                            client.Close();
                        }

                    } while (client.Connected);

                    // Release the socket. 
                  
                    

                }
                catch (Exception e)
                {
                    Logger.SaveLog( "Error: " + e.ToString());
                    
                }
            }

            private static void ConnectCallback(IAsyncResult ar)
            {
                try
                {
                    // Retrieve the socket from the state object.  
                    Socket client = (Socket)ar.AsyncState;

                    // Complete the connection.  
                    client.EndConnect(ar);

                  //  Console.WriteLine("Socket connected to {0}",
                       // client.RemoteEndPoint.ToString());

                    Logger.SaveLog("Socket connected to " +
                        client.RemoteEndPoint.ToString());

                    // Signal that the connection has been made.  
                    connectDone.Set();
                    Program.ConnectionData.Connected = true;
                }
                catch (Exception e)
                {
                    Logger.SaveLog("Error: " + e.ToString());
                }
            }

            private static void Receive(Socket client)
            {
                try
                {
                    // Create the state object.  
                    StateObject state = new StateObject();
                    state.workSocket = client;

                    // Begin receiving the data from the remote device.  
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);

                    Logger.SaveLog("Got data from server: " );
                }
                catch (Exception e)
                {
                    Logger.SaveLog("Error: " + e.ToString());
                }
            }

            private static void ReceiveCallback(IAsyncResult ar)
            {
                try
                {
                    // Retrieve the state object and the client socket   
                    // from the asynchronous state object.  
                    StateObject state = (StateObject)ar.AsyncState;
                    Socket client = state.workSocket;

                    // Read data from the remote device.  
                    int bytesRead = client.EndReceive(ar);

                    if (bytesRead > 0)
                    {
                        // There might be more data, so store the data received so far.  
                        state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                        // Get the rest of the data.  
                        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                            new AsyncCallback(ReceiveCallback), state);
                    }
                    else
                    {
                        // All the data has arrived; put it in response.  
                        if (state.sb.Length > 1)
                        {
                            response = state.sb.ToString();
                        }
                        // Signal that all bytes have been received.  
                        receiveDone.Set();
                    }
                }
                catch (Exception e)
                {
                    Logger.SaveLog("Error: " + e.ToString());
                }
            }

            private static void Send(Socket client, String data)
            {
                // Convert the string data to byte data using ASCII encoding.  
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                // Begin sending the data to the remote device.  
                client.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), client);
            }

            private static void SendCallback(IAsyncResult ar)
            {
                string x = "";
                
                try
                {
                    // Retrieve the socket from the state object.  
                    Socket client = (Socket)ar.AsyncState;

                    // Complete sending the data to the remote device.  
                    int bytesSent = client.EndSend(ar);
                    x = "Sent {0} bytes to server. " + bytesSent.ToString();
                    Logger.SaveLog(x);


                    //  Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                    // Signal that all bytes have been sent.  
                    sendDone.Set();
                }
                catch (Exception e)
                {
                    Logger.SaveLog("Error: " + e.ToString());
                }
               // return x;
            }

            public static void Disconnect (Socket client)
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }

            public static bool IsConnected (Socket s)
            {
                 bool part1 = s.Poll(1000, SelectMode.SelectRead);
                 bool part2 = (s.Available == 0);
                 if (part1 && part2)
                    return false;
                 else
                    return true;
            }

            public static void Start()
            {
                StartClient(Program.ConnectionData.IP, Program.ConnectionData.Port);
                
            }

        }
    }
}
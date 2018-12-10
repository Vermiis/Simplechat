using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientTCPApp
{
    enum Command
    {
        //Log into the server
        Login,
        //Logout of the server
        Logout,
        //Send a text message to all the chat clients     
        Message,
        //Get a list of users in the chat room from the server
        List
    }

  
    class Client
    {
    }
}

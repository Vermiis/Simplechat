using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ClientTCPApp
{
    public partial class ClientWindow : Form
    {
        public ClientWindow()
        {
            InitializeComponent();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            Program.ConnectionData.IP = textBoxIP.Text;
            Program.ConnectionData.Port = Int32.Parse(textBoxPort.Text);
            Program.ConnectionData.Nick = textBoxNick.Text;

            try
            {
                ClientCode.AsynchronousClient.Start();

            }
            catch (Exception)
            {

                throw;
            }

            if (Program.ConnectionData.Connected)
            {
                labelConnSt.Text = "Connected";
            }
            
            

        }

        public void X()
        {
            richTextBoxChat.Text = "ASD";
        }
    }
}

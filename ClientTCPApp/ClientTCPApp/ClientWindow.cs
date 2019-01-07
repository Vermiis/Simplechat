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
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
        }

        

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            Program.ConnectionData.IP = textBoxIP.Text;
            
            try 
            {
                Program.ConnectionData.Port = Int32.Parse(textBoxPort.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("wrong addr");
            }
            Program.ConnectionData.Nick = textBoxNick.Text;
            Logger.SaveLog("Logging started...");

            backgroundWorker1.RunWorkerAsync();

            if (Program.ConnectionData.Connected)
            {
                labelConnSt.Text = "Connected";
            }
            
            

        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            // ClientCode.AsynchronousClient.Disconnect();
            labelConnSt.Text = "Disonnected";
            Messages.InternalCommands.Enqueue("DC");
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string message = richTextBoxMessage.Text;
            Messages.messagesToSend.Enqueue(message);
            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ClientCode.AsynchronousClient.Start();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

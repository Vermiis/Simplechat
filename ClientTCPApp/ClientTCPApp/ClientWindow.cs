using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
namespace ClientTCPApp
{
    public partial class ClientWindow : Form
    {
        public ClientWindow()
        {
            InitializeComponent();
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker2.DoWork += backgroundWorker2_DoWork;
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
            backgroundWorker2.RunWorkerAsync();


        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {       
            Messages.InternalCommands.Add("DC");
            
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



        public void HandleTimerElapsedConnectionState(object sender, ElapsedEventArgs e)
        {
            if (Program.ConnectionData.Connected)
            {

                labelConnSt.Invoke((MethodInvoker)delegate
                {
                    labelConnSt.Text = "Connected";
                });
                
               // buttonConnect.Enabled = false;
            }
            else if (!Program.ConnectionData.Connected)
            {
                labelConnSt.Invoke((MethodInvoker)delegate
                {
                    labelConnSt.Text = "Disonnected";
                });
                
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {

            var timer = new System.Timers.Timer(500);
            timer.Elapsed += HandleTimerElapsedConnectionState;
            timer.Enabled = true;


           
        }
    }
}

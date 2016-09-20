using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using WebShare.Server;
using WebShare.Server.Settings;

namespace WebShare
{
    public partial class WebShare : Form
    {
        private HttpServer server;

        public WebShare()
        {
            InitializeComponent();

            int port = 8080;
            server = new HttpServer(@"C:\Users\jelle\Downloads", port);
            server.OnPermissionPrompt += OnpErmissionPrompt;
            server.Start();
            Debug.WriteLine("Starting HTTP server on port " + port);            
        }

        void OnpErmissionPrompt(object sender, PermissionEventArgs e)
        {
            DialogResult incoming = MessageBox.Show("Allow client?\n" + e.Client.Address + "\n" + Dns.GetHostEntry(e.Client.Address).HostName,
                "Incoming connection request", MessageBoxButtons.YesNo);
            if (incoming == DialogResult.Yes)
            {
                server.AllowClient(e.Client);
            }
            else if (incoming == DialogResult.No)
            {
                server.BlockClient(e.Client);
            }
        }
    }
}

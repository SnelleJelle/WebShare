using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
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
            server = new HttpServer(port);
            server.OnPermissionPrompt += onPermissionPrompt;
            server.Start();
            Debug.WriteLine("Starting HTTP server on port " + port);
            fillLists();
        }

        private void onPermissionPrompt(object sender, PermissionEventArgs e)
        {
            string host = "no hostname found for this client";
            try
            {
                host = e.Client.Hostname;
            }
            catch (SocketException se) {}
            DialogResult incoming = MessageBox.Show("Allow client?\n" + e.Client.IP + "\n" + host,
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

        private void fillLists()
        {
            lstSharedFolders.DataSource = null;
            lstBlacklistedClients.DataSource = null;
            lstWhitelistedClients.DataSource = null;
            lstSharedFolders.DataSource = server.SharedFolders;
            lstBlacklistedClients.DataSource = server.Clients.FindAll(Client => Client.Allowed == false);
            lstWhitelistedClients.DataSource = server.Clients.FindAll(Client => Client.Allowed == true);
        }

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            server.AddSharedFolders(new SharedFolder(txtNewFolder.Text));
            fillLists();


        }

        private void WebShare_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}

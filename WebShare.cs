using System.Diagnostics;
using System.Windows.Forms;
using WebShare.Server;
using WebShare.Server.Settings;

namespace WebShare
{
    public partial class WebShare : Form
    {
        public WebShare()
        {
            InitializeComponent();

            int port = 8080;
            HttpServer s = new HttpServer(@"C:\Users\jelle\Downloads", port);
            s.Start();
            Debug.WriteLine("Starting HTTP server on port " + port);
            
        }
    }
}

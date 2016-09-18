using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebShare.Server;

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

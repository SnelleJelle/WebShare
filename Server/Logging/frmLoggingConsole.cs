using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebShare.Server.Logging
{
    public partial class frmLoggingConsole : Form, ILoggerTarget
    {
        public frmLoggingConsole()
        {
            InitializeComponent();
            Show();
        }

        private void rtbLog_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        delegate void SetTextCallback(string text);

        public void Log(string message)
        {
            if (rtbLog.InvokeRequired)
            {
                SetTextCallback stcb = new SetTextCallback(Log);
                Invoke(stcb, new object[] { message });
            }
            else
            {
                rtbLog.AppendText(message + Environment.NewLine);
            }
        }
    }
}

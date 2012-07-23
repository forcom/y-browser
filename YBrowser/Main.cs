using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YWebView;

namespace YBrowser
{
    public partial class Main : Form
    {
        YWebView.YWebView webViewControl = new YWebView.YWebView();

        string StatusMessage = "Ready.";

        public Main()
        {
            InitializeComponent();
            webViewControl.Top = toolBar.Bottom;
            webViewControl.Left = 0;
            webViewControl.Width = this.ClientSize.Width;
            webViewControl.Height = this.ClientSize.Height - toolBar.Height - status.Height;
            webViewControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            webViewControl.OnPageLoaded += new EventHandler(webViewControl_OnPageLoaded);
            webViewControl.OnNavigating += new EventHandler(webViewControl_OnNavigating);
            this.Controls.Add(webViewControl);
        }

        void webViewControl_OnNavigating(object sender, EventArgs e)
        {
            StatusMessage = "Opening Page...";
        }

        void webViewControl_OnPageLoaded(object sender, EventArgs e)
        {
            this.Text = webViewControl.Title + " - YBrowser";
            txtAddress.Text = webViewControl.Url;
            StatusMessage = "Completed.";
            tmrStatus.Enabled = true;
        }

        private void btnNavigate_Click(object sender, EventArgs e)
        {
            webViewControl.Navigate(txtAddress.Text);
        }

        private void txtAddress_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnNavigate_Click(btnNavigate, new EventArgs());
            }
        }

        private void tmrStatus_Tick(object sender, EventArgs e)
        {
            if (webViewControl.StatusText != "")
            {
                lblStatus.Text = webViewControl.StatusText;
            }
            else
            {
                lblStatus.Text = StatusMessage;
            }
        }
    }
}

using System;
using System.Windows.Forms;

namespace Chess.Forms
{
    public partial class FrmAbout : Form
    {
        public FrmAbout()
        {
            InitializeComponent();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            lblProductName.Text = Application.ProductName;
            lblVersion.Text = Application.ProductVersion;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void llbWebSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://" + llbWebSite.Text);
        }
    }
}

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

        private void frmAbout_Load(object sender, System.EventArgs e)
        {
            lblProductName.Text = Application.ProductName;
            lblVersion.Text = Application.ProductVersion;
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void llbWebSite_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://" + llbWebSite.Text);
        }
    }
}

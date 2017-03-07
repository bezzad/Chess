using Chess.Core;

namespace Chess.Forms
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public partial class FrmOptions : System.Windows.Forms.Form
	{
		
		
		public FrmOptions()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}


		private void frmOptions_Load(object sender, System.EventArgs e)
		{
			this.chkShowThinking.Checked = Game.ShowThinking;
			this.chkDisplayMoveAnalysisTree.Checked = Game.DisplayMoveAnalysisTree;
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			Game.ShowThinking = this.chkShowThinking.Checked;
			Game.DisplayMoveAnalysisTree  = this.chkDisplayMoveAnalysisTree.Checked;
			this.Close();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

	}
}

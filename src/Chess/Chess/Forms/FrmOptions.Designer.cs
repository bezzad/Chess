using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Forms
{
    partial class FrmOptions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var resources = new System.Resources.ResourceManager(typeof(FrmOptions));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabCtl = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.grpThinking = new System.Windows.Forms.GroupBox();
            this.chkDisplayMoveAnalysisTree = new System.Windows.Forms.CheckBox();
            this.chkShowThinking = new System.Windows.Forms.CheckBox();
            this.grpClock = new System.Windows.Forms.GroupBox();
            this.lblClockMinutes = new System.Windows.Forms.Label();
            this.txtClockMinutes = new System.Windows.Forms.TextBox();
            this.lblClockMovesIn = new System.Windows.Forms.Label();
            this.txtClockMoves = new System.Windows.Forms.TextBox();
            this.tabCtl.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.grpThinking.SuspendLayout();
            this.grpClock.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(64, 256);
            this.btnOK.Name = "btnOK";
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(144, 256);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tabCtl
            // 
            this.tabCtl.Controls.Add(this.tabGeneral);
            this.tabCtl.Location = new System.Drawing.Point(0, 0);
            this.tabCtl.Name = "tabCtl";
            this.tabCtl.SelectedIndex = 0;
            this.tabCtl.Size = new System.Drawing.Size(224, 248);
            this.tabCtl.TabIndex = 2;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.grpThinking);
            this.tabGeneral.Controls.Add(this.grpClock);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new System.Drawing.Size(216, 222);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            // 
            // grpThinking
            // 
            this.grpThinking.Controls.Add(this.chkDisplayMoveAnalysisTree);
            this.grpThinking.Controls.Add(this.chkShowThinking);
            this.grpThinking.Location = new System.Drawing.Point(8, 8);
            this.grpThinking.Name = "grpThinking";
            this.grpThinking.Size = new System.Drawing.Size(200, 96);
            this.grpThinking.TabIndex = 2;
            this.grpThinking.TabStop = false;
            this.grpThinking.Text = "Thinking";
            // 
            // chkDisplayMoveAnalysisTree
            // 
            this.chkDisplayMoveAnalysisTree.Location = new System.Drawing.Point(16, 54);
            this.chkDisplayMoveAnalysisTree.Name = "chkDisplayMoveAnalysisTree";
            this.chkDisplayMoveAnalysisTree.Size = new System.Drawing.Size(176, 24);
            this.chkDisplayMoveAnalysisTree.TabIndex = 3;
            this.chkDisplayMoveAnalysisTree.Text = "&Display Move Analysis Tree";
            // 
            // chkShowThinking
            // 
            this.chkShowThinking.Location = new System.Drawing.Point(16, 22);
            this.chkShowThinking.Name = "chkShowThinking";
            this.chkShowThinking.TabIndex = 2;
            this.chkShowThinking.Text = "&Show thinking";
            // 
            // grpClock
            // 
            this.grpClock.Controls.Add(this.lblClockMinutes);
            this.grpClock.Controls.Add(this.txtClockMinutes);
            this.grpClock.Controls.Add(this.lblClockMovesIn);
            this.grpClock.Controls.Add(this.txtClockMoves);
            this.grpClock.Location = new System.Drawing.Point(8, 112);
            this.grpClock.Name = "grpClock";
            this.grpClock.Size = new System.Drawing.Size(200, 64);
            this.grpClock.TabIndex = 4;
            this.grpClock.TabStop = false;
            this.grpClock.Text = "Clock";
            // 
            // lblClockMinutes
            // 
            this.lblClockMinutes.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblClockMinutes.Location = new System.Drawing.Point(136, 24);
            this.lblClockMinutes.Name = "lblClockMinutes";
            this.lblClockMinutes.Size = new System.Drawing.Size(48, 16);
            this.lblClockMinutes.TabIndex = 9;
            this.lblClockMinutes.Text = "minutes";
            this.lblClockMinutes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtClockMinutes
            // 
            this.txtClockMinutes.Location = new System.Drawing.Point(104, 24);
            this.txtClockMinutes.MaxLength = 4;
            this.txtClockMinutes.Name = "txtClockMinutes";
            this.txtClockMinutes.Size = new System.Drawing.Size(30, 20);
            this.txtClockMinutes.TabIndex = 8;
            this.txtClockMinutes.Text = "60";
            // 
            // lblClockMovesIn
            // 
            this.lblClockMovesIn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblClockMovesIn.Location = new System.Drawing.Point(48, 24);
            this.lblClockMovesIn.Name = "lblClockMovesIn";
            this.lblClockMovesIn.Size = new System.Drawing.Size(56, 16);
            this.lblClockMovesIn.TabIndex = 7;
            this.lblClockMovesIn.Text = "moves in";
            this.lblClockMovesIn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtClockMoves
            // 
            this.txtClockMoves.Location = new System.Drawing.Point(16, 24);
            this.txtClockMoves.MaxLength = 4;
            this.txtClockMoves.Name = "txtClockMoves";
            this.txtClockMoves.Size = new System.Drawing.Size(30, 20);
            this.txtClockMoves.TabIndex = 6;
            this.txtClockMoves.Text = "120";
            // 
            // frmOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(226, 287);
            this.Controls.Add(this.tabCtl);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.frmOptions_Load);
            this.tabCtl.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.grpThinking.ResumeLayout(false);
            this.grpClock.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabControl tabCtl;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.CheckBox chkShowThinking;
        private System.Windows.Forms.GroupBox grpThinking;
        private System.Windows.Forms.GroupBox grpClock;
        private System.Windows.Forms.TextBox txtClockMoves;
        private System.Windows.Forms.TextBox txtClockMinutes;
        private System.Windows.Forms.Label lblClockMovesIn;
        private System.Windows.Forms.Label lblClockMinutes;
        private System.Windows.Forms.CheckBox chkDisplayMoveAnalysisTree;
    }
}

using System;

namespace Chess.Forms
{
    partial class FrmMain
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
            this.components = new System.ComponentModel.Container();
            var resources = new System.Resources.ResourceManager(typeof(FrmMain));
            this.sbr = new System.Windows.Forms.StatusBar();
            this.imgPieces = new System.Windows.Forms.ImageList(this.components);
            this.mnu = new System.Windows.Forms.MainMenu();
            this.mnuFile = new System.Windows.Forms.MenuItem();
            this.mnuNew = new System.Windows.Forms.MenuItem();
            this.mnuOpen = new System.Windows.Forms.MenuItem();
            this.mnuSave = new System.Windows.Forms.MenuItem();
            this.mnuSep1 = new System.Windows.Forms.MenuItem();
            this.mnuExit = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mnuResumePlay = new System.Windows.Forms.MenuItem();
            this.mnuPausePlay = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnuUndoMove = new System.Windows.Forms.MenuItem();
            this.mnuRedoMove = new System.Windows.Forms.MenuItem();
            this.mnuUndoAllMoves = new System.Windows.Forms.MenuItem();
            this.mnuRedoAllMoves = new System.Windows.Forms.MenuItem();
            this.mnuSep2 = new System.Windows.Forms.MenuItem();
            this.mnuForceComputerMove = new System.Windows.Forms.MenuItem();
            this.mnuOptions = new System.Windows.Forms.MenuItem();
            this.mnuShowThinking = new System.Windows.Forms.MenuItem();
            this.mnuDisplayMoveAnalysisTree = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.mnuMoreOptions = new System.Windows.Forms.MenuItem();
            this.mnuHelp = new System.Windows.Forms.MenuItem();
            this.mnuAbout = new System.Windows.Forms.MenuItem();
            this.tbr = new System.Windows.Forms.ToolBar();
            this.tbrNew = new System.Windows.Forms.ToolBarButton();
            this.tbrOpen = new System.Windows.Forms.ToolBarButton();
            this.tbrSave = new System.Windows.Forms.ToolBarButton();
            this.tbrSep1 = new System.Windows.Forms.ToolBarButton();
            this.tbrUndoAllMoves = new System.Windows.Forms.ToolBarButton();
            this.tbrUndoMove = new System.Windows.Forms.ToolBarButton();
            this.tbrResumePlay = new System.Windows.Forms.ToolBarButton();
            this.tbrPausePlay = new System.Windows.Forms.ToolBarButton();
            this.tbrRedoMove = new System.Windows.Forms.ToolBarButton();
            this.tbrRedoAllMoves = new System.Windows.Forms.ToolBarButton();
            this.tbrSep2 = new System.Windows.Forms.ToolBarButton();
            this.tbrForceComputerMove = new System.Windows.Forms.ToolBarButton();
            this.imgToolMenus = new System.Windows.Forms.ImageList(this.components);
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lblBlackMaterial = new System.Windows.Forms.Label();
            this.lblWhiteMaterial = new System.Windows.Forms.Label();
            this.lblMaterial = new System.Windows.Forms.Label();
            this.lblPlayer = new System.Windows.Forms.Label();
            this.lblBlackClock = new System.Windows.Forms.Label();
            this.lblBlackPosition = new System.Windows.Forms.Label();
            this.lblBlackScore = new System.Windows.Forms.Label();
            this.cboIntellegenceBlack = new System.Windows.Forms.ComboBox();
            this.lblBlackPoints = new System.Windows.Forms.Label();
            this.lblWhiteClock = new System.Windows.Forms.Label();
            this.lblWhitePosition = new System.Windows.Forms.Label();
            this.lblWhiteScore = new System.Windows.Forms.Label();
            this.cboIntellegenceWhite = new System.Windows.Forms.ComboBox();
            this.lblWhitePoints = new System.Windows.Forms.Label();
            this.lblPlayerClocks = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pbr = new System.Windows.Forms.ProgressBar();
            this.lblStage = new System.Windows.Forms.Label();
            this.lvwMoveHistory = new System.Windows.Forms.ListView();
            this.lvcMoveNo = new System.Windows.Forms.ColumnHeader();
            this.lvcTime = new System.Windows.Forms.ColumnHeader();
            this.lvcMove = new System.Windows.Forms.ColumnHeader();
            this.pnlEdging = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.imgTiles = new System.Windows.Forms.ImageList(this.components);
            this.tvwMoves = new System.Windows.Forms.TreeView();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // sbr
            // 
            this.sbr.Location = new System.Drawing.Point(0, 579);
            this.sbr.Name = "sbr";
            this.sbr.Size = new System.Drawing.Size(850, 16);
            this.sbr.SizingGrip = false;
            this.sbr.TabIndex = 7;
            // 
            // imgPieces
            // 
            this.imgPieces.ImageSize = new System.Drawing.Size(48, 48);
            this.imgPieces.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgPieces.ImageStream")));
            this.imgPieces.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // mnu
            // 
            this.mnu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                this.mnuFile,
                                                                                this.menuItem2,
                                                                                this.menuItem1,
                                                                                this.mnuOptions,
                                                                                this.mnuHelp});
            // 
            // mnuFile
            // 
            this.mnuFile.Index = 0;
            this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                    this.mnuNew,
                                                                                    this.mnuOpen,
                                                                                    this.mnuSave,
                                                                                    this.mnuSep1,
                                                                                    this.mnuExit});
            this.mnuFile.Text = "&File";
            // 
            // mnuNew
            // 
            this.mnuNew.Index = 0;
            this.mnuNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            this.mnuNew.Text = "&New";
            this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
            // 
            // mnuOpen
            // 
            this.mnuOpen.Index = 1;
            this.mnuOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.mnuOpen.Text = "&Open...";
            this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
            // 
            // mnuSave
            // 
            this.mnuSave.Index = 2;
            this.mnuSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.mnuSave.Text = "Save &As...";
            this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
            // 
            // mnuSep1
            // 
            this.mnuSep1.Index = 3;
            this.mnuSep1.Text = "-";
            // 
            // mnuExit
            // 
            this.mnuExit.Index = 4;
            this.mnuExit.Text = "E&xit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.mnuResumePlay,
                                                                                      this.mnuPausePlay});
            this.menuItem2.Text = "&Game";
            // 
            // mnuResumePlay
            // 
            this.mnuResumePlay.Index = 0;
            this.mnuResumePlay.Text = "&Resume";
            // 
            // mnuPausePlay
            // 
            this.mnuPausePlay.Enabled = false;
            this.mnuPausePlay.Index = 1;
            this.mnuPausePlay.Text = "&Pause";
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 2;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.mnuUndoMove,
                                                                                      this.mnuRedoMove,
                                                                                      this.mnuUndoAllMoves,
                                                                                      this.mnuRedoAllMoves,
                                                                                      this.mnuSep2,
                                                                                      this.mnuForceComputerMove});
            this.menuItem1.Text = "&Move";
            // 
            // mnuUndoMove
            // 
            this.mnuUndoMove.Index = 0;
            this.mnuUndoMove.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
            this.mnuUndoMove.Text = "&Undo Move";
            this.mnuUndoMove.Click += new System.EventHandler(this.mnuUndoMove_Click);
            // 
            // mnuRedoMove
            // 
            this.mnuRedoMove.Index = 1;
            this.mnuRedoMove.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
            this.mnuRedoMove.Text = "&Redo Move";
            // 
            // mnuUndoAllMoves
            // 
            this.mnuUndoAllMoves.Index = 2;
            this.mnuUndoAllMoves.Text = "U&ndo All Moves";
            // 
            // mnuRedoAllMoves
            // 
            this.mnuRedoAllMoves.Index = 3;
            this.mnuRedoAllMoves.Text = "Re&do All Moves";
            // 
            // mnuSep2
            // 
            this.mnuSep2.Index = 4;
            this.mnuSep2.Text = "-";
            // 
            // mnuForceComputerMove
            // 
            this.mnuForceComputerMove.Index = 5;
            this.mnuForceComputerMove.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
            this.mnuForceComputerMove.Text = "&Force Computer Move";
            this.mnuForceComputerMove.Click += new System.EventHandler(this.mnuForceComputerMove_Click);
            // 
            // mnuOptions
            // 
            this.mnuOptions.Index = 3;
            this.mnuOptions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                       this.mnuShowThinking,
                                                                                       this.mnuDisplayMoveAnalysisTree,
                                                                                       this.menuItem4,
                                                                                       this.mnuMoreOptions});
            this.mnuOptions.Text = "&Options";
            // 
            // mnuShowThinking
            // 
            this.mnuShowThinking.Index = 0;
            this.mnuShowThinking.Text = "&Show Thinking";
            this.mnuShowThinking.Click += new System.EventHandler(this.mnuShowThinking_Click);
            // 
            // mnuDisplayMoveAnalysisTree
            // 
            this.mnuDisplayMoveAnalysisTree.Index = 1;
            this.mnuDisplayMoveAnalysisTree.Text = "&Display Move Analysis Tree";
            this.mnuDisplayMoveAnalysisTree.Click += new System.EventHandler(this.mnuDisplayMoveAnalysisTree_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // mnuMoreOptions
            // 
            this.mnuMoreOptions.Index = 3;
            this.mnuMoreOptions.Text = "More Options...";
            this.mnuMoreOptions.Click += new System.EventHandler(this.mnuMoreOptions_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.Index = 4;
            this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                    this.mnuAbout});
            this.mnuHelp.Text = "&Help";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Index = 0;
            this.mnuAbout.Text = "&About SharpChess";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // tbr
            // 
            this.tbr.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.tbr.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
                                                                                   this.tbrNew,
                                                                                   this.tbrOpen,
                                                                                   this.tbrSave,
                                                                                   this.tbrSep1,
                                                                                   this.tbrUndoAllMoves,
                                                                                   this.tbrUndoMove,
                                                                                   this.tbrResumePlay,
                                                                                   this.tbrPausePlay,
                                                                                   this.tbrRedoMove,
                                                                                   this.tbrRedoAllMoves,
                                                                                   this.tbrSep2,
                                                                                   this.tbrForceComputerMove});
            this.tbr.DropDownArrows = true;
            this.tbr.ImageList = this.imgToolMenus;
            this.tbr.Location = new System.Drawing.Point(0, 0);
            this.tbr.Name = "tbr";
            this.tbr.ShowToolTips = true;
            this.tbr.Size = new System.Drawing.Size(850, 28);
            this.tbr.TabIndex = 32;
            this.tbr.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.tbr.Wrappable = false;
            this.tbr.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbr_ButtonClick);
            // 
            // tbrNew
            // 
            this.tbrNew.ImageIndex = 0;
            this.tbrNew.Tag = "New";
            this.tbrNew.ToolTipText = "Start a new chess game";
            // 
            // tbrOpen
            // 
            this.tbrOpen.ImageIndex = 1;
            this.tbrOpen.Tag = "Open";
            this.tbrOpen.ToolTipText = "Open a saved chess game";
            // 
            // tbrSave
            // 
            this.tbrSave.ImageIndex = 2;
            this.tbrSave.Tag = "Save";
            this.tbrSave.ToolTipText = "Save the current chess game";
            // 
            // tbrSep1
            // 
            this.tbrSep1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbrUndoAllMoves
            // 
            this.tbrUndoAllMoves.ImageIndex = 6;
            this.tbrUndoAllMoves.Tag = "UndoAllMoves";
            this.tbrUndoAllMoves.ToolTipText = "Undo all moves played so far";
            // 
            // tbrUndoMove
            // 
            this.tbrUndoMove.ImageIndex = 4;
            this.tbrUndoMove.Tag = "UndoMove";
            this.tbrUndoMove.ToolTipText = "Undo the last move";
            // 
            // tbrResumePlay
            // 
            this.tbrResumePlay.ImageIndex = 8;
            this.tbrResumePlay.Tag = "ResumePlay";
            this.tbrResumePlay.ToolTipText = "Resume play";
            // 
            // tbrPausePlay
            // 
            this.tbrPausePlay.Enabled = false;
            this.tbrPausePlay.ImageIndex = 9;
            this.tbrPausePlay.Tag = "PausePlay";
            this.tbrPausePlay.ToolTipText = "Pause play";
            // 
            // tbrRedoMove
            // 
            this.tbrRedoMove.ImageIndex = 5;
            this.tbrRedoMove.Tag = "RedoMove";
            this.tbrRedoMove.ToolTipText = "Redo move";
            // 
            // tbrRedoAllMoves
            // 
            this.tbrRedoAllMoves.ImageIndex = 7;
            this.tbrRedoAllMoves.Tag = "RedoAllMoves";
            this.tbrRedoAllMoves.ToolTipText = "Redo all moves";
            // 
            // tbrSep2
            // 
            this.tbrSep2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbrForceComputerMove
            // 
            this.tbrForceComputerMove.ImageIndex = 3;
            this.tbrForceComputerMove.Tag = "ForceComputerMove";
            this.tbrForceComputerMove.Text = "Force Computer Move";
            this.tbrForceComputerMove.ToolTipText = "Make the computer play the next move";
            // 
            // imgToolMenus
            // 
            this.imgToolMenus.ImageSize = new System.Drawing.Size(16, 16);
            this.imgToolMenus.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgToolMenus.ImageStream")));
            this.imgToolMenus.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.Transparent;
            this.pnlMain.Controls.Add(this.label5);
            this.pnlMain.Controls.Add(this.label3);
            this.pnlMain.Controls.Add(this.lblBlackMaterial);
            this.pnlMain.Controls.Add(this.lblWhiteMaterial);
            this.pnlMain.Controls.Add(this.lblMaterial);
            this.pnlMain.Controls.Add(this.lblPlayer);
            this.pnlMain.Controls.Add(this.lblBlackClock);
            this.pnlMain.Controls.Add(this.lblBlackPosition);
            this.pnlMain.Controls.Add(this.lblBlackScore);
            this.pnlMain.Controls.Add(this.cboIntellegenceBlack);
            this.pnlMain.Controls.Add(this.lblBlackPoints);
            this.pnlMain.Controls.Add(this.lblWhiteClock);
            this.pnlMain.Controls.Add(this.lblWhitePosition);
            this.pnlMain.Controls.Add(this.lblWhiteScore);
            this.pnlMain.Controls.Add(this.cboIntellegenceWhite);
            this.pnlMain.Controls.Add(this.lblWhitePoints);
            this.pnlMain.Controls.Add(this.lblPlayerClocks);
            this.pnlMain.Controls.Add(this.label2);
            this.pnlMain.Controls.Add(this.label4);
            this.pnlMain.Controls.Add(this.label1);
            this.pnlMain.Controls.Add(this.panel1);
            this.pnlMain.Controls.Add(this.panel3);
            this.pnlMain.Controls.Add(this.panel4);
            this.pnlMain.Controls.Add(this.panel2);
            this.pnlMain.Controls.Add(this.pbr);
            this.pnlMain.Controls.Add(this.lblStage);
            this.pnlMain.Controls.Add(this.lvwMoveHistory);
            this.pnlMain.Controls.Add(this.pnlEdging);
            this.pnlMain.Location = new System.Drawing.Point(0, 26);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(743, 554);
            this.pnlMain.TabIndex = 33;
            // 
            // lblBlackMaterial
            // 
            this.lblBlackMaterial.BackColor = System.Drawing.Color.Transparent;
            this.lblBlackMaterial.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBlackMaterial.CausesValidation = false;
            this.lblBlackMaterial.Location = new System.Drawing.Point(608, 168);
            this.lblBlackMaterial.Name = "lblBlackMaterial";
            this.lblBlackMaterial.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBlackMaterial.Size = new System.Drawing.Size(96, 23);
            this.lblBlackMaterial.TabIndex = 134;
            this.lblBlackMaterial.Text = "0";
            this.lblBlackMaterial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWhiteMaterial
            // 
            this.lblWhiteMaterial.BackColor = System.Drawing.Color.Transparent;
            this.lblWhiteMaterial.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWhiteMaterial.CausesValidation = false;
            this.lblWhiteMaterial.Location = new System.Drawing.Point(504, 168);
            this.lblWhiteMaterial.Name = "lblWhiteMaterial";
            this.lblWhiteMaterial.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblWhiteMaterial.Size = new System.Drawing.Size(96, 23);
            this.lblWhiteMaterial.TabIndex = 133;
            this.lblWhiteMaterial.Text = "0";
            this.lblWhiteMaterial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMaterial
            // 
            this.lblMaterial.BackColor = System.Drawing.Color.Transparent;
            this.lblMaterial.Location = new System.Drawing.Point(448, 168);
            this.lblMaterial.Name = "lblMaterial";
            this.lblMaterial.Size = new System.Drawing.Size(48, 24);
            this.lblMaterial.TabIndex = 132;
            this.lblMaterial.Text = "Material";
            this.lblMaterial.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPlayer
            // 
            this.lblPlayer.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayer.Location = new System.Drawing.Point(448, 32);
            this.lblPlayer.Name = "lblPlayer";
            this.lblPlayer.Size = new System.Drawing.Size(48, 24);
            this.lblPlayer.TabIndex = 131;
            this.lblPlayer.Text = "Player";
            this.lblPlayer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBlackClock
            // 
            this.lblBlackClock.BackColor = System.Drawing.Color.Transparent;
            this.lblBlackClock.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBlackClock.CausesValidation = false;
            this.lblBlackClock.Location = new System.Drawing.Point(608, 64);
            this.lblBlackClock.Name = "lblBlackClock";
            this.lblBlackClock.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBlackClock.Size = new System.Drawing.Size(96, 23);
            this.lblBlackClock.TabIndex = 130;
            this.lblBlackClock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBlackPosition
            // 
            this.lblBlackPosition.BackColor = System.Drawing.Color.Transparent;
            this.lblBlackPosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBlackPosition.CausesValidation = false;
            this.lblBlackPosition.Location = new System.Drawing.Point(608, 144);
            this.lblBlackPosition.Name = "lblBlackPosition";
            this.lblBlackPosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBlackPosition.Size = new System.Drawing.Size(96, 23);
            this.lblBlackPosition.TabIndex = 128;
            this.lblBlackPosition.Text = "0";
            this.lblBlackPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBlackScore
            // 
            this.lblBlackScore.BackColor = System.Drawing.Color.Transparent;
            this.lblBlackScore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBlackScore.CausesValidation = false;
            this.lblBlackScore.Location = new System.Drawing.Point(608, 96);
            this.lblBlackScore.Name = "lblBlackScore";
            this.lblBlackScore.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBlackScore.Size = new System.Drawing.Size(96, 23);
            this.lblBlackScore.TabIndex = 127;
            this.lblBlackScore.Text = "0";
            this.lblBlackScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboIntellegenceBlack
            // 
            this.cboIntellegenceBlack.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIntellegenceBlack.Items.AddRange(new object[] {
                                                                      "Human",
                                                                      "Computer"});
            this.cboIntellegenceBlack.Location = new System.Drawing.Point(608, 32);
            this.cboIntellegenceBlack.Name = "cboIntellegenceBlack";
            this.cboIntellegenceBlack.Size = new System.Drawing.Size(96, 21);
            this.cboIntellegenceBlack.TabIndex = 126;
            this.cboIntellegenceBlack.SelectedIndexChanged += new System.EventHandler(this.cboIntellegenceBlack_SelectedIndexChanged);
            // 
            // lblBlackPoints
            // 
            this.lblBlackPoints.BackColor = System.Drawing.Color.Transparent;
            this.lblBlackPoints.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBlackPoints.CausesValidation = false;
            this.lblBlackPoints.Location = new System.Drawing.Point(608, 120);
            this.lblBlackPoints.Name = "lblBlackPoints";
            this.lblBlackPoints.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBlackPoints.Size = new System.Drawing.Size(96, 23);
            this.lblBlackPoints.TabIndex = 125;
            this.lblBlackPoints.Text = "0";
            this.lblBlackPoints.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWhiteClock
            // 
            this.lblWhiteClock.BackColor = System.Drawing.Color.Transparent;
            this.lblWhiteClock.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWhiteClock.CausesValidation = false;
            this.lblWhiteClock.Location = new System.Drawing.Point(504, 64);
            this.lblWhiteClock.Name = "lblWhiteClock";
            this.lblWhiteClock.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblWhiteClock.Size = new System.Drawing.Size(96, 23);
            this.lblWhiteClock.TabIndex = 124;
            this.lblWhiteClock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWhitePosition
            // 
            this.lblWhitePosition.BackColor = System.Drawing.Color.Transparent;
            this.lblWhitePosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWhitePosition.CausesValidation = false;
            this.lblWhitePosition.Location = new System.Drawing.Point(504, 144);
            this.lblWhitePosition.Name = "lblWhitePosition";
            this.lblWhitePosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblWhitePosition.Size = new System.Drawing.Size(96, 23);
            this.lblWhitePosition.TabIndex = 122;
            this.lblWhitePosition.Text = "0";
            this.lblWhitePosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWhiteScore
            // 
            this.lblWhiteScore.BackColor = System.Drawing.Color.Transparent;
            this.lblWhiteScore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWhiteScore.CausesValidation = false;
            this.lblWhiteScore.Location = new System.Drawing.Point(504, 96);
            this.lblWhiteScore.Name = "lblWhiteScore";
            this.lblWhiteScore.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblWhiteScore.Size = new System.Drawing.Size(96, 23);
            this.lblWhiteScore.TabIndex = 121;
            this.lblWhiteScore.Text = "0";
            this.lblWhiteScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboIntellegenceWhite
            // 
            this.cboIntellegenceWhite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIntellegenceWhite.Items.AddRange(new object[] {
                                                                      "Human",
                                                                      "Computer"});
            this.cboIntellegenceWhite.Location = new System.Drawing.Point(504, 32);
            this.cboIntellegenceWhite.Name = "cboIntellegenceWhite";
            this.cboIntellegenceWhite.Size = new System.Drawing.Size(96, 21);
            this.cboIntellegenceWhite.TabIndex = 120;
            this.cboIntellegenceWhite.SelectedIndexChanged += new System.EventHandler(this.cboIntellegenceWhite_SelectedIndexChanged);
            // 
            // lblWhitePoints
            // 
            this.lblWhitePoints.BackColor = System.Drawing.Color.Transparent;
            this.lblWhitePoints.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWhitePoints.CausesValidation = false;
            this.lblWhitePoints.Location = new System.Drawing.Point(504, 120);
            this.lblWhitePoints.Name = "lblWhitePoints";
            this.lblWhitePoints.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblWhitePoints.Size = new System.Drawing.Size(96, 23);
            this.lblWhitePoints.TabIndex = 119;
            this.lblWhitePoints.Text = "0";
            this.lblWhitePoints.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPlayerClocks
            // 
            this.lblPlayerClocks.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayerClocks.Location = new System.Drawing.Point(448, 64);
            this.lblPlayerClocks.Name = "lblPlayerClocks";
            this.lblPlayerClocks.Size = new System.Drawing.Size(48, 24);
            this.lblPlayerClocks.TabIndex = 118;
            this.lblPlayerClocks.Text = "Clock";
            this.lblPlayerClocks.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(448, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 24);
            this.label2.TabIndex = 116;
            this.label2.Text = "Position";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(456, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 24);
            this.label4.TabIndex = 115;
            this.label4.Text = "Score";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(456, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 24);
            this.label1.TabIndex = 114;
            this.label1.Text = "Points";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(32, 398);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(390, 8);
            this.panel1.TabIndex = 55;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Black;
            this.panel3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel3.BackgroundImage")));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Location = new System.Drawing.Point(414, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(8, 400);
            this.panel3.TabIndex = 57;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Black;
            this.panel4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel4.BackgroundImage")));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Location = new System.Drawing.Point(24, 6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(390, 8);
            this.panel4.TabIndex = 58;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(24, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(8, 398);
            this.panel2.TabIndex = 56;
            // 
            // pbr
            // 
            this.pbr.Location = new System.Drawing.Point(0, 530);
            this.pbr.Name = "pbr";
            this.pbr.Size = new System.Drawing.Size(736, 23);
            this.pbr.TabIndex = 54;
            // 
            // lblStage
            // 
            this.lblStage.BackColor = System.Drawing.Color.Transparent;
            this.lblStage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblStage.CausesValidation = false;
            this.lblStage.Location = new System.Drawing.Point(424, 408);
            this.lblStage.Name = "lblStage";
            this.lblStage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblStage.Size = new System.Drawing.Size(312, 23);
            this.lblStage.TabIndex = 50;
            this.lblStage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lvwMoveHistory
            // 
            this.lvwMoveHistory.BackColor = System.Drawing.SystemColors.Control;
            this.lvwMoveHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                             this.lvcMoveNo,
                                                                                             this.lvcTime,
                                                                                             this.lvcMove});
            this.lvwMoveHistory.Location = new System.Drawing.Point(424, 200);
            this.lvwMoveHistory.Name = "lvwMoveHistory";
            this.lvwMoveHistory.Size = new System.Drawing.Size(312, 200);
            this.lvwMoveHistory.TabIndex = 39;
            this.lvwMoveHistory.View = System.Windows.Forms.View.Details;
            // 
            // lvcMoveNo
            // 
            this.lvcMoveNo.Text = "#";
            this.lvcMoveNo.Width = 19;
            // 
            // lvcTime
            // 
            this.lvcTime.Text = "Time";
            this.lvcTime.Width = 56;
            // 
            // lvcMove
            // 
            this.lvcMove.Text = "Move";
            this.lvcMove.Width = 215;
            // 
            // pnlEdging
            // 
            this.pnlEdging.BackColor = System.Drawing.SystemColors.Control;
            this.pnlEdging.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlEdging.Location = new System.Drawing.Point(28, 12);
            this.pnlEdging.Name = "pnlEdging";
            this.pnlEdging.Size = new System.Drawing.Size(392, 392);
            this.pnlEdging.TabIndex = 35;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Location = new System.Drawing.Point(0, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(800, 8);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            // 
            // imgTiles
            // 
            this.imgTiles.ImageSize = new System.Drawing.Size(48, 48);
            this.imgTiles.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgTiles.ImageStream")));
            this.imgTiles.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tvwMoves
            // 
            this.tvwMoves.BackColor = System.Drawing.SystemColors.Control;
            this.tvwMoves.ImageIndex = -1;
            this.tvwMoves.Location = new System.Drawing.Point(744, 8);
            this.tvwMoves.Name = "tvwMoves";
            this.tvwMoves.SelectedImageIndex = -1;
            this.tvwMoves.Size = new System.Drawing.Size(440, 584);
            this.tvwMoves.TabIndex = 63;
            this.tvwMoves.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvwMoves_AfterExpand);
            // 
            // timer
            // 
            this.timer.Interval = 200;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label3.Location = new System.Drawing.Point(504, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 23);
            this.label3.TabIndex = 135;
            this.label3.Text = "White";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label5.Location = new System.Drawing.Point(608, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 23);
            this.label5.TabIndex = 136;
            this.label5.Text = "Black";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(850, 595);
            this.Controls.Add(this.tvwMoves);
            this.Controls.Add(this.tbr);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.sbr);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Menu = this.mnu;
            this.Name = "frmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SharpChess";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.StatusBar sbr;
        private System.Windows.Forms.ImageList imgPieces;
        private System.Windows.Forms.MainMenu mnu;
        private System.Windows.Forms.MenuItem mnuFile;
        private System.Windows.Forms.MenuItem mnuExit;
        private System.Windows.Forms.MenuItem mnuHelp;
        private System.Windows.Forms.MenuItem mnuAbout;
        private System.Windows.Forms.MenuItem mnuNew;
        private System.Windows.Forms.MenuItem mnuSave;
        private System.Windows.Forms.MenuItem mnuOpen;
        private System.Windows.Forms.MenuItem mnuSep1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem mnuUndoMove;
        private System.Windows.Forms.MenuItem mnuForceComputerMove;
        private System.Windows.Forms.MenuItem mnuSep2;
        private System.Windows.Forms.ToolBar tbr;
        private System.Windows.Forms.ToolBarButton tbrNew;
        private System.Windows.Forms.ImageList imgToolMenus;
        private System.Windows.Forms.ToolBarButton tbrOpen;
        private System.Windows.Forms.ToolBarButton tbrSave;
        private System.Windows.Forms.ToolBarButton tbrSep1;
        private System.Windows.Forms.ToolBarButton tbrUndoMove;
        private System.Windows.Forms.ToolBarButton tbrSep2;
        private System.Windows.Forms.ToolBarButton tbrForceComputerMove;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblStage;
        private System.Windows.Forms.ListView lvwMoveHistory;
        private System.Windows.Forms.ColumnHeader lvcMoveNo;
        private System.Windows.Forms.ProgressBar pbr;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel pnlEdging;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ImageList imgTiles;
        private System.Windows.Forms.TreeView tvwMoves;
        private System.Windows.Forms.MenuItem mnuRedoMove;
        private System.Windows.Forms.MenuItem mnuUndoAllMoves;
        private System.Windows.Forms.ToolBarButton tbrRedoMove;
        private System.Windows.Forms.ToolBarButton tbrUndoAllMoves;
        private System.Windows.Forms.ToolBarButton tbrRedoAllMoves;
        private System.Windows.Forms.MenuItem mnuRedoAllMoves;
        private System.Windows.Forms.MenuItem mnuOptions;
        private System.Windows.Forms.MenuItem mnuShowThinking;
        private System.Windows.Forms.MenuItem mnuDisplayMoveAnalysisTree;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem mnuMoreOptions;
        private System.Windows.Forms.Label lblBlackClock;
        private System.Windows.Forms.Label lblBlackPosition;
        private System.Windows.Forms.Label lblBlackScore;
        private System.Windows.Forms.ComboBox cboIntellegenceBlack;
        private System.Windows.Forms.Label lblBlackPoints;
        private System.Windows.Forms.Label lblWhiteClock;
        private System.Windows.Forms.Label lblWhitePosition;
        private System.Windows.Forms.Label lblWhiteScore;
        private System.Windows.Forms.ComboBox cboIntellegenceWhite;
        private System.Windows.Forms.Label lblWhitePoints;
        private System.Windows.Forms.Label lblPlayerClocks;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPlayer;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.ToolBarButton tbrResumePlay;
        private System.Windows.Forms.ToolBarButton tbrPausePlay;
        private System.Windows.Forms.MenuItem mnuResumePlay;
        private System.Windows.Forms.MenuItem mnuPausePlay;
        private System.Windows.Forms.ColumnHeader lvcTime;
        private System.Windows.Forms.Label lblWhiteMaterial;
        private System.Windows.Forms.Label lblMaterial;
        private System.Windows.Forms.Label lblBlackMaterial;
        private System.Windows.Forms.ColumnHeader lvcMove;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
    }
}

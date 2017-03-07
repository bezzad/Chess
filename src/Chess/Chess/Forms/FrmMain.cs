using System;
using System.Drawing;
using System.Windows.Forms;
using Chess.Core;

namespace Chess.Forms
{
	/// <summary>
	/// Summary description for frmMain.
	/// </summary>

	public partial class FrmMain : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		
		const int SQUARE_SIZE = 48;
		const int INTELLEGENCE_HUMAN = 0;
		const int INTELLEGENCE_COMPUTER = 1;

		private System.Drawing.Color BOARD_SQUARE_COLOUR_WHITE = System.Drawing.Color.FromArgb(229,197,105);
		private System.Drawing.Color BOARD_SQUARE_COLOUR_BLACK = System.Drawing.Color.FromArgb(189,117,53);

		Square m_squareFrom = null;
		Moves m_movesPossible = new Moves();
		private Game m_game;
		PictureBox[,] m_picSquares;
		PictureBox[] m_picWhitesCaptures;
		PictureBox[] m_picBlacksCaptures;
        Square m_squareLastFrom = null;
        Square m_squareLastTo = null;

        public FrmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		




		public void MoveConsidered()
		{
			RenderStatus();

			if (m_squareLastFrom != null)
			{
				m_picSquares[m_squareLastFrom.File, m_squareLastFrom.Rank].BackColor = (m_squareLastFrom.Colour==Square.enmColour.White ? BOARD_SQUARE_COLOUR_WHITE : BOARD_SQUARE_COLOUR_BLACK);
				m_picSquares[m_squareLastFrom.File, m_squareLastFrom.Rank].Refresh();
			}
			if (Game.ShowThinking)
			{
				m_squareLastFrom = Game.PlayerToPlay.CurrentMove.From;
				m_picSquares[m_squareLastFrom.File, m_squareLastFrom.Rank].BackColor=System.Drawing.Color.Yellow;
				m_picSquares[m_squareLastFrom.File, m_squareLastFrom.Rank].Refresh();
			}

			if (m_squareLastTo != null)
			{
				m_picSquares[m_squareLastTo.File, m_squareLastTo.Rank].BackColor = (m_squareLastTo.Colour==Square.enmColour.White ? BOARD_SQUARE_COLOUR_WHITE : BOARD_SQUARE_COLOUR_BLACK);
				m_picSquares[m_squareLastTo.File, m_squareLastTo.Rank].Refresh();
			}
			if (Game.ShowThinking)
			{
				m_squareLastTo = Game.PlayerToPlay.CurrentMove.To;
				m_picSquares[m_squareLastTo.File, m_squareLastTo.Rank].BackColor=System.Drawing.Color.Yellow;
				m_picSquares[m_squareLastTo.File, m_squareLastTo.Rank].Refresh();
			}

			SetFormState();

			Application.DoEvents();
		}	

		private void frmMain_Load(object sender, System.EventArgs e)
		{
			m_game = new Game();

			Game.PlayerWhite.MoveConsidered += new delegateGameEvent(MoveConsidered);
			Game.PlayerBlack.MoveConsidered += new delegateGameEvent(MoveConsidered);
			
			cboIntellegenceWhite.SelectedIndex = Game.PlayerWhite.Intellegence==Player.enmIntellegence.Human ? INTELLEGENCE_HUMAN : INTELLEGENCE_COMPUTER;
			cboIntellegenceBlack.SelectedIndex = Game.PlayerBlack.Intellegence==Player.enmIntellegence.Human ? INTELLEGENCE_HUMAN : INTELLEGENCE_COMPUTER;


			CreateBoard();
			RenderBoard();
			RenderClocks();
			this.Text = Application.ProductName + " - " + Game.FileName;
			AssignMenuChecks();
			SizeMainForm();

			timer.Start();

			SetFormState();
		}

		private void btnGo_Click(object sender, System.EventArgs e)
		{
			MakeNextComputerMove();
		}


		private void RenderClocks()
		{
			lblWhiteClock.Text = Game.PlayerWhite.Clock.TimeElapsed.Hours.ToString().PadLeft(2,'0') + ":" + Game.PlayerWhite.Clock.TimeElapsed.Minutes.ToString().PadLeft(2,'0') + ":" + Game.PlayerWhite.Clock.TimeElapsed.Seconds.ToString().PadLeft(2,'0');
			lblBlackClock.Text = Game.PlayerBlack.Clock.TimeElapsed.Hours.ToString().PadLeft(2,'0') + ":" + Game.PlayerBlack.Clock.TimeElapsed.Minutes.ToString().PadLeft(2,'0') + ":" + Game.PlayerBlack.Clock.TimeElapsed.Seconds.ToString().PadLeft(2,'0');
		}

		private void RenderBoard()
		{
			Square square;

			for (int intOrdinal=0; intOrdinal<Board.SQUARE_COUNT; intOrdinal++)
			{
				square = Board.GetSquare(intOrdinal);
				
				if (square!=null)
				{
					if (square.Colour == Square.enmColour.White)
					{
						m_picSquares[square.File, square.Rank].BackColor = BOARD_SQUARE_COLOUR_WHITE;
					}
					else
					{
						m_picSquares[square.File, square.Rank].BackColor = BOARD_SQUARE_COLOUR_BLACK;
					}

					if (square.Piece == null)
					{
						m_picSquares[square.File, square.Rank].Image = null;
					}
					else
					{
						m_picSquares[square.File, square.Rank].Image = GetPieceImage(square.Piece);
					}

					m_picSquares[square.File, square.Rank].BorderStyle = System.Windows.Forms.BorderStyle.None;
				}
			}

			// Render selection highlights
			if (m_squareFrom!=null)
			{
				m_picSquares[m_squareFrom.File, m_squareFrom.Rank].BackColor = Color.Blue;
				foreach (Move move in m_movesPossible)
				{
					m_picSquares[move.To.File, move.To.Rank].BackColor = Color.LightBlue;
				}
			}

			// Render Last Move highlights
			if (Game.MoveHistory.Count>0)
			{
				m_picSquares[Game.MoveHistory.Item(Game.MoveHistory.Count-1).From.File, Game.MoveHistory.Item(Game.MoveHistory.Count-1).From.Rank].BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
				m_picSquares[Game.MoveHistory.Item(Game.MoveHistory.Count-1).To.File  , Game.MoveHistory.Item(Game.MoveHistory.Count-1).To.Rank  ].BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			}

			// Render pieces taken
			for (int intIndex=0; intIndex<15; intIndex++)
			{
				m_picWhitesCaptures[intIndex].Image = null;
				m_picBlacksCaptures[intIndex].Image = null;
			}
			for (int intIndex=0; intIndex<Game.PlayerWhite.CapturedEnemyPieces.Count; intIndex++)
			{
				m_picWhitesCaptures[intIndex].Image = GetPieceImage(Game.PlayerWhite.CapturedEnemyPieces.Item(intIndex));
			}
			for (int intIndex=0; intIndex<Game.PlayerBlack.CapturedEnemyPieces.Count; intIndex++)
			{
				m_picBlacksCaptures[intIndex].Image = GetPieceImage(Game.PlayerBlack.CapturedEnemyPieces.Item(intIndex));
			}

			// Render player status
			if (Game.PlayerToPlay == Game.PlayerWhite)
			{
				lblWhiteScore.BackColor = Game.PlayerWhite.Status==Player.enmStatus.InCheckMate ? Color.Red : (Game.PlayerWhite.IsInCheck ? Color.Orange: Color.FromName(System.Drawing.KnownColor.Control.ToString()) );
				lblBlackScore.BackColor = Color.FromName(System.Drawing.KnownColor.Control.ToString());
				lblWhiteClock.BackColor = Color.LightGray;
				lblBlackClock.BackColor = Color.FromName(System.Drawing.KnownColor.Control.ToString());
			}
			else
			{
				lblWhiteScore.BackColor = Color.FromName(System.Drawing.KnownColor.Control.ToString());
				lblBlackScore.BackColor = Game.PlayerBlack.Status==Player.enmStatus.InCheckMate ? Color.Red : (Game.PlayerBlack.IsInCheck ? Color.Orange : Color.FromName(System.Drawing.KnownColor.Control.ToString()) );
				lblBlackClock.BackColor = Color.LightGray;
				lblWhiteClock.BackColor = Color.FromName(System.Drawing.KnownColor.Control.ToString());
			}
			// Set form state
			lblWhiteMaterial.Text = (Game.PlayerWhite.MaterialBasicValue+Game.PlayerWhite.PawnsInPlay).ToString();
			lblBlackMaterial.Text = (Game.PlayerBlack.MaterialBasicValue+Game.PlayerBlack.PawnsInPlay).ToString();

			lblWhitePosition.Text = Game.PlayerWhite.PositionPoints.ToString();
			lblBlackPosition.Text = Game.PlayerBlack.PositionPoints.ToString();

			lblWhitePoints.Text = Game.PlayerWhite.Points.ToString();
			lblBlackPoints.Text = Game.PlayerBlack.Points.ToString();

			lblWhiteScore.Text = Game.PlayerWhite.Score.ToString();
			lblBlackScore.Text = Game.PlayerBlack.Score.ToString();

			lblStage.Text = Game.Stage.ToString() + " Game - " + Game.PlayerToPlay.Colour.ToString() + " to play";
//			lblStage.Text = "A: " + Board.HashCodeA.ToString() + "     B: " + Board.HashCodeB.ToString();

			// Update move history
			while (lvwMoveHistory.Items.Count < Game.MoveHistory.Count)
			{
				AddMoveToHistory(Game.MoveHistory.Item(lvwMoveHistory.Items.Count));
			}
			while (lvwMoveHistory.Items.Count > Game.MoveHistory.Count)
			{
				RemoveLastHistoryItem();
			}

			SetFormState();

			this.Refresh();
		}

		private Image GetPieceImage(Piece piece)
		{
			return imgPieces.Images[piece.ImageIndex];
		}

		private void RenderStatus()
		{
			Player playerToPlay = Game.PlayerToPlay;

			sbr.Text = (playerToPlay.SearchDepth==0) ? "" : 
				(
					"Ply: " + playerToPlay.SearchDepth.ToString() + "/" + playerToPlay.MaxSearchDepth.ToString() 
					+ ". Move: " + playerToPlay.CurrentMoveNo.ToString() + " / " + playerToPlay.TotalMoves.ToString() 
					+ ". Secs: " + ((int)(playerToPlay.ThinkingTimeRemaining.TotalSeconds)).ToString() + "/" + ((int)playerToPlay.ThinkingTimeAllotted.TotalSeconds).ToString() 
					+ (!Game.ShowThinking ? "" : (". Best: " + ((playerToPlay.BestMove==null) ? "" : playerToPlay.BestMove.Piece.Name.ToString() + " " + playerToPlay.BestMove.From.Name+"-"+playerToPlay.BestMove.To.Name + " " + playerToPlay.BestMove.Description + " Score: " + playerToPlay.BestMove.Score)))
					+ " Positions: " + playerToPlay.PositionsSearched + " MaxQDepth: " + playerToPlay.MaxQuiesDepth
					+ " P:" + HashTable.Probes
					+ " H:" + HashTable.Hits
					+ " W:" + HashTable.Writes 
					+ " C:" + HashTable.Collisions
					+ " O:" + HashTable.Overwrites
				);

			pbr.Maximum = Math.Max(playerToPlay.TotalMoves, playerToPlay.CurrentMoveNo);
			pbr.Value = playerToPlay.CurrentMoveNo;
			sbr.Refresh();
			pbr.Refresh();
		}

		private void SetFormState()
		{
			mnuNew.Enabled = !Game.PlayerToPlay.IsThinking;
			mnuOpen.Enabled = !Game.PlayerToPlay.IsThinking;
			mnuSave.Enabled = !Game.PlayerToPlay.IsThinking;
			mnuUndoMove.Enabled = (!Game.PlayerToPlay.IsThinking && Game.MoveHistory.Count>0);
			mnuRedoMove.Enabled = (!Game.PlayerToPlay.IsThinking && Game.MoveRedoList.Count>0);
			mnuUndoAllMoves.Enabled = mnuUndoMove.Enabled;
			mnuRedoAllMoves.Enabled = mnuRedoMove.Enabled;
			mnuForceComputerMove.Enabled = (!Game.PlayerToPlay.IsThinking && Game.PlayerToPlay.CanMove);
			mnuResumePlay.Enabled = (!Game.PlayerToPlay.IsThinking && !Game.PlayerToPlay.Clock.IsTicking); 
			mnuPausePlay.Enabled = (!Game.PlayerToPlay.IsThinking && Game.PlayerToPlay.Clock.IsTicking);

			tbrNew.Enabled = mnuNew.Enabled;
			tbrOpen.Enabled = mnuOpen.Enabled;
			tbrSave.Enabled = mnuSave.Enabled;
			tbrUndoMove.Enabled = mnuUndoMove.Enabled;
			tbrRedoMove.Enabled = mnuRedoMove.Enabled;
			tbrUndoAllMoves.Enabled = mnuUndoAllMoves.Enabled;
			tbrRedoAllMoves.Enabled = mnuRedoAllMoves.Enabled;
			tbrForceComputerMove.Enabled = mnuForceComputerMove.Enabled;
			tbrResumePlay.Enabled = mnuResumePlay.Enabled;
			tbrPausePlay.Enabled = mnuPausePlay.Enabled;
			
			cboIntellegenceWhite.Enabled = !Game.PlayerToPlay.IsThinking;
			cboIntellegenceBlack.Enabled = !Game.PlayerToPlay.IsThinking;
		}

		private void CreateBoard()
		{
			PictureBox picSquare;
			Square square;
			Label lblRank;
			Label lblFile;

			for (int intRank=0; intRank<Board.RANK_COUNT; intRank++)
			{
				lblRank = new System.Windows.Forms.Label();
				lblRank.BackColor = System.Drawing.Color.Transparent;
				lblRank.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
				lblRank.Name = "lblRank" + intRank.ToString();
				lblRank.Size = new System.Drawing.Size(24, 48);
				lblRank.TabIndex = 12;
				lblRank.Text = Board.GetSquare(0, intRank).RankName;
				lblRank.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
				lblRank.Left = 0;
				lblRank.Top = (Board.RANK_COUNT-1)*SQUARE_SIZE - intRank*SQUARE_SIZE + 16;
				pnlMain.Controls.Add( lblRank );
			}

			m_picSquares = new PictureBox[Board.FILE_COUNT, Board.RANK_COUNT];

			for (int intFile=0; intFile<Board.FILE_COUNT; intFile++)
			{

				lblFile = new System.Windows.Forms.Label();
				lblFile.BackColor = System.Drawing.Color.Transparent;
				lblFile.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
				lblFile.Name = "lblFile" + intFile.ToString();
				lblFile.Size = new System.Drawing.Size(48, 24);
				lblFile.TabIndex = 12;
				lblFile.Text = Board.GetSquare(intFile, 0).FileName;
				lblFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
				lblFile.Left = intFile*SQUARE_SIZE + 30;
				lblFile.Top = (Board.RANK_COUNT)*SQUARE_SIZE + 24;
				pnlMain.Controls.Add( lblFile );
				
			}

			for (int intOrdinal=0; intOrdinal<Board.SQUARE_COUNT; intOrdinal++)
			{
				square = Board.GetSquare(intOrdinal);

				if (square!=null)
				{
					picSquare = new System.Windows.Forms.PictureBox();

					picSquare.Left = square.File*SQUARE_SIZE + 1;
					picSquare.Top = (Board.RANK_COUNT-1)*SQUARE_SIZE - square.Rank*SQUARE_SIZE + 1;
					if (square.Colour == Square.enmColour.White)
					{
						picSquare.BackColor = BOARD_SQUARE_COLOUR_WHITE;
					}
					else
					{
						picSquare.BackColor = BOARD_SQUARE_COLOUR_BLACK;
					}
					picSquare.Name = "picSquare" + square.File.ToString() + square.Rank.ToString();
					picSquare.Size = new System.Drawing.Size(SQUARE_SIZE, SQUARE_SIZE);
					picSquare.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
					picSquare.TabIndex = 0;
					picSquare.TabStop = false;
					picSquare.Tag = square.Ordinal;
					picSquare.Click += new System.EventHandler(this.picSquare_Click);
					pnlEdging.Controls.Add( picSquare );
					m_picSquares[square.File, square.Rank] = picSquare;
				}
			}

			m_picWhitesCaptures = new PictureBox[15];
			m_picBlacksCaptures = new PictureBox[15];

			for (int intIndex=0; intIndex<15; intIndex++)
			{
				picSquare = new System.Windows.Forms.PictureBox();
				picSquare.Left = intIndex*(SQUARE_SIZE+1)+1;
				picSquare.Top = 432;
				picSquare.BackColor = System.Drawing.SystemColors.ControlDark;
				picSquare.Name = "picSquareWhite" + intIndex.ToString();
				picSquare.Size = new System.Drawing.Size(SQUARE_SIZE, SQUARE_SIZE);
				picSquare.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
				picSquare.TabIndex = 0;
				picSquare.TabStop = false;
				picSquare.Tag = intIndex;
				pnlMain.Controls.Add( picSquare );
				m_picWhitesCaptures[intIndex] = picSquare;

				picSquare = new System.Windows.Forms.PictureBox();
				picSquare.Left = intIndex*(SQUARE_SIZE+1)+1;
				picSquare.Top = 432 + SQUARE_SIZE+1;
				picSquare.BackColor = System.Drawing.SystemColors.ControlDark;
				picSquare.Name = "picSquareBlack" + intIndex.ToString();
				picSquare.Size = new System.Drawing.Size(SQUARE_SIZE, SQUARE_SIZE);
				picSquare.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
				picSquare.TabIndex = 0;
				picSquare.TabStop = false;
				picSquare.Tag = intIndex;
				pnlMain.Controls.Add( picSquare );
				m_picBlacksCaptures[intIndex] = picSquare;
			}
		}

		private void picSquare_Click(object sender, System.EventArgs e)
		{
			int intOrdinal = Convert.ToInt32( ((PictureBox)sender).Tag );

			if (m_squareFrom==null)
			{
				// No current selection

				Square squareClicked = Board.GetSquare(intOrdinal);

				Piece piece = squareClicked.Piece;
				if (piece!=null && piece.Player.Colour==Game.PlayerToPlay.Colour)
				{
					// Mark possible moves
					m_squareFrom = squareClicked;
					m_movesPossible = new Moves();
					piece.GenerateLegalMoves(m_movesPossible);
				}
				else
				{
					// No piece on square
					m_squareFrom = null;
					m_movesPossible = new Moves();
				}
			}
			else
			{
				Square squareClicked = Board.GetSquare(intOrdinal);

				Piece piece = squareClicked.Piece;
				if (piece==null || piece!=null && piece.Player.Colour!=Game.PlayerToPlay.Colour)
				{
					// Is square one of the possible moves? If it is, then move the piece
					foreach (Move move in m_movesPossible)
					{
						if (move.To == squareClicked)
						{
							m_squareFrom = null;
							m_movesPossible = new Moves();

							MakeAHumanMove(move.Name, move.Piece, move.To);

							CheckIfAutoNextMove();
							break; 
						}
					}
					m_squareFrom = null;
					m_movesPossible = new Moves();
				}
				else if (piece!=null && piece==m_squareFrom.Piece)
				{
					// Same piece clicked again, so unselect
					m_squareFrom = null;
					m_movesPossible = new Moves();
				}
				else if (piece!=null) // Must be own piece
				{
					// Mark possible moves
					m_movesPossible = new Moves();
					m_squareFrom = squareClicked;
					m_squareFrom.Piece.GenerateLegalMoves(m_movesPossible);
				}
				else
				{
					// No piece on square
					m_squareFrom = null;
					m_movesPossible = new Moves();
				}
			}

			RenderBoard();
		}

		private void CheckIfAutoNextMove()
		{
			if (Game.PlayerWhite.Intellegence==Player.enmIntellegence.Computer && Game.PlayerBlack.Intellegence==Player.enmIntellegence.Computer)
			{
				// Dont want an infinate loop of Computer moves
				return;
			}
			while (Game.PlayerToPlay.Intellegence==Player.enmIntellegence.Computer)
			{
				if (!Game.PlayerToPlay.CanMove)
				{
					break;
				}
				else
				{
					MakeNextComputerMove();
				}
			}
		}

		private void MakeAHumanMove(Move.enmName MoveName, Piece piece, Square square)
		{
			Game.MakeAMove(MoveName, piece, square);
			RenderBoard();
		}

		private void MakeNextComputerMove()
		{
			Move move;

			move = Game.PlayerToPlay.ComputeBestMove();
			Game.MakeAMove(move.Name, move.Piece, move.To);

//			sbr.Text += "  Moved: " + move.Piece.Name.ToString() + " " + move.From.Name+"-"+move.To.Name + " " + move.Description;
			sbr.Text += " SU:" + HashTable.SlotsUsed;
			pbr.Value = 0;

			RenderMoveAnalysis();
			RenderBoard();

			CheckIfAutoNextMove();
		}

		private void mnuUndoMove_Click(object sender, System.EventArgs e)
		{
			UndoMove();
		}

		private void AddMoveToHistory(Move move)
		{
			string[] lvi = {	move.MoveNo.ToString(), move.TimeStamp.Hours.ToString().PadLeft(2,'0') + ":" + move.TimeStamp.Minutes.ToString().PadLeft(2,'0') + ":" + move.TimeStamp.Seconds.ToString().PadLeft(2,'0') 
							   , move.Piece.Player.Colour.ToString() + " "
							   + move.Piece.Name.ToString() + " "
							   + move.From.Name 
							   + (move.PieceTaken!=null?"x":"-")
							   + move.To.Name 
							   + move.Description
						   };

			lvwMoveHistory.Items.Add( new ListViewItem( lvi ) );
			switch (move.Piece.Player.Colour)
			{
				case Player.enmColour.White:
					lvwMoveHistory.Items[lvwMoveHistory.Items.Count-1].BackColor = Color.White;
					lvwMoveHistory.Items[lvwMoveHistory.Items.Count-1].ForeColor = Color.Blue;
					break;

				case Player.enmColour.Black:
					lvwMoveHistory.Items[lvwMoveHistory.Items.Count-1].BackColor = Color.White;
					lvwMoveHistory.Items[lvwMoveHistory.Items.Count-1].ForeColor = Color.Black;
					break;
			}
			lvwMoveHistory.Items[lvwMoveHistory.Items.Count-1].EnsureVisible();
		}

		private void RemoveLastHistoryItem()
		{
			lvwMoveHistory.Items.RemoveAt(lvwMoveHistory.Items.Count-1);
			m_squareFrom = null;
			m_movesPossible = new Moves();
		}

		private void RenderMoveAnalysis()
		{
			tvwMoves.Nodes.Clear();
			AddBranch(2, Game.MoveAnalysis, tvwMoves.Nodes);
		}

		private void AddBranch(int intDepth, Moves moves, TreeNodeCollection treeNodes)
		{
			if (intDepth == 0) return;

			TreeNode treeNode;
			if (moves!=null)
			{
				foreach(Move move in moves)
				{
					treeNode = treeNodes.Add( move.DebugText );
					treeNode.Tag = move;
					if (move.Moves!=null && move.Moves.Count>0)
					{
						AddBranch(intDepth-1, move.Moves, treeNode.Nodes);
					}
				}
			}
		}

		private void tvwMoves_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			foreach (TreeNode tn in e.Node.Nodes)
			{
				AddBranch(1, ((Move)tn.Tag).Moves, tn.Nodes);
			}
		}

		private void mnuExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void NewGame()
		{
			lvwMoveHistory.Items.Clear();
			Game.New();
			RenderBoard();
			this.Text = Application.ProductName + " - " + Game.FileName;
		}

		private void OpenGame()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();

			openFileDialog.Title = "Load a saved chess game" ;
//			openFileDialog.InitialDirectory = @"c:\" ;
			openFileDialog.Filter = "SharpChess files (*.SharpChess)|*.SharpChess";
			openFileDialog.FilterIndex = 2 ;

			if(openFileDialog.ShowDialog() == DialogResult.OK)
			{
				if( openFileDialog.FileName!="" )
				{
					lvwMoveHistory.Items.Clear();
					Game.Load(openFileDialog.FileName);
					RenderBoard();
				}
			}
			this.Text = Application.ProductName + " - " + Game.FileName;
		}

		private void SaveGame()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
 
			saveFileDialog.Filter = "SharpChess files (*.SharpChess)|*.SharpChess";
			saveFileDialog.FilterIndex = 2;
			saveFileDialog.FileName = Game.FileName;
 
			if(saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				if( saveFileDialog.FileName!="" )
				{
					Game.Save(saveFileDialog.FileName);
				}
			}
			this.Text = Application.ProductName + " - " + Game.FileName;
		}

		private void UndoMove()
		{
			Game.UndoMove();
			RenderBoard();
		}

		private void RedoMove()
		{
			Game.RedoMove();
			RenderBoard();
		}

		private void UndoAllMoves()
		{
			Game.UndoAllMoves();
			RenderBoard();
		}

		private void RedoAllMoves()
		{
			Game.RedoAllMoves();
			RenderBoard();
		}

		private void ResumePlay()
		{
			Game.ResumePlay();
			SetFormState();
		}

		private void PausePlay()
		{
			Game.PausePlay();
			SetFormState();
		}

		private void cboIntellegenceWhite_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Game.PlayerWhite.Intellegence = cboIntellegenceWhite.SelectedIndex==INTELLEGENCE_HUMAN ? Player.enmIntellegence.Human : Player.enmIntellegence.Computer;
		}

		private void cboIntellegenceBlack_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Game.PlayerBlack.Intellegence = cboIntellegenceBlack.SelectedIndex==INTELLEGENCE_HUMAN ? Player.enmIntellegence.Human : Player.enmIntellegence.Computer;
		}

		private void mnuAbout_Click(object sender, System.EventArgs e)
		{
			var formAbout = new FrmAbout();
			formAbout.ShowDialog(this);
		}

		private void mnuNew_Click(object sender, System.EventArgs e)
		{
			NewGame();
		}

		private void mnuOpen_Click(object sender, System.EventArgs e)
		{
			OpenGame();
		}

		private void mnuSave_Click(object sender, System.EventArgs e)
		{
			SaveGame();
		}

		private void mnuForceComputerMove_Click(object sender, System.EventArgs e)
		{
			MakeNextComputerMove();
		}

		private void tbr_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			switch(e.Button.Tag.ToString())
			{
				case "New":
					NewGame();
					break; 

				case "Open":
					OpenGame();
					break; 

				case "Save":
					SaveGame();
					break; 

				case "UndoMove":
					UndoMove();
					break;

				case "RedoMove":
					RedoMove();
					break;

				case "UndoAllMoves":
					UndoAllMoves();
					break;

				case "RedoAllMoves":
					RedoAllMoves();
					break;

				case "ForceComputerMove":
					MakeNextComputerMove();
					break;

				case "ResumePlay":
					ResumePlay();
					break;

				case "PausePlay":
					PausePlay();
					break;
			}
		}

		private void mnuMoreOptions_Click(object sender, System.EventArgs e)
		{
			var formOptions = new FrmOptions();
			formOptions.ShowDialog(this);
			AssignMenuChecks();
		}

		private void mnuShowThinking_Click(object sender, System.EventArgs e)
		{
			Game.ShowThinking = !Game.ShowThinking;
			AssignMenuChecks();
		}

		private void mnuDisplayMoveAnalysisTree_Click(object sender, System.EventArgs e)
		{
			Game.DisplayMoveAnalysisTree = !Game.DisplayMoveAnalysisTree;
			AssignMenuChecks();
			SizeMainForm();
		}

		private void AssignMenuChecks()
		{
			mnuShowThinking.Checked = Game.ShowThinking;
			mnuDisplayMoveAnalysisTree.Checked = Game.DisplayMoveAnalysisTree;
		}

		private void SizeMainForm()
		{
			this.Width = Game.DisplayMoveAnalysisTree ? 1192: 744;
		}

		private void timer_Tick(object sender, System.EventArgs e)
		{
			RenderClocks();
		}
	}
}

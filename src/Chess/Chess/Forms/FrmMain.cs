using System;
using System.Drawing;
using System.Windows.Forms;
using Chess.Core;

namespace Chess.Forms
{
	/// <summary>
	/// Summary description for frmMain.
	/// </summary>

	public partial class FrmMain : Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private const int SquareSize = 48;

	    private const int IntellegenceHuman = 0;
	    private const int IntellegenceComputer = 1;

		private readonly Color _boardSquareColourWhite = Color.FromArgb(229,197,105);
		private readonly Color _boardSquareColourBlack = Color.FromArgb(189,117,53);

	    private Square _mSquareFrom;
	    private Moves _mMovesPossible = new Moves();
	    private PictureBox[,] _mPicSquares;
	    private PictureBox[] _mPicWhitesCaptures;
	    private PictureBox[] _mPicBlacksCaptures;
	    private Square _mSquareLastFrom;
	    private Square _mSquareLastTo;

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

			if (_mSquareLastFrom != null)
			{
				_mPicSquares[_mSquareLastFrom.File, _mSquareLastFrom.Rank].BackColor = _mSquareLastFrom.Colour==Square.EnmColour.White ? _boardSquareColourWhite : _boardSquareColourBlack;
				_mPicSquares[_mSquareLastFrom.File, _mSquareLastFrom.Rank].Refresh();
			}
			if (Game.ShowThinking)
			{
				_mSquareLastFrom = Game.PlayerToPlay.CurrentMove.From;
				_mPicSquares[_mSquareLastFrom.File, _mSquareLastFrom.Rank].BackColor=Color.Yellow;
				_mPicSquares[_mSquareLastFrom.File, _mSquareLastFrom.Rank].Refresh();
			}

			if (_mSquareLastTo != null)
			{
				_mPicSquares[_mSquareLastTo.File, _mSquareLastTo.Rank].BackColor = _mSquareLastTo.Colour==Square.EnmColour.White ? _boardSquareColourWhite : _boardSquareColourBlack;
				_mPicSquares[_mSquareLastTo.File, _mSquareLastTo.Rank].Refresh();
			}
			if (Game.ShowThinking)
			{
				_mSquareLastTo = Game.PlayerToPlay.CurrentMove.To;
				_mPicSquares[_mSquareLastTo.File, _mSquareLastTo.Rank].BackColor=Color.Yellow;
				_mPicSquares[_mSquareLastTo.File, _mSquareLastTo.Rank].Refresh();
			}

			SetFormState();

			Application.DoEvents();
		}	

		private void frmMain_Load(object sender, EventArgs e)
		{
			new Game();

			Game.PlayerWhite.MoveConsidered += new DelegateGameEvent(MoveConsidered);
			Game.PlayerBlack.MoveConsidered += new DelegateGameEvent(MoveConsidered);
			
			cboIntellegenceWhite.SelectedIndex = Game.PlayerWhite.Intellegence==Player.EnmIntellegence.Human ? IntellegenceHuman : IntellegenceComputer;
			cboIntellegenceBlack.SelectedIndex = Game.PlayerBlack.Intellegence==Player.EnmIntellegence.Human ? IntellegenceHuman : IntellegenceComputer;


			CreateBoard();
			RenderBoard();
			RenderClocks();
			Text = Application.ProductName + " - " + Game.FileName;
			AssignMenuChecks();
			SizeMainForm();

			timer.Start();

			SetFormState();
		}


		private void RenderClocks()
		{
			lblWhiteClock.Text = Game.PlayerWhite.Clock.TimeElapsed.Hours.ToString().PadLeft(2,'0') + @":" + Game.PlayerWhite.Clock.TimeElapsed.Minutes.ToString().PadLeft(2,'0') + @":" + Game.PlayerWhite.Clock.TimeElapsed.Seconds.ToString().PadLeft(2,'0');
			lblBlackClock.Text = Game.PlayerBlack.Clock.TimeElapsed.Hours.ToString().PadLeft(2,'0') + @":" + Game.PlayerBlack.Clock.TimeElapsed.Minutes.ToString().PadLeft(2,'0') + @":" + Game.PlayerBlack.Clock.TimeElapsed.Seconds.ToString().PadLeft(2,'0');
		}

		private void RenderBoard()
		{
			Square square;

			for (var intOrdinal=0; intOrdinal<Board.SquareCount; intOrdinal++)
			{
				square = Board.GetSquare(intOrdinal);
				
				if (square!=null)
				{
					if (square.Colour == Square.EnmColour.White)
					{
						_mPicSquares[square.File, square.Rank].BackColor = _boardSquareColourWhite;
					}
					else
					{
						_mPicSquares[square.File, square.Rank].BackColor = _boardSquareColourBlack;
					}

					if (square.Piece == null)
					{
						_mPicSquares[square.File, square.Rank].Image = null;
					}
					else
					{
						_mPicSquares[square.File, square.Rank].Image = GetPieceImage(square.Piece);
					}

					_mPicSquares[square.File, square.Rank].BorderStyle = BorderStyle.None;
				}
			}

			// Render selection highlights
			if (_mSquareFrom!=null)
			{
				_mPicSquares[_mSquareFrom.File, _mSquareFrom.Rank].BackColor = Color.Blue;
				foreach (Move move in _mMovesPossible)
				{
					_mPicSquares[move.To.File, move.To.Rank].BackColor = Color.LightBlue;
				}
			}

			// Render Last Move highlights
			if (Game.MoveHistory.Count>0)
			{
				_mPicSquares[Game.MoveHistory.Item(Game.MoveHistory.Count-1).From.File, Game.MoveHistory.Item(Game.MoveHistory.Count-1).From.Rank].BorderStyle = BorderStyle.Fixed3D;
				_mPicSquares[Game.MoveHistory.Item(Game.MoveHistory.Count-1).To.File  , Game.MoveHistory.Item(Game.MoveHistory.Count-1).To.Rank  ].BorderStyle = BorderStyle.Fixed3D;
			}

			// Render pieces taken
			for (var intIndex=0; intIndex<15; intIndex++)
			{
				_mPicWhitesCaptures[intIndex].Image = null;
				_mPicBlacksCaptures[intIndex].Image = null;
			}
			for (var intIndex=0; intIndex<Game.PlayerWhite.CapturedEnemyPieces.Count; intIndex++)
			{
				_mPicWhitesCaptures[intIndex].Image = GetPieceImage(Game.PlayerWhite.CapturedEnemyPieces.Item(intIndex));
			}
			for (var intIndex=0; intIndex<Game.PlayerBlack.CapturedEnemyPieces.Count; intIndex++)
			{
				_mPicBlacksCaptures[intIndex].Image = GetPieceImage(Game.PlayerBlack.CapturedEnemyPieces.Item(intIndex));
			}

			// Render player status
			if (Game.PlayerToPlay == Game.PlayerWhite)
			{
				lblWhiteScore.BackColor = Game.PlayerWhite.Status==Player.EnmStatus.InCheckMate ? Color.Red : (Game.PlayerWhite.IsInCheck ? Color.Orange: Color.FromName(KnownColor.Control.ToString()) );
				lblBlackScore.BackColor = Color.FromName(KnownColor.Control.ToString());
				lblWhiteClock.BackColor = Color.LightGray;
				lblBlackClock.BackColor = Color.FromName(KnownColor.Control.ToString());
			}
			else
			{
				lblWhiteScore.BackColor = Color.FromName(KnownColor.Control.ToString());
				lblBlackScore.BackColor = Game.PlayerBlack.Status==Player.EnmStatus.InCheckMate ? Color.Red : (Game.PlayerBlack.IsInCheck ? Color.Orange : Color.FromName(KnownColor.Control.ToString()) );
				lblBlackClock.BackColor = Color.LightGray;
				lblWhiteClock.BackColor = Color.FromName(KnownColor.Control.ToString());
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

			Refresh();
		}

		private Image GetPieceImage(Piece piece)
		{
			return imgPieces.Images[piece.ImageIndex];
		}

		private void RenderStatus()
		{
			var playerToPlay = Game.PlayerToPlay;

			sbr.Text = playerToPlay.SearchDepth==0 ? "" : 
				"Ply: " + playerToPlay.SearchDepth.ToString() + "/" + playerToPlay.MaxSearchDepth.ToString() 
				+ ". Move: " + playerToPlay.CurrentMoveNo.ToString() + " / " + playerToPlay.TotalMoves.ToString() 
				+ ". Secs: " + ((int)playerToPlay.ThinkingTimeRemaining.TotalSeconds).ToString() + "/" + ((int)playerToPlay.ThinkingTimeAllotted.TotalSeconds).ToString() 
				+ (!Game.ShowThinking ? "" : ". Best: " + (playerToPlay.BestMove==null ? "" : playerToPlay.BestMove.Piece.Name.ToString() + " " + playerToPlay.BestMove.From.Name+"-"+playerToPlay.BestMove.To.Name + " " + playerToPlay.BestMove.Description + " Score: " + playerToPlay.BestMove.Score))
				+ " Positions: " + playerToPlay.PositionsSearched + " MaxQDepth: " + playerToPlay.MaxQuiesDepth
				+ " P:" + HashTable.Probes
				+ " H:" + HashTable.Hits
				+ " W:" + HashTable.Writes 
				+ " C:" + HashTable.Collisions
				+ " O:" + HashTable.Overwrites;

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
			mnuUndoMove.Enabled = !Game.PlayerToPlay.IsThinking && Game.MoveHistory.Count>0;
			mnuRedoMove.Enabled = !Game.PlayerToPlay.IsThinking && Game.MoveRedoList.Count>0;
			mnuUndoAllMoves.Enabled = mnuUndoMove.Enabled;
			mnuRedoAllMoves.Enabled = mnuRedoMove.Enabled;
			mnuForceComputerMove.Enabled = !Game.PlayerToPlay.IsThinking && Game.PlayerToPlay.CanMove;
			mnuResumePlay.Enabled = !Game.PlayerToPlay.IsThinking && !Game.PlayerToPlay.Clock.IsTicking; 
			mnuPausePlay.Enabled = !Game.PlayerToPlay.IsThinking && Game.PlayerToPlay.Clock.IsTicking;

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

			for (var intRank=0; intRank<Board.RankCount; intRank++)
			{
				lblRank = new Label();
				lblRank.BackColor = Color.Transparent;
				lblRank.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point, (Byte)0);
				lblRank.Name = "lblRank" + intRank.ToString();
				lblRank.Size = new Size(24, 48);
				lblRank.TabIndex = 12;
				lblRank.Text = Board.GetSquare(0, intRank).RankName;
				lblRank.TextAlign = ContentAlignment.MiddleCenter;
				lblRank.Left = 0;
				lblRank.Top = (Board.RankCount-1)*SquareSize - intRank*SquareSize + 16;
				pnlMain.Controls.Add( lblRank );
			}

			_mPicSquares = new PictureBox[Board.FileCount, Board.RankCount];

			for (var intFile=0; intFile<Board.FileCount; intFile++)
			{

				lblFile = new Label();
				lblFile.BackColor = Color.Transparent;
				lblFile.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point, (Byte)0);
				lblFile.Name = "lblFile" + intFile.ToString();
				lblFile.Size = new Size(48, 24);
				lblFile.TabIndex = 12;
				lblFile.Text = Board.GetSquare(intFile, 0).FileName;
				lblFile.TextAlign = ContentAlignment.MiddleCenter;
				lblFile.Left = intFile*SquareSize + 30;
				lblFile.Top = Board.RankCount*SquareSize + 24;
				pnlMain.Controls.Add( lblFile );
				
			}

			for (var intOrdinal=0; intOrdinal<Board.SquareCount; intOrdinal++)
			{
				square = Board.GetSquare(intOrdinal);

				if (square!=null)
				{
					picSquare = new PictureBox();

					picSquare.Left = square.File*SquareSize + 1;
					picSquare.Top = (Board.RankCount-1)*SquareSize - square.Rank*SquareSize + 1;
					if (square.Colour == Square.EnmColour.White)
					{
						picSquare.BackColor = _boardSquareColourWhite;
					}
					else
					{
						picSquare.BackColor = _boardSquareColourBlack;
					}
					picSquare.Name = "picSquare" + square.File.ToString() + square.Rank.ToString();
					picSquare.Size = new Size(SquareSize, SquareSize);
					picSquare.SizeMode = PictureBoxSizeMode.CenterImage;
					picSquare.TabIndex = 0;
					picSquare.TabStop = false;
					picSquare.Tag = square.Ordinal;
					picSquare.Click += new EventHandler(picSquare_Click);
					pnlEdging.Controls.Add( picSquare );
					_mPicSquares[square.File, square.Rank] = picSquare;
				}
			}

			_mPicWhitesCaptures = new PictureBox[15];
			_mPicBlacksCaptures = new PictureBox[15];

			for (var intIndex=0; intIndex<15; intIndex++)
			{
				picSquare = new PictureBox();
				picSquare.Left = intIndex*(SquareSize+1)+1;
				picSquare.Top = 432;
				picSquare.BackColor = SystemColors.ControlDark;
				picSquare.Name = "picSquareWhite" + intIndex.ToString();
				picSquare.Size = new Size(SquareSize, SquareSize);
				picSquare.SizeMode = PictureBoxSizeMode.CenterImage;
				picSquare.TabIndex = 0;
				picSquare.TabStop = false;
				picSquare.Tag = intIndex;
				pnlMain.Controls.Add( picSquare );
				_mPicWhitesCaptures[intIndex] = picSquare;

				picSquare = new PictureBox();
				picSquare.Left = intIndex*(SquareSize+1)+1;
				picSquare.Top = 432 + SquareSize+1;
				picSquare.BackColor = SystemColors.ControlDark;
				picSquare.Name = "picSquareBlack" + intIndex.ToString();
				picSquare.Size = new Size(SquareSize, SquareSize);
				picSquare.SizeMode = PictureBoxSizeMode.CenterImage;
				picSquare.TabIndex = 0;
				picSquare.TabStop = false;
				picSquare.Tag = intIndex;
				pnlMain.Controls.Add( picSquare );
				_mPicBlacksCaptures[intIndex] = picSquare;
			}
		}

		private void picSquare_Click(object sender, EventArgs e)
		{
			var intOrdinal = Convert.ToInt32( ((PictureBox)sender).Tag );

			if (_mSquareFrom==null)
			{
				// No current selection

				var squareClicked = Board.GetSquare(intOrdinal);

				var piece = squareClicked.Piece;
				if (piece!=null && piece.Player.Colour==Game.PlayerToPlay.Colour)
				{
					// Mark possible moves
					_mSquareFrom = squareClicked;
					_mMovesPossible = new Moves();
					piece.GenerateLegalMoves(_mMovesPossible);
				}
				else
				{
					// No piece on square
					_mSquareFrom = null;
					_mMovesPossible = new Moves();
				}
			}
			else
			{
				var squareClicked = Board.GetSquare(intOrdinal);

				var piece = squareClicked.Piece;
				if (piece==null || piece!=null && piece.Player.Colour!=Game.PlayerToPlay.Colour)
				{
					// Is square one of the possible moves? If it is, then move the piece
					foreach (Move move in _mMovesPossible)
					{
						if (move.To == squareClicked)
						{
							_mSquareFrom = null;
							_mMovesPossible = new Moves();

							MakeAHumanMove(move.Name, move.Piece, move.To);

							CheckIfAutoNextMove();
							break; 
						}
					}
					_mSquareFrom = null;
					_mMovesPossible = new Moves();
				}
				else if (piece!=null && piece==_mSquareFrom.Piece)
				{
					// Same piece clicked again, so unselect
					_mSquareFrom = null;
					_mMovesPossible = new Moves();
				}
				else if (piece!=null) // Must be own piece
				{
					// Mark possible moves
					_mMovesPossible = new Moves();
					_mSquareFrom = squareClicked;
					_mSquareFrom.Piece.GenerateLegalMoves(_mMovesPossible);
				}
				else
				{
					// No piece on square
					_mSquareFrom = null;
					_mMovesPossible = new Moves();
				}
			}

			RenderBoard();
		}

		private void CheckIfAutoNextMove()
		{
			if (Game.PlayerWhite.Intellegence==Player.EnmIntellegence.Computer && Game.PlayerBlack.Intellegence==Player.EnmIntellegence.Computer)
			{
				// Dont want an infinate loop of Computer moves
				return;
			}
			while (Game.PlayerToPlay.Intellegence==Player.EnmIntellegence.Computer)
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

		private void MakeAHumanMove(Move.EnmName moveName, Piece piece, Square square)
		{
			Game.MakeAMove(moveName, piece, square);
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

		private void mnuUndoMove_Click(object sender, EventArgs e)
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
				case Player.EnmColour.White:
					lvwMoveHistory.Items[lvwMoveHistory.Items.Count-1].BackColor = Color.White;
					lvwMoveHistory.Items[lvwMoveHistory.Items.Count-1].ForeColor = Color.Blue;
					break;

				case Player.EnmColour.Black:
					lvwMoveHistory.Items[lvwMoveHistory.Items.Count-1].BackColor = Color.White;
					lvwMoveHistory.Items[lvwMoveHistory.Items.Count-1].ForeColor = Color.Black;
					break;
			}
			lvwMoveHistory.Items[lvwMoveHistory.Items.Count-1].EnsureVisible();
		}

		private void RemoveLastHistoryItem()
		{
			lvwMoveHistory.Items.RemoveAt(lvwMoveHistory.Items.Count-1);
			_mSquareFrom = null;
			_mMovesPossible = new Moves();
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

		private void tvwMoves_AfterExpand(object sender, TreeViewEventArgs e)
		{
			foreach (TreeNode tn in e.Node.Nodes)
			{
				AddBranch(1, ((Move)tn.Tag).Moves, tn.Nodes);
			}
		}

		private void mnuExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void NewGame()
		{
			lvwMoveHistory.Items.Clear();
			Game.New();
			RenderBoard();
			Text = Application.ProductName + " - " + Game.FileName;
		}

		private void OpenGame()
		{
			var openFileDialog = new OpenFileDialog();

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
			Text = Application.ProductName + " - " + Game.FileName;
		}

		private void SaveGame()
		{
			var saveFileDialog = new SaveFileDialog();
 
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
			Text = Application.ProductName + " - " + Game.FileName;
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

		private void cboIntellegenceWhite_SelectedIndexChanged(object sender, EventArgs e)
		{
			Game.PlayerWhite.Intellegence = cboIntellegenceWhite.SelectedIndex==IntellegenceHuman ? Player.EnmIntellegence.Human : Player.EnmIntellegence.Computer;
		}

		private void cboIntellegenceBlack_SelectedIndexChanged(object sender, EventArgs e)
		{
			Game.PlayerBlack.Intellegence = cboIntellegenceBlack.SelectedIndex==IntellegenceHuman ? Player.EnmIntellegence.Human : Player.EnmIntellegence.Computer;
		}

		private void mnuAbout_Click(object sender, EventArgs e)
		{
			var formAbout = new FrmAbout();
			formAbout.ShowDialog(this);
		}

		private void mnuNew_Click(object sender, EventArgs e)
		{
			NewGame();
		}

		private void mnuOpen_Click(object sender, EventArgs e)
		{
			OpenGame();
		}

		private void mnuSave_Click(object sender, EventArgs e)
		{
			SaveGame();
		}

		private void mnuForceComputerMove_Click(object sender, EventArgs e)
		{
			MakeNextComputerMove();
		}

		private void tbr_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
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

		private void mnuMoreOptions_Click(object sender, EventArgs e)
		{
			var formOptions = new FrmOptions();
			formOptions.ShowDialog(this);
			AssignMenuChecks();
		}

		private void mnuShowThinking_Click(object sender, EventArgs e)
		{
			Game.ShowThinking = !Game.ShowThinking;
			AssignMenuChecks();
		}

		private void mnuDisplayMoveAnalysisTree_Click(object sender, EventArgs e)
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
			Width = Game.DisplayMoveAnalysisTree ? 1192: 744;
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			RenderClocks();
		}
	}
}

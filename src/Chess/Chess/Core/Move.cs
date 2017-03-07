using System;

namespace Chess.Core
{
	public class Move: IComparable
	{
		public enum EnmName
		{
				Standard
			,	CastleQueenSide
			,	CastleKingSide
			,	PawnPromotion
			,	EnPassent
			,	NullMove
		}

		private Piece _mPiece;
		private readonly Square _mFrom;
		private readonly Square _mTo;
		private readonly Piece _mPieceTaken;
		private Moves _mMoves;
		private readonly EnmName _mName;
		private readonly int _mTurnNo;
		private readonly int _mLastMoveTurnNo;
		private readonly int _mPieceTakenOrdinal;
		private int _mScore;
		private int _mPoints;
		private int _mAlpha;
		private int _mBeta;
		private ulong _mHashCodeA;
		private ulong _mHashCodeB;
		private bool _mIsInCheck;
		private bool _mIsEnemyInCheck;
		private Player.EnmStatus _mEnemyStatus = Player.EnmStatus.Normal;
		private TimeSpan _mTsnTimeStamp;

		public Move(int turnNo, int lastMoveTurnNo, EnmName name, Piece piece, Square @from, Square to, Piece pieceTaken, int pieceTakenOrdinal, int score)
		{
			_mTurnNo = turnNo;
			_mLastMoveTurnNo = lastMoveTurnNo;
			_mName = name;
			_mPiece = piece;
			_mFrom = @from;
			_mTo = to;
			_mPieceTaken = pieceTaken;
			_mPieceTakenOrdinal = pieceTakenOrdinal;

			_mScore = score;
		}

		public int CompareTo(object move)
		{
			if ( _mScore < ((Move)move).Score) return 1;
			if ( _mScore > ((Move)move).Score) return -1;
			return 0;
		}

		public string DebugText => (Piece!=null ? Piece.Player.Colour.ToString() + " " + Piece.Name.ToString() : "") + " " + From.Name+"-"+To.Name + " " + Description + " A: " + Alpha + " B: " + Beta + " Score: " + Score;

	    public string Description
		{
			get
			{
				var strDescription = "";
				
				if (_mPieceTaken!=null)
				{
					strDescription += "-" + _mPieceTaken.Name.ToString() + ".";
				}

				switch (_mEnemyStatus)
				{
					case Player.EnmStatus.InCheckMate:
						strDescription += " CHECKMATE!";
						break;

					case Player.EnmStatus.InStaleMate:
						strDescription += " Stalemate";
						break;
					
					case Player.EnmStatus.InCheck:
						strDescription += " CHECK!";
						break;
				}

				switch (_mName)
				{
					case EnmName.CastleKingSide:
						strDescription += " Castle king-side";
						break;

					case EnmName.CastleQueenSide:
						strDescription += " Castle queen-side";
						break;

					case EnmName.PawnPromotion:
						strDescription += " Promoted: " + Piece.Name;
						break;

					case EnmName.EnPassent:
						strDescription += " En passent ";
						break;
				}

				return strDescription;
			}
		}

		public Moves Moves
		{
			get { return _mMoves; }
			set { _mMoves = value; }
		}

		public int TurnNo => _mTurnNo;

	    public int LastMoveTurnNo => _mLastMoveTurnNo;

	    public int MoveNo => _mTurnNo/2+1;

	    public EnmName Name => _mName;

	    public Piece Piece
		{
			get {return _mPiece;}
			set {_mPiece = value;}
		}

		public Player.EnmStatus EnemyStatus
		{
			get {return _mEnemyStatus;}
			set {_mEnemyStatus = value;}
		}

		public Square From => _mFrom;

	    public Square To => _mTo;

	    public Piece PieceTaken => _mPieceTaken;

	    public int PieceTakenOrdinal => _mPieceTakenOrdinal;

	    public ulong HashCodeA
		{
			get {return _mHashCodeA;}
			set {_mHashCodeA = value;}
		}

		public ulong HashCodeB
		{
			get {return _mHashCodeB;}
			set {_mHashCodeB = value;}
		}

		public int Score
		{
			get {return _mScore;}
			set {_mScore = value;}
		}

		public int Points
		{
			get {return _mPoints;}
			set {_mPoints = value;}
		}

		public int Alpha
		{
			get {return _mAlpha;}
			set {_mAlpha = value;}
		}

		public int Beta
		{
			get {return _mBeta;}
			set {_mBeta = value;}
		}

		public bool IsInCheck
		{
			get {return _mIsInCheck;}
			set {_mIsInCheck = value;}
		}

		public bool IsEnemyInCheck
		{
			get {return _mIsEnemyInCheck;}
			set {_mIsEnemyInCheck = value;}
		}

		public TimeSpan TimeStamp
		{
			get { return _mTsnTimeStamp; }
			set { _mTsnTimeStamp = value; }
		}

		public static void Undo(Move move)
		{
			Board.HashCodeA ^= move.To.Piece.HashCodeA; // un_XOR the piece from where it was previously moved to
			Board.HashCodeB ^= move.To.Piece.HashCodeB; // un_XOR the piece from where it was previously moved to
			if (move.Piece.Name==Piece.EnmName.Pawn) 
			{
				Board.PawnHashCodeA ^= move.To.Piece.HashCodeA;
				Board.PawnHashCodeB ^= move.To.Piece.HashCodeB;
			}

			move.Piece.Square = move.From;			// Set piece board location
			move.From.Piece = move.Piece;			// Set piece on board
			move.Piece.LastMoveTurnNo = move.LastMoveTurnNo;
			move.Piece.NoOfMoves--;

			if (move.Name!=EnmName.EnPassent)
			{
				move.To.Piece = move.PieceTaken;	// Return piece taken
			}
			else
			{
				move.To.Piece = null;	// Blank square where this pawn was
				Board.GetSquare(move.To.Ordinal - move.Piece.Player.PawnForwardOffset ).Piece = move.PieceTaken; // Return En Passent pawn taken
			}

			if (move.PieceTaken != null)
			{
				move.PieceTaken.Player.Pieces.Insert(move.PieceTakenOrdinal, move.PieceTaken); 
				move.PieceTaken.Player.OtherPlayer.CapturedEnemyPieces.Remove(move.PieceTaken);
				move.PieceTaken.IsInPlay = true;
				if (move.PieceTaken.Name == Piece.EnmName.Pawn)
				{
					move.PieceTaken.Player.IncreasePawnCount();
				}
				else
				{
					move.PieceTaken.Player.IncreaseMaterialCount();
				}
				Board.HashCodeA ^= move.PieceTaken.HashCodeA; // XOR back into play the piece that was taken
				Board.HashCodeB ^= move.PieceTaken.HashCodeB; // XOR back into play the piece that was taken
				if (move.PieceTaken.Name==Piece.EnmName.Pawn) 
				{
					Board.PawnHashCodeA ^= move.PieceTaken.HashCodeA;
					Board.PawnHashCodeB ^= move.PieceTaken.HashCodeB;
				}
			}

			switch (move.Name)
			{
				case EnmName.CastleKingSide:
					Board.HashCodeA ^= move.Piece.Player.KingsRook.HashCodeA;
					Board.HashCodeB ^= move.Piece.Player.KingsRook.HashCodeB;
					move.Piece.Player.KingsRook.Square = Board.GetSquare(7, move.Piece.Square.Rank);
					move.Piece.Player.KingsRook.LastMoveTurnNo = move.LastMoveTurnNo;
					move.Piece.Player.KingsRook.NoOfMoves--;
					Board.GetSquare(7, move.Piece.Square.Rank).Piece = move.Piece.Player.KingsRook;
					Board.GetSquare(5, move.Piece.Square.Rank).Piece = null;
					move.Piece.Player.HasCastled = false;
					Board.HashCodeA ^= move.Piece.Player.KingsRook.HashCodeA;
					Board.HashCodeB ^= move.Piece.Player.KingsRook.HashCodeB;
					break;

				case EnmName.CastleQueenSide:
					Board.HashCodeA ^= move.Piece.Player.QueensRook.HashCodeA;
					Board.HashCodeB ^= move.Piece.Player.QueensRook.HashCodeB;
					move.Piece.Player.QueensRook.Square = Board.GetSquare(0, move.Piece.Square.Rank);
					move.Piece.Player.QueensRook.LastMoveTurnNo = move.LastMoveTurnNo;
					move.Piece.Player.QueensRook.NoOfMoves--;
					Board.GetSquare(0, move.Piece.Square.Rank).Piece = move.Piece.Player.QueensRook;
					Board.GetSquare(3, move.Piece.Square.Rank).Piece = null;
					move.Piece.Player.HasCastled = false;
					Board.HashCodeA ^= move.Piece.Player.QueensRook.HashCodeA;
					Board.HashCodeB ^= move.Piece.Player.QueensRook.HashCodeB;
					break;

				case EnmName.PawnPromotion:
					move.Piece.Demote();
					break;
			}

			Board.HashCodeA ^= move.From.Piece.HashCodeA; // XOR the piece back into the square it moved back to
			Board.HashCodeB ^= move.From.Piece.HashCodeB; // XOR the piece back into the square it moved back to
			if (move.From.Piece.Name==Piece.EnmName.Pawn) 
			{
				Board.PawnHashCodeA ^= move.From.Piece.HashCodeA;
				Board.PawnHashCodeB ^= move.From.Piece.HashCodeB;
			}

			Game.TurnNo--;

			Game.MoveHistory.RemoveLast();
		}
		
		public static void SeeUndo(Move move)
		{
			move.Piece.Square = move.From;			// Set piece board location
			move.From.Piece = move.Piece;			// Set piece on board
			move.Piece.LastMoveTurnNo = move.LastMoveTurnNo;
			move.Piece.NoOfMoves--;

			if (move.Name!=EnmName.EnPassent)
			{
				move.To.Piece = move.PieceTaken;	// Return piece taken
			}
			else
			{
				move.To.Piece = null;	// Blank square where this pawn was
				Board.GetSquare(move.To.Ordinal - move.Piece.Player.PawnForwardOffset ).Piece = move.PieceTaken; // Return En Passent pawn taken
			}

			if (move.PieceTaken != null)
			{
				move.PieceTaken.Player.Pieces.Insert(move.PieceTakenOrdinal, move.PieceTaken); 
				move.PieceTaken.Player.OtherPlayer.CapturedEnemyPieces.Remove(move.PieceTaken);
				move.PieceTaken.IsInPlay = true;
				if (move.PieceTaken.Name == Piece.EnmName.Pawn)
				{
					move.PieceTaken.Player.IncreasePawnCount();
				}
				else
				{
					move.PieceTaken.Player.IncreaseMaterialCount();
				}
			}

			switch (move.Name)
			{
				case EnmName.PawnPromotion:
					move.Piece.Demote();
					break;
			}
			Game.TurnNo--;
		}
		
		public static EnmName MoveNameFromString(string strMoveName)
		{
			if (strMoveName==EnmName.Standard.ToString()) return EnmName.Standard;
			if (strMoveName==EnmName.CastleKingSide.ToString()) return EnmName.CastleKingSide;
			if (strMoveName==EnmName.CastleQueenSide.ToString()) return EnmName.CastleQueenSide;
			if (strMoveName==EnmName.EnPassent.ToString()) return EnmName.EnPassent;
			if (strMoveName==EnmName.PawnPromotion.ToString()) return EnmName.PawnPromotion;
			return 0;
		}

	}
}

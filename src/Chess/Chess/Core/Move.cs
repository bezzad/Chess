using System;

namespace Chess.Core
{
	public class Move: IComparable
	{
		public enum enmName
		{
				Standard
			,	CastleQueenSide
			,	CastleKingSide
			,	PawnPromotion
			,	EnPassent
			,	NullMove
		}

		private Piece m_Piece;
		private Square m_From;
		private Square m_To;
		private Piece m_PieceTaken;
		private Moves m_moves;
		private enmName m_Name;
		private int m_TurnNo;
		private int m_LastMoveTurnNo;
		private int m_PieceTakenOrdinal;
		private int m_Score;
		private int m_Points;
		private int m_Alpha;
		private int m_Beta;
		private ulong m_HashCodeA;
		private ulong m_HashCodeB;
		private bool m_IsInCheck = false;
		private bool m_IsEnemyInCheck = false;
		private Player.enmStatus m_EnemyStatus = Player.enmStatus.Normal;
		private TimeSpan m_tsnTimeStamp;

		public Move(int TurnNo, int LastMoveTurnNo, Move.enmName Name, Piece Piece, Square From, Square To, Piece PieceTaken, int PieceTakenOrdinal, int Score)
		{
			m_TurnNo = TurnNo;
			m_LastMoveTurnNo = LastMoveTurnNo;
			m_Name = Name;
			m_Piece = Piece;
			m_From = From;
			m_To = To;
			m_PieceTaken = PieceTaken;
			m_PieceTakenOrdinal = PieceTakenOrdinal;

			m_Score = Score;
		}

		public int CompareTo(object move)
		{
			if ( this.m_Score < ((Move)move).Score) return 1;
			if ( this.m_Score > ((Move)move).Score) return -1;
			return 0;
		}

		public string DebugText
		{
			get
			{
				return (Piece!=null ? this.Piece.Player.Colour.ToString() + " " + this.Piece.Name.ToString() : "") + " " + this.From.Name+"-"+this.To.Name + " " + this.Description + " A: " + this.Alpha + " B: " + this.Beta + " Score: " + this.Score;// + " h: " + this.m_HashEntries.ToString() + " c:" + this.m_HashCaptures.ToString();
			}
		}

		public string Description
		{
			get
			{
				string strDescription = "";
				
				if (m_PieceTaken!=null)
				{
					strDescription += "-" + m_PieceTaken.Name.ToString() + ".";
				}

				switch (m_EnemyStatus)
				{
					case Player.enmStatus.InCheckMate:
						strDescription += " CHECKMATE!";
						break;

					case Player.enmStatus.InStaleMate:
						strDescription += " Stalemate";
						break;
					
					case Player.enmStatus.InCheck:
						strDescription += " CHECK!";
						break;
				}

				switch (m_Name)
				{
					case Move.enmName.CastleKingSide:
						strDescription += " Castle king-side";
						break;

					case Move.enmName.CastleQueenSide:
						strDescription += " Castle queen-side";
						break;

					case Move.enmName.PawnPromotion:
						strDescription += " Promoted: " + this.Piece.Name;
						break;

					case Move.enmName.EnPassent:
						strDescription += " En passent ";
						break;
				}

				return strDescription;
			}
		}

		public Moves Moves
		{
			get { return m_moves; }
			set { m_moves = value; }
		}

		public int TurnNo
		{
			get {return m_TurnNo;}
		}

		public int LastMoveTurnNo
		{
			get {return m_LastMoveTurnNo;}
		}

		public int MoveNo
		{
			get { return m_TurnNo/2+1; }
		}

		public enmName Name
		{
			get {return m_Name;}
		}

		public Piece Piece
		{
			get {return m_Piece;}
			set {m_Piece = value;}
		}

		public Player.enmStatus EnemyStatus
		{
			get {return m_EnemyStatus;}
			set {m_EnemyStatus = value;}
		}

		public Square From
		{
			get {return m_From;}
		}

		public Square To
		{
			get {return m_To;}
		}

		public Piece PieceTaken
		{
			get {return m_PieceTaken;}
		}

		public int PieceTakenOrdinal
		{
			get {return m_PieceTakenOrdinal;}
		}

		public ulong HashCodeA
		{
			get {return m_HashCodeA;}
			set {m_HashCodeA = value;}
		}

		public ulong HashCodeB
		{
			get {return m_HashCodeB;}
			set {m_HashCodeB = value;}
		}

		public int Score
		{
			get {return m_Score;}
			set {m_Score = value;}
		}

		public int Points
		{
			get {return m_Points;}
			set {m_Points = value;}
		}

		public int Alpha
		{
			get {return m_Alpha;}
			set {m_Alpha = value;}
		}

		public int Beta
		{
			get {return m_Beta;}
			set {m_Beta = value;}
		}

		public bool IsInCheck
		{
			get {return m_IsInCheck;}
			set {m_IsInCheck = value;}
		}

		public bool IsEnemyInCheck
		{
			get {return m_IsEnemyInCheck;}
			set {m_IsEnemyInCheck = value;}
		}

		public TimeSpan TimeStamp
		{
			get { return m_tsnTimeStamp; }
			set { m_tsnTimeStamp = value; }
		}

		public static void Undo(Move move)
		{
			Board.HashCodeA ^= move.To.Piece.HashCodeA; // un_XOR the piece from where it was previously moved to
			Board.HashCodeB ^= move.To.Piece.HashCodeB; // un_XOR the piece from where it was previously moved to
			if (move.Piece.Name==Piece.enmName.Pawn) 
			{
				Board.PawnHashCodeA ^= move.To.Piece.HashCodeA;
				Board.PawnHashCodeB ^= move.To.Piece.HashCodeB;
			}

			move.Piece.Square = move.From;			// Set piece board location
			move.From.Piece = move.Piece;			// Set piece on board
			move.Piece.LastMoveTurnNo = move.LastMoveTurnNo;
			move.Piece.NoOfMoves--;

			if (move.Name!=Move.enmName.EnPassent)
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
				if (move.PieceTaken.Name == Piece.enmName.Pawn)
				{
					move.PieceTaken.Player.IncreasePawnCount();
				}
				else
				{
					move.PieceTaken.Player.IncreaseMaterialCount();
				}
				Board.HashCodeA ^= move.PieceTaken.HashCodeA; // XOR back into play the piece that was taken
				Board.HashCodeB ^= move.PieceTaken.HashCodeB; // XOR back into play the piece that was taken
				if (move.PieceTaken.Name==Piece.enmName.Pawn) 
				{
					Board.PawnHashCodeA ^= move.PieceTaken.HashCodeA;
					Board.PawnHashCodeB ^= move.PieceTaken.HashCodeB;
				}
			}

			switch (move.Name)
			{
				case Move.enmName.CastleKingSide:
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

				case Move.enmName.CastleQueenSide:
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

				case Move.enmName.PawnPromotion:
					move.Piece.Demote();
					break;
			}

			Board.HashCodeA ^= move.From.Piece.HashCodeA; // XOR the piece back into the square it moved back to
			Board.HashCodeB ^= move.From.Piece.HashCodeB; // XOR the piece back into the square it moved back to
			if (move.From.Piece.Name==Piece.enmName.Pawn) 
			{
				Board.PawnHashCodeA ^= move.From.Piece.HashCodeA;
				Board.PawnHashCodeB ^= move.From.Piece.HashCodeB;
			}

			Game.TurnNo--;

			Game.MoveHistory.RemoveLast();
		}
		
		public static void SEEUndo(Move move)
		{
			move.Piece.Square = move.From;			// Set piece board location
			move.From.Piece = move.Piece;			// Set piece on board
			move.Piece.LastMoveTurnNo = move.LastMoveTurnNo;
			move.Piece.NoOfMoves--;

			if (move.Name!=Move.enmName.EnPassent)
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
				if (move.PieceTaken.Name == Piece.enmName.Pawn)
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
				case Move.enmName.PawnPromotion:
					move.Piece.Demote();
					break;
			}
			Game.TurnNo--;
		}
		
		public static Move.enmName MoveNameFromString(string strMoveName)
		{
			if (strMoveName==Move.enmName.Standard.ToString()) return Move.enmName.Standard;
			if (strMoveName==Move.enmName.CastleKingSide.ToString()) return Move.enmName.CastleKingSide;
			if (strMoveName==Move.enmName.CastleQueenSide.ToString()) return Move.enmName.CastleQueenSide;
			if (strMoveName==Move.enmName.EnPassent.ToString()) return Move.enmName.EnPassent;
			if (strMoveName==Move.enmName.PawnPromotion.ToString()) return Move.enmName.PawnPromotion;
			return 0;
		}

	}
}

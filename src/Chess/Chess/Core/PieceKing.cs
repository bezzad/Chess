namespace Chess.Core
{
	public class PieceKing: IPieceTop
	{
		static int[] CheckValues = { 0, 60, 180, 360, 500};

		Piece m_Base = null;

		public PieceKing(Piece pieceBase)
		{
			m_Base = pieceBase;
		}

		public Piece Base
		{
			get { return m_Base; }
		}

		public int BasicValue
		{
			get
			{
				return 15;
			}
		}

		public int Value
		{
			get
			{
				return 10000;
			}
		}

		private static int[] m_aintSquareValues =
		{
			1, 1, 1, 1, 1, 1, 1, 1,    0,0,0,0,0,0,0,0,
			1, 7, 7, 7, 7, 7, 7, 1,    0,0,0,0,0,0,0,0,
			1, 7,18,18,18,18, 7, 1,    0,0,0,0,0,0,0,0,
			1, 7,18,27,27,18, 7, 1,    0,0,0,0,0,0,0,0,
			1, 7,18,27,27,18, 7, 1,    0,0,0,0,0,0,0,0,
			1, 7,18,18,18,18, 7, 1,    0,0,0,0,0,0,0,0,
			1, 7, 7, 7, 7, 7, 7, 1 ,   0,0,0,0,0,0,0,0,
			1, 1, 1, 1, 1, 1, 1, 1 ,   0,0,0,0,0,0,0,0
		};

		public int PositionalValue
		{
			get
			{
				int intPoints = 0;

				if (Game.Stage==Game.enmStage.Middle)
				{
					// Penalty for number of open lines to king
					intPoints -= Openness(this.m_Base.Square);

					// Penalty for half-open adjacent files
					bool blnHasFiendlyPawn;
					Square squareThis;
					Piece piece;

					blnHasFiendlyPawn = false;
					squareThis = Board.GetSquare(this.m_Base.Square.File+1, m_Base.Square.Rank);
					while (squareThis!=null)
					{
						piece = squareThis.Piece;
						if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==this.m_Base.Player.Colour)
						{
							blnHasFiendlyPawn = true;
							break;
						}
						squareThis = Board.GetSquare(squareThis.Ordinal + m_Base.Player.PawnForwardOffset);
					}
					if (!blnHasFiendlyPawn)
					{
						intPoints -= 200;
					}

					blnHasFiendlyPawn = false;
					squareThis = Board.GetSquare(this.m_Base.Square.File-1, m_Base.Square.Rank);
					while (squareThis!=null)
					{
						piece = squareThis.Piece;
						if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==this.m_Base.Player.Colour)
						{
							blnHasFiendlyPawn = true;
							break;
						}
						squareThis = Board.GetSquare(squareThis.Ordinal + m_Base.Player.PawnForwardOffset);
					}
					if (!blnHasFiendlyPawn)
					{
						intPoints -= 200;
					}
				}

				switch (Game.Stage)
				{
					case Game.enmStage.End:
						// Bonus for number of moves available
						Moves moves = new Moves();
						this.GenerateLazyMoves(moves, Moves.enmMovesType.All);
						intPoints += moves.Count*10;

						// Bonus for being in centre of board
						intPoints += m_aintSquareValues[m_Base.Square.Ordinal];
						break;

					default: // Opening & Middle
						// Penalty for being in centre of board
						intPoints -= m_aintSquareValues[m_Base.Square.Ordinal];

						break;
				}
				return intPoints;
			}
		}

		private int Openness(Square squareKing)
		{
			Square square = squareKing;

			int intOpenness = 0;
			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, 16); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, 17); if (intOpenness>900) goto exitpoint;
//			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,  1); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,-15); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,-16); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,-17); if (intOpenness>900) goto exitpoint;
//			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, -1); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, 15); if (intOpenness>900) goto exitpoint;
/*
			square = Board.GetSquare(squareKing.Ordinal-1);
			if (square!=null)
			{
				intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, 17); if (intOpenness>900) goto exitpoint;
				intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,-15); if (intOpenness>900) goto exitpoint;
				intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,-17); if (intOpenness>900) goto exitpoint;
				intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, 15); if (intOpenness>900) goto exitpoint;
			}

			square = Board.GetSquare(squareKing.Ordinal+1);
			if (square!=null)
			{
				intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, 17); if (intOpenness>900) goto exitpoint;
				intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,-15); if (intOpenness>900) goto exitpoint;
				intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,-17); if (intOpenness>900) goto exitpoint;
				intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, 15); if (intOpenness>900) goto exitpoint;
			}
*/
			exitpoint:
			return intOpenness;
		}

		private bool PawnIsAdjacent(int intOrdinal)
		{
			Piece piece;
			piece = Board.GetPiece( intOrdinal+15 ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==m_Base.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal+16 ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==m_Base.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal+17 ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==m_Base.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal-15 ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==m_Base.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal-16 ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==m_Base.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal-17 ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==m_Base.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal+1  ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==m_Base.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal-1  ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==m_Base.Player.Colour) return true; 
			return false;
		}

		public int ImageIndex
		{
			get { return (this.m_Base.Player.Colour==Player.enmColour.White ? 5 : 4); }
		}
	
		public bool DetermineCheckStatus()
		{
			return m_Base.Square.CanBeMovedToBy(m_Base.Player.OtherPlayer);
		}

		public bool CanCastleKingSide
		{
			get
			{
				// King hasnt moved
				if (m_Base.HasMoved) return false; 
				// Rook is still there i.e. hasnt been taken
				if ( !this.m_Base.Player.KingsRook.IsInPlay ) return false;
				// King's Rook hasnt moved
				if (m_Base.Player.KingsRook.HasMoved) return false; 
				// All squares between King and Rook are unoccupied
				if ( Board.GetPiece(m_Base.Square.Ordinal+1)!=null ) return false;
				if ( Board.GetPiece(m_Base.Square.Ordinal+2)!=null ) return false;
				// King is not in check
				if (m_Base.Player.IsInCheck) return false;
				// The king does not move over a square that is attacked by an enemy piece during the castling move
				if ( Board.GetSquare(m_Base.Square.Ordinal+1).CanBeMovedToBy(m_Base.Player.OtherPlayer)  ) return false;
				if ( Board.GetSquare(m_Base.Square.Ordinal+2).CanBeMovedToBy(m_Base.Player.OtherPlayer)  ) return false;

				return true;
			}
		}

		public bool CanCastleQueenSide
		{
			get
			{
				// King hasnt moved
				if (m_Base.HasMoved) return false; 
				// Rook is still there i.e. hasnt been taken
				if ( !this.m_Base.Player.QueensRook.IsInPlay ) return false;
				// King's Rook hasnt moved
				if (m_Base.Player.QueensRook.HasMoved) return false; 
				// All squares between King and Rook are unoccupied
				if ( Board.GetPiece(m_Base.Square.Ordinal-1)!=null ) return false;
				if ( Board.GetPiece(m_Base.Square.Ordinal-2)!=null ) return false;
				if ( Board.GetPiece(m_Base.Square.Ordinal-3)!=null ) return false;
				// King is not in check
				if (m_Base.Player.IsInCheck) return false;
				// The king does not move over a square that is attacked by an enemy piece during the castling move
				if ( Board.GetSquare(m_Base.Square.Ordinal-1).CanBeMovedToBy(m_Base.Player.OtherPlayer)  ) return false;
				if ( Board.GetSquare(m_Base.Square.Ordinal-2).CanBeMovedToBy(m_Base.Player.OtherPlayer)  ) return false;

				return true;
			}
		}

		public string Abbreviation
		{
			get {return "K";}
		}

		public Piece.enmName Name
		{
			get {return Piece.enmName.King;}
		}

		public bool CanBeTaken
		{
			get
			{
				return false;
			}
		}

		public void GenerateLazyMoves(Moves moves, Moves.enmMovesType movesType)
		{
			Square square;
			switch (movesType)
			{
				case Moves.enmMovesType.All:
					square = Board.GetSquare(m_Base.Square.Ordinal-1); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+15); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+16); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+17); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+1); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-15); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-16); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-17); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);

					if (this.CanCastleKingSide)
					{
						moves.Add(0, 0, Move.enmName.CastleKingSide, m_Base, m_Base.Square, Board.GetSquare(m_Base.Square.Ordinal+2), null, 0, 0);
					}
					if (this.CanCastleQueenSide)
					{
						moves.Add(Game.TurnNo, m_Base.LastMoveTurnNo, Move.enmName.CastleQueenSide, m_Base, m_Base.Square, Board.GetSquare(m_Base.Square.Ordinal-2), null, 0, 0);
					}

					break;

				case Moves.enmMovesType.Recaptures_Promotions:
				case Moves.enmMovesType.CapturesChecksPromotions:
					square = Board.GetSquare(m_Base.Square.Ordinal-1); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+15); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+16); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+17); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+1); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-15); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-16); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-17); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					break;
			}

		}

	}
}

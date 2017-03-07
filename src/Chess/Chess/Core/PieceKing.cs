namespace Chess.Core
{
	public class PieceKing: IPieceTop
	{
	    private static int[] _checkValues = { 0, 60, 180, 360, 500};

	    private readonly Piece _mBase;

		public PieceKing(Piece pieceBase)
		{
			_mBase = pieceBase;
		}

		public Piece Base => _mBase;

	    public int BasicValue => 15;

	    public int Value => 10000;

	    private static readonly int[] MAintSquareValues =
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
				var intPoints = 0;

				if (Game.Stage==Game.EnmStage.Middle)
				{
					// Penalty for number of open lines to king
					intPoints -= Openness(_mBase.Square);

					// Penalty for half-open adjacent files
					bool blnHasFiendlyPawn;
					Square squareThis;
					Piece piece;

					blnHasFiendlyPawn = false;
					squareThis = Board.GetSquare(_mBase.Square.File+1, _mBase.Square.Rank);
					while (squareThis!=null)
					{
						piece = squareThis.Piece;
						if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==_mBase.Player.Colour)
						{
							blnHasFiendlyPawn = true;
							break;
						}
						squareThis = Board.GetSquare(squareThis.Ordinal + _mBase.Player.PawnForwardOffset);
					}
					if (!blnHasFiendlyPawn)
					{
						intPoints -= 200;
					}

					blnHasFiendlyPawn = false;
					squareThis = Board.GetSquare(_mBase.Square.File-1, _mBase.Square.Rank);
					while (squareThis!=null)
					{
						piece = squareThis.Piece;
						if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==_mBase.Player.Colour)
						{
							blnHasFiendlyPawn = true;
							break;
						}
						squareThis = Board.GetSquare(squareThis.Ordinal + _mBase.Player.PawnForwardOffset);
					}
					if (!blnHasFiendlyPawn)
					{
						intPoints -= 200;
					}
				}

				switch (Game.Stage)
				{
					case Game.EnmStage.End:
						// Bonus for number of moves available
						var moves = new Moves();
						GenerateLazyMoves(moves, Moves.EnmMovesType.All);
						intPoints += moves.Count*10;

						// Bonus for being in centre of board
						intPoints += MAintSquareValues[_mBase.Square.Ordinal];
						break;

					default: // Opening & Middle
						// Penalty for being in centre of board
						intPoints -= MAintSquareValues[_mBase.Square.Ordinal];

						break;
				}
				return intPoints;
			}
		}

		private int Openness(Square squareKing)
		{
			var square = squareKing;

			var intOpenness = 0;
			intOpenness += Board.LineIsOpen(_mBase.Player.Colour, square, 16); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(_mBase.Player.Colour, square, 17); if (intOpenness>900) goto exitpoint;
//			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,  1); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(_mBase.Player.Colour, square,-15); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(_mBase.Player.Colour, square,-16); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(_mBase.Player.Colour, square,-17); if (intOpenness>900) goto exitpoint;
//			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, -1); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(_mBase.Player.Colour, square, 15); if (intOpenness>900) goto exitpoint;
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
			piece = Board.GetPiece( intOrdinal+15 ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==_mBase.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal+16 ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==_mBase.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal+17 ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==_mBase.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal-15 ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==_mBase.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal-16 ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==_mBase.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal-17 ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==_mBase.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal+1  ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==_mBase.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal-1  ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==_mBase.Player.Colour) return true; 
			return false;
		}

		public int ImageIndex => _mBase.Player.Colour==Player.EnmColour.White ? 5 : 4;

	    public bool DetermineCheckStatus()
		{
			return _mBase.Square.CanBeMovedToBy(_mBase.Player.OtherPlayer);
		}

		public bool CanCastleKingSide
		{
			get
			{
				// King hasnt moved
				if (_mBase.HasMoved) return false; 
				// Rook is still there i.e. hasnt been taken
				if ( !_mBase.Player.KingsRook.IsInPlay ) return false;
				// King's Rook hasnt moved
				if (_mBase.Player.KingsRook.HasMoved) return false; 
				// All squares between King and Rook are unoccupied
				if ( Board.GetPiece(_mBase.Square.Ordinal+1)!=null ) return false;
				if ( Board.GetPiece(_mBase.Square.Ordinal+2)!=null ) return false;
				// King is not in check
				if (_mBase.Player.IsInCheck) return false;
				// The king does not move over a square that is attacked by an enemy piece during the castling move
				if ( Board.GetSquare(_mBase.Square.Ordinal+1).CanBeMovedToBy(_mBase.Player.OtherPlayer)  ) return false;
				if ( Board.GetSquare(_mBase.Square.Ordinal+2).CanBeMovedToBy(_mBase.Player.OtherPlayer)  ) return false;

				return true;
			}
		}

		public bool CanCastleQueenSide
		{
			get
			{
				// King hasnt moved
				if (_mBase.HasMoved) return false; 
				// Rook is still there i.e. hasnt been taken
				if ( !_mBase.Player.QueensRook.IsInPlay ) return false;
				// King's Rook hasnt moved
				if (_mBase.Player.QueensRook.HasMoved) return false; 
				// All squares between King and Rook are unoccupied
				if ( Board.GetPiece(_mBase.Square.Ordinal-1)!=null ) return false;
				if ( Board.GetPiece(_mBase.Square.Ordinal-2)!=null ) return false;
				if ( Board.GetPiece(_mBase.Square.Ordinal-3)!=null ) return false;
				// King is not in check
				if (_mBase.Player.IsInCheck) return false;
				// The king does not move over a square that is attacked by an enemy piece during the castling move
				if ( Board.GetSquare(_mBase.Square.Ordinal-1).CanBeMovedToBy(_mBase.Player.OtherPlayer)  ) return false;
				if ( Board.GetSquare(_mBase.Square.Ordinal-2).CanBeMovedToBy(_mBase.Player.OtherPlayer)  ) return false;

				return true;
			}
		}

		public string Abbreviation => "K";

	    public Piece.EnmName Name => Piece.EnmName.King;

	    public bool CanBeTaken => false;

	    public void GenerateLazyMoves(Moves moves, Moves.EnmMovesType movesType)
		{
			Square square;
			switch (movesType)
			{
				case Moves.EnmMovesType.All:
					square = Board.GetSquare(_mBase.Square.Ordinal-1); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+15); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+16); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+17); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+1); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-15); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-16); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-17); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);

					if (CanCastleKingSide)
					{
						moves.Add(0, 0, Move.EnmName.CastleKingSide, _mBase, _mBase.Square, Board.GetSquare(_mBase.Square.Ordinal+2), null, 0, 0);
					}
					if (CanCastleQueenSide)
					{
						moves.Add(Game.TurnNo, _mBase.LastMoveTurnNo, Move.EnmName.CastleQueenSide, _mBase, _mBase.Square, Board.GetSquare(_mBase.Square.Ordinal-2), null, 0, 0);
					}

					break;

				case Moves.EnmMovesType.RecapturesPromotions:
				case Moves.EnmMovesType.CapturesChecksPromotions:
					square = Board.GetSquare(_mBase.Square.Ordinal-1); if ( square!=null && square.Piece!=null && square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+15); if ( square!=null && square.Piece!=null && square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+16); if ( square!=null && square.Piece!=null && square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+17); if ( square!=null && square.Piece!=null && square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+1); if ( square!=null && square.Piece!=null && square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-15); if ( square!=null && square.Piece!=null && square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-16); if ( square!=null && square.Piece!=null && square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-17); if ( square!=null && square.Piece!=null && square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					break;
			}

		}

	}
}

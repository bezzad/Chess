namespace Chess.Core
{
	public class PieceKing: IPieceTop
	{
	    private static int[] _checkValues = { 0, 60, 180, 360, 500};

	    private Piece _mBase = null;

		public PieceKing(Piece pieceBase)
		{
			_mBase = pieceBase;
		}

		public Piece Base
		{
			get { return _mBase; }
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

		private static int[] _mAintSquareValues =
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

				if (Game.Stage==Game.EnmStage.Middle)
				{
					// Penalty for number of open lines to king
					intPoints -= Openness(this._mBase.Square);

					// Penalty for half-open adjacent files
					bool blnHasFiendlyPawn;
					Square squareThis;
					Piece piece;

					blnHasFiendlyPawn = false;
					squareThis = Board.GetSquare(this._mBase.Square.File+1, _mBase.Square.Rank);
					while (squareThis!=null)
					{
						piece = squareThis.Piece;
						if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==this._mBase.Player.Colour)
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
					squareThis = Board.GetSquare(this._mBase.Square.File-1, _mBase.Square.Rank);
					while (squareThis!=null)
					{
						piece = squareThis.Piece;
						if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==this._mBase.Player.Colour)
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
						Moves moves = new Moves();
						this.GenerateLazyMoves(moves, Moves.EnmMovesType.All);
						intPoints += moves.Count*10;

						// Bonus for being in centre of board
						intPoints += _mAintSquareValues[_mBase.Square.Ordinal];
						break;

					default: // Opening & Middle
						// Penalty for being in centre of board
						intPoints -= _mAintSquareValues[_mBase.Square.Ordinal];

						break;
				}
				return intPoints;
			}
		}

		private int Openness(Square squareKing)
		{
			Square square = squareKing;

			int intOpenness = 0;
			intOpenness += Board.LineIsOpen(this._mBase.Player.Colour, square, 16); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(this._mBase.Player.Colour, square, 17); if (intOpenness>900) goto exitpoint;
//			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,  1); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(this._mBase.Player.Colour, square,-15); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(this._mBase.Player.Colour, square,-16); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(this._mBase.Player.Colour, square,-17); if (intOpenness>900) goto exitpoint;
//			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, -1); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(this._mBase.Player.Colour, square, 15); if (intOpenness>900) goto exitpoint;
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

		public int ImageIndex
		{
			get { return (this._mBase.Player.Colour==Player.EnmColour.White ? 5 : 4); }
		}
	
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
				if ( !this._mBase.Player.KingsRook.IsInPlay ) return false;
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
				if ( !this._mBase.Player.QueensRook.IsInPlay ) return false;
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

		public string Abbreviation
		{
			get {return "K";}
		}

		public Piece.EnmName Name
		{
			get {return Piece.EnmName.King;}
		}

		public bool CanBeTaken
		{
			get
			{
				return false;
			}
		}

		public void GenerateLazyMoves(Moves moves, Moves.EnmMovesType movesType)
		{
			Square square;
			switch (movesType)
			{
				case Moves.EnmMovesType.All:
					square = Board.GetSquare(_mBase.Square.Ordinal-1); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, this._mBase, this._mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+15); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, this._mBase, this._mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+16); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, this._mBase, this._mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+17); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, this._mBase, this._mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+1); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, this._mBase, this._mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-15); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, this._mBase, this._mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-16); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, this._mBase, this._mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-17); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, this._mBase, this._mBase.Square, square, square.Piece, 0, 0);

					if (this.CanCastleKingSide)
					{
						moves.Add(0, 0, Move.EnmName.CastleKingSide, _mBase, _mBase.Square, Board.GetSquare(_mBase.Square.Ordinal+2), null, 0, 0);
					}
					if (this.CanCastleQueenSide)
					{
						moves.Add(Game.TurnNo, _mBase.LastMoveTurnNo, Move.EnmName.CastleQueenSide, _mBase, _mBase.Square, Board.GetSquare(_mBase.Square.Ordinal-2), null, 0, 0);
					}

					break;

				case Moves.EnmMovesType.RecapturesPromotions:
				case Moves.EnmMovesType.CapturesChecksPromotions:
					square = Board.GetSquare(_mBase.Square.Ordinal-1); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, this._mBase, this._mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+15); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, this._mBase, this._mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+16); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, this._mBase, this._mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+17); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, this._mBase, this._mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+1); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, this._mBase, this._mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-15); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, this._mBase, this._mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-16); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, this._mBase, this._mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-17); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, this._mBase, this._mBase.Square, square, square.Piece, 0, 0);
					break;
			}

		}

	}
}

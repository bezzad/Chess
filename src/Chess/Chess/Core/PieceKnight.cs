namespace Chess.Core
{
	public class PieceKnight: IPieceTop
	{
	    private readonly Piece _mBase;

		public PieceKnight(Piece pieceBase)
		{
			_mBase = pieceBase;
		}

		public Piece Base
		{
			get { return _mBase; }
		}

		public string Abbreviation
		{
			get {return "N";}
		}

		public Piece.EnmName Name
		{
			get {return Piece.EnmName.Knight;}
		}

		public int BasicValue
		{
			get { return 3;	}
		}

		public int Value
		{
			get
			{
				return 3250; // + ((m_Base.Player.PawnsInPlay-5) * 63);  // raise the knight's value by 1/16 for each pawn above five of the side being valued, with the opposite adjustment for each pawn short of five;
			}
		}

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

				if (Game.Stage==Game.EnmStage.End)
				{
					intPoints -= _mBase.TaxiCabDistanceToEnemyKingPenalty()<<4;
				}
				else
				{
					intPoints += (MAintSquareValues[_mBase.Square.Ordinal]<<3);

					if (_mBase.CanBeDrivenAwayByPawn())
					{
						intPoints-=30;
					}

				}

				intPoints += _mBase.DefensePoints;

				return intPoints;
			}
		}

		public int ImageIndex
		{
			get { return (_mBase.Player.Colour==Player.EnmColour.White ? 7 : 6 ); }
		}
	
		public bool CanBeTaken
		{
			get
			{
				return true;
			}
		}

		public void GenerateLazyMoves(Moves moves, Moves.EnmMovesType movesType)
		{
			Square square;

			switch (movesType)
			{
				case Moves.EnmMovesType.All:
					square = Board.GetSquare(_mBase.Square.Ordinal+33); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+18); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-14); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-31); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-33); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-18); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+14); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+31); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					break;

				case Moves.EnmMovesType.RecapturesPromotions:
				case Moves.EnmMovesType.CapturesChecksPromotions:
					square = Board.GetSquare(_mBase.Square.Ordinal+33); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+18); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-14); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-31); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-33); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal-18); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+14); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(_mBase.Square.Ordinal+31); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
					break;
			}
		}

	}
}

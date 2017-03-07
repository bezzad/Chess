namespace Chess.Core
{
	public class PieceRook: IPieceTop
	{
	    private readonly Piece _mBase;

		public PieceRook(Piece pieceBase)
		{
			_mBase = pieceBase;
		}

		public Piece Base
		{
			get { return _mBase; }
		}

		public string Abbreviation
		{
			get {return "R";}
		}

		public Piece.EnmName Name
		{
			get {return Piece.EnmName.Rook;}
		}

		public int BasicValue
		{
			get { return 5;	}
		}

		public int Value
		{
			get
			{
				return 5000;// - ((m_Base.Player.PawnsInPlay-5) * 125);  // lower the rook's value by 1/8 for each pawn above five of the side being valued, with the opposite adjustment for each pawn short of five
			}
		}

		private static int[] _mAintSquareValues =
		{
			10,10,10,10,10,10,10,10,    0,0,0,0,0,0,0,0,
			10,20,20,20,20,20,20,10,    0,0,0,0,0,0,0,0,
			10,20,30,30,30,30,20,10,    0,0,0,0,0,0,0,0,
			10,20,30,40,40,30,20,10,    0,0,0,0,0,0,0,0,
			10,20,30,40,40,30,20,10,    0,0,0,0,0,0,0,0,
			10,20,30,30,30,30,20,10,    0,0,0,0,0,0,0,0,
			10,20,20,20,20,20,20,10 ,   0,0,0,0,0,0,0,0,
			10,10,10,10,10,10,10,10 ,   0,0,0,0,0,0,0,0
		};

		public int PositionalValue
		{
			get
			{
				var intPoints = 0;

				// After the opening, Rooks are penalized slightly depending on "taxicab" distance to the enemy king.
				if (Game.Stage != Game.EnmStage.Opening)
				{
					intPoints -= _mBase.TaxiCabDistanceToEnemyKingPenalty();
				}

				if (Game.Stage != Game.EnmStage.End)
				{
					// Rooks are given a bonus of 10(0) points for occupying a file with no friendly pawns and a bonus of 
					// 4(0) points if no enemy pawns lie on that file. 
					var blnHasFiendlyPawn = false;
					var blnHasEnemyPawn = false;
					var squareThis = Board.GetSquare(_mBase.Square.File, 0);
					Piece piece;
					while (squareThis!=null)
					{
						piece = squareThis.Piece;
						if (piece!=null && piece.Name==Piece.EnmName.Pawn)
						{
							if (piece.Player.Colour==_mBase.Player.Colour)
							{
								blnHasFiendlyPawn = true;
							}
							else
							{
								blnHasEnemyPawn = true;
							}
							if (blnHasFiendlyPawn && blnHasEnemyPawn) break;
						}
						squareThis = Board.GetSquare(squareThis.Ordinal + 16);
					}
					if (!blnHasFiendlyPawn)				
					{
						intPoints += 20;
					}
					if (!blnHasEnemyPawn)				
					{
						intPoints += 10;
					}


					// 7th rank
					if ( _mBase.Player.Colour==Player.EnmColour.White && _mBase.Square.Rank==6
						||
						_mBase.Player.Colour==Player.EnmColour.Black && _mBase.Square.Rank==1
						)
					{
						intPoints += 30;
					}

					intPoints += _mBase.DefensePoints;
				}

				return intPoints;
			}
		}

		public int ImageIndex
		{
			get { return (_mBase.Player.Colour==Player.EnmColour.White ? 3 : 2); }
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
			Board.AppendPiecePath(moves, _mBase, _mBase.Player,  1, movesType);
			Board.AppendPiecePath(moves, _mBase, _mBase.Player, -1, movesType);
			Board.AppendPiecePath(moves, _mBase, _mBase.Player, -16, movesType);
			Board.AppendPiecePath(moves, _mBase, _mBase.Player, 16, movesType);
		}

	}
}

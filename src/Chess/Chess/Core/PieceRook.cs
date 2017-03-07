namespace Chess.Core
{
	public class PieceRook: IPieceTop
	{
	    public PieceRook(Piece pieceBase)
		{
			Base = pieceBase;
		}

		public Piece Base { get; }

	    public string Abbreviation => "R";

	    public Piece.EnmName Name => Piece.EnmName.Rook;

	    public int BasicValue => 5;

	    public int Value => 5000;

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
					intPoints -= Base.TaxiCabDistanceToEnemyKingPenalty();
				}

				if (Game.Stage != Game.EnmStage.End)
				{
					// Rooks are given a bonus of 10(0) points for occupying a file with no friendly pawns and a bonus of 
					// 4(0) points if no enemy pawns lie on that file. 
					var blnHasFiendlyPawn = false;
					var blnHasEnemyPawn = false;
					var squareThis = Board.GetSquare(Base.Square.File, 0);
					Piece piece;
					while (squareThis!=null)
					{
						piece = squareThis.Piece;
						if (piece!=null && piece.Name==Piece.EnmName.Pawn)
						{
							if (piece.Player.Colour==Base.Player.Colour)
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
					if ( Base.Player.Colour==Player.EnmColour.White && Base.Square.Rank==6
						||
						Base.Player.Colour==Player.EnmColour.Black && Base.Square.Rank==1
						)
					{
						intPoints += 30;
					}

					intPoints += Base.DefensePoints;
				}

				return intPoints;
			}
		}

		public int ImageIndex => Base.Player.Colour==Player.EnmColour.White ? 3 : 2;

	    public bool CanBeTaken => true;

	    public void GenerateLazyMoves(Moves moves, Moves.EnmMovesType movesType)
		{
			Board.AppendPiecePath(moves, Base, Base.Player,  1, movesType);
			Board.AppendPiecePath(moves, Base, Base.Player, -1, movesType);
			Board.AppendPiecePath(moves, Base, Base.Player, -16, movesType);
			Board.AppendPiecePath(moves, Base, Base.Player, 16, movesType);
		}

	}
}

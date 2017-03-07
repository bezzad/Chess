namespace Chess.Core
{
	public class PieceQueen: IPieceTop
	{
	    public PieceQueen(Piece pieceBase)
		{
			Base = pieceBase;
		}

		public Piece Base { get; }

	    public string Abbreviation => "Q";

	    public Piece.EnmName Name => Piece.EnmName.Queen;

	    public int BasicValue => 9;

	    public int Value => 9750;

	    public int PositionalValue
		{
			get
			{
				var intPoints = 0;

				// The queen is that after the opening it is penalized slightly for 
				// "taxicab" distance to the enemy king.
				if (Game.Stage == Game.EnmStage.Opening)
				{
					if (Base.Player.Colour==Player.EnmColour.White)
					{
						intPoints -= Base.Square.Rank * 7;
					}
					else
					{
						intPoints -= (7-Base.Square.Rank) * 7;
					}
				}
				else
				{
					intPoints -= Base.TaxiCabDistanceToEnemyKingPenalty();
				}

				intPoints += Base.DefensePoints;

				return intPoints;
			}
		}

		public int ImageIndex => Base.Player.Colour==Player.EnmColour.White ? 11 : 10;

	    public bool CanBeTaken => true;

	    public void GenerateLazyMoves(Moves moves, Moves.EnmMovesType movesType)
		{
			Board.AppendPiecePath(moves, Base, Base.Player, 17, movesType);
			Board.AppendPiecePath(moves, Base, Base.Player, 15, movesType);
			Board.AppendPiecePath(moves, Base, Base.Player, -15, movesType);
			Board.AppendPiecePath(moves, Base, Base.Player, -17, movesType);
			Board.AppendPiecePath(moves, Base, Base.Player, 16, movesType);
			Board.AppendPiecePath(moves, Base, Base.Player,  1, movesType);
			Board.AppendPiecePath(moves, Base, Base.Player, -1, movesType);
			Board.AppendPiecePath(moves, Base, Base.Player, -16, movesType);
		}

	}
}

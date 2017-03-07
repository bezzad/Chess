namespace Chess.Core
{
	public class PieceQueen: IPieceTop
	{
	    private readonly Piece _mBase;

		public PieceQueen(Piece pieceBase)
		{
			_mBase = pieceBase;
		}

		public Piece Base => _mBase;

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
					if (_mBase.Player.Colour==Player.EnmColour.White)
					{
						intPoints -= _mBase.Square.Rank * 7;
					}
					else
					{
						intPoints -= (7-_mBase.Square.Rank) * 7;
					}
				}
				else
				{
					intPoints -= _mBase.TaxiCabDistanceToEnemyKingPenalty();
				}

				intPoints += _mBase.DefensePoints;

				return intPoints;
			}
		}

		public int ImageIndex => _mBase.Player.Colour==Player.EnmColour.White ? 11 : 10;

	    public bool CanBeTaken => true;

	    public void GenerateLazyMoves(Moves moves, Moves.EnmMovesType movesType)
		{
			Board.AppendPiecePath(moves, _mBase, _mBase.Player, 17, movesType);
			Board.AppendPiecePath(moves, _mBase, _mBase.Player, 15, movesType);
			Board.AppendPiecePath(moves, _mBase, _mBase.Player, -15, movesType);
			Board.AppendPiecePath(moves, _mBase, _mBase.Player, -17, movesType);
			Board.AppendPiecePath(moves, _mBase, _mBase.Player, 16, movesType);
			Board.AppendPiecePath(moves, _mBase, _mBase.Player,  1, movesType);
			Board.AppendPiecePath(moves, _mBase, _mBase.Player, -1, movesType);
			Board.AppendPiecePath(moves, _mBase, _mBase.Player, -16, movesType);
		}

	}
}

namespace Chess.Core
{
	public class PieceQueen: IPieceTop
	{
	    private Piece _mBase = null;

		public PieceQueen(Piece pieceBase)
		{
			_mBase = pieceBase;
		}

		public Piece Base
		{
			get { return _mBase; }
		}

		public string Abbreviation
		{
			get {return "Q";}
		}

		public Piece.EnmName Name
		{
			get {return Piece.EnmName.Queen;}
		}

		public int BasicValue
		{
			get { return 9;	}
		}

		public int Value
		{
			get
			{
				return 9750;
			}
		}

		public int PositionalValue
		{
			get
			{
				int intPoints = 0;

				// The queen is that after the opening it is penalized slightly for 
				// "taxicab" distance to the enemy king.
				if (Game.Stage == Game.EnmStage.Opening)
				{
					if (_mBase.Player.Colour==Player.EnmColour.White)
					{
						intPoints -= this._mBase.Square.Rank * 7;
					}
					else
					{
						intPoints -= (7-this._mBase.Square.Rank) * 7;
					}
				}
				else
				{
					intPoints -= this._mBase.TaxiCabDistanceToEnemyKingPenalty();
				}

				intPoints += _mBase.DefensePoints;

				return intPoints;
			}
		}

		public int ImageIndex
		{
			get { return (this._mBase.Player.Colour==Player.EnmColour.White ? 11 : 10); }
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

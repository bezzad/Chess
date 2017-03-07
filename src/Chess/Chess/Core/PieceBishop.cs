namespace Chess.Core
{
	public class PieceBishop: IPieceTop
	{
	    private readonly Piece _mBase;

		public PieceBishop(Piece pieceBase)
		{
			_mBase = pieceBase;
		}

		public Piece Base
		{
			get { return _mBase; }
		}

		public string Abbreviation
		{
			get {return "B";}
		}

		public Piece.EnmName Name
		{
			get {return Piece.EnmName.Bishop;}
		}

		public int BasicValue
		{
			get { return 3;	}
		}

		public int Value
		{
			get
			{
				return 3250;
			}
		}

		private static readonly int[] MAintSquareValues =
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

				intPoints += (MAintSquareValues[_mBase.Square.Ordinal]<<1);

				if (Game.Stage!=Game.EnmStage.End)
				{
					if (_mBase.CanBeDrivenAwayByPawn())
					{
						intPoints-=30;
					}
				}

/*				// Mobility
				Squares squares = new Squares();
				squares.Add(m_Base.Square);
				Board.LineThreatenedBy(m_Base.Player, squares, m_Base.Square, 15);
				Board.LineThreatenedBy(m_Base.Player, squares, m_Base.Square, 17);
				Board.LineThreatenedBy(m_Base.Player, squares, m_Base.Square, -15);
				Board.LineThreatenedBy(m_Base.Player, squares, m_Base.Square, -17);
				int intSquareValue = 0;
				foreach (Square square in squares)
				{
					intSquareValue += m_aintSquareValues[square.Ordinal];
				}
				intPoints += (intSquareValue >> 2);
*/
				intPoints += _mBase.DefensePoints;

				return intPoints;
			}
		}

		public int ImageIndex
		{
			get { return (_mBase.Player.Colour==Player.EnmColour.White ? 1 : 0); }
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
		}
	}
}

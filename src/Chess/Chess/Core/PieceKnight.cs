namespace Chess.Core
{
	public class PieceKnight: IPieceTop
	{
	    public PieceKnight(Piece pieceBase)
		{
			Base = pieceBase;
		}

		public Piece Base { get; }

	    public string Abbreviation => "N";

	    public Piece.EnmName Name => Piece.EnmName.Knight;

	    public int BasicValue => 3;

	    public int Value => 3250;

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
					intPoints -= Base.TaxiCabDistanceToEnemyKingPenalty()<<4;
				}
				else
				{
					intPoints += MAintSquareValues[Base.Square.Ordinal]<<3;

					if (Base.CanBeDrivenAwayByPawn())
					{
						intPoints-=30;
					}

				}

				intPoints += Base.DefensePoints;

				return intPoints;
			}
		}

		public int ImageIndex => Base.Player.Colour==Player.EnmColour.White ? 7 : 6;

	    public bool CanBeTaken => true;

	    public void GenerateLazyMoves(Moves moves, Moves.EnmMovesType movesType)
		{
			Square square;

			switch (movesType)
			{
				case Moves.EnmMovesType.All:
					square = Board.GetSquare(Base.Square.Ordinal+33); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(Base.Square.Ordinal+18); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(Base.Square.Ordinal-14); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(Base.Square.Ordinal-31); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(Base.Square.Ordinal-33); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(Base.Square.Ordinal-18); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(Base.Square.Ordinal+14); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(Base.Square.Ordinal+31); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=Base.Player.Colour && square.Piece.CanBeTaken))) moves.Add(0, 0, Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
					break;

				case Moves.EnmMovesType.RecapturesPromotions:
				case Moves.EnmMovesType.CapturesChecksPromotions:
					square = Board.GetSquare(Base.Square.Ordinal+33); if ( square!=null && square.Piece!=null && square.Piece.Player.Colour!=Base.Player.Colour && square.Piece.CanBeTaken) moves.Add(0, 0, Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(Base.Square.Ordinal+18); if ( square!=null && square.Piece!=null && square.Piece.Player.Colour!=Base.Player.Colour && square.Piece.CanBeTaken) moves.Add(0, 0, Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(Base.Square.Ordinal-14); if ( square!=null && square.Piece!=null && square.Piece.Player.Colour!=Base.Player.Colour && square.Piece.CanBeTaken) moves.Add(0, 0, Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(Base.Square.Ordinal-31); if ( square!=null && square.Piece!=null && square.Piece.Player.Colour!=Base.Player.Colour && square.Piece.CanBeTaken) moves.Add(0, 0, Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(Base.Square.Ordinal-33); if ( square!=null && square.Piece!=null && square.Piece.Player.Colour!=Base.Player.Colour && square.Piece.CanBeTaken) moves.Add(0, 0, Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(Base.Square.Ordinal-18); if ( square!=null && square.Piece!=null && square.Piece.Player.Colour!=Base.Player.Colour && square.Piece.CanBeTaken) moves.Add(0, 0, Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(Base.Square.Ordinal+14); if ( square!=null && square.Piece!=null && square.Piece.Player.Colour!=Base.Player.Colour && square.Piece.CanBeTaken) moves.Add(0, 0, Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(Base.Square.Ordinal+31); if ( square!=null && square.Piece!=null && square.Piece.Player.Colour!=Base.Player.Colour && square.Piece.CanBeTaken) moves.Add(0, 0, Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
					break;
			}
		}

	}
}

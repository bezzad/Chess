namespace Chess.Core
{
	public class PiecePawn: IPieceTop
	{
	    public PiecePawn(Piece pieceBase)
		{
			Base = pieceBase;
		}

		public Piece Base { get; }

	    public string Abbreviation => "P";

	    public Piece.EnmName Name => Piece.EnmName.Pawn;

	    public int BasicValue => 1;

	    public int Value => Base.Square.Rank==0 || Base.Square.Rank==7 ? 850 : 1000;

	    private static readonly int[] AintAdvancementBonus = {0,0,0,45,75,120,240,999};
	    private static readonly int[] AintFileBonus = {0,4,8,16,16,8,4,0};

		public int PositionalValue
		{
			get
			{
				var intPoints = 0;
				int intIndex;
				Piece piece;

				// PENALITIES

				// Isolated or Doubled pawn
				var blnIsIsolatedLeft = true;
				var blnIsIsolatedRight = true;
				var blnIsDouble = false;
				for (intIndex=Base.Player.Pawns.Count-1; intIndex>=0; intIndex--)
				{
					piece = Base.Player.Pawns.Item(intIndex);
					if (piece.IsInPlay && piece!=Base)
					{
						if (piece.Square.File==Base.Square.File)
						{
							blnIsDouble = true;
						}
						if (piece.Square.File==Base.Square.File-1)
						{
							blnIsIsolatedLeft = false;
						}
						if (piece.Square.File==Base.Square.File+1)
						{
							blnIsIsolatedRight = false;
						}
					}
				}
				if (blnIsIsolatedLeft)
				{
					switch (Base.Square.File)
					{
						case 0: intPoints -=  10; break;
						case 1: intPoints -=  30; break;
						case 2: intPoints -=  40; break;
						case 3: intPoints -=  50; break;
						case 4: intPoints -=  50; break;
						case 5: intPoints -=  40; break;
						case 6: intPoints -=  30; break;
						case 7: intPoints -=  10; break;
					}
				}
				if (blnIsIsolatedRight)
				{
					switch (Base.Square.File)
					{
						case 0: intPoints -=  10; break;
						case 1: intPoints -=  30; break;
						case 2: intPoints -=  40; break;
						case 3: intPoints -=  50; break;
						case 4: intPoints -=  50; break;
						case 5: intPoints -=  40; break;
						case 6: intPoints -=  30; break;
						case 7: intPoints -=  10; break;
					}
				}
				if (blnIsDouble)
				{
					intPoints -= 150;
				}
				else if (blnIsIsolatedLeft && blnIsIsolatedRight)
				{
					intPoints -= 75;
				}

				// Backward pawn
				var blnIsBackward = true;
				if ( blnIsBackward && (piece = Board.GetPiece(Base.Square.Ordinal-1))!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==Base.Player.Colour) blnIsBackward = false;
				if ( blnIsBackward && (piece = Board.GetPiece(Base.Square.Ordinal+1))!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==Base.Player.Colour) blnIsBackward = false;
				if ( blnIsBackward && (piece = Board.GetPiece(Base.Square.Ordinal-Base.Player.PawnAttackLeftOffset ))!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==Base.Player.Colour) blnIsBackward = false;
				if ( blnIsBackward && (piece = Board.GetPiece(Base.Square.Ordinal-Base.Player.PawnAttackRightOffset))!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==Base.Player.Colour) blnIsBackward = false;
				if (blnIsBackward)
				{
					intPoints -= 30;

				}

				// Add further subtraction, then add on a defense bonus
				if (blnIsIsolatedLeft || blnIsIsolatedRight || blnIsBackward)
				{
					switch (Game.Stage)
					{
						case Game.EnmStage.Opening:
							intPoints += (Base.DefensePoints>>1) - 30;
							break;
						case Game.EnmStage.Middle:
							intPoints += (Base.DefensePoints>>2) - 15;
							break;
					}
				}

				if (Game.Stage==Game.EnmStage.Opening)
				{
					//	Pawns on D or E files
					if (Base.Square.File==3 || Base.Square.File==4)
					{
						//	still at rank 2
						if (Base.Player.Colour==Player.EnmColour.White && Base.Square.Rank==1
							||
							Base.Player.Colour==Player.EnmColour.Black && Base.Square.Rank==6
							)
						{
							intPoints -= 300;
						}
					}
				}

				// BONUSES

				// Encourage capturing towards the centre
				intPoints += AintFileBonus[Base.Square.File];

				// Advancement
				var intAdvancementBonus = AintAdvancementBonus[Base.Player.Colour==Player.EnmColour.White ? Base.Square.Rank : 7-Base.Square.Rank];

				if (Game.Stage==Game.EnmStage.End)
				{
					intAdvancementBonus<<=1;
				}

				// Passed Pawns
				var blnIsPassed = true;
				for (intIndex=Base.Player.OtherPlayer.Pawns.Count-1; intIndex>=0; intIndex--)
				{
					piece = Base.Player.OtherPlayer.Pawns.Item(intIndex);
					if (piece.IsInPlay)
					{
						if	(	(
							Base.Player.Colour==Player.EnmColour.White && piece.Square.Rank > Base.Square.Rank
							||
							Base.Player.Colour==Player.EnmColour.Black && piece.Square.Rank < Base.Square.Rank
							)
							&&
							(piece.Square.File==Base.Square.File || piece.Square.File==Base.Square.File-1 || piece.Square.File==Base.Square.File+1)
								
							)
						{
							blnIsPassed = false;
						}
					}
				}

				if (blnIsPassed)
				{
					intPoints += 80;
					intAdvancementBonus <<= 1;
				}

				intPoints += intAdvancementBonus;

				return intPoints;
			}
		}

		public int ImageIndex => Base.Player.Colour==Player.EnmColour.White ? 9 : 8;

	    public bool CanBeTaken => true;

	    private Move.EnmName MoveName(Player.EnmColour colourPlayer, Square squareTo)
		{
			if (colourPlayer==Player.EnmColour.White && squareTo.Rank==7 || colourPlayer==Player.EnmColour.Black && squareTo.Rank==0)
			{
				return Move.EnmName.PawnPromotion;
			}
			else
			{
				return Move.EnmName.Standard;
			}
		}

		public void GenerateLazyMoves(Moves moves, Moves.EnmMovesType movesType)
		{
			Square square;
			var blnIsPromotion = Base.Player.Colour==Player.EnmColour.White && Base.Square.Rank==6
									||
									Base.Player.Colour==Player.EnmColour.Black && Base.Square.Rank==1;

			// Forward one
			if (movesType==Moves.EnmMovesType.All || blnIsPromotion)
			{
				if ( (square = Board.GetSquare(Base.Square.Ordinal+Base.Player.PawnForwardOffset))!=null && square.Piece == null)
				{
					moves.Add(0, 0, blnIsPromotion ? Move.EnmName.PawnPromotion : Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
				}
			}

			// Forward two
			if (movesType==Moves.EnmMovesType.All)
			{
				if (!Base.HasMoved)
				{
					// Check one square ahead is not occupied
					if ( (square = Board.GetSquare(Base.Square.Ordinal+Base.Player.PawnForwardOffset))!=null && square.Piece == null)
					{
						if ( (square = Board.GetSquare(Base.Square.Ordinal+Base.Player.PawnForwardOffset+Base.Player.PawnForwardOffset))!=null && square.Piece == null)
						{
							moves.Add(0, 0, Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
						}
					}
				}
			}

			// Take right
			if ( (square = Board.GetSquare(Base.Square.Ordinal+Base.Player.PawnAttackRightOffset))!=null)
			{
				if (square.Piece != null && square.Piece.Player.Colour!=Base.Player.Colour && square.Piece.CanBeTaken)
				{
					moves.Add(0, 0, Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
				}
			}

			// Take left
			if ( (square = Board.GetSquare(Base.Square.Ordinal+Base.Player.PawnAttackLeftOffset))!=null)
			{
				if (square.Piece != null && square.Piece.Player.Colour!=Base.Player.Colour && square.Piece.CanBeTaken)
				{
					moves.Add(0, 0, Move.EnmName.Standard, Base, Base.Square, square, square.Piece, 0, 0);
				}
			}

			// En Passent 
			if ( 
				Base.Square.Rank==4 && Base.Player.Colour==Player.EnmColour.White 
				||
				Base.Square.Rank==3 && Base.Player.Colour==Player.EnmColour.Black
				)
			{
				Piece piecePassed;
				// Left
				if ((piecePassed = Board.GetPiece(Base.Square.Ordinal-1))!=null && piecePassed.NoOfMoves==1 && piecePassed.LastMoveTurnNo==Game.TurnNo && piecePassed.Name==Piece.EnmName.Pawn && piecePassed.Player.Colour!=Base.Player.Colour)
				{
					square = Board.GetSquare(Base.Square.Ordinal+Base.Player.PawnAttackLeftOffset);
					moves.Add(0, 0, Move.EnmName.EnPassent, Base, Base.Square, square, piecePassed, 0, 0);
				}
				// Right
				if ((piecePassed = Board.GetPiece(Base.Square.Ordinal+1))!=null && piecePassed.NoOfMoves==1 && piecePassed.LastMoveTurnNo==Game.TurnNo && piecePassed.Name==Piece.EnmName.Pawn && piecePassed.Player.Colour!=Base.Player.Colour)
				{
					square = Board.GetSquare(Base.Square.Ordinal+Base.Player.PawnAttackRightOffset);
					moves.Add(0, 0, Move.EnmName.EnPassent, Base, Base.Square, square, piecePassed, 0, 0);
				}
			}

		}

	}
}

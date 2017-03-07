namespace Chess.Core
{
	public class PiecePawn: IPieceTop
	{
	    private readonly Piece _mBase;

		public PiecePawn(Piece pieceBase)
		{
			_mBase = pieceBase;
		}

		public Piece Base
		{
			get { return _mBase; }
		}

		public string Abbreviation
		{
			get {return "P";}
		}

		public Piece.EnmName Name
		{
			get {return Piece.EnmName.Pawn;}
		}

		public int BasicValue
		{
			get { return 1;	}
		}

		public int Value
		{
			get
			{
				return _mBase.Square.Rank==0 || _mBase.Square.Rank==7 ? 850 : 1000;
			}
		}

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
				for (intIndex=_mBase.Player.Pawns.Count-1; intIndex>=0; intIndex--)
				{
					piece = _mBase.Player.Pawns.Item(intIndex);
					if (piece.IsInPlay && piece!=_mBase)
					{
						if (piece.Square.File==_mBase.Square.File)
						{
							blnIsDouble = true;
						}
						if (piece.Square.File==_mBase.Square.File-1)
						{
							blnIsIsolatedLeft = false;
						}
						if (piece.Square.File==_mBase.Square.File+1)
						{
							blnIsIsolatedRight = false;
						}
					}
				}
				if (blnIsIsolatedLeft)
				{
					switch (_mBase.Square.File)
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
					switch (_mBase.Square.File)
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
				if ( blnIsBackward && (piece = Board.GetPiece(_mBase.Square.Ordinal-1                                       ))!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==_mBase.Player.Colour) blnIsBackward = false;
				if ( blnIsBackward && (piece = Board.GetPiece(_mBase.Square.Ordinal+1                                       ))!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==_mBase.Player.Colour) blnIsBackward = false;
				if ( blnIsBackward && (piece = Board.GetPiece(_mBase.Square.Ordinal-_mBase.Player.PawnAttackLeftOffset ))!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==_mBase.Player.Colour) blnIsBackward = false;
				if ( blnIsBackward && (piece = Board.GetPiece(_mBase.Square.Ordinal-_mBase.Player.PawnAttackRightOffset))!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==_mBase.Player.Colour) blnIsBackward = false;
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
							intPoints += (_mBase.DefensePoints>>1) - 30;
							break;
						case Game.EnmStage.Middle:
							intPoints += (_mBase.DefensePoints>>2) - 15;
							break;
					}
				}

				if (Game.Stage==Game.EnmStage.Opening)
				{
					//	Pawns on D or E files
					if (_mBase.Square.File==3 || _mBase.Square.File==4)
					{
						//	still at rank 2
						if (_mBase.Player.Colour==Player.EnmColour.White && _mBase.Square.Rank==1
							||
							_mBase.Player.Colour==Player.EnmColour.Black && _mBase.Square.Rank==6
							)
						{
							intPoints -= 300;
						}
					}
				}

				// BONUSES

				// Encourage capturing towards the centre
				intPoints += AintFileBonus[_mBase.Square.File];

				// Advancement
				var intAdvancementBonus = AintAdvancementBonus[_mBase.Player.Colour==Player.EnmColour.White ? _mBase.Square.Rank : 7-_mBase.Square.Rank];

				if (Game.Stage==Game.EnmStage.End)
				{
					intAdvancementBonus<<=1;
				}

				// Passed Pawns
				var blnIsPassed = true;
				for (intIndex=_mBase.Player.OtherPlayer.Pawns.Count-1; intIndex>=0; intIndex--)
				{
					piece = _mBase.Player.OtherPlayer.Pawns.Item(intIndex);
					if (piece.IsInPlay)
					{
						if	(	(
							_mBase.Player.Colour==Player.EnmColour.White && piece.Square.Rank > _mBase.Square.Rank
							||
							_mBase.Player.Colour==Player.EnmColour.Black && piece.Square.Rank < _mBase.Square.Rank
							)
							&&
							(piece.Square.File==_mBase.Square.File || piece.Square.File==_mBase.Square.File-1 || piece.Square.File==_mBase.Square.File+1)
								
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

		public int ImageIndex
		{
			get { return (_mBase.Player.Colour==Player.EnmColour.White ? 9 : 8); }
		}
	
		public bool CanBeTaken
		{
			get
			{
				return true;
			}
		}

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
			var blnIsPromotion = _mBase.Player.Colour==Player.EnmColour.White && _mBase.Square.Rank==6
									||
									_mBase.Player.Colour==Player.EnmColour.Black && _mBase.Square.Rank==1;

			// Forward one
			if (movesType==Moves.EnmMovesType.All || blnIsPromotion)
			{
				if ( (square = Board.GetSquare(_mBase.Square.Ordinal+_mBase.Player.PawnForwardOffset))!=null && square.Piece == null)
				{
					moves.Add(0, 0, (blnIsPromotion ? Move.EnmName.PawnPromotion : Move.EnmName.Standard), _mBase, _mBase.Square, square, square.Piece, 0, 0);
				}
			}

			// Forward two
			if (movesType==Moves.EnmMovesType.All)
			{
				if (!_mBase.HasMoved)
				{
					// Check one square ahead is not occupied
					if ( (square = Board.GetSquare(_mBase.Square.Ordinal+_mBase.Player.PawnForwardOffset))!=null && square.Piece == null)
					{
						if ( (square = Board.GetSquare(_mBase.Square.Ordinal+_mBase.Player.PawnForwardOffset+_mBase.Player.PawnForwardOffset))!=null && square.Piece == null)
						{
							moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
						}
					}
				}
			}

			// Take right
			if ( (square = Board.GetSquare(_mBase.Square.Ordinal+_mBase.Player.PawnAttackRightOffset))!=null)
			{
				if (square.Piece != null && square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken)
				{
					moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
				}
			}

			// Take left
			if ( (square = Board.GetSquare(_mBase.Square.Ordinal+_mBase.Player.PawnAttackLeftOffset))!=null)
			{
				if (square.Piece != null && square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken)
				{
					moves.Add(0, 0, Move.EnmName.Standard, _mBase, _mBase.Square, square, square.Piece, 0, 0);
				}
			}

			// En Passent 
			if ( 
				_mBase.Square.Rank==4 && _mBase.Player.Colour==Player.EnmColour.White 
				||
				_mBase.Square.Rank==3 && _mBase.Player.Colour==Player.EnmColour.Black
				)
			{
				Piece piecePassed;
				// Left
				if ((piecePassed = Board.GetPiece(_mBase.Square.Ordinal-1))!=null && piecePassed.NoOfMoves==1 && piecePassed.LastMoveTurnNo==Game.TurnNo && piecePassed.Name==Piece.EnmName.Pawn && piecePassed.Player.Colour!=_mBase.Player.Colour)
				{
					square = Board.GetSquare(_mBase.Square.Ordinal+_mBase.Player.PawnAttackLeftOffset);
					moves.Add(0, 0, Move.EnmName.EnPassent, _mBase, _mBase.Square, square, piecePassed, 0, 0);
				}
				// Right
				if ((piecePassed = Board.GetPiece(_mBase.Square.Ordinal+1))!=null && piecePassed.NoOfMoves==1 && piecePassed.LastMoveTurnNo==Game.TurnNo && piecePassed.Name==Piece.EnmName.Pawn && piecePassed.Player.Colour!=_mBase.Player.Colour)
				{
					square = Board.GetSquare(_mBase.Square.Ordinal+_mBase.Player.PawnAttackRightOffset);
					moves.Add(0, 0, Move.EnmName.EnPassent, _mBase, _mBase.Square, square, piecePassed, 0, 0);
				}
			}

		}

	}
}

namespace Chess.Core
{
	public class PiecePawn: IPieceTop
	{
	    private Piece _mBase = null;

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

	    private static int[] _aintAdvancementBonus = {0,0,0,45,75,120,240,999};
	    private static int[] _aintFileBonus = {0,4,8,16,16,8,4,0};

		public int PositionalValue
		{
			get
			{
				int intPoints = 0;
				int intIndex;
				Piece piece;

				// PENALITIES

				// Isolated or Doubled pawn
				bool blnIsIsolatedLeft = true;
				bool blnIsIsolatedRight = true;
				bool blnIsDouble = false;
				for (intIndex=this._mBase.Player.Pawns.Count-1; intIndex>=0; intIndex--)
				{
					piece = this._mBase.Player.Pawns.Item(intIndex);
					if (piece.IsInPlay && piece!=this._mBase)
					{
						if (piece.Square.File==this._mBase.Square.File)
						{
							blnIsDouble = true;
						}
						if (piece.Square.File==this._mBase.Square.File-1)
						{
							blnIsIsolatedLeft = false;
						}
						if (piece.Square.File==this._mBase.Square.File+1)
						{
							blnIsIsolatedRight = false;
						}
					}
				}
				if (blnIsIsolatedLeft)
				{
					switch (this._mBase.Square.File)
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
					switch (this._mBase.Square.File)
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
				bool blnIsBackward = true;
				if ( blnIsBackward && (piece = Board.GetPiece(this._mBase.Square.Ordinal-1                                       ))!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==this._mBase.Player.Colour) blnIsBackward = false;
				if ( blnIsBackward && (piece = Board.GetPiece(this._mBase.Square.Ordinal+1                                       ))!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==this._mBase.Player.Colour) blnIsBackward = false;
				if ( blnIsBackward && (piece = Board.GetPiece(this._mBase.Square.Ordinal-this._mBase.Player.PawnAttackLeftOffset ))!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==this._mBase.Player.Colour) blnIsBackward = false;
				if ( blnIsBackward && (piece = Board.GetPiece(this._mBase.Square.Ordinal-this._mBase.Player.PawnAttackRightOffset))!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==this._mBase.Player.Colour) blnIsBackward = false;
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
					if (this._mBase.Square.File==3 || this._mBase.Square.File==4)
					{
						//	still at rank 2
						if (this._mBase.Player.Colour==Player.EnmColour.White && this._mBase.Square.Rank==1
							||
							this._mBase.Player.Colour==Player.EnmColour.Black && this._mBase.Square.Rank==6
							)
						{
							intPoints -= 300;
						}
					}
				}

				// BONUSES

				// Encourage capturing towards the centre
				intPoints += _aintFileBonus[_mBase.Square.File];

				// Advancement
				int intAdvancementBonus = _aintAdvancementBonus[_mBase.Player.Colour==Player.EnmColour.White ? _mBase.Square.Rank : 7-_mBase.Square.Rank];

				if (Game.Stage==Game.EnmStage.End)
				{
					intAdvancementBonus<<=1;
				}

				// Passed Pawns
				bool blnIsPassed = true;
				for (intIndex=this._mBase.Player.OtherPlayer.Pawns.Count-1; intIndex>=0; intIndex--)
				{
					piece = this._mBase.Player.OtherPlayer.Pawns.Item(intIndex);
					if (piece.IsInPlay)
					{
						if	(	(
							this._mBase.Player.Colour==Player.EnmColour.White && piece.Square.Rank > this._mBase.Square.Rank
							||
							this._mBase.Player.Colour==Player.EnmColour.Black && piece.Square.Rank < this._mBase.Square.Rank
							)
							&&
							(piece.Square.File==this._mBase.Square.File || piece.Square.File==this._mBase.Square.File-1 || piece.Square.File==this._mBase.Square.File+1)
								
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
			get { return (this._mBase.Player.Colour==Player.EnmColour.White ? 9 : 8); }
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
			bool blnIsPromotion = this._mBase.Player.Colour==Player.EnmColour.White && this._mBase.Square.Rank==6
									||
									this._mBase.Player.Colour==Player.EnmColour.Black && this._mBase.Square.Rank==1;

			// Forward one
			if (movesType==Moves.EnmMovesType.All || blnIsPromotion)
			{
				if ( (square = Board.GetSquare(_mBase.Square.Ordinal+_mBase.Player.PawnForwardOffset))!=null && square.Piece == null)
				{
					moves.Add(0, 0, (blnIsPromotion ? Move.EnmName.PawnPromotion : Move.EnmName.Standard), this._mBase, this._mBase.Square, square, square.Piece, 0, 0);
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
							moves.Add(0, 0, Move.EnmName.Standard, this._mBase, this._mBase.Square, square, square.Piece, 0, 0);
						}
					}
				}
			}

			// Take right
			if ( (square = Board.GetSquare(_mBase.Square.Ordinal+_mBase.Player.PawnAttackRightOffset))!=null)
			{
				if (square.Piece != null && square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken)
				{
					moves.Add(0, 0, Move.EnmName.Standard, this._mBase, this._mBase.Square, square, square.Piece, 0, 0);
				}
			}

			// Take left
			if ( (square = Board.GetSquare(_mBase.Square.Ordinal+_mBase.Player.PawnAttackLeftOffset))!=null)
			{
				if (square.Piece != null && square.Piece.Player.Colour!=_mBase.Player.Colour && square.Piece.CanBeTaken)
				{
					moves.Add(0, 0, Move.EnmName.Standard, this._mBase, this._mBase.Square, square, square.Piece, 0, 0);
				}
			}

			// En Passent 
			if ( 
				this._mBase.Square.Rank==4 && this._mBase.Player.Colour==Player.EnmColour.White 
				||
				this._mBase.Square.Rank==3 && this._mBase.Player.Colour==Player.EnmColour.Black
				)
			{
				Piece piecePassed;
				// Left
				if ((piecePassed = Board.GetPiece(_mBase.Square.Ordinal-1))!=null && piecePassed.NoOfMoves==1 && piecePassed.LastMoveTurnNo==Game.TurnNo && piecePassed.Name==Piece.EnmName.Pawn && piecePassed.Player.Colour!=_mBase.Player.Colour)
				{
					square = Board.GetSquare(_mBase.Square.Ordinal+_mBase.Player.PawnAttackLeftOffset);
					moves.Add(0, 0, Move.EnmName.EnPassent, this._mBase, this._mBase.Square, square, piecePassed, 0, 0);
				}
				// Right
				if ((piecePassed = Board.GetPiece(_mBase.Square.Ordinal+1))!=null && piecePassed.NoOfMoves==1 && piecePassed.LastMoveTurnNo==Game.TurnNo && piecePassed.Name==Piece.EnmName.Pawn && piecePassed.Player.Colour!=_mBase.Player.Colour)
				{
					square = Board.GetSquare(_mBase.Square.Ordinal+_mBase.Player.PawnAttackRightOffset);
					moves.Add(0, 0, Move.EnmName.EnPassent, this._mBase, this._mBase.Square, square, piecePassed, 0, 0);
				}
			}

		}

	}
}

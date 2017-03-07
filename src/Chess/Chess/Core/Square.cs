using System;

namespace Chess.Core
{
	public class Square
	{
	    private static int[] _mAintVectors =
		{
			0,  0,  0,  0,  0,  0,  0,  0,   -15,-17,  0,  0,  0,  0,  0,  0,
			-16,  0,  0,  0,  0,  0,  0,-15,     0,  0,-17,  0,  0,  0,  0,  0,
			-16,  0,  0,  0,  0,  0,-15,  0,     0,  0,  0,-17,  0,  0,  0,  0,
			-16,  0,  0,  0,  0,-15,  0,  0,     0,  0,  0,  0,-17,  0,  0,  0,
			-16,  0,  0,  0,-15,  0,  0,  0,     0,  0,  0,  0,  0,-17,  0,  0,
			-16,  0,  0,-15,  0,  0,  0,  0,     0,  0,  0,  0,  0,  0,-17,100,
			-16,100,-15,  0,  0,  0,  0,  0 ,    0,  0,  0,  0,  0,  0,100,-17,
			-16,-15,100,  0,  0,  0,  0,  0 ,    0, -1, -1, -1, -1, -1, -1, -1,
			0,
			1,  1,  1,  1,  1,  1,  1,  0,     0,  0,  0,  0,  0,100, 15, 16,
			17,100,  0,  0,  0,  0,  0,  0,     0,  0,  0,  0,  0, 15,100, 16,
			100, 17,  0,  0,  0,  0,  0,  0,     0,  0,  0,  0, 15,  0,  0, 16,
			0,  0, 17,  0,  0,  0,  0,  0,     0,  0,  0, 15,  0,  0,  0, 16,
			0,  0,  0, 17,  0,  0,  0,  0,     0,  0, 15,  0,  0,  0,  0, 16,
			0,  0,  0,  0, 17,  0,  0,  0,     0, 15,  0,  0,  0,  0,  0, 16,
			0,  0,  0,  0,  0, 17,  0,  0 ,   15,  0,  0,  0,  0,  0,  0, 16,
			0,  0,  0,  0,  0,  0, 17, 15 ,    0,  0,  0,  0,  0,  0,  0,  0
		};

	    private static char[] _mAintMinorAttackers =
		{
			'.','.','.','.','.','.','.','.',   'B','B','.','.','.','.','.','.',
			'R','.','.','.','.','.','.','B',   '.','.','B','.','.','.','.','.',
			'R','.','.','.','.','.','B','.',   '.','.','.','B','.','.','.','.',
			'R','.','.','.','.','B','.','.',   '.','.','.','.','B','.','.','.',
			'R','.','.','.','B','.','.','.',   '.','.','.','.','.','B','.','.',
			'R','.','.','B','.','.','.','.',   '.','.','.','.','.','.','B','N',
			'R','N','B','.','.','.','.','.' ,  '.','.','.','.','.','.','N','B',
			'R','B','N','.','.','.','.','.' ,  '.','R','R','R','R','R','R','R',
			'.',
			'R','R','R','R','R','R','R','.',   '.','.','.','.','.','N','B','R',
			'B','N','.','.','.','.','.','.',   '.','.','.','.','.','B','N','R',
			'N','B','.','.','.','.','.','.',   '.','.','.','.','B','.','.','R',
			'.','.','B','.','.','.','.','.',   '.','.','.','B','.','.','.','R',
			'.','.','.','B','.','.','.','.',   '.','.','B','.','.','.','.','R',
			'.','.','.','.','B','.','.','.',   '.','B','.','.','.','.','.','R',
			'.','.','.','.','.','B','.','.' ,  'B','.','.','.','.','.','.','R',
			'.','.','.','.','.','.','B','B' ,  '.','.','.','.','.','.','.','.'
		};

	    private static char[] _mAintQueenAttackers =
		{
			'.','.','.','.','.','.','.','.',   'Q','Q','.','.','.','.','.','.',
			'Q','.','.','.','.','.','.','Q',   '.','.','Q','.','.','.','.','.',
			'Q','.','.','.','.','.','Q','.',   '.','.','.','Q','.','.','.','.',
			'Q','.','.','.','.','Q','.','.',   '.','.','.','.','Q','.','.','.',
			'Q','.','.','.','Q','.','.','.',   '.','.','.','.','.','Q','.','.',
			'Q','.','.','Q','.','.','.','.',   '.','.','.','.','.','.','Q','.',
			'Q','.','Q','.','.','.','.','.' ,  '.','.','.','.','.','.','.','Q',
			'Q','Q','.','.','.','.','.','.' ,  '.','Q','Q','Q','Q','Q','Q','Q',
			'.',
			'Q','Q','Q','Q','Q','Q','Q','.',   '.','.','.','.','.','.','Q','Q',
			'Q','.','.','.','.','.','.','.',   '.','.','.','.','.','Q','.','Q',
			'.','Q','.','.','.','.','.','.',   '.','.','.','.','Q','.','.','Q',
			'.','.','Q','.','.','.','.','.',   '.','.','.','Q','.','.','.','Q',
			'.','.','.','Q','.','.','.','.',   '.','.','Q','.','.','.','.','Q',
			'.','.','.','.','Q','.','.','.',   '.','Q','.','.','.','.','.','Q',
			'.','.','.','.','.','Q','.','.' ,  'Q','.','.','.','.','.','.','Q',
			'.','.','.','.','.','.','Q','Q' ,  '.','.','.','.','.','.','.','.'
		};

	    private static char[] _mAintKingAttackers =
		{
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.' ,  '.','.','.','.','.','.','.','K',
			'K','K','.','.','.','.','.','.' ,  '.','.','.','.','.','.','.','K',
			'.',
			'K','.','.','.','.','.','.','.',   '.','.','.','.','.','.','K','K',
			'K','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.' ,  '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.' ,  '.','.','.','.','.','.','.','.'
		};

		private static int[] _mAintSquareValues =
		{
			1, 1, 1, 1, 1, 1, 1, 1,    0,0,0,0,0,0,0,0,
			1,10,10,10,10,10,10, 1,    0,0,0,0,0,0,0,0,
			1,10,25,25,25,25,10, 1,    0,0,0,0,0,0,0,0,
			1,10,25,50,50,25,10, 1,    0,0,0,0,0,0,0,0,
			1,10,25,50,50,25,10, 1,    0,0,0,0,0,0,0,0,
			1,10,25,25,25,25,10, 1,    0,0,0,0,0,0,0,0,
			1,10,10,10,10,10,10, 1 ,   0,0,0,0,0,0,0,0,
			1, 1, 1, 1, 1, 1, 1, 1 ,   0,0,0,0,0,0,0,0
		};

		public enum EnmColour
		{
				White
			,	Black
		}

		private EnmColour _mColour;
		private Piece _mPiece;
		private int _mIntFile;
		private int _mIntRank;
		private int _mIntOrdinal;

		public Square(int ordinal)
		{
			_mIntOrdinal = ordinal;
			_mIntFile = ordinal % Board.MatrixWidth;
			_mIntRank = ordinal / Board.MatrixWidth;

			if (_mIntFile==0 || _mIntFile==2 || _mIntFile==4 || _mIntFile==6)
			{
				if (_mIntRank==0 || _mIntRank==2 || _mIntRank==4 || _mIntRank==6)
				{
					_mColour = EnmColour.Black;
				}
				else
				{
					_mColour = EnmColour.White;
				}
			}
			else
			{
				if (_mIntRank==0 || _mIntRank==2 || _mIntRank==4 || _mIntRank==6)
				{
					_mColour = EnmColour.White;
				}
				else
				{
					_mColour = EnmColour.Black;
				}
			}

		}

		public int Value
		{
			get
			{
				return _mAintSquareValues[Ordinal];
			}
		}

		public EnmColour Colour
		{
			get { return _mColour; }
		}
	
		public int File
		{
			get { return _mIntFile; }
		}

		public int Rank
		{
			get { return _mIntRank; }
		}

		public int Ordinal
		{
			get { return _mIntOrdinal; }
		}

		public ulong HashCodeA
		{
			get
			{
				return Piece==null ? 0UL : Piece.HashCodeAForSquareOrdinal(Ordinal) ;
			}
		}

		public ulong HashCodeB
		{
			get
			{
				return Piece==null ? 0UL : Piece.HashCodeBForSquareOrdinal(Ordinal) ;
			}
		}

		public string FileName
		{
			get
			{
				string[] fileNames = {"a", "b", "c", "d", "e", "f", "g", "h"};
				return fileNames[_mIntFile];
			}
		}

		public string RankName
		{
			get
			{
				return (_mIntRank+1).ToString();
			}
		}

		public string Name
		{
			get
			{
				return FileName + RankName;
			}
		}

		public Piece Piece
		{
			get	{ return _mPiece; }
			set {  _mPiece = value; }
		}

		public bool CanBeMovedToBy(Player player)
		{
			Piece piece;

			// Pawns
			piece = Board.GetPiece( _mIntOrdinal-player.PawnAttackLeftOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) return true;
			piece = Board.GetPiece( _mIntOrdinal-player.PawnAttackRightOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) return true;

			// Knights
			if (player.QueensKnight.IsInPlay && _mAintMinorAttackers[_mIntOrdinal-player.QueensKnight.Square.Ordinal+128]=='N') return true;
			if (player.KingsKnight.IsInPlay && _mAintMinorAttackers[_mIntOrdinal-player.KingsKnight.Square.Ordinal+128]=='N') return true;

			// Bishops
			if (player.QueensBishop.IsInPlay && _mAintMinorAttackers[_mIntOrdinal-player.QueensBishop.Square.Ordinal+128]=='B' && CanSlideToHereFrom(player.QueensBishop.Square, _mAintVectors[_mIntOrdinal-player.QueensBishop.Square.Ordinal+128]))  return true;
			if (player.KingsBishop.IsInPlay && _mAintMinorAttackers[_mIntOrdinal-player.KingsBishop.Square.Ordinal+128]=='B' && CanSlideToHereFrom(player.KingsBishop.Square, _mAintVectors[_mIntOrdinal-player.KingsBishop.Square.Ordinal+128]))  return true;
				
			// Rooks
			if (player.QueensRook.IsInPlay && _mAintMinorAttackers[_mIntOrdinal-player.QueensRook.Square.Ordinal+128]=='R' && CanSlideToHereFrom(player.QueensRook.Square, _mAintVectors[_mIntOrdinal-player.QueensRook.Square.Ordinal+128]))  return true;
			if (player.KingsRook.IsInPlay && _mAintMinorAttackers[_mIntOrdinal-player.KingsRook.Square.Ordinal+128]=='R' && CanSlideToHereFrom(player.KingsRook.Square, _mAintVectors[_mIntOrdinal-player.KingsRook.Square.Ordinal+128]))  return true;

			// Queen
			if (player.Queen.IsInPlay && _mAintQueenAttackers[_mIntOrdinal-player.Queen.Square.Ordinal+128]=='Q' && CanSlideToHereFrom(player.Queen.Square, _mAintVectors[_mIntOrdinal-player.Queen.Square.Ordinal+128]))  return true;

			// King
			if (_mAintKingAttackers[_mIntOrdinal-player.King.Square.Ordinal+128]=='K')  return true;

			return false;
		}
	
		public void AttackerMoveList(Moves moves, Player player)
		{
			Piece piece;

			// Pawns
			piece = Board.GetPiece( _mIntOrdinal-player.PawnAttackLeftOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.EnmName.Standard, Board.GetPiece(_mIntOrdinal-player.PawnAttackLeftOffset), Board.GetSquare(_mIntOrdinal-player.PawnAttackLeftOffset), this, Piece, 0, 0);
			piece = Board.GetPiece( _mIntOrdinal-player.PawnAttackRightOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.EnmName.Standard, Board.GetPiece(_mIntOrdinal-player.PawnAttackRightOffset), Board.GetSquare(_mIntOrdinal-player.PawnAttackRightOffset), this, Piece, 0, 0);

			// Knights
			if (player.QueensKnight.IsInPlay && _mAintMinorAttackers[_mIntOrdinal-player.QueensKnight.Square.Ordinal+128]=='N') moves.Add(0, 0, Move.EnmName.Standard, player.QueensKnight, player.QueensKnight.Square, this, Piece, 0, 0);
			if (player.KingsKnight.IsInPlay && _mAintMinorAttackers[_mIntOrdinal-player.KingsKnight.Square.Ordinal+128]=='N') moves.Add(0, 0, Move.EnmName.Standard, player.KingsKnight, player.KingsKnight.Square, this, Piece, 0, 0);

			// Bishops
			if (player.QueensBishop.IsInPlay && _mAintMinorAttackers[_mIntOrdinal-player.QueensBishop.Square.Ordinal+128]=='B' && CanSlideToHereFrom(player.QueensBishop.Square, _mAintVectors[_mIntOrdinal-player.QueensBishop.Square.Ordinal+128])) moves.Add(0, 0, Move.EnmName.Standard, player.QueensBishop, player.QueensBishop.Square, this, Piece, 0, 0);
			if (player.KingsBishop.IsInPlay && _mAintMinorAttackers[_mIntOrdinal-player.KingsBishop.Square.Ordinal+128]=='B' && CanSlideToHereFrom(player.KingsBishop.Square, _mAintVectors[_mIntOrdinal-player.KingsBishop.Square.Ordinal+128])) moves.Add(0, 0, Move.EnmName.Standard, player.KingsBishop, player.KingsBishop.Square, this, Piece, 0, 0);
				
			// Rooks
			if (player.QueensRook.IsInPlay && _mAintMinorAttackers[_mIntOrdinal-player.QueensRook.Square.Ordinal+128]=='R' && CanSlideToHereFrom(player.QueensRook.Square, _mAintVectors[_mIntOrdinal-player.QueensRook.Square.Ordinal+128])) moves.Add(0, 0, Move.EnmName.Standard, player.QueensRook, player.QueensRook.Square, this, Piece, 0, 0);
			if (player.KingsRook.IsInPlay && _mAintMinorAttackers[_mIntOrdinal-player.KingsRook.Square.Ordinal+128]=='R' && CanSlideToHereFrom(player.KingsRook.Square, _mAintVectors[_mIntOrdinal-player.KingsRook.Square.Ordinal+128])) moves.Add(0, 0, Move.EnmName.Standard, player.KingsRook, player.KingsRook.Square, this, Piece, 0, 0);

			// Queen
			if (player.Queen.IsInPlay && _mAintQueenAttackers[_mIntOrdinal-player.Queen.Square.Ordinal+128]=='Q' && CanSlideToHereFrom(player.Queen.Square, _mAintVectors[_mIntOrdinal-player.Queen.Square.Ordinal+128])) moves.Add(0, 0, Move.EnmName.Standard, player.Queen, player.Queen.Square, this, Piece, 0, 0);

			// King
			if (_mAintKingAttackers[_mIntOrdinal-player.King.Square.Ordinal+128]=='K')moves.Add(0, 0, Move.EnmName.Standard, player.King, player.King.Square, this, Piece, 0, 0);
		}

		public bool CanSlideToHereFrom(Square squareStart, int offset)
		{
			int intOrdinal = squareStart.Ordinal;
			Square square;

			intOrdinal += offset;
			while ( (square = Board.GetSquare(intOrdinal))!=null )
			{
				if ( square==this )
				{
					return true;
				}
				if ( square.Piece!=null )
				{
					return false;
				}
				intOrdinal += offset;
			}
			throw new ApplicationException("CanSlideToHereFrom: Hit edge of board!");
		}

		public int DefencePointsFor(Player player)
		{
			Piece piece;
			int value = 0;
			int bestValue = 0;

			// Pawn
			piece = Board.GetPiece( _mIntOrdinal-player.PawnAttackLeftOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( _mIntOrdinal-player.PawnAttackRightOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) return piece.Value;
				
			// Knight
			piece = Board.GetPiece( _mIntOrdinal+33 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( _mIntOrdinal+18 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( _mIntOrdinal-14 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( _mIntOrdinal-31 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( _mIntOrdinal-33 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( _mIntOrdinal-18 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( _mIntOrdinal+14 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( _mIntOrdinal+31 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;

			// Bishop & Queen
			piece = Board.LinesFirstPiece(player.Colour, Piece.EnmName.Bishop, this, 15); value = piece!=null ? piece.Value : 0; if (value>0 && value <9000 ) return value; if (value>0) bestValue=value;
			piece = Board.LinesFirstPiece(player.Colour, Piece.EnmName.Bishop, this, 17); value = piece!=null ? piece.Value : 0; if (value>0 && value <9000 ) return value; if (value>0) bestValue=value;
			piece = Board.LinesFirstPiece(player.Colour, Piece.EnmName.Bishop, this, -15); value = piece!=null ? piece.Value : 0; if (value>0 && value <9000 ) return value; if (value>0) bestValue=value;
			piece = Board.LinesFirstPiece(player.Colour, Piece.EnmName.Bishop, this, -17); value = piece!=null ? piece.Value : 0; if (value>0 && value <9000 ) return value; if (value>0) bestValue=value;

			// Rook & Queen
			piece = Board.LinesFirstPiece(player.Colour, Piece.EnmName.Rook, this, 1); value = piece!=null ? piece.Value : 0; if (value>0 && value <9000 ) return value; if (value>0) bestValue=value;
			piece = Board.LinesFirstPiece(player.Colour, Piece.EnmName.Rook, this, -1); value = piece!=null ? piece.Value : 0; if (value>0 && value <9000 ) return value; if (value>0) bestValue=value;
			piece = Board.LinesFirstPiece(player.Colour, Piece.EnmName.Rook, this, 16); value = piece!=null ? piece.Value : 0; if (value>0 && value <9000 ) return value; if (value>0) bestValue=value;
			piece = Board.LinesFirstPiece(player.Colour, Piece.EnmName.Rook, this, -16); value = piece!=null ? piece.Value : 0; if (value>0 && value <9000 ) return value; if (value>0) bestValue=value;

			if (bestValue > 0) return bestValue; // This means a queen was found, but not a Bishop or Rook

			// King!
			piece = Board.GetPiece( _mIntOrdinal+16 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( _mIntOrdinal+17 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( _mIntOrdinal+1  ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( _mIntOrdinal-15 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( _mIntOrdinal-16 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( _mIntOrdinal-17 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( _mIntOrdinal-1  ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( _mIntOrdinal+15 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece.Value; 

			return 15000;
		}

		public Piece DefencedBy(Player player)
		{
			Piece piece;
			Piece pieceBest = null;

			// Pawn
			piece = Board.GetPiece( _mIntOrdinal-player.PawnAttackLeftOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( _mIntOrdinal-player.PawnAttackRightOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) return piece;
				
			// Knight
			piece = Board.GetPiece( _mIntOrdinal+33 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( _mIntOrdinal+18 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( _mIntOrdinal-14 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( _mIntOrdinal-31 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( _mIntOrdinal-33 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( _mIntOrdinal-18 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( _mIntOrdinal+14 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( _mIntOrdinal+31 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece;

			// Bishop & Queen
			piece = Board.LinesFirstPiece(player.Colour, Piece.EnmName.Bishop, this, 15); if (piece!=null) { switch (piece.Name) { case Piece.EnmName.Bishop: return piece; case Piece.EnmName.Queen: pieceBest=piece; break; } }
			piece = Board.LinesFirstPiece(player.Colour, Piece.EnmName.Bishop, this, 17); if (piece!=null) { switch (piece.Name) { case Piece.EnmName.Bishop: return piece; case Piece.EnmName.Queen: pieceBest=piece; break; } }
			piece = Board.LinesFirstPiece(player.Colour, Piece.EnmName.Bishop, this, -15); if (piece!=null) { switch (piece.Name) { case Piece.EnmName.Bishop: return piece; case Piece.EnmName.Queen: pieceBest=piece; break; } }
			piece = Board.LinesFirstPiece(player.Colour, Piece.EnmName.Bishop, this, -17); if (piece!=null) { switch (piece.Name) { case Piece.EnmName.Bishop: return piece; case Piece.EnmName.Queen: pieceBest=piece; break; } }

			// Rook & Queen
			piece = Board.LinesFirstPiece(player.Colour, Piece.EnmName.Rook, this, 1); if (piece!=null) { switch (piece.Name) { case Piece.EnmName.Rook: return piece; case Piece.EnmName.Queen: pieceBest=piece; break; } }
			piece = Board.LinesFirstPiece(player.Colour, Piece.EnmName.Rook, this, -1); if (piece!=null) { switch (piece.Name) { case Piece.EnmName.Rook: return piece; case Piece.EnmName.Queen: pieceBest=piece; break; } }
			piece = Board.LinesFirstPiece(player.Colour, Piece.EnmName.Rook, this, 16); if (piece!=null) { switch (piece.Name) { case Piece.EnmName.Rook: return piece; case Piece.EnmName.Queen: pieceBest=piece; break; } }
			piece = Board.LinesFirstPiece(player.Colour, Piece.EnmName.Rook, this, -16); if (piece!=null) { switch (piece.Name) { case Piece.EnmName.Rook: return piece; case Piece.EnmName.Queen: pieceBest=piece; break; } }

			if (pieceBest!=null) return pieceBest; // This means a queen was found, but not a Bishop or Rook

			// King!
			piece = Board.GetPiece( _mIntOrdinal+16 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( _mIntOrdinal+17 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( _mIntOrdinal+1  ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( _mIntOrdinal-15 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( _mIntOrdinal-16 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( _mIntOrdinal-17 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( _mIntOrdinal-1  ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( _mIntOrdinal+15 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece; 

			return null;
		}

		public Pieces AttackerPieceList(Player player)
		{
			Piece piece;
			Pieces pieces = new Pieces(player);

			// Pawn
			piece = Board.GetPiece( _mIntOrdinal-player.PawnAttackLeftOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( _mIntOrdinal-player.PawnAttackRightOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) pieces.Add(piece);
				
			// Knight
			piece = Board.GetPiece( _mIntOrdinal+33 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( _mIntOrdinal+18 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( _mIntOrdinal-14 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( _mIntOrdinal-31 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( _mIntOrdinal-33 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( _mIntOrdinal-18 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( _mIntOrdinal+14 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( _mIntOrdinal+31 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);

			// Bishop & Queen
			if (Board.LinesFirstPiece(player.Colour, Piece.EnmName.Bishop, this, 15)!=null) pieces.Add(piece);
			if (Board.LinesFirstPiece(player.Colour, Piece.EnmName.Bishop, this, 17)!=null) pieces.Add(piece);
			if (Board.LinesFirstPiece(player.Colour, Piece.EnmName.Bishop, this, -15)!=null) pieces.Add(piece);
			if (Board.LinesFirstPiece(player.Colour, Piece.EnmName.Bishop, this, -17)!=null) pieces.Add(piece);
				
			// Rook & Queen
			if (Board.LinesFirstPiece(player.Colour, Piece.EnmName.Rook, this, 1)!=null) pieces.Add(piece);
			if (Board.LinesFirstPiece(player.Colour, Piece.EnmName.Rook, this, -1)!=null) pieces.Add(piece);
			if (Board.LinesFirstPiece(player.Colour, Piece.EnmName.Rook, this, 16)!=null) pieces.Add(piece);
			if (Board.LinesFirstPiece(player.Colour, Piece.EnmName.Rook, this, -16)!=null) pieces.Add(piece);

			// King!
			piece = Board.GetPiece( _mIntOrdinal+16 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( _mIntOrdinal+17 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( _mIntOrdinal+1  ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( _mIntOrdinal-15 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( _mIntOrdinal-16 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( _mIntOrdinal-17 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( _mIntOrdinal-1  ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( _mIntOrdinal+15 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 

			return pieces;
		}



	}
}

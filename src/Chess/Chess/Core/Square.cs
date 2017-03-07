using System;

namespace Chess.Core
{
	public class Square
	{
	    private static readonly int[] MAintVectors =
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

	    private static readonly char[] MAintMinorAttackers =
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

	    private static readonly char[] MAintQueenAttackers =
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

	    private static readonly char[] MAintKingAttackers =
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

		private static readonly int[] MAintSquareValues =
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

	    public Square(int ordinal)
		{
			Ordinal = ordinal;
			File = ordinal % Board.MatrixWidth;
			Rank = ordinal / Board.MatrixWidth;

			if (File==0 || File==2 || File==4 || File==6)
			{
				if (Rank==0 || Rank==2 || Rank==4 || Rank==6)
				{
					Colour = EnmColour.Black;
				}
				else
				{
					Colour = EnmColour.White;
				}
			}
			else
			{
				if (Rank==0 || Rank==2 || Rank==4 || Rank==6)
				{
					Colour = EnmColour.White;
				}
				else
				{
					Colour = EnmColour.Black;
				}
			}

		}

		public int Value => MAintSquareValues[Ordinal];

	    public EnmColour Colour { get; }

	    public int File { get; }

	    public int Rank { get; }

	    public int Ordinal { get; }

	    public ulong HashCodeA => Piece==null ? 0UL : Piece.HashCodeAForSquareOrdinal(Ordinal);

	    public ulong HashCodeB => Piece==null ? 0UL : Piece.HashCodeBForSquareOrdinal(Ordinal);

	    public string FileName
		{
			get
			{
				string[] fileNames = {"a", "b", "c", "d", "e", "f", "g", "h"};
				return fileNames[File];
			}
		}

		public string RankName => (Rank+1).ToString();

	    public string Name => FileName + RankName;

	    public Piece Piece { get; set; }

	    public bool CanBeMovedToBy(Player player)
		{
			Piece piece;

			// Pawns
			piece = Board.GetPiece( Ordinal-player.PawnAttackLeftOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) return true;
			piece = Board.GetPiece( Ordinal-player.PawnAttackRightOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) return true;

			// Knights
			if (player.QueensKnight.IsInPlay && MAintMinorAttackers[Ordinal-player.QueensKnight.Square.Ordinal+128]=='N') return true;
			if (player.KingsKnight.IsInPlay && MAintMinorAttackers[Ordinal-player.KingsKnight.Square.Ordinal+128]=='N') return true;

			// Bishops
			if (player.QueensBishop.IsInPlay && MAintMinorAttackers[Ordinal-player.QueensBishop.Square.Ordinal+128]=='B' && CanSlideToHereFrom(player.QueensBishop.Square, MAintVectors[Ordinal-player.QueensBishop.Square.Ordinal+128]))  return true;
			if (player.KingsBishop.IsInPlay && MAintMinorAttackers[Ordinal-player.KingsBishop.Square.Ordinal+128]=='B' && CanSlideToHereFrom(player.KingsBishop.Square, MAintVectors[Ordinal-player.KingsBishop.Square.Ordinal+128]))  return true;
				
			// Rooks
			if (player.QueensRook.IsInPlay && MAintMinorAttackers[Ordinal-player.QueensRook.Square.Ordinal+128]=='R' && CanSlideToHereFrom(player.QueensRook.Square, MAintVectors[Ordinal-player.QueensRook.Square.Ordinal+128]))  return true;
			if (player.KingsRook.IsInPlay && MAintMinorAttackers[Ordinal-player.KingsRook.Square.Ordinal+128]=='R' && CanSlideToHereFrom(player.KingsRook.Square, MAintVectors[Ordinal-player.KingsRook.Square.Ordinal+128]))  return true;

			// Queen
			if (player.Queen.IsInPlay && MAintQueenAttackers[Ordinal-player.Queen.Square.Ordinal+128]=='Q' && CanSlideToHereFrom(player.Queen.Square, MAintVectors[Ordinal-player.Queen.Square.Ordinal+128]))  return true;

			// King
			if (MAintKingAttackers[Ordinal-player.King.Square.Ordinal+128]=='K')  return true;

			return false;
		}
	
		public void AttackerMoveList(Moves moves, Player player)
		{
		    // Pawns
			var piece = Board.GetPiece( Ordinal-player.PawnAttackLeftOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.EnmName.Standard, Board.GetPiece(Ordinal-player.PawnAttackLeftOffset), Board.GetSquare(Ordinal-player.PawnAttackLeftOffset), this, Piece, 0, 0);
			piece = Board.GetPiece( Ordinal-player.PawnAttackRightOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.EnmName.Standard, Board.GetPiece(Ordinal-player.PawnAttackRightOffset), Board.GetSquare(Ordinal-player.PawnAttackRightOffset), this, Piece, 0, 0);

			// Knights
			if (player.QueensKnight.IsInPlay && MAintMinorAttackers[Ordinal-player.QueensKnight.Square.Ordinal+128]=='N') moves.Add(0, 0, Move.EnmName.Standard, player.QueensKnight, player.QueensKnight.Square, this, Piece, 0, 0);
			if (player.KingsKnight.IsInPlay && MAintMinorAttackers[Ordinal-player.KingsKnight.Square.Ordinal+128]=='N') moves.Add(0, 0, Move.EnmName.Standard, player.KingsKnight, player.KingsKnight.Square, this, Piece, 0, 0);

			// Bishops
			if (player.QueensBishop.IsInPlay && MAintMinorAttackers[Ordinal-player.QueensBishop.Square.Ordinal+128]=='B' && CanSlideToHereFrom(player.QueensBishop.Square, MAintVectors[Ordinal-player.QueensBishop.Square.Ordinal+128])) moves.Add(0, 0, Move.EnmName.Standard, player.QueensBishop, player.QueensBishop.Square, this, Piece, 0, 0);
			if (player.KingsBishop.IsInPlay && MAintMinorAttackers[Ordinal-player.KingsBishop.Square.Ordinal+128]=='B' && CanSlideToHereFrom(player.KingsBishop.Square, MAintVectors[Ordinal-player.KingsBishop.Square.Ordinal+128])) moves.Add(0, 0, Move.EnmName.Standard, player.KingsBishop, player.KingsBishop.Square, this, Piece, 0, 0);
				
			// Rooks
			if (player.QueensRook.IsInPlay && MAintMinorAttackers[Ordinal-player.QueensRook.Square.Ordinal+128]=='R' && CanSlideToHereFrom(player.QueensRook.Square, MAintVectors[Ordinal-player.QueensRook.Square.Ordinal+128])) moves.Add(0, 0, Move.EnmName.Standard, player.QueensRook, player.QueensRook.Square, this, Piece, 0, 0);
			if (player.KingsRook.IsInPlay && MAintMinorAttackers[Ordinal-player.KingsRook.Square.Ordinal+128]=='R' && CanSlideToHereFrom(player.KingsRook.Square, MAintVectors[Ordinal-player.KingsRook.Square.Ordinal+128])) moves.Add(0, 0, Move.EnmName.Standard, player.KingsRook, player.KingsRook.Square, this, Piece, 0, 0);

			// Queen
			if (player.Queen.IsInPlay && MAintQueenAttackers[Ordinal-player.Queen.Square.Ordinal+128]=='Q' && CanSlideToHereFrom(player.Queen.Square, MAintVectors[Ordinal-player.Queen.Square.Ordinal+128])) moves.Add(0, 0, Move.EnmName.Standard, player.Queen, player.Queen.Square, this, Piece, 0, 0);

			// King
			if (MAintKingAttackers[Ordinal-player.King.Square.Ordinal+128]=='K')moves.Add(0, 0, Move.EnmName.Standard, player.King, player.King.Square, this, Piece, 0, 0);
		}

		public bool CanSlideToHereFrom(Square squareStart, int offset)
		{
			var intOrdinal = squareStart.Ordinal;
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
		    var bestValue = 0;

			// Pawn
			var piece = Board.GetPiece( Ordinal-player.PawnAttackLeftOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( Ordinal-player.PawnAttackRightOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) return piece.Value;
				
			// Knight
			piece = Board.GetPiece( Ordinal+33 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( Ordinal+18 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( Ordinal-14 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( Ordinal-31 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( Ordinal-33 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( Ordinal-18 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( Ordinal+14 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( Ordinal+31 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;

			// Bishop & Queen
			piece = Board.LinesFirstPiece(player.Colour, Piece.EnmName.Bishop, this, 15); var value = piece!=null ? piece.Value : 0; if (value>0 && value <9000 ) return value; if (value>0) bestValue=value;
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
			piece = Board.GetPiece( Ordinal+16 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( Ordinal+17 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( Ordinal+1  ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( Ordinal-15 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( Ordinal-16 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( Ordinal-17 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( Ordinal-1  ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( Ordinal+15 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece.Value; 

			return 15000;
		}

		public Piece DefencedBy(Player player)
		{
		    Piece pieceBest = null;

			// Pawn
			var piece = Board.GetPiece( Ordinal-player.PawnAttackLeftOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( Ordinal-player.PawnAttackRightOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) return piece;
				
			// Knight
			piece = Board.GetPiece( Ordinal+33 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( Ordinal+18 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( Ordinal-14 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( Ordinal-31 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( Ordinal-33 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( Ordinal-18 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( Ordinal+14 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( Ordinal+31 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) return piece;

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
			piece = Board.GetPiece( Ordinal+16 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( Ordinal+17 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( Ordinal+1  ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( Ordinal-15 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( Ordinal-16 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( Ordinal-17 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( Ordinal-1  ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( Ordinal+15 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) return piece; 

			return null;
		}

		public Pieces AttackerPieceList(Player player)
		{
			Piece piece;
			var pieces = new Pieces(player);

			// Pawn
			piece = Board.GetPiece( Ordinal-player.PawnAttackLeftOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( Ordinal-player.PawnAttackRightOffset ); if (piece!=null && piece.Name==Piece.EnmName.Pawn && piece.Player.Colour==player.Colour) pieces.Add(piece);
				
			// Knight
			piece = Board.GetPiece( Ordinal+33 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( Ordinal+18 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( Ordinal-14 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( Ordinal-31 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( Ordinal-33 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( Ordinal-18 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( Ordinal+14 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( Ordinal+31 ); if (piece!=null && piece.Name==Piece.EnmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);

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
			piece = Board.GetPiece( Ordinal+16 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( Ordinal+17 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( Ordinal+1  ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( Ordinal-15 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( Ordinal-16 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( Ordinal-17 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( Ordinal-1  ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( Ordinal+15 ); if (piece!=null && piece.Name==Piece.EnmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 

			return pieces;
		}



	}
}

using System;

namespace Chess.Core
{
	public class Board
	{
		public const byte RankCount = 8;
		public const byte FileCount = 8;
		public const byte MatrixWidth = 16;
		public const byte SquareCount = 128;
		public static ulong HashCodeA;
		public static ulong HashCodeB;
		public static ulong PawnHashCodeA;
		public static ulong PawnHashCodeB;

	    private static readonly Square[] MArrSquare = new Square[RankCount * MatrixWidth];

		static Board()
		{
			for (var intOrdinal=0; intOrdinal<SquareCount; intOrdinal++)
			{
				MArrSquare[intOrdinal] = new Square(intOrdinal);
			}
		}

		public static Square GetSquare(int ordinal)
		{
			return (ordinal & 0x88) == 0 ? MArrSquare[ordinal] : null;
		}

		public static Square GetSquare(int file, int rank)
		{
			return (OrdinalFromFileRank(file, rank) & 0x88) == 0 ? MArrSquare[OrdinalFromFileRank(file, rank)] : null;
		}

		public static Square GetSquare(string label)
		{
			return MArrSquare[ OrdinalFromFileRank( FileFromName(label.Substring(0,1)), int.Parse(label.Substring(1,1))-1 ) ] ;
		}

		private static int FileFromName(string fileName)
		{
			switch (fileName)
			{
				case "a":
					return 0;
				case "b":
					return 1;
				case "c":
					return 2;
				case "d":
					return 3;
				case "e":
					return 4;
				case "f":
					return 5;
				case "g":
					return 6;
				case "h":
					return 7;
			}
			return -1;
		}

		public static Piece GetPiece(int ordinal)
		{
			return (ordinal & 0x88) == 0 ? MArrSquare[ordinal].Piece : null;
		}

		public static Piece GetPiece(int file, int rank)
		{
			return (OrdinalFromFileRank(file, rank) & 0x88) == 0 ? MArrSquare[OrdinalFromFileRank(file, rank)].Piece : null;
		}

		private static int OrdinalFromFileRank(int file, int rank)
		{
			return (rank << 4) | file;
		}

		public static void LineThreatenedBy(Player player, Squares squares, Square squareStart, int offset)
		{
			var intOrdinal = squareStart.Ordinal;
			Square square;

			intOrdinal += offset;
			while ( (square = GetSquare(intOrdinal))!=null )
			{

				if ( square.Piece==null )
				{
					squares.Add(square);
				}
				else if ( square.Piece.Player.Colour!=player.Colour && square.Piece.CanBeTaken )
				{
					squares.Add(square);
					break;
				}
				else
				{
					break;
				}
				intOrdinal += offset;
			}				
		}

		public static void AppendPiecePath(Moves moves, Piece piece, Player player, int offset, Moves.EnmMovesType movesType)
		{
			var intOrdinal = piece.Square.Ordinal;
			Square square;

			intOrdinal += offset;
			while ( (square = GetSquare(intOrdinal))!=null )
			{

				if ( square.Piece==null )
				{
					if (movesType==Moves.EnmMovesType.All)
					{
						moves.Add(0, 0, Move.EnmName.Standard, piece, piece.Square, square, null, 0, 0);
					}
				}
				else if ( square.Piece.Player.Colour!=player.Colour && square.Piece.CanBeTaken )
				{
					moves.Add(0, 0, Move.EnmName.Standard, piece, piece.Square, square, square.Piece, 0, 0);
					break;
				}
				else
				{
					break;
				}
				intOrdinal += offset;
			}				
		}

		public static Piece LinesFirstPiece(Player.EnmColour colour, Piece.EnmName pieceName, Square squareStart, int offset)
		{
			var intOrdinal = squareStart.Ordinal;
			Square square;

			intOrdinal += offset;
			while ( (square = GetSquare(intOrdinal))!=null )
			{

				if ( square.Piece==null )
				{
				}
				else if ( square.Piece.Player.Colour!=colour )
				{
					return null;
				}
				else if ( square.Piece.Name==pieceName || square.Piece.Name==Piece.EnmName.Queen )
				{
					return square.Piece;
				}
				else
				{
					return null;
				}
				intOrdinal += offset;
			}
			return null;
		}

		public static int LineIsOpen(Player.EnmColour colour, Square squareStart, int offset)
		{
			var intOrdinal = squareStart.Ordinal;
			var intSquareCount = 0;
			var intPenalty = 0;
			Square square;

			intOrdinal += offset;
			 
			while ( intSquareCount<=2 && (square = GetSquare(intOrdinal))!=null && (square.Piece==null || (square.Piece.Name!=Piece.EnmName.Pawn && square.Piece.Name!=Piece.EnmName.Rook) || square.Piece.Player.Colour!=colour))
			{
				intPenalty += 75;
				intSquareCount++;
				intOrdinal += offset;
			}
			return intPenalty;
		}

		public static void EstablishHashKey()
		{
			Piece piece;
			HashCodeA = 0UL;
			HashCodeB = 0UL;
			PawnHashCodeA = 0UL;
			PawnHashCodeB = 0UL;
			for (var intOrdinal=0; intOrdinal<SquareCount; intOrdinal++)
			{
				piece = GetPiece(intOrdinal);
				if (piece!=null)
				{
					HashCodeA ^= piece.HashCodeAForSquareOrdinal(intOrdinal);
					HashCodeB ^= piece.HashCodeBForSquareOrdinal(intOrdinal);
					if (piece.Name==Piece.EnmName.Pawn)
					{
						PawnHashCodeA ^= piece.HashCodeAForSquareOrdinal(intOrdinal);
						PawnHashCodeB ^= piece.HashCodeBForSquareOrdinal(intOrdinal);
					}
				}
			}
		}
	

		public static string DebugString
		{
			get
			{
				Square square;
				Piece piece;
				var strOutput = "";
				var intOrdinal = SquareCount-1;

				for (var intRank=0; intRank<RankCount; intRank++)
				{
					for (var intFile=0; intFile<FileCount; intFile++)
					{
						square = GetSquare(intOrdinal);
						if (square!=null)
						{
							if ((piece=square.Piece)!=null)
							{
								strOutput += piece.Abbreviation;
							}
							else
							{
								strOutput += square.Colour==Square.EnmColour.White ? "." : "#";
							}
						}
						strOutput += Convert.ToChar(13) + Convert.ToChar(10);

						intOrdinal--;
					}
				}
				return strOutput;
			}
		}

	}
}

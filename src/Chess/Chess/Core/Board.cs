using System;

namespace Chess.Core
{
	public class Board
	{
		public const byte RANK_COUNT = 8;
		public const byte FILE_COUNT = 8;
		public const byte MATRIX_WIDTH = 16;
		public const byte SQUARE_COUNT = 128;
		public static ulong HashCodeA = 0;
		public static ulong HashCodeB = 0;
		public static ulong PawnHashCodeA = 0;
		public static ulong PawnHashCodeB = 0;

		static Square[] m_arrSquare = new Square[RANK_COUNT * MATRIX_WIDTH];

		static Board()
		{
			for (int intOrdinal=0; intOrdinal<SQUARE_COUNT; intOrdinal++)
			{
				m_arrSquare[intOrdinal] = new Square(intOrdinal);
			}
		}

		public static Square GetSquare(int Ordinal)
		{
			return (Ordinal & 0x88) == 0 ? m_arrSquare[Ordinal] : null;
		}

		public static Square GetSquare(int File, int Rank)
		{
			return (OrdinalFromFileRank(File, Rank) & 0x88) == 0 ? m_arrSquare[OrdinalFromFileRank(File, Rank)] : null;
		}

		public static Square GetSquare(string Label)
		{
			return m_arrSquare[ OrdinalFromFileRank( FileFromName(Label.Substring(0,1)), int.Parse(Label.Substring(1,1))-1 ) ] ;
		}

		private static int FileFromName(string FileName)
		{
			switch (FileName)
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

		public static Piece GetPiece(int Ordinal)
		{
			return (Ordinal & 0x88) == 0 ? m_arrSquare[Ordinal].Piece : null;
		}

		public static Piece GetPiece(int File, int Rank)
		{
			return (OrdinalFromFileRank(File, Rank) & 0x88) == 0 ? m_arrSquare[OrdinalFromFileRank(File, Rank)].Piece : null;
		}

		private static int OrdinalFromFileRank(int File, int Rank)
		{
			return (Rank << 4) | File;
		}

		public static void LineThreatenedBy(Player player, Squares squares, Square squareStart, int Offset)
		{
			int intOrdinal = squareStart.Ordinal;
			Square square;

			intOrdinal += Offset;
			while ( (square = Board.GetSquare(intOrdinal))!=null )
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
				intOrdinal += Offset;
			}				
		}

		public static void AppendPiecePath(Moves moves, Piece piece, Player player, int Offset, Moves.enmMovesType movesType)
		{
			int intOrdinal = piece.Square.Ordinal;
			Square square;

			intOrdinal += Offset;
			while ( (square = Board.GetSquare(intOrdinal))!=null )
			{

				if ( square.Piece==null )
				{
					if (movesType==Moves.enmMovesType.All)
					{
						moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, square, null, 0, 0);
					}
				}
				else if ( square.Piece.Player.Colour!=player.Colour && square.Piece.CanBeTaken )
				{
					moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, square, square.Piece, 0, 0);
					break;
				}
				else
				{
					break;
				}
				intOrdinal += Offset;
			}				
		}

		public static Piece LinesFirstPiece(Player.enmColour colour, Piece.enmName PieceName, Square squareStart, int Offset)
		{
			int intOrdinal = squareStart.Ordinal;
			Square square;

			intOrdinal += Offset;
			while ( (square = Board.GetSquare(intOrdinal))!=null )
			{

				if ( square.Piece==null )
				{
				}
				else if ( square.Piece.Player.Colour!=colour )
				{
					return null;
				}
				else if ( square.Piece.Name==PieceName || square.Piece.Name==Piece.enmName.Queen )
				{
					return square.Piece;
				}
				else
				{
					return null;
				}
				intOrdinal += Offset;
			}
			return null;
		}

		public static int LineIsOpen(Player.enmColour colour, Square squareStart, int Offset)
		{
			int intOrdinal = squareStart.Ordinal;
			int intSquareCount = 0;
			int intPenalty = 0;
			Square square;

			intOrdinal += Offset;
			 
			while ( intSquareCount<=2 && ((square = Board.GetSquare(intOrdinal))!=null && (square.Piece==null || (square.Piece.Name!=Piece.enmName.Pawn && square.Piece.Name!=Piece.enmName.Rook) || square.Piece.Player.Colour!=colour)))
			{
				intPenalty += 75;
				intSquareCount++;
				intOrdinal += Offset;
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
			for (int intOrdinal=0; intOrdinal<SQUARE_COUNT; intOrdinal++)
			{
				piece = Board.GetPiece(intOrdinal);
				if (piece!=null)
				{
					HashCodeA ^= piece.HashCodeAForSquareOrdinal(intOrdinal);
					HashCodeB ^= piece.HashCodeBForSquareOrdinal(intOrdinal);
					if (piece.Name==Piece.enmName.Pawn)
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
				string strOutput = "";
				int intOrdinal = Board.SQUARE_COUNT-1;

				for (int intRank=0; intRank<Board.RANK_COUNT; intRank++)
				{
					for (int intFile=0; intFile<Board.FILE_COUNT; intFile++)
					{
						square = Board.GetSquare(intOrdinal);
						if (square!=null)
						{
							if ((piece=square.Piece)!=null)
							{
								strOutput += piece.Abbreviation;
							}
							else
							{
								strOutput += (square.Colour==Square.enmColour.White ? "." : "#");
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

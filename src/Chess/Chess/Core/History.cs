namespace Chess.Core
{
	public class History
	{
	    private static readonly int[,] AHistoryEntryWhite = new int[Board.SquareCount,Board.SquareCount];
	    private static readonly int[,] AHistoryEntryBlack = new int[Board.SquareCount,Board.SquareCount];

		static public void Clear()
		{
			for (var i=0; i<Board.SquareCount; i++)
			{
				for (var j=0; j<Board.SquareCount; j++)
				{
					AHistoryEntryWhite[i,j] = 0;
					AHistoryEntryBlack[i,j] = 0;
				}
			}
		}

		static public void Record(Player.EnmColour colour, int ordinalFrom, int ordinalTo, int increase, int value)
		{
			if (colour==Player.EnmColour.White)
			{
				AHistoryEntryWhite[ordinalFrom, ordinalTo] += value;
			}
			else 
			{
				AHistoryEntryBlack[ordinalFrom, ordinalTo] += value;
			}
		}

		static public int Retrieve(Player.EnmColour colour, int ordinalFrom, int ordinalTo)
		{
			return colour==Player.EnmColour.White ? AHistoryEntryWhite[ordinalFrom, ordinalTo] : AHistoryEntryBlack[ordinalFrom, ordinalTo];
		}
	}
}

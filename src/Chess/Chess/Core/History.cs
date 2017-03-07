namespace Chess.Core
{
	public class History
	{
	    private static int[,] _aHistoryEntryWhite = new int[Board.SquareCount,Board.SquareCount];
	    private static int[,] _aHistoryEntryBlack = new int[Board.SquareCount,Board.SquareCount];

		static public void Clear()
		{
			for (int i=0; i<Board.SquareCount; i++)
			{
				for (int j=0; j<Board.SquareCount; j++)
				{
					_aHistoryEntryWhite[i,j] = 0;
					_aHistoryEntryBlack[i,j] = 0;
				}
			}
		}

		static public void Record(Player.EnmColour colour, int ordinalFrom, int ordinalTo, int increase, int value)
		{
			if (colour==Player.EnmColour.White)
			{
				_aHistoryEntryWhite[ordinalFrom, ordinalTo] += value;
			}
			else 
			{
				_aHistoryEntryBlack[ordinalFrom, ordinalTo] += value;
			}
		}

		static public int Retrieve(Player.EnmColour colour, int ordinalFrom, int ordinalTo)
		{
			return colour==Player.EnmColour.White ? _aHistoryEntryWhite[ordinalFrom, ordinalTo] : _aHistoryEntryBlack[ordinalFrom, ordinalTo];
		}
	}
}

namespace Chess.Core
{
	public class HashTableCheck
	{
	    public static int Probes { get; private set; }

	    public static int Hits { get; private set; }

	    public static int Writes { get; private set; }

	    public static int Collisions { get; private set; }

	    public static int Overwrites { get; private set; }

	    private struct HashEntry
		{
			public ulong	HashCodeA;
			public ulong	HashCodeB;
			public bool		IsInCheck;
		}

		public const int HashTableSize = 1000777;
	    private static readonly HashEntry[] MArrHashEntry = new HashEntry[HashTableSize];

		static HashTableCheck()
		{
			Clear();
		}

		public static void ResetStats()
		{
			Probes = 0;
			Hits = 0;
			Writes = 0;
			Collisions = 0;
			Overwrites = 0;
		}
		public static void Clear()
		{
			ResetStats();
			for (uint intIndex=0; intIndex<HashTableSize; intIndex++)
			{
				MArrHashEntry[intIndex].HashCodeA = 0;
				MArrHashEntry[intIndex].HashCodeB = 0;
				MArrHashEntry[intIndex].IsInCheck = false;
			}
		}

		public static unsafe bool IsPlayerInCheck(Player player)
		{
			fixed (HashEntry* phashBase = &MArrHashEntry[0])
			{
				var hashCodeA = Board.HashCodeA;
				var hashCodeB = Board.HashCodeB;

				if (player.Colour==Player.EnmColour.Black)
				{
					hashCodeA |= 0x1;
					hashCodeB |= 0x1;
				}
				else
				{
					hashCodeA &= 0xFFFFFFFFFFFFFFFE;
					hashCodeB &= 0xFFFFFFFFFFFFFFFE;
				}

				var phashEntry = phashBase;
				phashEntry += (uint)(hashCodeA % HashTableSize);
				
				if (phashEntry->HashCodeA!=hashCodeA || phashEntry->HashCodeB!=hashCodeB)
				{
					phashEntry->HashCodeA = hashCodeA;
					phashEntry->HashCodeB = hashCodeB;
					phashEntry->IsInCheck = player.DetermineCheckStatus();
				}
				return phashEntry->IsInCheck;
			}
		}
		
	}
}

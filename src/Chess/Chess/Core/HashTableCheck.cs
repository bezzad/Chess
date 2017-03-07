namespace Chess.Core
{
	public class HashTableCheck
	{
		private static int _mIntProbes;
		private static int _mIntHits;
		private static int _mIntWrites;
		private static int _mIntCollisions;
		private static int _mIntOverwrites;

		public static int Probes => _mIntProbes;

	    public static int Hits => _mIntHits;

	    public static int Writes => _mIntWrites;

	    public static int Collisions => _mIntCollisions;

	    public static int Overwrites => _mIntOverwrites;

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
			_mIntProbes = 0;
			_mIntHits = 0;
			_mIntWrites = 0;
			_mIntCollisions = 0;
			_mIntOverwrites = 0;
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

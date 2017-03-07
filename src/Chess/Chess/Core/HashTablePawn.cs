namespace Chess.Core
{
	public class HashTablePawn
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
			public int		Points;
		}

		public const int Unknown = int.MinValue;
		public const int HashTableSize = 1000013;
	    private static readonly HashEntry[] MArrHashEntry = new HashEntry[HashTableSize];

		public static int SlotsUsed
		{
			get
			{
				var intCounter = 0;

				for (uint intIndex=0; intIndex<HashTableSize; intIndex++)
				{
					if (MArrHashEntry[intIndex].HashCodeA != 0)
					{
						intCounter++;
					}
				}
				return intCounter;
			}
		}

		static HashTablePawn()
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
				MArrHashEntry[intIndex].Points = Unknown;
			}
		}

		public static unsafe int ProbeHash(Player.EnmColour colour)
		{
			var hashCodeA = Board.HashCodeA;
			var hashCodeB = Board.HashCodeB;

			if (colour==Player.EnmColour.Black)
			{
				hashCodeA |= 0x1;
				hashCodeB |= 0x1;
			}
			else
			{
				hashCodeA &= 0xFFFFFFFFFFFFFFFE;
				hashCodeB &= 0xFFFFFFFFFFFFFFFE;
			}

			Probes++;

			fixed (HashEntry* phashBase = &MArrHashEntry[0])
			{
				var phashEntry = phashBase;
				phashEntry += (uint)(hashCodeA % HashTableSize);
				
				if (phashEntry->HashCodeA == hashCodeA && phashEntry->HashCodeB == hashCodeB)
				{
					Hits++;
					return phashEntry->Points;
				}
			}
			return Unknown;
		}
		
		public static unsafe void RecordHash(int val, Player.EnmColour colour)
		{
			var hashCodeA = Board.HashCodeA;
			var hashCodeB = Board.HashCodeB;

			if (colour==Player.EnmColour.Black)
			{
				hashCodeA |= 0x1;
				hashCodeB |= 0x1;
			}
			else
			{
				hashCodeA &= 0xFFFFFFFFFFFFFFFE;
				hashCodeB &= 0xFFFFFFFFFFFFFFFE;
			}


			fixed (HashEntry* phashBase = &MArrHashEntry[0])
			{
				var phashEntry = phashBase;
				phashEntry += (uint)(hashCodeA % HashTableSize);
				phashEntry->HashCodeA = hashCodeA;
				phashEntry->HashCodeB = hashCodeB;
				phashEntry->Points = val;
			}
			Writes++;
		}

	}
}

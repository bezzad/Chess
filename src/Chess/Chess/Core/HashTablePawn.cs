namespace Chess.Core
{
	public class HashTablePawn
	{
		private static int _mIntProbes = 0;
		private static int _mIntHits = 0;
		private static int _mIntWrites = 0;
		private static int _mIntCollisions = 0;
		private static int _mIntOverwrites = 0;

		public static int Probes
		{
			get {return _mIntProbes;}
		}

		public static int Hits
		{
			get {return _mIntHits;}
		}

		public static int Writes
		{
			get {return _mIntWrites;}
		}

		public static int Collisions
		{
			get { return _mIntCollisions; }
		}

		public static int Overwrites
		{
			get { return _mIntOverwrites; }
		}

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

			_mIntProbes++;

			fixed (HashEntry* phashBase = &MArrHashEntry[0])
			{
				var phashEntry = phashBase;
				phashEntry += ((uint)(hashCodeA % HashTableSize));
				
				if (phashEntry->HashCodeA == hashCodeA && phashEntry->HashCodeB == hashCodeB)
				{
					_mIntHits++;
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
				phashEntry += ((uint)(hashCodeA % HashTableSize));
				phashEntry->HashCodeA = hashCodeA;
				phashEntry->HashCodeB = hashCodeB;
				phashEntry->Points = val;
			}
			_mIntWrites++;
		}

	}
}

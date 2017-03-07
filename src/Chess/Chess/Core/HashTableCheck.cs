namespace Chess.Core
{
	public class HashTableCheck
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
			public bool		IsInCheck;
		}

		public const int HashTableSize = 1000777;
	    private static HashEntry[] _mArrHashEntry = new HashEntry[HashTableSize];

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
				_mArrHashEntry[intIndex].HashCodeA = 0;
				_mArrHashEntry[intIndex].HashCodeB = 0;
				_mArrHashEntry[intIndex].IsInCheck = false;
			}
		}

		public unsafe static bool IsPlayerInCheck(Player player)
		{
			fixed (HashEntry* phashBase = &_mArrHashEntry[0])
			{
				ulong hashCodeA = Board.HashCodeA;
				ulong hashCodeB = Board.HashCodeB;

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

				HashEntry* phashEntry = phashBase;
				phashEntry += ((uint)(hashCodeA % HashTableSize));
				
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

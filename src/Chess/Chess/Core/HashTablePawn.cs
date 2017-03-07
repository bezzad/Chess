namespace Chess.Core
{
	public class HashTablePawn
	{
		private static int m_intProbes = 0;
		private static int m_intHits = 0;
		private static int m_intWrites = 0;
		private static int m_intCollisions = 0;
		private static int m_intOverwrites = 0;

		public static int Probes
		{
			get {return m_intProbes;}
		}

		public static int Hits
		{
			get {return m_intHits;}
		}

		public static int Writes
		{
			get {return m_intWrites;}
		}

		public static int Collisions
		{
			get { return m_intCollisions; }
		}

		public static int Overwrites
		{
			get { return m_intOverwrites; }
		}

		private struct HashEntry
		{
			public ulong	HashCodeA;
			public ulong	HashCodeB;
			public int		Points;
		}

		public const int UNKNOWN = int.MinValue;
		public const int HASH_TABLE_SIZE = 1000013;
		static HashEntry[] m_arrHashEntry = new HashEntry[HASH_TABLE_SIZE];

		public static int SlotsUsed
		{
			get
			{
				int intCounter = 0;

				for (uint intIndex=0; intIndex<HASH_TABLE_SIZE; intIndex++)
				{
					if (m_arrHashEntry[intIndex].HashCodeA != 0)
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
			m_intProbes = 0;
			m_intHits = 0;
			m_intWrites = 0;
			m_intCollisions = 0;
			m_intOverwrites = 0;
		}

		public static void Clear()
		{
			ResetStats();
			for (uint intIndex=0; intIndex<HASH_TABLE_SIZE; intIndex++)
			{
				m_arrHashEntry[intIndex].HashCodeA = 0;
				m_arrHashEntry[intIndex].HashCodeB = 0;
				m_arrHashEntry[intIndex].Points = UNKNOWN;
			}
		}

		public unsafe static int ProbeHash(Player.enmColour colour)
		{
			ulong HashCodeA = Board.HashCodeA;
			ulong HashCodeB = Board.HashCodeB;

			if (colour==Player.enmColour.Black)
			{
				HashCodeA |= 0x1;
				HashCodeB |= 0x1;
			}
			else
			{
				HashCodeA &= 0xFFFFFFFFFFFFFFFE;
				HashCodeB &= 0xFFFFFFFFFFFFFFFE;
			}

			m_intProbes++;

			fixed (HashEntry* phashBase = &m_arrHashEntry[0])
			{
				HashEntry* phashEntry = phashBase;
				phashEntry += ((uint)(HashCodeA % HASH_TABLE_SIZE));
				
				if (phashEntry->HashCodeA == HashCodeA && phashEntry->HashCodeB == HashCodeB)
				{
					m_intHits++;
					return phashEntry->Points;
				}
			}
			return UNKNOWN;
		}
		
		public unsafe static void RecordHash(int val, Player.enmColour colour)
		{
			ulong HashCodeA = Board.HashCodeA;
			ulong HashCodeB = Board.HashCodeB;

			if (colour==Player.enmColour.Black)
			{
				HashCodeA |= 0x1;
				HashCodeB |= 0x1;
			}
			else
			{
				HashCodeA &= 0xFFFFFFFFFFFFFFFE;
				HashCodeB &= 0xFFFFFFFFFFFFFFFE;
			}


			fixed (HashEntry* phashBase = &m_arrHashEntry[0])
			{
				HashEntry* phashEntry = phashBase;
				phashEntry += ((uint)(HashCodeA % HASH_TABLE_SIZE));
				phashEntry->HashCodeA = HashCodeA;
				phashEntry->HashCodeB = HashCodeB;
				phashEntry->Points = val;
			}
			m_intWrites++;
		}

	}
}

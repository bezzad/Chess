namespace Chess.Core
{
	public class HashTableCheck
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
			public bool		IsInCheck;
		}

		public const int HASH_TABLE_SIZE = 1000777;
		static HashEntry[] m_arrHashEntry = new HashEntry[HASH_TABLE_SIZE];

		static HashTableCheck()
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
				m_arrHashEntry[intIndex].IsInCheck = false;
			}
		}

		public unsafe static bool IsPlayerInCheck(Player player)
		{
			fixed (HashEntry* phashBase = &m_arrHashEntry[0])
			{
				ulong HashCodeA = Board.HashCodeA;
				ulong HashCodeB = Board.HashCodeB;

				if (player.Colour==Player.enmColour.Black)
				{
					HashCodeA |= 0x1;
					HashCodeB |= 0x1;
				}
				else
				{
					HashCodeA &= 0xFFFFFFFFFFFFFFFE;
					HashCodeB &= 0xFFFFFFFFFFFFFFFE;
				}

				HashEntry* phashEntry = phashBase;
				phashEntry += ((uint)(HashCodeA % HASH_TABLE_SIZE));
				
				if (phashEntry->HashCodeA!=HashCodeA || phashEntry->HashCodeB!=HashCodeB)
				{
					phashEntry->HashCodeA = HashCodeA;
					phashEntry->HashCodeB = HashCodeB;
					phashEntry->IsInCheck = player.DetermineCheckStatus();
				}
				return phashEntry->IsInCheck;
			}
		}
		
	}
}

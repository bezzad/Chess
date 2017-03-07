namespace Chess.Core
{
	public class HashTable
	{
		public const int HashTableSlotDepth = 3;
		public const int HashTableSize = 1000777;


	    public static int Probes { get; private set; }

	    public static int Hits { get; private set; }

	    public static int Writes { get; private set; }

	    public static int Collisions { get; private set; }

	    public static int Overwrites { get; private set; }

	    public enum EnmHashType
		{
				Exact
			,	Alpha
			,	Beta
		}

		private struct HashEntry
		{
			public ulong	HashCodeA;
			public ulong	HashCodeB;
			public sbyte	Depth;
			public EnmHashType Type;
			public Player.EnmColour Colour;
			public int		Result;
			public Move.EnmName WhiteMoveName;
			public Move.EnmName BlackMoveName;
			public sbyte	WhiteFrom;
			public sbyte	WhiteTo;
			public sbyte	BlackFrom;
			public sbyte	BlackTo;
		}

		public const int Unknown = int.MinValue;
	    private static readonly HashEntry[] MArrHashEntry = new HashEntry[HashTableSize];

		static HashTable()
		{
			Clear();
		}

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
				MArrHashEntry[intIndex].Depth = sbyte.MinValue;
				MArrHashEntry[intIndex].WhiteFrom = -1;
				MArrHashEntry[intIndex].BlackFrom = -1;
			}
		}

		public static unsafe int ProbeHash(ulong hashCodeA, ulong hashCodeB, int depth, int alpha, int beta, Player.EnmColour colour)
		{
			Probes++;

			fixed (HashEntry* phashBase = &MArrHashEntry[0])
			{
				var phashEntry = phashBase;
				phashEntry += (uint)(hashCodeA % HashTableSize);

				var intAttempt = 0;
				while (phashEntry>=phashBase && (phashEntry->HashCodeA!=hashCodeA || phashEntry->HashCodeB!=hashCodeB || phashEntry->Depth < depth) )
				{
					phashEntry--;
					intAttempt++;
					if (intAttempt==HashTableSlotDepth)
					{
						break;
					}
				}

				if (phashEntry<phashBase)
				{
					phashEntry = phashBase;
				}

				if (phashEntry->HashCodeA==hashCodeA && phashEntry->HashCodeB==hashCodeB && phashEntry->Depth >= depth )
				{
					if (phashEntry->Colour==colour)
					{
						if ( phashEntry->Type==EnmHashType.Exact )
						{
							Hits++;
							return phashEntry->Result;
						}
						if ( (phashEntry->Type==EnmHashType.Alpha) && (phashEntry->Result<=alpha))
						{
							Hits++;
							return alpha;
						}
						if ( (phashEntry->Type==EnmHashType.Beta) && (phashEntry->Result>=beta))
						{
							Hits++;
							return beta;
						}
					}
				}
			}
			return Unknown;
		}
		
		public static unsafe void RecordHash(ulong hashCodeA, ulong hashCodeB, int depth, int val, EnmHashType type, int @from, int to, Move.EnmName moveName, Player.EnmColour colour)
		{
			Writes++;
			fixed (HashEntry* phashBase = &MArrHashEntry[0])
			{
			    var phashEntry = phashBase;
				phashEntry += (uint)(hashCodeA % HashTableSize);

			    var intAttempt = 0;
				while (phashEntry>=phashBase && phashEntry->HashCodeA!=0 && phashEntry->Depth > depth)
				{
					phashEntry--;
					intAttempt++;
					if (intAttempt==HashTableSlotDepth)
					{
						break;
					}
				}

				if (phashEntry<phashBase)
				{
					phashEntry = phashBase;
				}

				if (phashEntry->HashCodeA!=0)
				{
					Collisions++;
					if (phashEntry->HashCodeA!=hashCodeA || phashEntry->HashCodeB!=hashCodeB)
					{
						Overwrites++;
						phashEntry->WhiteFrom = -1;
						phashEntry->BlackFrom = -1;
					}
				}

				phashEntry->HashCodeA = hashCodeA;
				phashEntry->HashCodeB = hashCodeB;
				phashEntry->Result = val;
				phashEntry->Type = type;
				phashEntry->Depth = (sbyte)depth;
				phashEntry->Colour = colour;
				if (@from>-1)
				{
					if (colour==Player.EnmColour.White)
					{
						phashEntry->WhiteMoveName = moveName;
						phashEntry->WhiteFrom = (sbyte)@from;
						phashEntry->WhiteTo = (sbyte)to;
					}
					else
					{
						phashEntry->BlackMoveName = moveName;
						phashEntry->BlackFrom = (sbyte)@from;
						phashEntry->BlackTo = (sbyte)to;
					}
				}

			}
		}

		public static unsafe Move ProbeForBestMove(Player.EnmColour colour)
		{
			fixed (HashEntry* phashBase = &MArrHashEntry[0])
			{
				var hashCodeA = Board.HashCodeA;
				var hashCodeB = Board.HashCodeB;

				var phashEntry = phashBase;
				phashEntry += (uint)(hashCodeA % HashTableSize);
				
				var intAttempt = 0;
				while (phashEntry>=phashBase && (phashEntry->HashCodeA!=hashCodeA || phashEntry->HashCodeB!=hashCodeB) )
				{
					phashEntry--;
					intAttempt++;
					if (intAttempt==HashTableSlotDepth)
					{
						break;
					}
				}

				if (phashEntry<phashBase)
				{
					phashEntry = phashBase;
				}

				if (phashEntry->HashCodeA==hashCodeA && phashEntry->HashCodeB==hashCodeB)
				{
					if (colour==Player.EnmColour.White)
					{
						if (phashEntry->WhiteFrom >= 0)
						{
							return new Move(0, 0, phashEntry->WhiteMoveName, Board.GetPiece(phashEntry->WhiteFrom), Board.GetSquare(phashEntry->WhiteFrom), Board.GetSquare(phashEntry->WhiteTo), null, 0, phashEntry->Result);
						}
					}
					else
					{
						if (phashEntry->BlackFrom >= 0)
						{
							return new Move(0, 0, phashEntry->BlackMoveName, Board.GetPiece(phashEntry->BlackFrom), Board.GetSquare(phashEntry->BlackFrom), Board.GetSquare(phashEntry->BlackTo), null, 0, phashEntry->Result);
						}
					}
				}
			}
			return null;
		}
	
	}
}

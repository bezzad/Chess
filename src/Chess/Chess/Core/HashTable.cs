namespace Chess.Core
{
	public class HashTable
	{
		public const int HashTableSlotDepth = 3;
		public const int HashTableSize = 1000777;


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
	    private static HashEntry[] _mArrHashEntry = new HashEntry[HashTableSize];

		static HashTable()
		{
			Clear();
		}

		public static int SlotsUsed
		{
			get
			{
				int intCounter = 0;

				for (uint intIndex=0; intIndex<HashTableSize; intIndex++)
				{
					if (_mArrHashEntry[intIndex].HashCodeA != 0)
					{
						intCounter++;
					}
				}
				return intCounter;
			}
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
				_mArrHashEntry[intIndex].Depth = sbyte.MinValue;
				_mArrHashEntry[intIndex].WhiteFrom = -1;
				_mArrHashEntry[intIndex].BlackFrom = -1;
			}
		}

		public unsafe static int ProbeHash(ulong hashCodeA, ulong hashCodeB, int depth, int alpha, int beta, Player.EnmColour colour)
		{
			_mIntProbes++;

			fixed (HashEntry* phashBase = &_mArrHashEntry[0])
			{
				HashEntry* phashEntry = phashBase;
				phashEntry += ((uint)(hashCodeA % HashTableSize));

				int intAttempt = 0;
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
							_mIntHits++;
							return phashEntry->Result;
						}
						if ( (phashEntry->Type==EnmHashType.Alpha) && (phashEntry->Result<=alpha))
						{
							_mIntHits++;
							return alpha;
						}
						if ( (phashEntry->Type==EnmHashType.Beta) && (phashEntry->Result>=beta))
						{
							_mIntHits++;
							return beta;
						}
					}
				}
			}
			return Unknown;
		}
		
		public unsafe static void RecordHash(ulong hashCodeA, ulong hashCodeB, int depth, int val, EnmHashType type, int @from, int to, Move.EnmName moveName, Player.EnmColour colour)
		{
			_mIntWrites++;
			fixed (HashEntry* phashBase = &_mArrHashEntry[0])
			{
				int intAttempt;
				HashEntry* phashEntry = phashBase;
				phashEntry += ((uint)(hashCodeA % HashTableSize));
				HashEntry* phashFirst = phashEntry;

				intAttempt = 0;
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
					_mIntCollisions++;
					if (phashEntry->HashCodeA!=hashCodeA || phashEntry->HashCodeB!=hashCodeB)
					{
						_mIntOverwrites++;
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

		public unsafe static Move ProbeForBestMove(Player.EnmColour colour)
		{
			fixed (HashEntry* phashBase = &_mArrHashEntry[0])
			{
				ulong hashCodeA = Board.HashCodeA;
				ulong hashCodeB = Board.HashCodeB;

				HashEntry* phashEntry = phashBase;
				phashEntry += ((uint)(hashCodeA % HashTableSize));
				
				int intAttempt = 0;
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

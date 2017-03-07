using System;

namespace Chess.Core
{
	public abstract class Player
	{

		public enum EnmColour
		{
				White
			,	Black
		}

		public enum EnmIntellegence
		{
				Human
			,	Computer
		}

		public enum EnmStatus
		{
				Normal
			,	InCheck
			,	InStaleMate
			,	InCheckMate
		}

		public event DelegateGameEvent MoveConsidered;
		private const int MinScore = int.MinValue+1;
		private const int MaxScore = int.MaxValue;
		private Move _mMoveCurrent;
	    protected EnmColour MColour;
		protected Piece MKing;
		protected Piece MQueen;
		protected Piece MKingsPawn;
		protected Piece MQueensPawn;
		protected Piece MKingsRook;
		protected Piece MQueensRook;
		protected Piece MKingsBishop;
		protected Piece MQueensBishop;
		protected Piece MKingsKnight;
		protected Piece MQueensKnight;
		protected Pieces MColPieces;
		protected Pieces MColPawns;
		protected Pieces MColMaterial;
		protected Pieces MColCapturedEnemyPieces;
		protected int MNoOfPawnsInPlay = 8;
		protected int MMaterialCount = 7;
		protected int MIntTotalMoves;
		protected int MIntCurrentMoveNo;
		protected int MIntEvaluations;
		protected int MIntPositionsSearched;
		protected bool MBlnIsThinking;

	    private const int MIntGameMoves = 120;

		private TimeSpan _mTsnThinkingTimeMaxAllowed;
	    private TimeSpan _mTsnThinkingTimeHalved;

		public EnmIntellegence Intellegence;

	    private const int MIntMinSearchDepth = 1;
		private const int MIntMaxSearchDepth = 32;
		private const int MIntMinimumPlys = 4;
		private int _r = 3;
		private int _mIntRootScore;

	    private static int[] _mAintAttackBonus = {0, 3, 25, 53, 79, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99 };
	
		protected PlayerClock MPlayerClock;

		public class PlayerClock
		{
		    private TimeSpan _mTsnTimeElapsed;
		    private Player _mPlayer;

			public PlayerClock(Player player)
			{
				_mPlayer = player;
				Reset();
			}

			public DateTime TurnStartTime { get; private set; }

		    public bool IsTicking { get; private set; }

		    public TimeSpan TimeElapsed
			{
				get
				{
					return IsTicking ? _mTsnTimeElapsed + (DateTime.Now - TurnStartTime) : _mTsnTimeElapsed;
				}
				set
				{
					_mTsnTimeElapsed = value;
				}
			}

			public TimeSpan TimeLimit { get; } = new TimeSpan(0,60,0);

		    public TimeSpan TimeRemaining => TimeLimit - TimeElapsed;

		    public void Reset()
			{
				_mTsnTimeElapsed = new TimeSpan(0,0,0);
				TurnStartTime = DateTime.Now;
			}

			public void Start()
			{
				IsTicking = true;
				TurnStartTime = DateTime.Now;
			}

			public void Stop()
			{
				IsTicking = false;
				_mTsnTimeElapsed += DateTime.Now - TurnStartTime;
			}

			public void Revert()
			{
				IsTicking = false;
				TurnStartTime = DateTime.Now;
			}
		}

		public abstract PlayerClock Clock
		{
			get;
		}

		public abstract int PawnForwardOffset
		{
			get;
		}

		public abstract int PawnAttackRightOffset
		{
			get;
		}

		public abstract int PawnAttackLeftOffset
		{
			get;
		}

		public bool IsThinking => MBlnIsThinking;

	    public int Evaluations => MIntEvaluations;

	    public int PositionsSearched => MIntEvaluations;

	    public int MaxSearchDepth => MIntMaxSearchDepth;

	    public int SearchDepth { get; private set; }

	    public TimeSpan ThinkingTimeAllotted { get; private set; }

	    public TimeSpan ThinkingTimeRemaining => ThinkingTimeAllotted-(DateTime.Now-MPlayerClock.TurnStartTime);

	    public EnmStatus Status
		{
			get
			{
				if (IsInCheckMate ) return EnmStatus.InCheckMate;
				if (!CanMove) return EnmStatus.InStaleMate;
				if (IsInCheck) return EnmStatus.InCheck;
				return EnmStatus.Normal;
			}
		}

		public int MaterialCount => MMaterialCount;

	    public int MaterialBasicValue
		{
			get
			{
				var intMaterialBasicValue = 0;
				foreach (Piece piece in MColMaterial)
				{
					if (piece.IsInPlay)
					{
						intMaterialBasicValue += piece.BasicValue;
					}
				}
				return intMaterialBasicValue;
			}
		}

		public int PositionPoints
		{
			get
			{
				var intTotalValue = 0;
				int intIndex;
				Piece piece;
				for (intIndex=MColPieces.Count-1; intIndex>=0; intIndex--)
				{
					piece = MColPieces.Item(intIndex);
					intTotalValue += piece.PositionalValue;
				}
				return intTotalValue;
			}
		}


		public int PawnPoints
		{
			get
			{
				int intPoints;
				int intIndex;

				if ( (intPoints = HashTablePawn.ProbeHash(Colour)) == HashTablePawn.Unknown )
				{
					Piece piece;
					intPoints = 0;
					for (intIndex=MColPieces.Count-1; intIndex>=0; intIndex--)
					{
						piece = MColPieces.Item(intIndex);
						if (piece.Name==Piece.EnmName.Pawn)
						{
							intPoints += piece.PointsTotal;
						}
					}
					HashTablePawn.RecordHash(intPoints, Colour);
				}

				return intPoints;
			}
		}

		public bool HasCastled { get; set; }

	    public Piece King
		{
			get { return MKing; }
			set { MKing = value; }
		}

		public Piece Queen
		{
			get { return MQueen; }
			set { MQueen = value; }
		}

		public Piece KingsPawn
		{
			get { return MKingsPawn; }
			set { MKingsPawn = value; }
		}

		public Piece QueensPawn
		{
			get { return MQueensPawn; }
			set { MQueensPawn = value; }
		}

		public Piece KingsRook
		{
			get { return MKingsRook; }
			set { MKingsRook = value; }
		}

		public Piece QueensRook
		{
			get { return MQueensRook; }
			set { MQueensRook = value; }
		}

		public Piece KingsBishop
		{
			get { return MKingsBishop; }
			set { MKingsBishop = value; }
		}

		public Piece QueensBishop
		{
			get { return MQueensBishop; }
			set { MQueensBishop = value; }
		}

		public Piece KingsKnight
		{
			get { return MKingsKnight; }
			set { MKingsKnight = value; }
		}

		public Piece QueensKnight
		{
			get { return MQueensKnight; }
			set { MQueensKnight = value; }
		}

		public int TotalMoves => MIntTotalMoves;

	    public int MaxQuiesDepth { get; private set; }

	    public Move CurrentMove => _mMoveCurrent;

	    public Move BestMove { get; private set; }

	    public int CurrentMoveNo => MIntCurrentMoveNo;


	    public Player()
		{
			MColPieces = new Pieces(this);
			MColPawns = new Pieces(this);
			MColMaterial = new Pieces(this);
			MColCapturedEnemyPieces = new Pieces(this);
		}

		public int PawnsInPlay => MNoOfPawnsInPlay;

	    public void DecreasePawnCount()
		{
			MNoOfPawnsInPlay--;
		}

		public void IncreasePawnCount()
		{
			MNoOfPawnsInPlay++;
		}

		public void DecreaseMaterialCount()
		{
			MMaterialCount--;
		}

		public void IncreaseMaterialCount()
		{
			MMaterialCount++;
		}

		public bool CanMove
		{
			get
			{
				Moves moves;
				foreach (Piece piece in MColPieces )
				{
					moves = new Moves();
					piece.GenerateLegalMoves(moves);
					if (moves.Count>0)
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool DetermineCheckStatus()
		{
			return ((PieceKing)King.Top).DetermineCheckStatus();
		}

		public bool IsInCheck => HashTableCheck.IsPlayerInCheck(this);

	    public bool IsInCheckMate
		{
			get
			{ 
				if (!IsInCheck) return false;

				var moves = new Moves();
				GenerateLegalMoves(moves);
				return moves.Count==0;
			}
		}

		public void GenerateLegalMoves(Moves moves)
		{
			foreach (Piece piece in MColPieces)
			{
				piece.GenerateLegalMoves(moves);
			}
		}

		public void GenerateLazyMoves(int depth, Moves moves, Moves.EnmMovesType movesType, Square squareAttacking)
		{
//			if (squareAttacking==null)
//			{
				// All moves as defined by movesType
				foreach (Piece piece in MColPieces)
				{
					piece.GenerateLazyMoves(moves, movesType);

					if (movesType!=Moves.EnmMovesType.All)
					{
						Move move;
						int intIndex;
						for (intIndex=moves.Count-1; intIndex>=0; intIndex--)
						{
							move = moves.Item(intIndex);
							if ( !(
								move.Name==Move.EnmName.PawnPromotion
								||
								move.To.Ordinal==squareAttacking.Ordinal 
//								|| 
//								(depth>=-2 && move.From.Piece.BasicValue<move.To.Piece.BasicValue)
								||
								(depth>=-4 && !move.To.CanBeMovedToBy(move.Piece.Player.OtherPlayer)) // &&  
								)
								)  
							{
								moves.Remove(move);
							}
						}
					}
				}
//			}
//			else
//			{
//				// Just re-capture moves
//				squareAttacking.AttackerMoveList(moves, this);
//			}

/*			

*/

		}

		public Move ComputeBestMove()
		{
			MBlnIsThinking = true;

			var player = this;
			BestMove = null;
			Move moveHash = null;
			var alphaStart = Score;

/* Uncomment to switch-on opening book moves, once we have a decent opening book!
			// Query Opening Book
			if ((m_moveBest = OpeningBook.SearchForGoodMove(Board.HashKey, this.Colour) )!=null) 
			{
				m_moveCurrent = m_moveBest;
				this.MoveConsidered();
				return m_moveBest.Piece.Move(m_moveBest.Name, m_moveBest.To);
			}
*/
			Move moveDepthBest = null;
			var alpha = MinScore;
			var beta = MaxScore;

			_mIntRootScore = Score;

			ThinkingTimeAllotted = new TimeSpan( MPlayerClock.TimeRemaining.Ticks / Math.Max(MIntGameMoves-Game.TurnNo/2, 1) );
			_mTsnThinkingTimeMaxAllowed = new TimeSpan( ThinkingTimeAllotted.Ticks*3 );
			_mTsnThinkingTimeHalved = new TimeSpan( ThinkingTimeAllotted.Ticks/3);

			MIntEvaluations = 0;
			MIntPositionsSearched = 0;

			HashTable.ResetStats();
//			HashTable.Clear();   Uncomment this to clear the hashtable at the beginning of each move
			HashTableCheck.ResetStats();
			HashTablePawn.ResetStats();
			History.Clear();

			for (SearchDepth=MIntMinSearchDepth; SearchDepth<=MIntMaxSearchDepth; SearchDepth++)
			{


if (Game.DisplayMoveAnalysisTree)
{
	Game.MoveAnalysis = new Moves();
}
				MaxQuiesDepth = SearchDepth;

				// Get last iteration's best move from the HashTable
				moveHash = HashTable.ProbeForBestMove(player.Colour);

				// Generate and sort moves
				var movesPossible = new Moves();
				player.GenerateLegalMoves(movesPossible);
				MIntTotalMoves = movesPossible.Count;

				// If only one move is available, then just play it!
				if (movesPossible.Count==1)
				{
					moveDepthBest = _mMoveCurrent = movesPossible.Item(0);
					goto MoveSelected;
				}
				// Sort moves
				foreach (Move movex in movesPossible)
				{
					movex.Score = 0;

					if (moveHash!=null && movex.From.Ordinal==moveHash.From.Ordinal && movex.To.Ordinal==moveHash.To.Ordinal)
					{
						movex.Score += 1000000;
					}

					if (movex.Name==Move.EnmName.PawnPromotion)
					{
						movex.Score += 10000;
					}
						
					if (movex.PieceTaken!=null)
					{
						var moveSee = movex.Piece.SeeMove(movex.Name, movex.To);
						movex.Score += -See(player.OtherPlayer, int.MinValue, int.MaxValue, movex.To);
						Move.SeeUndo(moveSee);
					}
					else
					{
						movex.Score += History.Retrieve(player.Colour, movex.From.Ordinal, movex.To.Ordinal);
					}
				}
				movesPossible.SortByScore();

				alpha = MinScore;
				beta  = MaxScore;

				MIntCurrentMoveNo = 0;

				foreach (Move move in movesPossible)
				{
					_mMoveCurrent = move.Piece.Move(move.Name, move.To);

					if (_mMoveCurrent.IsInCheck) { Move.Undo(_mMoveCurrent); continue; }

					move.Score = _mMoveCurrent.Score = -AlphaBeta(player.OtherPlayer, SearchDepth-1, -alpha-1, -alpha, true, ref _mMoveCurrent);
					if (DateTime.Now-MPlayerClock.TurnStartTime > _mTsnThinkingTimeMaxAllowed) { Move.Undo(_mMoveCurrent); goto TimeExpired;}

					if ((move.Score > alpha) && (move.Score < beta)) /* fail */
					{
						move.Score = _mMoveCurrent.Score = -AlphaBeta(player.OtherPlayer, SearchDepth-1, -beta, -alpha, true, ref _mMoveCurrent);
						if (DateTime.Now-MPlayerClock.TurnStartTime > _mTsnThinkingTimeMaxAllowed) { Move.Undo(_mMoveCurrent); goto TimeExpired;}
					}
					
					MoveConsidered();

if (Game.DisplayMoveAnalysisTree)
{
	Game.MoveAnalysis.Add(_mMoveCurrent);
}

					Move.Undo(_mMoveCurrent);

					MIntCurrentMoveNo++;

					if (_mMoveCurrent.Score > alpha)
					{
						alpha = _mMoveCurrent.Score;
						moveDepthBest = _mMoveCurrent;
						History.Record(player.Colour, _mMoveCurrent.From.Ordinal, _mMoveCurrent.To.Ordinal, alpha-alphaStart, 1<<(SearchDepth+6)); // Update history heuristic data
					}

					_mMoveCurrent.Alpha = alpha;
					_mMoveCurrent.Beta = beta;

				}

			MoveSelected:

				BestMove = moveDepthBest;

				// Record best move
				HashTable.RecordHash(Board.HashCodeA, Board.HashCodeB, SearchDepth, BestMove.Score, HashTable.EnmHashType.Exact, BestMove.From.Ordinal, BestMove.To.Ordinal, BestMove.Name, player.Colour);

				MoveConsidered();

				if (DateTime.Now-MPlayerClock.TurnStartTime > _mTsnThinkingTimeHalved && SearchDepth >= MIntMinimumPlys ) goto TimeExpired;

				if (BestMove.Score > 99999) break; // Checkmate found so dont bother searching any deeper
			}


			TimeExpired:
			MoveConsidered();

			MBlnIsThinking = false;
			MoveConsidered();

			return BestMove;
		}


		private int AlphaBeta(Player player, int depth, int alpha, int beta, bool verify, ref Move moveAnalysed)
		{
			var val = int.MinValue;
			var hashType = HashTable.EnmHashType.Alpha;
			Move moveBest = null;
			Move moveHash = null;
			var blnPvNode = false;
			var intScoreAtEntry = 0;
//			bool failhigh = false;
			bool blnAllMovesWereGenerated;
			var intLegalMovesAttempted = 0;

			MIntPositionsSearched++;

			if ( (val = HashTable.ProbeHash(Board.HashCodeA, Board.HashCodeB, depth, alpha, beta, player.Colour)) != HashTable.Unknown )
			{
				// High values of "val" indicate that a checkmate has been found
				if (val>1000000 || val<-1000000)
				{
					val /= MIntMaxSearchDepth-depth;
				}
				return val;
			}

			if (player.CanClaimThreeMoveRepetition)
			{
				return -player.OtherPlayer.Score;
			}

			// Depth <=0 means we're into Quiescence searching
			if (depth <= 0)
			{
				if (depth < MaxQuiesDepth)
				{	
					MaxQuiesDepth = depth;
					if (MaxQuiesDepth<0)
					{
						MaxQuiesDepth+=0;
					}
				}
				intScoreAtEntry = val = -player.OtherPlayer.Score;	MIntEvaluations++;

				if (val>100000000 || val<-100000000) 
				{
					val /= MIntMaxSearchDepth-depth;
				}
				// Allow a deeper ply of search if a piece was captured or if a pawn was promoted, 
				// or either side is in check.
				if ( !( 
						moveAnalysed.PieceTaken != null 
						||
						moveAnalysed.Name == Move.EnmName.PawnPromotion || moveAnalysed.IsInCheck || moveAnalysed.IsEnemyInCheck
					  )
					)
				{
					return val;
				}

			}

if (Game.DisplayMoveAnalysisTree)
{
	moveAnalysed.Moves = new Moves();
}

			Move moveThis = null;

			// Get last iteration's best move from the Transition Table
			moveHash = HashTable.ProbeForBestMove(player.Colour);

			// Verified Null-move forward pruning
//			if (false && !player.IsInCheck && (!verify || depth > 1))
//			{

			// "Adaptive" Null-move forward pruning
			_r = depth>6 && Game.Stage!=Game.EnmStage.End ? 3 : 2; //  << This is the "adaptive" bit
			// The rest is normal Null-move forward pruning
			if (depth >= 3 )
			{
				var moveNull = new Move(Game.TurnNo, 0, Move.EnmName.NullMove, null, null, null, null, 0, 0);
				val = -AlphaBeta(player.OtherPlayer, depth - _r - 1, -beta, -beta + 1, verify, ref moveNull);
				if (DateTime.Now-MPlayerClock.TurnStartTime > _mTsnThinkingTimeMaxAllowed) goto TimeExpired;
				if (val >= beta)
				{
//					if (verify) 
//					{
//						depth--; /* reduce the depth by one ply */
//						/* turn verification off for the sub-tree */
//						verify = false;
//						/* mark a fail-high flag, to detect zugzwangs later*/
//						failhigh = true;
//					}
//					else /* cutoff in a sub-tree with fail-high report */
//					{
//						return val;
						return beta;
//					}
				}
			}

			// Generate moves
			var movesPossible = new Moves();

			blnAllMovesWereGenerated=depth>0 || moveAnalysed.IsInCheck || moveAnalysed.IsEnemyInCheck;
			if ( blnAllMovesWereGenerated ) 
			{
				player.GenerateLazyMoves(depth, movesPossible, Moves.EnmMovesType.All, null);
			}
			else
			{
				// Captures only
				player.GenerateLazyMoves(depth, movesPossible, Moves.EnmMovesType.RecapturesPromotions, moveAnalysed.To);
			}


			// Enhanced Transposition Cutoff
			foreach (Move movex in movesPossible)
			{
				if ( ((val = HashTable.ProbeHash(movex.HashCodeA, movex.HashCodeB, depth, alpha, beta, player.Colour)) != HashTable.Unknown) && val>=beta)
				{
					return beta;
				}
			}

			// Sort moves
			foreach (Move movex in movesPossible)
			{
				movex.Score = 0;

				if (moveHash!=null && movex.From.Ordinal==moveHash.From.Ordinal && movex.To.Ordinal==moveHash.To.Ordinal)
				{
					movex.Score += 1000000;
				}

				if (movex.Name==Move.EnmName.PawnPromotion)
				{
					movex.Score += 10000;
				}
					
				if (movex.PieceTaken!=null)
				{
//						if (depth>=5)
//						{
						// SEE (Static Exchange Evaluation)
//							Move moveSee = movex.Piece.SEEMove(movex.Name, movex.To);
//							movex.Score += -SEE(player.OtherPlayer, int.MinValue, int.MaxValue, movex.To);
//							Move.SEEUndo(moveSee);
//						}
//						else
//						{
						// MVV/LVA (Most Valuable Victim/Least Valuable Attacker)
						movex.Score += movex.PieceTaken.Value - movex.Piece.Value/10;
//						}
				}
				else
				{
					movex.Score += History.Retrieve(player.Colour, movex.From.Ordinal, movex.To.Ordinal)>>6;
				}
			}
			movesPossible.SortByScore();

//			ReSearch:

			foreach (Move move in movesPossible)
			{
				moveThis = move.Piece.Move(move.Name, move.To);

				if (moveThis.IsInCheck) { Move.Undo(moveThis); continue; }

				intLegalMovesAttempted++;

				if (moveBest==null)
				{
					moveBest = moveThis;
				}
/*
				// Futility Pruning
				switch (depth)
				{
					case 2:
					case 3:
						if (move.PieceTaken==null && !moveThis.IsInCheck && !move.IsEnemyInCheck)
						{
							if ( (val = HashTable.ProbeHash(Board.HashCodeA, Board.HashCodeB, depth, alpha, beta, player.Colour)) == HashTable.UNKNOWN )
							{
								val = -player.OtherPlayer.Score;	m_intEvaluations++;
								HashTable.RecordHash(Board.HashCodeA, Board.HashCodeB, depth, val, HashTable.enmHashType.Exact, -1, -1, Move.enmName.NullMove, player.Colour);
							}

							switch (depth)
							{
								case 2:
									// Standard Futility Pruning
									if (val+2500<=alpha)
									{
										depth--;
									}
									break;

								case 3:
									// Extended Futility Pruning
									if (val+6500<=alpha)
									{
										depth--;
									}
									break;
							}
						}
						break;
				}
*/				
if (Game.DisplayMoveAnalysisTree)
{
// Add moves to post-move analysis tree, if option set by user
moveAnalysed.Moves.Add(moveThis);
}
				if (blnPvNode)
				{
					val = -AlphaBeta(player.OtherPlayer, depth - 1, -alpha-1, -alpha, verify, ref moveThis);
					if (DateTime.Now-MPlayerClock.TurnStartTime > _mTsnThinkingTimeMaxAllowed) { Move.Undo(moveThis); goto TimeExpired;}
					if ((val > alpha) && (val < beta)) /* fail */
					{
						val = -AlphaBeta(player.OtherPlayer, depth - 1, -beta, -alpha, verify, ref moveThis);
						if (DateTime.Now-MPlayerClock.TurnStartTime > _mTsnThinkingTimeMaxAllowed) { Move.Undo(moveThis); goto TimeExpired;}
					}
				}
				else
				{
					val = -AlphaBeta(player.OtherPlayer, depth - 1, -beta, -alpha, verify, ref moveThis);
					if (DateTime.Now-MPlayerClock.TurnStartTime > _mTsnThinkingTimeMaxAllowed) { Move.Undo(moveThis); goto TimeExpired;}
				}

				if (!blnAllMovesWereGenerated && val<intScoreAtEntry)
				{
					// This code is executed mostly in quiescence when not all moves are tried (maybe just captures)
					// and the best score we've got is worse than the score we had before we considered any moves
					// then revert to that score, because we dont want the computer to think that it HAS to make a capture
					val = intScoreAtEntry;
				}

				move.Score = moveThis.Score = val;

				Move.Undo(moveThis);

				if (val >= beta)
				{
					alpha = beta;
					moveThis.Beta = beta;
					hashType = HashTable.EnmHashType.Beta;
					moveBest = moveThis;
					goto Exit;
				}

				if (val > alpha)
				{
					blnPvNode = true; /* This is a PV node */
					alpha = val;
					hashType = HashTable.EnmHashType.Exact;
					moveBest = moveThis;
				}

				moveThis.Alpha = alpha;
				moveThis.Beta = beta;
			}

			// Check for Stalemate
			if (intLegalMovesAttempted==0) // depth>0 && !player.OtherPlayer.IsInCheck
			{
				//	alpha = this.Score;
				alpha = -player.OtherPlayer.Score;
			}

/*			if(failhigh && alpha < beta)
			{
				depth++;
				failhigh = false;
				verify = true;
				goto ReSearch;
			}
*/

		Exit:

			// Record best move
			if (moveBest!=null)
			{
				History.Record(player.Colour, moveBest.From.Ordinal, moveBest.To.Ordinal, 0, 1<<(depth+6)); 
				HashTable.RecordHash(Board.HashCodeA, Board.HashCodeB, depth, alpha, hashType, moveBest.From.Ordinal, moveBest.To.Ordinal, moveBest.Name, player.Colour);
			}
			else
				HashTable.RecordHash(Board.HashCodeA, Board.HashCodeB, depth, alpha, hashType, -1, -1, Move.EnmName.NullMove, player.Colour);

TimeExpired:
			return alpha;
		}

		private int See(Player player, int alpha, int beta, Square squareCaptured)
		{
			// Static Exchange Evaluator

			var val = int.MinValue;
			var intScoreAtEntry = 0;

			Move moveThis = null;

			// Generate moves
			var movesPossible = new Moves();

			squareCaptured.AttackerMoveList(movesPossible, player);

			if (movesPossible.Count==0)
			{
				//	alpha = this.Score;
				alpha = -player.OtherPlayer.SeeScore;
			}
			else
			{
				// Sort moves
				foreach (Move movex in movesPossible)
				{
					movex.Score = movex.PieceTaken.Value - movex.Piece.Value/10;
				}
				movesPossible.SortByScore();

				foreach (Move move in movesPossible)
				{
					moveThis = move.Piece.SeeMove(move.Name, move.To);

					if (moveThis.IsInCheck) { Move.SeeUndo(moveThis); continue; }

					val = -See(player.OtherPlayer, -beta, -alpha, squareCaptured);

					if (val<intScoreAtEntry)
					{
						val = intScoreAtEntry;
					}

					Move.SeeUndo(moveThis);

					if (val >= beta)
					{
						return beta;
					}

					if (val > alpha)
					{
						alpha = val;
					}
				}

			}

			return alpha;
		}

		public Player OtherPlayer => MColour==EnmColour.White ? Game.PlayerBlack : Game.PlayerWhite;

	    public EnmColour Colour
		{
			get { return MColour; }
			set	{ MColour = value;	}
		}

		public Pieces Pieces => MColPieces;

	    public Pieces Pawns => MColPawns;

	    public Pieces Material => MColMaterial;

	    public Pieces CapturedEnemyPieces => MColCapturedEnemyPieces;

	    public int CapturedEnemyPiecesTotalBasicValue
		{
			get
			{
				var intValue = 0;
				foreach (Piece piece in MColCapturedEnemyPieces)
				{
					intValue += piece.BasicValue;
				}
				return intValue;
			}
		}

		public bool CanClaimThreeMoveRepetition
		{
			get
			{
				if (Game.MoveHistory.Count==0)
				{
					return false;
				}
				if (Colour==Game.MoveHistory.Last.Piece.Player.Colour)
				{
					return false;
				}
				Move move;
				var intRepetitionCount = 0;
				var intIndex=Game.MoveHistory.Count-1;
				for (; intIndex>=0; intIndex--, intIndex--)
				{
					move = Game.MoveHistory.Item(intIndex);
					if (move.HashCodeA==Board.HashCodeA && move.HashCodeB==Board.HashCodeB)
					{
						if (intRepetitionCount>=2)
						{
							return true;
						}
						intRepetitionCount++;
					}
					if (move.Piece.Name==Piece.EnmName.Pawn || move.PieceTaken!=null)
					{
						return false;
					}
				}
				return false;
			}
		}

		public int Points
		{
			get	
			{ 
				//SPECIAL
				//    If more than one piece is "hung" (attacked and not defended or 
				//    attacked by an enemy piece of lower value) an extra penalty of 10 
				//    points is invoked for that side and the search may be extended one 
				//    ply. Pinned or trapped pieces are treated similarly. A special mating 
				//    routine is used if one side has only a king and the other has mating 
				//    material. 
				var intPoints = 0;
				int intIndex;
				Piece piece;

				intPoints += PawnPoints;

				for (intIndex=MColPieces.Count-1; intIndex>=0; intIndex--)
				{
					piece = MColPieces.Item(intIndex);
					if (piece.Name!=Piece.EnmName.Pawn)
					{
						intPoints += piece.PointsTotal;
					}
				}


				// Multiple attack bonus
//				for (intIndex=this.OtherPlayer.m_colPieces.Count-1; intIndex>=0; intIndex--)
//				{
//					piece = this.OtherPlayer.m_colPieces.Item(intIndex);
//					intPoints += m_aintAttackBonus[piece.Square.NoOfAttacksBy(this)];
//				}

				// Factor in human 3 move repition draw condition
				// If this player is "human" then a draw if scored high, else a draw is scored low
				if (CanClaimThreeMoveRepetition)
				{
					intPoints += Intellegence==EnmIntellegence.Human ? 1000000000: 0;
				}


				if (QueensBishop.IsInPlay && KingsBishop.IsInPlay)
				{
					intPoints += 500;
				}

				if (QueensRook.IsInPlay && KingsRook.IsInPlay)
				{
					intPoints += 250;
				}

				if (HasCastled)
				{
					intPoints += 117;
				}
				else
				{
					if (King.HasMoved)
					{
						intPoints -= 147;
					}
					else
					{
						if (KingsRook.HasMoved)
						{
							intPoints -= 107;
						}
						if (QueensRook.HasMoved)
						{
							intPoints -= 107;
						}
					}
				}


				if (IsInCheck)
				{
					if (IsInCheckMate)
					{
						intPoints -= 999999999;
					}

				}

				return intPoints;
			}

		}

		public int Score => Points - OtherPlayer.Points;

	    public int SeePoints
		{
			get	
			{ 
				var intPoints = 0;
				int intIndex;
				Piece piece;

				for (intIndex=MColPieces.Count-1; intIndex>=0; intIndex--)
				{
					piece = MColPieces.Item(intIndex);
					intPoints += piece.Value;
				}
				return intPoints;
			}
		}

		public int SeeScore => SeePoints - OtherPlayer.SeePoints;
	}
}

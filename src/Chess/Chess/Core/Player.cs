using System;

namespace Chess.Core
{
	public abstract class Player
	{

		public enum enmColour
		{
				White
			,	Black
		}

		public enum enmIntellegence
		{
				Human
			,	Computer
		}

		public enum enmStatus
		{
				Normal
			,	InCheck
			,	InStaleMate
			,	InCheckMate
		}

		public event delegateGameEvent MoveConsidered;
		private const int MIN_SCORE = int.MinValue+1;
		private const int MAX_SCORE = int.MaxValue;
		private Move m_moveCurrent = null;
		private Move m_moveBest = null;
		private bool m_blnHasCastled = false;
		protected enmColour m_colour;
		protected Piece m_King;
		protected Piece m_Queen;
		protected Piece m_KingsPawn;
		protected Piece m_QueensPawn;
		protected Piece m_KingsRook;
		protected Piece m_QueensRook;
		protected Piece m_KingsBishop;
		protected Piece m_QueensBishop;
		protected Piece m_KingsKnight;
		protected Piece m_QueensKnight;
		protected Pieces m_colPieces;
		protected Pieces m_colPawns;
		protected Pieces m_colMaterial;
		protected Pieces m_colCapturedEnemyPieces;
		protected int m_NoOfPawnsInPlay = 8;
		protected int m_MaterialCount = 7;
		protected int m_intTotalMoves = 0;
		protected int m_intCurrentMoveNo = 0;
		protected int m_intEvaluations = 0;
		protected int m_intPositionsSearched = 0;
		protected bool m_blnIsThinking = false;
		private int m_intSearchDepth = 0;

		private const int m_intGameMoves = 120;

		private TimeSpan m_tsnThinkingTimeMaxAllowed;
		private TimeSpan m_tsnThinkingTimeAllotted;
		private TimeSpan m_tsnThinkingTimeHalved;

		public enmIntellegence Intellegence;
		private int m_intMaxQDepth = 0;
		
		private const int m_intMinSearchDepth = 1;
		private const int m_intMaxSearchDepth = 32;
		private const int m_intMinimumPlys = 4;
		private int R = 3;
		private int m_intRootScore;

		static int[] m_aintAttackBonus = {0, 3, 25, 53, 79, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99 };
	
		protected PlayerClock m_PlayerClock;

		public class PlayerClock
		{
			private TimeSpan m_tsnTimeLimit = new TimeSpan(0,60,0);
			private TimeSpan m_tsnTimeElapsed;
			private DateTime m_dtmTurnStart;
			private bool m_blnIsTicking = false;
			Player m_player;

			public PlayerClock(Player player)
			{
				m_player = player;
				Reset();
			}

			public DateTime TurnStartTime
			{
				get { return m_dtmTurnStart; }
			}

			public bool IsTicking
			{
				get { return m_blnIsTicking; }
			}

			public TimeSpan TimeElapsed
			{
				get
				{
					return m_blnIsTicking ? m_tsnTimeElapsed + (DateTime.Now - m_dtmTurnStart) : m_tsnTimeElapsed;
				}
				set
				{
					m_tsnTimeElapsed = value;
				}
			}

			public TimeSpan TimeLimit
			{
				get
				{
					return m_tsnTimeLimit;
				}
			}

			public TimeSpan TimeRemaining
			{
				get
				{
					return TimeLimit - TimeElapsed;
				}
			}

			public void Reset()
			{
				m_tsnTimeElapsed = new TimeSpan(0,0,0);
				m_dtmTurnStart = DateTime.Now;
			}

			public void Start()
			{
				m_blnIsTicking = true;
				m_dtmTurnStart = DateTime.Now;
			}

			public void Stop()
			{
				m_blnIsTicking = false;
				m_tsnTimeElapsed += (DateTime.Now - m_dtmTurnStart);
			}

			public void Revert()
			{
				m_blnIsTicking = false;
				m_dtmTurnStart = DateTime.Now;
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

		public bool IsThinking
		{
			get {return m_blnIsThinking;}
		}

		public int Evaluations
		{
			get {return m_intEvaluations;}
		}

		public int PositionsSearched
		{
			get {return m_intEvaluations;}
		}

		public int MaxSearchDepth
		{
			get { return m_intMaxSearchDepth; }
		}

		public int SearchDepth
		{
			get { return m_intSearchDepth; }
		}

		public TimeSpan ThinkingTimeAllotted
		{
			get { return m_tsnThinkingTimeAllotted; }
		}

		public TimeSpan ThinkingTimeRemaining
		{
			get { return m_tsnThinkingTimeAllotted-(DateTime.Now-m_PlayerClock.TurnStartTime); }
		}

		public enmStatus Status
		{
			get
			{
				if (this.IsInCheckMate ) return Player.enmStatus.InCheckMate;
				if (!this.CanMove) return Player.enmStatus.InStaleMate;
				if (this.IsInCheck) return Player.enmStatus.InCheck;
				return Player.enmStatus.Normal;
			}
		}

		public int MaterialCount
		{
			get
			{
				return this.m_MaterialCount;
			}
		}

		public int MaterialBasicValue
		{
			get
			{
				int intMaterialBasicValue = 0;
				foreach (Piece piece in m_colMaterial)
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
				int intTotalValue = 0;
				int intIndex;
				Piece piece;
				for (intIndex=this.m_colPieces.Count-1; intIndex>=0; intIndex--)
				{
					piece = this.m_colPieces.Item(intIndex);
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

				if ( (intPoints = HashTablePawn.ProbeHash(this.Colour)) == HashTablePawn.UNKNOWN )
				{
					Piece piece;
					intPoints = 0;
					for (intIndex=this.m_colPieces.Count-1; intIndex>=0; intIndex--)
					{
						piece = this.m_colPieces.Item(intIndex);
						if (piece.Name==Piece.enmName.Pawn)
						{
							intPoints += piece.PointsTotal;
						}
					}
					HashTablePawn.RecordHash(intPoints, this.Colour);
				}

				return intPoints;
			}
		}

		public bool HasCastled
		{
			get { return m_blnHasCastled; }
			set { m_blnHasCastled = value; }
		}

		public Piece King
		{
			get { return m_King; }
			set { m_King = value; }
		}

		public Piece Queen
		{
			get { return m_Queen; }
			set { m_Queen = value; }
		}

		public Piece KingsPawn
		{
			get { return m_KingsPawn; }
			set { m_KingsPawn = value; }
		}

		public Piece QueensPawn
		{
			get { return m_QueensPawn; }
			set { m_QueensPawn = value; }
		}

		public Piece KingsRook
		{
			get { return m_KingsRook; }
			set { m_KingsRook = value; }
		}

		public Piece QueensRook
		{
			get { return m_QueensRook; }
			set { m_QueensRook = value; }
		}

		public Piece KingsBishop
		{
			get { return m_KingsBishop; }
			set { m_KingsBishop = value; }
		}

		public Piece QueensBishop
		{
			get { return m_QueensBishop; }
			set { m_QueensBishop = value; }
		}

		public Piece KingsKnight
		{
			get { return m_KingsKnight; }
			set { m_KingsKnight = value; }
		}

		public Piece QueensKnight
		{
			get { return m_QueensKnight; }
			set { m_QueensKnight = value; }
		}

		public int TotalMoves
		{
			get { return m_intTotalMoves; }
		}

		public int MaxQuiesDepth
		{
			get { return m_intMaxQDepth; }
		}

		public Move CurrentMove
		{
			get { return m_moveCurrent; }
		}

		public Move BestMove
		{
			get { return m_moveBest; }
		}

		public int CurrentMoveNo
		{
			get { return m_intCurrentMoveNo; }
		}


		public Player()
		{
			m_colPieces = new Pieces(this);
			m_colPawns = new Pieces(this);
			m_colMaterial = new Pieces(this);
			m_colCapturedEnemyPieces = new Pieces(this);
		}

		public int PawnsInPlay
		{
			get { return m_NoOfPawnsInPlay; } 
		}

		public void DecreasePawnCount()
		{
			m_NoOfPawnsInPlay--;
		}

		public void IncreasePawnCount()
		{
			m_NoOfPawnsInPlay++;
		}

		public void DecreaseMaterialCount()
		{
			m_MaterialCount--;
		}

		public void IncreaseMaterialCount()
		{
			m_MaterialCount++;
		}

		public bool CanMove
		{
			get
			{
				Moves moves;
				foreach (Piece piece in m_colPieces )
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
			return ((PieceKing)this.King.Top).DetermineCheckStatus();
		}

		public bool IsInCheck
		{
			get
			{
				return HashTableCheck.IsPlayerInCheck(this);
			}
		}

		public bool IsInCheckMate
		{
			get
			{ 
				if (!IsInCheck) return false;

				Moves moves = new Moves();
				GenerateLegalMoves(moves);
				return (moves.Count==0);
			}
		}

		public void GenerateLegalMoves(Moves moves)
		{
			foreach (Piece piece in m_colPieces)
			{
				piece.GenerateLegalMoves(moves);
			}
		}

		public void GenerateLazyMoves(int depth, Moves moves, Moves.enmMovesType movesType, Square squareAttacking)
		{
//			if (squareAttacking==null)
//			{
				// All moves as defined by movesType
				foreach (Piece piece in this.m_colPieces)
				{
					piece.GenerateLazyMoves(moves, movesType);

					if (movesType!=Moves.enmMovesType.All)
					{
						Move move;
						int intIndex;
						for (intIndex=moves.Count-1; intIndex>=0; intIndex--)
						{
							move = moves.Item(intIndex);
							if ( !(
								move.Name==Move.enmName.PawnPromotion
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
			m_blnIsThinking = true;

			Player player = this;
			m_moveBest = null;
			Move moveHash = null;
			int alpha_start = this.Score;

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
			int alpha = MIN_SCORE;
			int beta = MAX_SCORE;

			m_intRootScore = this.Score;

			m_tsnThinkingTimeAllotted = new TimeSpan( this.m_PlayerClock.TimeRemaining.Ticks / Math.Max(m_intGameMoves-(Game.TurnNo/2), 1) );
			m_tsnThinkingTimeMaxAllowed = new TimeSpan( m_tsnThinkingTimeAllotted.Ticks*3 );
			m_tsnThinkingTimeHalved = new TimeSpan( m_tsnThinkingTimeAllotted.Ticks/3);

			m_intEvaluations = 0;
			m_intPositionsSearched = 0;

			HashTable.ResetStats();
//			HashTable.Clear();   Uncomment this to clear the hashtable at the beginning of each move
			HashTableCheck.ResetStats();
			HashTablePawn.ResetStats();
			History.Clear();

			for (m_intSearchDepth=m_intMinSearchDepth; m_intSearchDepth<=m_intMaxSearchDepth; m_intSearchDepth++)
			{


if (Game.DisplayMoveAnalysisTree)
{
	Game.MoveAnalysis = new Moves();
}
				m_intMaxQDepth = m_intSearchDepth;

				// Get last iteration's best move from the HashTable
				moveHash = HashTable.ProbeForBestMove(player.Colour);

				// Generate and sort moves
				Moves movesPossible = new Moves();
				player.GenerateLegalMoves(movesPossible);
				m_intTotalMoves = movesPossible.Count;

				// If only one move is available, then just play it!
				if (movesPossible.Count==1)
				{
					moveDepthBest = m_moveCurrent = movesPossible.Item(0);
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

					if (movex.Name==Move.enmName.PawnPromotion)
					{
						movex.Score += 10000;
					}
						
					if (movex.PieceTaken!=null)
					{
						Move moveSee = movex.Piece.SEEMove(movex.Name, movex.To);
						movex.Score += -SEE(player.OtherPlayer, int.MinValue, int.MaxValue, movex.To);
						Move.SEEUndo(moveSee);
					}
					else
					{
						movex.Score += History.Retrieve(player.Colour, movex.From.Ordinal, movex.To.Ordinal);
					}
				}
				movesPossible.SortByScore();

				alpha = MIN_SCORE;
				beta  = MAX_SCORE;

				m_intCurrentMoveNo = 0;

				foreach (Move move in movesPossible)
				{
					m_moveCurrent = move.Piece.Move(move.Name, move.To);

					if (m_moveCurrent.IsInCheck) { Move.Undo(m_moveCurrent); continue; }

					move.Score = m_moveCurrent.Score = -AlphaBeta(player.OtherPlayer, m_intSearchDepth-1, -alpha-1, -alpha, true, ref m_moveCurrent);
					if ((DateTime.Now-m_PlayerClock.TurnStartTime) > m_tsnThinkingTimeMaxAllowed) { Move.Undo(m_moveCurrent); goto TimeExpired;}

					if ((move.Score > alpha) && (move.Score < beta)) /* fail */
					{
						move.Score = m_moveCurrent.Score = -AlphaBeta(player.OtherPlayer, m_intSearchDepth-1, -beta, -alpha, true, ref m_moveCurrent);
						if ((DateTime.Now-m_PlayerClock.TurnStartTime) > m_tsnThinkingTimeMaxAllowed) { Move.Undo(m_moveCurrent); goto TimeExpired;}
					}
					
					this.MoveConsidered();

if (Game.DisplayMoveAnalysisTree)
{
	Game.MoveAnalysis.Add(m_moveCurrent);
}

					Move.Undo(m_moveCurrent);

					m_intCurrentMoveNo++;

					if (m_moveCurrent.Score > alpha)
					{
						alpha = m_moveCurrent.Score;
						moveDepthBest = m_moveCurrent;
						History.Record(player.Colour, m_moveCurrent.From.Ordinal, m_moveCurrent.To.Ordinal, alpha-alpha_start, 1<<(m_intSearchDepth+6)); // Update history heuristic data
					}

					m_moveCurrent.Alpha = alpha;
					m_moveCurrent.Beta = beta;

				}

			MoveSelected:

				m_moveBest = moveDepthBest;

				// Record best move
				HashTable.RecordHash(Board.HashCodeA, Board.HashCodeB, m_intSearchDepth, m_moveBest.Score, HashTable.enmHashType.Exact, m_moveBest.From.Ordinal, m_moveBest.To.Ordinal, m_moveBest.Name, player.Colour);

				this.MoveConsidered();

				if ((DateTime.Now-m_PlayerClock.TurnStartTime) > m_tsnThinkingTimeHalved && m_intSearchDepth >= m_intMinimumPlys ) goto TimeExpired;

				if (m_moveBest.Score > 99999) break; // Checkmate found so dont bother searching any deeper
			}


			TimeExpired:
			this.MoveConsidered();

			m_blnIsThinking = false;
			this.MoveConsidered();

			return m_moveBest;
		}


		private int AlphaBeta(Player player, int depth, int alpha, int beta, bool verify, ref Move moveAnalysed)
		{
			int val = int.MinValue;
			HashTable.enmHashType hashType = HashTable.enmHashType.Alpha;
			Move moveBest = null;
			Move moveHash = null;
			bool blnPVNode = false;
			int intScoreAtEntry = 0;
//			bool failhigh = false;
			bool blnAllMovesWereGenerated;
			int intLegalMovesAttempted = 0;

			m_intPositionsSearched++;

			if ( (val = HashTable.ProbeHash(Board.HashCodeA, Board.HashCodeB, depth, alpha, beta, player.Colour)) != HashTable.UNKNOWN )
			{
				// High values of "val" indicate that a checkmate has been found
				if (val>1000000 || val<-1000000)
				{
					val /= (m_intMaxSearchDepth-depth);
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
				if (depth < m_intMaxQDepth)
				{	
					m_intMaxQDepth = depth;
					if (m_intMaxQDepth<0)
					{
						m_intMaxQDepth+=0;
					}
				}
				intScoreAtEntry = val = -player.OtherPlayer.Score;	m_intEvaluations++;

				if (val>100000000 || val<-100000000) 
				{
					val /= (m_intMaxSearchDepth-depth);
				}
				// Allow a deeper ply of search if a piece was captured or if a pawn was promoted, 
				// or either side is in check.
				if ( !( 
						moveAnalysed.PieceTaken != null 
						||
						moveAnalysed.Name == Move.enmName.PawnPromotion
						||
						( moveAnalysed.IsInCheck || moveAnalysed.IsEnemyInCheck )
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
			R = (depth>6 && Game.Stage!=Game.enmStage.End) ? 3 : 2; //  << This is the "adaptive" bit
			// The rest is normal Null-move forward pruning
			if (depth >= 3 )
			{
				Move moveNull = new Move(Game.TurnNo, 0, Move.enmName.NullMove, null, null, null, null, 0, 0);
				val = -AlphaBeta(player.OtherPlayer, depth - R - 1, -beta, -beta + 1, verify, ref moveNull);
				if ((DateTime.Now-m_PlayerClock.TurnStartTime) > m_tsnThinkingTimeMaxAllowed) goto TimeExpired;
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
			Moves movesPossible = new Moves();

			blnAllMovesWereGenerated=( depth>0 || (moveAnalysed.IsInCheck || moveAnalysed.IsEnemyInCheck));
			if ( blnAllMovesWereGenerated ) 
			{
				player.GenerateLazyMoves(depth, movesPossible, Moves.enmMovesType.All, null);
			}
			else
			{
				// Captures only
				player.GenerateLazyMoves(depth, movesPossible, Moves.enmMovesType.Recaptures_Promotions, moveAnalysed.To);
			}


			// Enhanced Transposition Cutoff
			foreach (Move movex in movesPossible)
			{
				if ( ((val = HashTable.ProbeHash(movex.HashCodeA, movex.HashCodeB, depth, alpha, beta, player.Colour)) != HashTable.UNKNOWN) && val>=beta)
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

				if (movex.Name==Move.enmName.PawnPromotion)
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
						movex.Score += (movex.PieceTaken.Value - movex.Piece.Value/10);
//						}
				}
				else
				{
					movex.Score += (History.Retrieve(player.Colour, movex.From.Ordinal, movex.To.Ordinal)>>6);
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
				if (blnPVNode)
				{
					val = -AlphaBeta(player.OtherPlayer, depth - 1, -alpha-1, -alpha, verify, ref moveThis);
					if ((DateTime.Now-m_PlayerClock.TurnStartTime) > m_tsnThinkingTimeMaxAllowed) { Move.Undo(moveThis); goto TimeExpired;}
					if ((val > alpha) && (val < beta)) /* fail */
					{
						val = -AlphaBeta(player.OtherPlayer, depth - 1, -beta, -alpha, verify, ref moveThis);
						if ((DateTime.Now-m_PlayerClock.TurnStartTime) > m_tsnThinkingTimeMaxAllowed) { Move.Undo(moveThis); goto TimeExpired;}
					}
				}
				else
				{
					val = -AlphaBeta(player.OtherPlayer, depth - 1, -beta, -alpha, verify, ref moveThis);
					if ((DateTime.Now-m_PlayerClock.TurnStartTime) > m_tsnThinkingTimeMaxAllowed) { Move.Undo(moveThis); goto TimeExpired;}
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
					hashType = HashTable.enmHashType.Beta;
					moveBest = moveThis;
					goto Exit;
				}

				if (val > alpha)
				{
					blnPVNode = true; /* This is a PV node */
					alpha = val;
					hashType = HashTable.enmHashType.Exact;
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
				HashTable.RecordHash(Board.HashCodeA, Board.HashCodeB, depth, alpha, hashType, -1, -1, Move.enmName.NullMove, player.Colour);

TimeExpired:
			return alpha;
		}

		private int SEE(Player player, int alpha, int beta, Square squareCaptured)
		{
			// Static Exchange Evaluator

			int val = int.MinValue;
			int intScoreAtEntry = 0;

			Move moveThis = null;

			// Generate moves
			Moves movesPossible = new Moves();

			squareCaptured.AttackerMoveList(movesPossible, player);

			if (movesPossible.Count==0)
			{
				//	alpha = this.Score;
				alpha = -player.OtherPlayer.SEEScore;
			}
			else
			{
				// Sort moves
				foreach (Move movex in movesPossible)
				{
					movex.Score = (movex.PieceTaken.Value - movex.Piece.Value/10);
				}
				movesPossible.SortByScore();

				foreach (Move move in movesPossible)
				{
					moveThis = move.Piece.SEEMove(move.Name, move.To);

					if (moveThis.IsInCheck) { Move.SEEUndo(moveThis); continue; }

					val = -SEE(player.OtherPlayer, -beta, -alpha, squareCaptured);

					if (val<intScoreAtEntry)
					{
						val = intScoreAtEntry;
					}

					Move.SEEUndo(moveThis);

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

		public Player OtherPlayer
		{
			get { return this.m_colour==Player.enmColour.White ? Game.PlayerBlack : Game.PlayerWhite; }
		}

		public enmColour Colour
		{
			get { return m_colour; }
			set	{ m_colour = value;	}
		}

		public Pieces Pieces
		{
			get	{ return m_colPieces; }
		}

		public Pieces Pawns
		{
			get	{ return m_colPawns; }
		}

		public Pieces Material
		{
			get	{ return m_colMaterial; }
		}

		public Pieces CapturedEnemyPieces
		{
			get	{ return m_colCapturedEnemyPieces; }
		}

		public int CapturedEnemyPiecesTotalBasicValue
		{
			get
			{
				int intValue = 0;
				foreach (Piece piece in m_colCapturedEnemyPieces)
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
				if (this.Colour==Game.MoveHistory.Last.Piece.Player.Colour)
				{
					return false;
				}
				Move move;
				int intRepetitionCount = 0;
				int intIndex=Game.MoveHistory.Count-1;
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
					if (move.Piece.Name==Piece.enmName.Pawn || move.PieceTaken!=null)
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
				int intPoints = 0;
				int intIndex;
				Piece piece;

				intPoints += this.PawnPoints;

				for (intIndex=this.m_colPieces.Count-1; intIndex>=0; intIndex--)
				{
					piece = this.m_colPieces.Item(intIndex);
					if (piece.Name!=Piece.enmName.Pawn)
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
				if (this.CanClaimThreeMoveRepetition)
				{
					intPoints += (this.Intellegence==Player.enmIntellegence.Human ? 1000000000: 0 );
				}


				if (this.QueensBishop.IsInPlay && this.KingsBishop.IsInPlay)
				{
					intPoints += 500;
				}

				if (this.QueensRook.IsInPlay && this.KingsRook.IsInPlay)
				{
					intPoints += 250;
				}

				if (this.HasCastled)
				{
					intPoints += 117;
				}
				else
				{
					if (this.King.HasMoved)
					{
						intPoints -= 147;
					}
					else
					{
						if (this.KingsRook.HasMoved)
						{
							intPoints -= 107;
						}
						if (this.QueensRook.HasMoved)
						{
							intPoints -= 107;
						}
					}
				}


				if (this.IsInCheck)
				{
					if (this.IsInCheckMate)
					{
						intPoints -= 999999999;
					}

				}

				return intPoints;
			}

		}

		public int Score
		{
			get { return this.Points - this.OtherPlayer.Points; }
		}

		public int SEEPoints
		{
			get	
			{ 
				int intPoints = 0;
				int intIndex;
				Piece piece;

				for (intIndex=this.m_colPieces.Count-1; intIndex>=0; intIndex--)
				{
					piece = this.m_colPieces.Item(intIndex);
					intPoints += piece.Value;
				}
				return intPoints;
			}
		}

		public int SEEScore
		{
			get { return this.SEEPoints - this.OtherPlayer.SEEPoints; }
		}

	}
}

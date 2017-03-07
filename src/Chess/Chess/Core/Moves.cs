using System.Collections;

namespace Chess.Core
{
	public class Moves: IEnumerable
	{
		private Piece m_pieceParent = null;
		private ArrayList m_colMoves = new ArrayList(48);

		public enum enmMovesType
		{
					All
				,	Recaptures_Promotions
				,	CapturesChecksPromotions
		}

		public Moves()
		{
		}

		public Moves(Piece pieceParent)
		{
			m_pieceParent = pieceParent;
		}

		public IEnumerator GetEnumerator()
		{
			return m_colMoves.GetEnumerator();
		}

		public Piece Parent
		{
			get { return m_pieceParent; }
		}

		public int Count
		{
			get { return m_colMoves.Count; }
		}

		public Move Item(int intIndex)
		{
			return (Move)m_colMoves[intIndex];
		}

		public Move Last
		{
			get { return m_colMoves.Count>0 ? (Move)m_colMoves[m_colMoves.Count-1] : null; }
		}

		public Move PenultimateForSameSide
		{
			get { return m_colMoves.Count>2 ? (Move)m_colMoves[m_colMoves.Count-3] : null;  }
		}

		public Move Penultimate
		{
			get { return m_colMoves.Count>1 ? (Move)m_colMoves[m_colMoves.Count-2] : null;  }
		}

		public void Add(int TurnNo, int LastMoveTurnNo, Move.enmName Name, Piece Piece, Square From, Square To, Piece PieceTaken, int PieceTakenOrdinal, int Score)
		{
			m_colMoves.Add(new Move(TurnNo, LastMoveTurnNo, Name, Piece, From, To, PieceTaken, PieceTakenOrdinal, Score));
		}

		public void Add(Move move)
		{
			m_colMoves.Add(move);
		}

		public void Remove(Move Move)
		{
			m_colMoves.Remove(Move);
		}

		public void RemoveLast()
		{
			m_colMoves.RemoveAt(m_colMoves.Count-1);
		}

		public void Clear()
		{
			m_colMoves.Clear();
		}

		public void Replace(int intIndex, Move moveNew )
		{
			m_colMoves[intIndex] = moveNew;
		}

		public void SortByScore()
		{
			m_colMoves.Sort();
		}

	}
}

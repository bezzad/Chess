using System.Collections;

namespace Chess.Core
{
	public class Moves: IEnumerable
	{
	    private readonly ArrayList _mColMoves = new ArrayList(48);

		public enum EnmMovesType
		{
					All
				,	RecapturesPromotions
				,	CapturesChecksPromotions
		}

		public Moves()
		{
		}

		public Moves(Piece pieceParent)
		{
			Parent = pieceParent;
		}

		public IEnumerator GetEnumerator()
		{
			return _mColMoves.GetEnumerator();
		}

		public Piece Parent { get; }

	    public int Count => _mColMoves.Count;

	    public Move Item(int intIndex)
		{
			return (Move)_mColMoves[intIndex];
		}

		public Move Last => _mColMoves.Count>0 ? (Move)_mColMoves[_mColMoves.Count-1] : null;

	    public Move PenultimateForSameSide => _mColMoves.Count>2 ? (Move)_mColMoves[_mColMoves.Count-3] : null;

	    public Move Penultimate => _mColMoves.Count>1 ? (Move)_mColMoves[_mColMoves.Count-2] : null;

	    public void Add(int turnNo, int lastMoveTurnNo, Move.EnmName name, Piece piece, Square @from, Square to, Piece pieceTaken, int pieceTakenOrdinal, int score)
		{
			_mColMoves.Add(new Move(turnNo, lastMoveTurnNo, name, piece, @from, to, pieceTaken, pieceTakenOrdinal, score));
		}

		public void Add(Move move)
		{
			_mColMoves.Add(move);
		}

		public void Remove(Move move)
		{
			_mColMoves.Remove(move);
		}

		public void RemoveLast()
		{
			_mColMoves.RemoveAt(_mColMoves.Count-1);
		}

		public void Clear()
		{
			_mColMoves.Clear();
		}

		public void Replace(int intIndex, Move moveNew )
		{
			_mColMoves[intIndex] = moveNew;
		}

		public void SortByScore()
		{
			_mColMoves.Sort();
		}

	}
}

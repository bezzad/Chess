using System.Collections;

namespace Chess.Core
{
	public class Pieces: IEnumerable
	{
	    private readonly ArrayList _mColPieces = new ArrayList();

		public Pieces(Player player)
		{
			Player = player;
		}

		public Player Player { get; }

	    public IEnumerator GetEnumerator()
		{
			return _mColPieces.GetEnumerator();
		}

		public Piece Item(int intIndex)
		{
			return (Piece)_mColPieces[intIndex];
		}

		public int Count => _mColPieces.Count;

	    public void Add(Piece piece)
		{
			_mColPieces.Add(piece);
		}

		public void Insert(int ordinal, Piece piece)
		{
			_mColPieces.Insert(ordinal, piece);
		}

		public int IndexOf(Piece piece)
		{
			return _mColPieces.IndexOf(piece);
		}

		public void Remove(Piece piece)
		{
			_mColPieces.Remove(piece);
		}

		public void SortByScore()
		{
			_mColPieces.Sort();
		}

		public object Clone()
		{
			return _mColPieces.Clone();
		}

	}
}

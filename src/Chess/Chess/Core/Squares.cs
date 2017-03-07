using System.Collections;

namespace Chess.Core
{
	public class Squares: IEnumerable
	{
		private readonly ArrayList _mColSquares = new ArrayList(24);

		public IEnumerator GetEnumerator()
		{
			return _mColSquares.GetEnumerator();
		}

		public Square Item(int intIndex)
		{
			return (Square)_mColSquares[intIndex];
		}

		public int Count => _mColSquares.Count;

	    public void Add(Square square)
		{
			_mColSquares.Add(square);
		}

		public void Insert(int ordinal, Square square)
		{
			_mColSquares.Insert(ordinal, square);
		}

		public int IndexOf(Square square)
		{
			return _mColSquares.IndexOf(square);
		}

		public void Remove(Square square)
		{
			_mColSquares.Remove(square);
		}
	}
}

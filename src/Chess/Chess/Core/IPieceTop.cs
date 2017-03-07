namespace Chess.Core
{
	public interface IPieceTop
	{
		Piece Base
		{
			get;
		}

		string Abbreviation
		{
			get;
		}

		Piece.EnmName Name
		{
			get;
		}
	
		int BasicValue
		{
			get;
		}

		int Value
		{
			get;
		}

		int PositionalValue
		{
			get;
		}

		int ImageIndex
		{
			get;
		}
	
		bool CanBeTaken
		{
			get;
		}


		void GenerateLazyMoves(Moves moves, Moves.EnmMovesType movesType);
	}
}

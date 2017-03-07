namespace Chess.Core
{
	public class PlayerWhite: Player
	{
		public PlayerWhite()
		{
			Piece piece;

			m_PlayerClock = new PlayerClock(this);

			this.Colour = Player.enmColour.White;
			this.Intellegence = Player.enmIntellegence.Human;

			this.m_colPieces.Add( this.King = (new Piece(Piece.enmName.King, this, 4,0, Piece.enmID.WhiteKing)) );

			this.Queen = (new Piece(Piece.enmName.Queen, this, 3,0, Piece.enmID.WhiteQueen )); this.m_colPieces.Add(this.Queen); this.m_colMaterial.Add(this.Queen);

			this.QueensRook = (new Piece(Piece.enmName.Rook, this, 0,0, Piece.enmID.WhiteQueensRook )); this.m_colPieces.Add(this.QueensRook); this.m_colMaterial.Add(this.QueensRook);
			this.KingsRook  = (new Piece(Piece.enmName.Rook, this, 7,0, Piece.enmID.WhiteKingsRook )); this.m_colPieces.Add(this.KingsRook ); this.m_colMaterial.Add(this.KingsRook );
			
			this.QueensBishop = (new Piece(Piece.enmName.Bishop, this, 2,0, Piece.enmID.WhiteQueensBishop )); this.m_colPieces.Add(this.QueensBishop); this.m_colMaterial.Add(this.QueensBishop);
			this.KingsBishop  = (new Piece(Piece.enmName.Bishop, this, 5,0, Piece.enmID.WhiteKingsBishop )); this.m_colPieces.Add(this.KingsBishop ); this.m_colMaterial.Add(this.KingsBishop );
			
			this.QueensKnight = (new Piece(Piece.enmName.Knight, this, 1,0, Piece.enmID.WhiteQueensKnight )); this.m_colPieces.Add(this.QueensKnight); this.m_colMaterial.Add(this.QueensKnight);
			this.KingsKnight  = (new Piece(Piece.enmName.Knight, this, 6,0, Piece.enmID.WhiteKingsKnight )); this.m_colPieces.Add(this.KingsKnight ); this.m_colMaterial.Add(this.KingsKnight );
			
			piece = new Piece(Piece.enmName.Pawn, this, 0,1, Piece.enmID.WhitePawn1); this.m_colPieces.Add(piece); this.m_colPawns.Add(piece); 
			piece = new Piece(Piece.enmName.Pawn, this, 1,1, Piece.enmID.WhitePawn2); this.m_colPieces.Add(piece); this.m_colPawns.Add(piece); 
			piece = new Piece(Piece.enmName.Pawn, this, 2,1, Piece.enmID.WhitePawn3); this.m_colPieces.Add(piece); this.m_colPawns.Add(piece); 
			piece = new Piece(Piece.enmName.Pawn, this, 5,1, Piece.enmID.WhitePawn6); this.m_colPieces.Add(piece); this.m_colPawns.Add(piece); 
			piece = new Piece(Piece.enmName.Pawn, this, 6,1, Piece.enmID.WhitePawn7); this.m_colPieces.Add(piece); this.m_colPawns.Add(piece); 
			piece = new Piece(Piece.enmName.Pawn, this, 7,1, Piece.enmID.WhitePawn8); this.m_colPieces.Add(piece); this.m_colPawns.Add(piece); 

			this.QueensPawn = new Piece(Piece.enmName.Pawn, this, 3,1, Piece.enmID.WhitePawn4); this.m_colPieces.Add(this.QueensPawn); this.m_colPawns.Add(this.QueensPawn); 
			this.KingsPawn  = new Piece(Piece.enmName.Pawn, this, 4,1, Piece.enmID.WhitePawn5); this.m_colPieces.Add(this.KingsPawn ); this.m_colPawns.Add(this.KingsPawn ); 
		}

		public override int PawnForwardOffset
		{
			get { return 16; }
		}

		public override int PawnAttackRightOffset
		{
			get { return 17; }
		}

		public override int PawnAttackLeftOffset
		{
			get { return 15; }
		}

		public override Player.PlayerClock Clock
		{
			get
			{
				return m_PlayerClock;
			}
		}

	}
}

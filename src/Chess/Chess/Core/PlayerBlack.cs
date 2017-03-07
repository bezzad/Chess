namespace Chess.Core
{
	public class PlayerBlack: Player
	{
		public PlayerBlack()
		{
			Piece piece;

			m_PlayerClock = new PlayerClock(this);

			this.Colour = Player.enmColour.Black;
			this.Intellegence = Player.enmIntellegence.Computer;

			this.m_colPieces.Add( this.King = (new Piece(Piece.enmName.King, this, 4,7, Piece.enmID.BlackKing )) );

			this.Queen = (new Piece(Piece.enmName.Queen, this, 3,7, Piece.enmID.BlackQueen )); this.m_colPieces.Add(this.Queen); this.m_colMaterial.Add(this.Queen);

			this.QueensRook = (new Piece(Piece.enmName.Rook, this, 0,7, Piece.enmID.BlackQueensRook )); this.m_colPieces.Add(this.QueensRook); this.m_colMaterial.Add(this.QueensRook);
			this.KingsRook  = (new Piece(Piece.enmName.Rook, this, 7,7, Piece.enmID.BlackKingsRook )); this.m_colPieces.Add(this.KingsRook ); this.m_colMaterial.Add(this.KingsRook );
			
			this.QueensBishop = (new Piece(Piece.enmName.Bishop, this, 2,7, Piece.enmID.BlackQueensBishop)); this.m_colPieces.Add(this.QueensBishop); this.m_colMaterial.Add(this.QueensBishop);
			this.KingsBishop  = (new Piece(Piece.enmName.Bishop, this, 5,7, Piece.enmID.BlackKingsBishop )); this.m_colPieces.Add(this.KingsBishop ); this.m_colMaterial.Add(this.KingsBishop );
			
			this.QueensKnight = (new Piece(Piece.enmName.Knight, this, 1,7, Piece.enmID.BlackQueensKnight )); this.m_colPieces.Add(this.QueensKnight); this.m_colMaterial.Add(this.QueensKnight);
			this.KingsKnight  = (new Piece(Piece.enmName.Knight, this, 6,7, Piece.enmID.BlackKingsKnight )); this.m_colPieces.Add(this.KingsKnight ); this.m_colMaterial.Add(this.KingsKnight );
			
			piece = new Piece(Piece.enmName.Pawn, this, 0,6, Piece.enmID.BlackPawn1); this.m_colPieces.Add(piece); this.m_colPawns.Add(piece); 
			piece = new Piece(Piece.enmName.Pawn, this, 1,6, Piece.enmID.BlackPawn2); this.m_colPieces.Add(piece); this.m_colPawns.Add(piece); 
			piece = new Piece(Piece.enmName.Pawn, this, 2,6, Piece.enmID.BlackPawn3); this.m_colPieces.Add(piece); this.m_colPawns.Add(piece); 
			piece = new Piece(Piece.enmName.Pawn, this, 5,6, Piece.enmID.BlackPawn6); this.m_colPieces.Add(piece); this.m_colPawns.Add(piece); 
			piece = new Piece(Piece.enmName.Pawn, this, 6,6, Piece.enmID.BlackPawn7); this.m_colPieces.Add(piece); this.m_colPawns.Add(piece); 
			piece = new Piece(Piece.enmName.Pawn, this, 7,6, Piece.enmID.BlackPawn8); this.m_colPieces.Add(piece); this.m_colPawns.Add(piece); 

			this.QueensPawn = new Piece(Piece.enmName.Pawn, this, 3,6, Piece.enmID.BlackPawn4); this.m_colPieces.Add(this.QueensPawn); this.m_colPawns.Add(this.QueensPawn); 
			this.KingsPawn  = new Piece(Piece.enmName.Pawn, this, 4,6, Piece.enmID.BlackPawn5); this.m_colPieces.Add(this.KingsPawn ); this.m_colPawns.Add(this.KingsPawn ); 
		}

		public override int PawnForwardOffset
		{
			get { return -16; }
		}

		public override int PawnAttackRightOffset
		{
			get { return -15; }
		}

		public override int PawnAttackLeftOffset
		{
			get { return -17; }
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

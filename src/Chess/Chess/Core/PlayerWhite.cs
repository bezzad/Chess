namespace Chess.Core
{
	public class PlayerWhite: Player
	{
		public PlayerWhite()
		{
			Piece piece;

			MPlayerClock = new PlayerClock(this);

			Colour = EnmColour.White;
			Intellegence = EnmIntellegence.Human;

			MColPieces.Add( King = new Piece(Piece.EnmName.King, this, 4,0, Piece.EnmId.WhiteKing) );

			Queen = new Piece(Piece.EnmName.Queen, this, 3,0, Piece.EnmId.WhiteQueen ); MColPieces.Add(Queen); MColMaterial.Add(Queen);

			QueensRook = new Piece(Piece.EnmName.Rook, this, 0,0, Piece.EnmId.WhiteQueensRook ); MColPieces.Add(QueensRook); MColMaterial.Add(QueensRook);
			KingsRook  = new Piece(Piece.EnmName.Rook, this, 7,0, Piece.EnmId.WhiteKingsRook ); MColPieces.Add(KingsRook ); MColMaterial.Add(KingsRook );
			
			QueensBishop = new Piece(Piece.EnmName.Bishop, this, 2,0, Piece.EnmId.WhiteQueensBishop ); MColPieces.Add(QueensBishop); MColMaterial.Add(QueensBishop);
			KingsBishop  = new Piece(Piece.EnmName.Bishop, this, 5,0, Piece.EnmId.WhiteKingsBishop ); MColPieces.Add(KingsBishop ); MColMaterial.Add(KingsBishop );
			
			QueensKnight = new Piece(Piece.EnmName.Knight, this, 1,0, Piece.EnmId.WhiteQueensKnight ); MColPieces.Add(QueensKnight); MColMaterial.Add(QueensKnight);
			KingsKnight  = new Piece(Piece.EnmName.Knight, this, 6,0, Piece.EnmId.WhiteKingsKnight ); MColPieces.Add(KingsKnight ); MColMaterial.Add(KingsKnight );
			
			piece = new Piece(Piece.EnmName.Pawn, this, 0,1, Piece.EnmId.WhitePawn1); MColPieces.Add(piece); MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 1,1, Piece.EnmId.WhitePawn2); MColPieces.Add(piece); MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 2,1, Piece.EnmId.WhitePawn3); MColPieces.Add(piece); MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 5,1, Piece.EnmId.WhitePawn6); MColPieces.Add(piece); MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 6,1, Piece.EnmId.WhitePawn7); MColPieces.Add(piece); MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 7,1, Piece.EnmId.WhitePawn8); MColPieces.Add(piece); MColPawns.Add(piece); 

			QueensPawn = new Piece(Piece.EnmName.Pawn, this, 3,1, Piece.EnmId.WhitePawn4); MColPieces.Add(QueensPawn); MColPawns.Add(QueensPawn); 
			KingsPawn  = new Piece(Piece.EnmName.Pawn, this, 4,1, Piece.EnmId.WhitePawn5); MColPieces.Add(KingsPawn ); MColPawns.Add(KingsPawn ); 
		}

		public override int PawnForwardOffset => 16;

	    public override int PawnAttackRightOffset => 17;

	    public override int PawnAttackLeftOffset => 15;

	    public override PlayerClock Clock => MPlayerClock;
	}
}

namespace Chess.Core
{
	public class PlayerBlack: Player
	{
		public PlayerBlack()
		{
			Piece piece;

			MPlayerClock = new PlayerClock(this);

			Colour = EnmColour.Black;
			Intellegence = EnmIntellegence.Computer;

			MColPieces.Add( King = new Piece(Piece.EnmName.King, this, 4,7, Piece.EnmId.BlackKing ) );

			Queen = new Piece(Piece.EnmName.Queen, this, 3,7, Piece.EnmId.BlackQueen ); MColPieces.Add(Queen); MColMaterial.Add(Queen);

			QueensRook = new Piece(Piece.EnmName.Rook, this, 0,7, Piece.EnmId.BlackQueensRook ); MColPieces.Add(QueensRook); MColMaterial.Add(QueensRook);
			KingsRook  = new Piece(Piece.EnmName.Rook, this, 7,7, Piece.EnmId.BlackKingsRook ); MColPieces.Add(KingsRook ); MColMaterial.Add(KingsRook );
			
			QueensBishop = new Piece(Piece.EnmName.Bishop, this, 2,7, Piece.EnmId.BlackQueensBishop); MColPieces.Add(QueensBishop); MColMaterial.Add(QueensBishop);
			KingsBishop  = new Piece(Piece.EnmName.Bishop, this, 5,7, Piece.EnmId.BlackKingsBishop ); MColPieces.Add(KingsBishop ); MColMaterial.Add(KingsBishop );
			
			QueensKnight = new Piece(Piece.EnmName.Knight, this, 1,7, Piece.EnmId.BlackQueensKnight ); MColPieces.Add(QueensKnight); MColMaterial.Add(QueensKnight);
			KingsKnight  = new Piece(Piece.EnmName.Knight, this, 6,7, Piece.EnmId.BlackKingsKnight ); MColPieces.Add(KingsKnight ); MColMaterial.Add(KingsKnight );
			
			piece = new Piece(Piece.EnmName.Pawn, this, 0,6, Piece.EnmId.BlackPawn1); MColPieces.Add(piece); MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 1,6, Piece.EnmId.BlackPawn2); MColPieces.Add(piece); MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 2,6, Piece.EnmId.BlackPawn3); MColPieces.Add(piece); MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 5,6, Piece.EnmId.BlackPawn6); MColPieces.Add(piece); MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 6,6, Piece.EnmId.BlackPawn7); MColPieces.Add(piece); MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 7,6, Piece.EnmId.BlackPawn8); MColPieces.Add(piece); MColPawns.Add(piece); 

			QueensPawn = new Piece(Piece.EnmName.Pawn, this, 3,6, Piece.EnmId.BlackPawn4); MColPieces.Add(QueensPawn); MColPawns.Add(QueensPawn); 
			KingsPawn  = new Piece(Piece.EnmName.Pawn, this, 4,6, Piece.EnmId.BlackPawn5); MColPieces.Add(KingsPawn ); MColPawns.Add(KingsPawn ); 
		}

		public override int PawnForwardOffset => -16;

	    public override int PawnAttackRightOffset => -15;

	    public override int PawnAttackLeftOffset => -17;

	    public override PlayerClock Clock => MPlayerClock;
	}
}

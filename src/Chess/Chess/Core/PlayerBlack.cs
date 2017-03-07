namespace Chess.Core
{
	public class PlayerBlack: Player
	{
		public PlayerBlack()
		{
			Piece piece;

			MPlayerClock = new PlayerClock(this);

			this.Colour = Player.EnmColour.Black;
			this.Intellegence = Player.EnmIntellegence.Computer;

			this.MColPieces.Add( this.King = (new Piece(Piece.EnmName.King, this, 4,7, Piece.EnmId.BlackKing )) );

			this.Queen = (new Piece(Piece.EnmName.Queen, this, 3,7, Piece.EnmId.BlackQueen )); this.MColPieces.Add(this.Queen); this.MColMaterial.Add(this.Queen);

			this.QueensRook = (new Piece(Piece.EnmName.Rook, this, 0,7, Piece.EnmId.BlackQueensRook )); this.MColPieces.Add(this.QueensRook); this.MColMaterial.Add(this.QueensRook);
			this.KingsRook  = (new Piece(Piece.EnmName.Rook, this, 7,7, Piece.EnmId.BlackKingsRook )); this.MColPieces.Add(this.KingsRook ); this.MColMaterial.Add(this.KingsRook );
			
			this.QueensBishop = (new Piece(Piece.EnmName.Bishop, this, 2,7, Piece.EnmId.BlackQueensBishop)); this.MColPieces.Add(this.QueensBishop); this.MColMaterial.Add(this.QueensBishop);
			this.KingsBishop  = (new Piece(Piece.EnmName.Bishop, this, 5,7, Piece.EnmId.BlackKingsBishop )); this.MColPieces.Add(this.KingsBishop ); this.MColMaterial.Add(this.KingsBishop );
			
			this.QueensKnight = (new Piece(Piece.EnmName.Knight, this, 1,7, Piece.EnmId.BlackQueensKnight )); this.MColPieces.Add(this.QueensKnight); this.MColMaterial.Add(this.QueensKnight);
			this.KingsKnight  = (new Piece(Piece.EnmName.Knight, this, 6,7, Piece.EnmId.BlackKingsKnight )); this.MColPieces.Add(this.KingsKnight ); this.MColMaterial.Add(this.KingsKnight );
			
			piece = new Piece(Piece.EnmName.Pawn, this, 0,6, Piece.EnmId.BlackPawn1); this.MColPieces.Add(piece); this.MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 1,6, Piece.EnmId.BlackPawn2); this.MColPieces.Add(piece); this.MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 2,6, Piece.EnmId.BlackPawn3); this.MColPieces.Add(piece); this.MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 5,6, Piece.EnmId.BlackPawn6); this.MColPieces.Add(piece); this.MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 6,6, Piece.EnmId.BlackPawn7); this.MColPieces.Add(piece); this.MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 7,6, Piece.EnmId.BlackPawn8); this.MColPieces.Add(piece); this.MColPawns.Add(piece); 

			this.QueensPawn = new Piece(Piece.EnmName.Pawn, this, 3,6, Piece.EnmId.BlackPawn4); this.MColPieces.Add(this.QueensPawn); this.MColPawns.Add(this.QueensPawn); 
			this.KingsPawn  = new Piece(Piece.EnmName.Pawn, this, 4,6, Piece.EnmId.BlackPawn5); this.MColPieces.Add(this.KingsPawn ); this.MColPawns.Add(this.KingsPawn ); 
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
				return MPlayerClock;
			}
		}

	}
}

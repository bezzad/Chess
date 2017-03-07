namespace Chess.Core
{
	public class PlayerWhite: Player
	{
		public PlayerWhite()
		{
			Piece piece;

			MPlayerClock = new PlayerClock(this);

			this.Colour = Player.EnmColour.White;
			this.Intellegence = Player.EnmIntellegence.Human;

			this.MColPieces.Add( this.King = (new Piece(Piece.EnmName.King, this, 4,0, Piece.EnmId.WhiteKing)) );

			this.Queen = (new Piece(Piece.EnmName.Queen, this, 3,0, Piece.EnmId.WhiteQueen )); this.MColPieces.Add(this.Queen); this.MColMaterial.Add(this.Queen);

			this.QueensRook = (new Piece(Piece.EnmName.Rook, this, 0,0, Piece.EnmId.WhiteQueensRook )); this.MColPieces.Add(this.QueensRook); this.MColMaterial.Add(this.QueensRook);
			this.KingsRook  = (new Piece(Piece.EnmName.Rook, this, 7,0, Piece.EnmId.WhiteKingsRook )); this.MColPieces.Add(this.KingsRook ); this.MColMaterial.Add(this.KingsRook );
			
			this.QueensBishop = (new Piece(Piece.EnmName.Bishop, this, 2,0, Piece.EnmId.WhiteQueensBishop )); this.MColPieces.Add(this.QueensBishop); this.MColMaterial.Add(this.QueensBishop);
			this.KingsBishop  = (new Piece(Piece.EnmName.Bishop, this, 5,0, Piece.EnmId.WhiteKingsBishop )); this.MColPieces.Add(this.KingsBishop ); this.MColMaterial.Add(this.KingsBishop );
			
			this.QueensKnight = (new Piece(Piece.EnmName.Knight, this, 1,0, Piece.EnmId.WhiteQueensKnight )); this.MColPieces.Add(this.QueensKnight); this.MColMaterial.Add(this.QueensKnight);
			this.KingsKnight  = (new Piece(Piece.EnmName.Knight, this, 6,0, Piece.EnmId.WhiteKingsKnight )); this.MColPieces.Add(this.KingsKnight ); this.MColMaterial.Add(this.KingsKnight );
			
			piece = new Piece(Piece.EnmName.Pawn, this, 0,1, Piece.EnmId.WhitePawn1); this.MColPieces.Add(piece); this.MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 1,1, Piece.EnmId.WhitePawn2); this.MColPieces.Add(piece); this.MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 2,1, Piece.EnmId.WhitePawn3); this.MColPieces.Add(piece); this.MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 5,1, Piece.EnmId.WhitePawn6); this.MColPieces.Add(piece); this.MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 6,1, Piece.EnmId.WhitePawn7); this.MColPieces.Add(piece); this.MColPawns.Add(piece); 
			piece = new Piece(Piece.EnmName.Pawn, this, 7,1, Piece.EnmId.WhitePawn8); this.MColPieces.Add(piece); this.MColPawns.Add(piece); 

			this.QueensPawn = new Piece(Piece.EnmName.Pawn, this, 3,1, Piece.EnmId.WhitePawn4); this.MColPieces.Add(this.QueensPawn); this.MColPawns.Add(this.QueensPawn); 
			this.KingsPawn  = new Piece(Piece.EnmName.Pawn, this, 4,1, Piece.EnmId.WhitePawn5); this.MColPieces.Add(this.KingsPawn ); this.MColPawns.Add(this.KingsPawn ); 
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
				return MPlayerClock;
			}
		}

	}
}

using System.Collections.Generic;

public class NullPiece : Piece {

    private static readonly List<Move> EmptyList = new List<Move>();
    private static NullPiece _instance;

    private NullPiece() : base(Color.NONE) {
        
    }

    public static NullPiece Instance() {
        return _instance ?? new NullPiece();
    }
    
    public override List<Move> GetPseudoLegalMoves(Board board, Square from) {
        return EmptyList;
    }
    
}
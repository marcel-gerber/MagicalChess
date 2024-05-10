using System.Collections.Generic;

public class NullPiece : Piece {

    private static readonly List<Move> EmptyList = new List<Move>();
    
    public override List<Move> GetPseudoLegalMoves() {
        return EmptyList;
    }
    
}
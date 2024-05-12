using System.Collections.Generic;

public class Pawn : Piece {

    public Pawn(Color color) : base(color) {
        
    }

    // TODO
    public override List<Move> GetPseudoLegalMoves(Board board, Square from) {
        List<Move> pseudoLegalMoves = new List<Move>();
        return pseudoLegalMoves;
    }
}
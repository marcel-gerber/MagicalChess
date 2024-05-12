using System.Collections.Generic;

public class Bishop : Piece {

    public Bishop(Color color) : base(color) {
        
    }

    public override List<Move> GetPseudoLegalMoves(Board board, Square from) {
        return base.GetPseudoBishopMoves(board, from);
    }
}
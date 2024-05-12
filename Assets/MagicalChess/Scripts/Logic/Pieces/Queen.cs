using System.Collections.Generic;

public class Queen : Piece {

    public Queen(Color color) : base(color) {
        
    }

    public override List<Move> GetPseudoLegalMoves(Board board, Square from) {
        List<Move> pseudoLegalMoves = base.GetPseudoBishopMoves(board, from);
        pseudoLegalMoves.AddRange(base.GetPseudoRookMoves(board, from));

        return pseudoLegalMoves;
    }
}
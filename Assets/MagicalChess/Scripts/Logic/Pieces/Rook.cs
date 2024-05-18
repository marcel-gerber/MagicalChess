using System.Collections.Generic;

public class Rook : Piece {

    public Rook(Color color) : base(color) {
        
    }

    public override List<Move> GetPseudoLegalMoves(Board board, Square from) {
        return base.GetPseudoRookMoves(board, from);
    }
    
    public override char GetChar() {
        return GetColor() == Color.WHITE ? 'R' : 'r';
    }
}
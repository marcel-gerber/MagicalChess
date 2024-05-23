using System.Collections.Generic;

public class Rook : Piece {

    public Rook(Color color) : base(color) {
        
    }

    public override List<Move> GetPseudoLegalMoves(Board board, Square from) {
        List<Move> pseudoLegalMoves = new List<Move>();

        foreach (Square to in base.GetRookAttacks(board, from, board.IsKing)) {
            pseudoLegalMoves.Add(new Move(from, to));
        }

        return pseudoLegalMoves;
    }
    
    public override List<Square> GetAttackedSquares(Board board, Square from) {
        return base.GetRookAttacks(board, from, square => false);
    }
    
    public override char GetChar() {
        return GetColor() == Color.WHITE ? 'R' : 'r';
    }

    public override PieceType GetPieceType() {
        return PieceType.ROOK;
    }
}
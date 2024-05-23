using System.Collections.Generic;

public class Queen : Piece {

    public Queen(Color color) : base(color) {
        
    }

    public override List<Move> GetPseudoLegalMoves(Board board, Square from) {
        List<Move> pseudoLegalMoves = new List<Move>();

        foreach (Square to in base.GetRookAttacks(board, from, board.IsKing)) {
            pseudoLegalMoves.Add(new Move(from, to));
        }
        
        foreach (Square to in base.GetBishopAttacks(board, from, board.IsKing)) {
            pseudoLegalMoves.Add(new Move(from, to));
        }

        return pseudoLegalMoves;
    }
    
    public override List<Square> GetAttackedSquares(Board board, Square from) {
        List<Square> attackedSquares = base.GetRookAttacks(board, from, square => false);
        attackedSquares.AddRange(base.GetBishopAttacks(board, from, square => false));
        return attackedSquares;
    }
    
    public override char GetChar() {
        return GetColor() == Color.WHITE ? 'Q' : 'q';
    }

    public override PieceType GetPieceType() {
        return PieceType.QUEEN;
    }
}
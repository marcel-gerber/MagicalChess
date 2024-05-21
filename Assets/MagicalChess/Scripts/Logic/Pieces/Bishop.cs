using System;
using System.Collections.Generic;

public class Bishop : Piece {

    public Bishop(Color color) : base(color) {
        
    }

    public override List<Move> GetPseudoLegalMoves(Board board, Square from) {
        List<Move> pseudoLegalMoves = new List<Move>();

        foreach (Square to in base.GetBishopAttacks(board, from, board.IsKing)) {
            pseudoLegalMoves.Add(new Move(from, to));
        }

        return pseudoLegalMoves;
    }

    public override List<Square> GetAttackedSquares(Board board, Square from) {
        return base.GetBishopAttacks(board, from, square => false);
    }

    public override char GetChar() {
        return GetColor() == Color.WHITE ? 'B' : 'b';
    }
}
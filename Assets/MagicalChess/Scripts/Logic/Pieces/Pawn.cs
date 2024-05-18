using System.Collections.Generic;

public class Pawn : Piece {

    private static readonly Direction[] WhiteAttacks = { Direction.NORTH_EAST, Direction.NORTH_WEST };
    private static readonly Direction[] BlackAttacks = { Direction.SOUTH_EAST, Direction.SOUTH_WEST };

    public Pawn(Color color) : base(color) {
        
    }

    public override List<Move> GetPseudoLegalMoves(Board board, Square from) {
        List<Move> pseudoLegalMoves = new List<Move>();
        Color color = GetColor();

        Direction push = color == Color.WHITE ? Direction.NORTH : Direction.SOUTH;
        Square to = from + push;

        if (board.IsEmpty(to)) {
            if (IsOnPromotionRank(color, to)) {
                GetPromotionMoves(ref pseudoLegalMoves, from, to);
            }
            else {
                pseudoLegalMoves.Add(new Move(from, to));
            }
        }

        if (IsOnDoublePushRank(color, from)) {
            to = from + push + push;
            pseudoLegalMoves.Add(new Move(from, to));
        }

        foreach (Direction attack in GetAttackDirections(color)) {
            to = from + attack;

            if (to.GetIndex() == board.GetEnPassantSquare().GetIndex()) {
                pseudoLegalMoves.Add(new Move(MoveType.ENPASSANT, from, to));
                continue;
            }

            if (board.IsOpponent(to, board.GetPiece(to))) {
                if (IsOnPromotionRank(color, to)) {
                    GetPromotionMoves(ref pseudoLegalMoves, from, to);
                }
                else {
                    pseudoLegalMoves.Add(new Move(from, to));
                }
            }
        }
        
        return pseudoLegalMoves;
    }

    public override char GetChar() {
        return GetColor() == Color.WHITE ? 'P' : 'p';
    }

    private static void GetPromotionMoves(ref List<Move> moves, Square from, Square to) {
        moves.Add(new Move(MoveType.PROMOTION, from, to, PromotionType.KNIGHT));
        moves.Add(new Move(MoveType.PROMOTION, from, to, PromotionType.BISHOP));
        moves.Add(new Move(MoveType.PROMOTION, from, to, PromotionType.ROOK));
        moves.Add(new Move(MoveType.PROMOTION, from, to, PromotionType.QUEEN));
    }

    private static bool IsOnDoublePushRank(Color color, Square from) {
        switch (color) {
            case Color.WHITE:
                return from.GetRankIndex() == 2;
            case Color.BLACK:
                return from.GetRankIndex() == 7;
            default:
                return false;
        }
    }
    
    private static bool IsOnPromotionRank(Color color, Square to) {
        switch (color) {
            case Color.WHITE:
                return to.GetRankIndex() == 8;
            case Color.BLACK:
                return to.GetRankIndex() == 1;
            default:
                return false;
        }
    }

    
    private static Direction[] GetAttackDirections(Color color) {
        switch (color) {
            case Color.WHITE:
                return WhiteAttacks;
            case Color.BLACK:
                return BlackAttacks;
            default:
                return new [] { Direction.NONE };
        }
    }
}
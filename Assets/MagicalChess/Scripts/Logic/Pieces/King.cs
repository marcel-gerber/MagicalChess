using System.Collections.Generic;

public class King : Piece {

    private static readonly Direction[] LegalDirections = { Direction.NORTH, Direction.NORTH_EAST, Direction.EAST, 
        Direction.SOUTH_EAST, Direction.SOUTH, Direction.SOUTH_WEST, Direction.WEST, Direction.NORTH_WEST 
    };

    public King(Color color) : base(color) {
        
    }

    public override List<Move> GetPseudoLegalMoves(Board board, Square from) {
        List<Move> pseudoLegalMoves = new List<Move>();

        foreach (Direction direction in LegalDirections) {
            Square to = from + direction;

            if (board.IsEmptyOrOpponent(to, this)) {
                pseudoLegalMoves.Add(new Move(from, to));
            }
        }

        CastlingValue[] castlings = board.GetCastling().GetCastlings(this.GetColor());

        foreach (CastlingValue castlingValue in castlings) {
            if (board.GetCastling().Has(castlingValue)) {
                byte[] emptySquares = board.GetCastling().GetEmptySquares(castlingValue);
                
                if(!board.AreEmpty(emptySquares)) continue;
                
                byte targetKindIndex = board.GetCastling().GetKingIndex(castlingValue);
                Square targetSquare = new Square(targetKindIndex);
                
                pseudoLegalMoves.Add(new Move(MoveType.CASTLING, from, targetSquare));
            }
        }
        
        return pseudoLegalMoves;
    }
}
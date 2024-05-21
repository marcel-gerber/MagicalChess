using System.Collections.Generic;

public class Knight : Piece {

    private static readonly Direction[] LegalDirections = { Direction.KNIGHT_NORTH_NORTH_WEST, 
        Direction.KNIGHT_NORTH_NORTH_EAST, Direction.KNIGHT_NORTH_EAST_EAST, Direction.KNIGHT_SOUTH_EAST_EAST,
        Direction.KNIGHT_SOUTH_SOUTH_EAST, Direction.KNIGHT_SOUTH_SOUTH_WEST, Direction.KNIGHT_SOUTH_WEST_WEST,
        Direction.KNIGHT_NORTH_WEST_WEST
    };

    public Knight(Color color) : base(color) {
        
    }
    
    public override List<Move> GetPseudoLegalMoves(Board board, Square from) {
        List<Move> pseudoLegalMoves = new List<Move>();
        
        foreach (Direction direction in LegalDirections) {
            Square to = from + direction;
            
            if(to.GetValue() == SquareValue.NONE) continue;

            if (board.IsEmptyOrOpponent(to, this)) {
                pseudoLegalMoves.Add(new Move(from, to));
            }
        }
        return pseudoLegalMoves;
    }
    
    public override char GetChar() {
        return GetColor() == Color.WHITE ? 'N' : 'n';
    }
}
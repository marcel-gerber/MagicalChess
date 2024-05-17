using System.Collections.Generic;

public abstract class Piece {
    
    private readonly Color _color;

    protected Piece(Color color) {
        _color = color;
    }
    
    public abstract List<Move> GetPseudoLegalMoves(Board board, Square from);

    private void AddDirectionMoves(ref List<Move> moves, Direction direction, Board board, Square from) {
        Square to = from + direction;
        
        while (to.GetValue() != SquareValue.NONE) {
            if (board.IsFriendly(to, this)) return;
            
            moves.Add(new Move(from, to));
            
            if (board.IsOpponent(to, this)) return;
            to += direction;
        }
    }

    protected List<Move> GetPseudoRookMoves(Board board, Square from) {
        List<Move> pseudoLegalMoves = new List<Move>();

        AddDirectionMoves(ref pseudoLegalMoves, Direction.NORTH, board, from);
        AddDirectionMoves(ref pseudoLegalMoves, Direction.EAST, board, from);
        AddDirectionMoves(ref pseudoLegalMoves, Direction.SOUTH, board, from);
        AddDirectionMoves(ref pseudoLegalMoves, Direction.WEST, board, from);

        return pseudoLegalMoves;
    }
    
    protected List<Move> GetPseudoBishopMoves(Board board, Square from) {
        List<Move> pseudoLegalMoves = new List<Move>();

        AddDirectionMoves(ref pseudoLegalMoves, Direction.NORTH_EAST, board, from);
        AddDirectionMoves(ref pseudoLegalMoves, Direction.SOUTH_EAST, board, from);
        AddDirectionMoves(ref pseudoLegalMoves, Direction.SOUTH_WEST, board, from);
        AddDirectionMoves(ref pseudoLegalMoves, Direction.NORTH_WEST, board, from);

        return pseudoLegalMoves;
    }

    public Color GetColor() {
        return _color;
    }
    
    public static Piece FromChar(char c) {
        switch(c) {
            case 'P':
                return new Pawn(Color.WHITE);
            case 'N':
                return new Knight(Color.WHITE);
            case 'B':
                return new Bishop(Color.WHITE);
            case 'R':
                return new Rook(Color.WHITE);
            case 'Q':
                return new Queen(Color.WHITE);
            case 'K':
                return new King(Color.WHITE);
            case 'p':
                return new Pawn(Color.BLACK);
            case 'n':
                return new Knight(Color.BLACK);
            case 'b':
                return new Bishop(Color.BLACK);
            case 'r':
                return new Rook(Color.BLACK);
            case 'q':
                return new Queen(Color.BLACK);
            case 'k':
                return new King(Color.BLACK);
            default:
                return NullPiece.Instance();
        }
    }

}
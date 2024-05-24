using System;
using System.Collections.Generic;
using Chess;

public enum PieceType : byte {
    PAWN,
    KNIGHT,
    BISHOP,
    ROOK,
    QUEEN,
    KING,
    NONE
}

public static class PieceTypeExtension {

    public static Piece GetPiece(this PieceType pieceType, Color color) {
        switch (pieceType) {
            case PieceType.KNIGHT:
                return new Knight(color);
            case PieceType.BISHOP:
                return new Bishop(color);
            case PieceType.ROOK:
                return new Rook(color);
            case PieceType.QUEEN:
                return new Queen(color);
            default:
                return NullPiece.Instance();
        }
    }
}

public abstract class Piece {
    
    private readonly Color _color;

    protected Piece(Color color) {
        _color = color;
    }
    
    public abstract List<Move> GetPseudoLegalMoves(Board board, Square from);

    public abstract List<Square> GetAttackedSquares(Board board, Square from);

    public abstract char GetChar();

    public abstract PieceType GetPieceType();

    private void GetAttackRay(ref List<Square> squares, Direction direction, Board board, Square from, Func<Square, bool> kingCheck) {
        Square to = from + direction;
        
        while (to.GetValue() != SquareValue.NONE) {
            if (board.IsFriendly(to, this) || kingCheck(to)) return;
            
            squares.Add(to);
            
            if (board.IsOpponent(to, this)) return;
            to += direction;
        }
    }

    protected List<Square> GetRookAttacks(Board board, Square from, Func<Square, bool> kingCheck) {
        List<Square> attacks = new List<Square>();

        GetAttackRay(ref attacks, Direction.NORTH, board, from, kingCheck);
        GetAttackRay(ref attacks, Direction.EAST, board, from, kingCheck);
        GetAttackRay(ref attacks, Direction.SOUTH, board, from, kingCheck);
        GetAttackRay(ref attacks, Direction.WEST, board, from, kingCheck);

        return attacks;
    }
    
    protected List<Square> GetBishopAttacks(Board board, Square from, Func<Square, bool> kingCheck) {
        List<Square> attacks = new List<Square>();

        GetAttackRay(ref attacks, Direction.NORTH_EAST, board, from, kingCheck);
        GetAttackRay(ref attacks, Direction.SOUTH_EAST, board, from, kingCheck);
        GetAttackRay(ref attacks, Direction.SOUTH_WEST, board, from, kingCheck);
        GetAttackRay(ref attacks, Direction.NORTH_WEST, board, from, kingCheck);

        return attacks;
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
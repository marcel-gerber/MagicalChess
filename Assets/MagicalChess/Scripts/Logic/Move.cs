using Chess;

public enum MoveType : byte {
    NORMAL,
    PROMOTION,
    ENPASSANT,
    CASTLING
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

public class Move {

    private readonly MoveType _moveType;
    private readonly Square _from;
    private readonly Square _to;
    private readonly PieceType _pieceType;

    public Move(Square from, Square to) {
        this._moveType = MoveType.NORMAL;
        this._from = from;
        this._to = to;
    }
    
    public Move(MoveType moveType, Square from, Square to, PieceType pieceType = PieceType.KNIGHT) {
        this._moveType = moveType;
        this._from = from;
        this._to = to;
        this._pieceType = pieceType;
    }

    public Square GetFrom() {
        return _from;
    }

    public Square GetTo() {
        return _to;
    }

    public MoveType GetMoveType() {
        return _moveType;
    }

    public PieceType GetPromotionType() {
        return _pieceType;
    }
}
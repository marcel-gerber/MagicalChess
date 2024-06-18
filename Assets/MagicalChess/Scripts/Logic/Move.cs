public enum MoveType : byte {
    NORMAL,
    CAPTURE,
    PROMOTION,
    ENPASSANT,
    CASTLING
}

public class Move {

    private readonly MoveType _moveType;
    private readonly Square _from;
    private readonly Square _to;
    private readonly PieceType _promotion;

    public Move(Square from, Square to) {
        _moveType = MoveType.NORMAL;
        _from = from;
        _to = to;
        _promotion = PieceType.NONE;
    }
    
    public Move(MoveType moveType, Square from, Square to, PieceType promotion = PieceType.KNIGHT) {
        _moveType = moveType;
        _from = from;
        _to = to;
        _promotion = promotion;
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
        return _promotion;
    }
}
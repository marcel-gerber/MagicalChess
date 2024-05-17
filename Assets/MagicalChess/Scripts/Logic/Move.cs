public enum MoveType : byte {
    NORMAL,
    PROMOTION,
    ENPASSANT,
    CASTLING
}

public enum PromotionType : byte {
    KNIGHT,
    BISHOP,
    ROOK,
    QUEEN
}

public class Move {

    private readonly MoveType _moveType;
    private readonly Square _from;
    private readonly Square _to;
    private readonly PromotionType _promotionType;

    public Move(Square from, Square to) {
        this._moveType = MoveType.NORMAL;
        this._from = from;
        this._to = to;
    }
    
    public Move(MoveType moveType, Square from, Square to, PromotionType promotionType = PromotionType.KNIGHT) {
        this._moveType = moveType;
        this._from = from;
        this._to = to;
        this._promotionType = promotionType;
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
}
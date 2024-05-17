public enum CastlingValue : byte {
    NO_CASTLING = 0,
    WHITE_00 = 0b00000001,
    WHITE_000 = 0b00000010,
    BLACK_00 = 0b00000100,
    BLACK_000 = 0b00001000
}

public class Castling {

    private byte _castlingRights;

    private static readonly CastlingValue[] BlackCastlings = { CastlingValue.BLACK_00, CastlingValue.BLACK_000 };
    private static readonly CastlingValue[] WhiteCastlings = { CastlingValue.WHITE_00, CastlingValue.WHITE_000 };

    private static readonly byte[] Black00EmptySquares = { 62, 61 };
    private static readonly byte[] Black000EmptySquares = { 57, 58, 59 };
    
    private static readonly byte[] White00EmptySquares = { 5, 6 };
    private static readonly byte[] White000EmptySquares = { 1, 2, 3 };

    public Castling() {
        _castlingRights = (byte) CastlingValue.NO_CASTLING;
    }

    public void Set(CastlingValue value) {
        _castlingRights |= (byte) value;
    }
    
    public void UnSet(CastlingValue value) {
        _castlingRights &= (byte) ~((byte) value);
    }

    public bool Has(CastlingValue value) {
        return (_castlingRights & (byte) value) == (byte) value;
    }

    public CastlingValue[] GetCastlings(Color color) {
        switch (color) {
            case Color.BLACK:
                return BlackCastlings;
            case Color.WHITE:
                return WhiteCastlings;
            default:
                return null;
        }
    }

    public byte GetKingIndex(CastlingValue castlingValue) {
        switch (castlingValue) {
            case CastlingValue.BLACK_00:
                return 62;
            case CastlingValue.BLACK_000:
                return 58;
            case CastlingValue.WHITE_00:
                return 6;
            case CastlingValue.WHITE_000:
                return 2;
            default:
                return 255;
        }
    }

    // Diese Felder müssen leer sein, um rochieren zu dürfen
    public byte[] GetEmptySquares(CastlingValue castlingValue) {
        switch (castlingValue) {
            case CastlingValue.BLACK_00:
                return Black00EmptySquares;
            case CastlingValue.BLACK_000:
                return Black000EmptySquares;
            case CastlingValue.WHITE_00:
                return White00EmptySquares;
            case CastlingValue.WHITE_000:
                return White000EmptySquares;
            default:
                return null;
        }
    }

}
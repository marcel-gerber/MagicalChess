public enum CastlingValue : byte {
    NO_CASTLING = 0,
    WHITE_00 = 0b00000001,
    WHITE_000 = 0b00000010,
    BLACK_00 = 0b00000100,
    BLACK_000 = 0b00001000
}

public class Castling {

    private byte _castlingRights;

    public Castling() {
        _castlingRights = (byte) CastlingValue.NO_CASTLING;
    }

    public void Set(CastlingValue value) {
        _castlingRights |= (byte) value;
    }
    
    public void UnSet(CastlingValue value) {
        _castlingRights &= (byte) ~((byte) value);
    }

}
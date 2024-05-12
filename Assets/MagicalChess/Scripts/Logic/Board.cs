public class Board {

    private Piece[] _pieces;
    private Castling _castling;

    public Board() {
        _pieces = new Piece[64];
    }

    public Piece GetPiece(byte index) {
        return _pieces[index];
    }

    public Piece GetPiece(Square square) {
        return GetPiece(square.GetIndex());
    }

    public bool IsEmpty(Square square) {
        return GetPiece(square) is NullPiece;
    }

    public bool IsFriendly(Square square, Piece piece) {
        return GetPiece(square).GetColor() == piece.GetColor();
    }

    public bool IsOpponent(Square square, Piece piece) {
        Piece targetPiece = GetPiece(square);
        return (!(targetPiece is NullPiece) && targetPiece.GetColor() != piece.GetColor());
    }

    public bool IsEmptyOrOpponent(Square square, Piece piece) {
        Piece targetPiece = GetPiece(square);
        return targetPiece is NullPiece || targetPiece.GetColor() != piece.GetColor();
    }

}
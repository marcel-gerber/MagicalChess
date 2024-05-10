public class Board {

    private Piece[] _pieces;

    public Board() {
        _pieces = new Piece[64];
    }

    public Piece GetPiece(byte index) {
        return _pieces[index];
    }

    public Piece GetPiece(Square square) {
        return GetPiece(square.GetIndex());
    }

    public bool IsEmptyOrOpponent(Square target, Piece piece) {
        Piece targetPiece = GetPiece(target);
        return targetPiece is NullPiece || targetPiece.GetColor() != piece.GetColor();
    }

}
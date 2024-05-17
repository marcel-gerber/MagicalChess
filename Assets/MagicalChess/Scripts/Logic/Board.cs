public class Board {

    private Piece[] _pieces;
    private Castling _castling;

    public Board() {
        _pieces = new Piece[64];
        _castling = new Castling();
    }

    public Piece GetPiece(byte index) {
        return _pieces[index];
    }

    public Piece GetPiece(Square square) {
        return GetPiece(square.GetIndex());
    }

    public Castling GetCastling() {
        return _castling;
    }

    public void PlacePiece(byte index, Piece piece) {
        _pieces[index] = piece;
    }
    
    public void RemovePiece(byte index) {
        _pieces[index] = NullPiece.Instance();
    }

    public bool IsEmpty(Square square) {
        return GetPiece(square) is NullPiece;
    }

    public bool IsEmpty(byte index) {
        return GetPiece(index) is NullPiece;
    }

    public bool AreEmpty(byte[] indexes) {
        foreach (byte index in indexes) {
            if (!IsEmpty(index)) return false;
        }

        return true;
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

    private void Init() {
        // Weiße Figuren
        PlacePiece(0, new Rook(Color.WHITE));
        PlacePiece(1, new Knight(Color.WHITE));
        PlacePiece(2, new Bishop(Color.WHITE));
        PlacePiece(3, new Queen(Color.WHITE));
        PlacePiece(4, new King(Color.WHITE));
        PlacePiece(5, new Bishop(Color.WHITE));
        PlacePiece(6, new Knight(Color.WHITE));
        PlacePiece(7, new Rook(Color.WHITE));

        for (byte b = 8; b < 15; b++) {
            PlacePiece(b, new Pawn(Color.WHITE));
        }
        
        // Schwarze Figuren
        PlacePiece(56, new Rook(Color.BLACK));
        PlacePiece(57, new Knight(Color.BLACK));
        PlacePiece(58, new Bishop(Color.BLACK));
        PlacePiece(59, new Queen(Color.BLACK));
        PlacePiece(60, new King(Color.BLACK));
        PlacePiece(61, new Bishop(Color.BLACK));
        PlacePiece(62, new Knight(Color.BLACK));
        PlacePiece(63, new Rook(Color.BLACK));
        
        for (byte b = 48; b < 55; b++) {
            PlacePiece(b, new Pawn(Color.BLACK));
        }
    }

}
using System;

public class Board {

    private Piece[] _pieces;
    private Castling _castling;
    private Color _sideToMove;
    private Square _enPassantSquare;

    public Board() {
        _pieces = new Piece[64];
        _castling = new Castling();
        _sideToMove = Color.WHITE;
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

    public Color GetSideToMove() {
        return _sideToMove;
    }

    public Square GetEnPassantSquare() {
        return _enPassantSquare;
    }

    private void PlacePiece(byte index, Piece piece) {
        _pieces[index] = piece;
    }
    
    private void RemovePiece(byte index) {
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

    public void SetFen(String fen) {
        String[] split = fen.Split(' ');
        String pieces = split[0];
        String sideToMove = split.Length > 1 ? split[1] : "w";
        String castling = split.Length > 2 ? split[2] : "-";
        String enPassant = split.Length > 3 ? split[3] : "-";

        _sideToMove = sideToMove == "w" ? Color.WHITE : Color.BLACK;
        _enPassantSquare = new Square(enPassant);

        byte index = 56;
        foreach (char c in pieces) {
            Piece piece = Piece.FromChar(c);

            if (!(piece is NullPiece)) {
                PlacePiece(index, piece);
                index++;
                continue;
            }

            if (c == '/') {
                index -= 16;
                continue;
            }

            if (Char.IsDigit(c)) {
                index += (byte) (c - '0');
            }
        }
        
        if(castling == "-") return;

        foreach (char c in castling) {
            if (c == 'K') {
                _castling.Set(CastlingValue.WHITE_00);
                continue;
            }
            
            if (c == 'Q') {
                _castling.Set(CastlingValue.WHITE_000);
                continue;
            }
            
            if (c == 'k') {
                _castling.Set(CastlingValue.BLACK_00);
                continue;
            }
            
            if (c == 'q') {
                _castling.Set(CastlingValue.BLACK_000);
            }
        }
    }

    public void Init() {
        // Weiße Figuren
        PlacePiece(0, new Rook(Color.WHITE));
        PlacePiece(1, new Knight(Color.WHITE));
        PlacePiece(2, new Bishop(Color.WHITE));
        PlacePiece(3, new Queen(Color.WHITE));
        PlacePiece(4, new King(Color.WHITE));
        PlacePiece(5, new Bishop(Color.WHITE));
        PlacePiece(6, new Knight(Color.WHITE));
        PlacePiece(7, new Rook(Color.WHITE));

        for (byte b = 8; b <= 15; b++) {
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
        
        for (byte b = 48; b <= 55; b++) {
            PlacePiece(b, new Pawn(Color.BLACK));
        }
        
        _castling.Set(CastlingValue.WHITE_00);
        _castling.Set(CastlingValue.WHITE_000);
        _castling.Set(CastlingValue.BLACK_00);
        _castling.Set(CastlingValue.BLACK_000);
    }

}
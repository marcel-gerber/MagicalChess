using System.Collections.Generic;

public abstract class Piece {
    
    private Color _color;
    private Square _square;

    private Board _board;

    public abstract List<Move> GetPseudoLegalMoves();

    public Color GetColor() {
        return _color;
    }

    public Square GetSquare() {
        return _square;
    }

    public Board GetBoard() {
        return _board;
    }
}
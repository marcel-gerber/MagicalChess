using System.Collections.Generic;
using Chess;

/// <summary>
/// Stellt eine leere Figur dar. Repräsentiert damit ein leeres Feld und verhindert lästige null-checks. 
/// </summary>
public class NullPiece : Piece {

    private static readonly List<Move> EmptyMoveList = new List<Move>();
    private static readonly List<Square> EmptySquareList = new List<Square>();
    private static NullPiece _instance;

    private NullPiece() : base(Color.NONE) {
        
    }

    public static NullPiece Instance() {
        return _instance ?? new NullPiece();
    }
    
    public override List<Move> GetPseudoLegalMoves(Board board, Square from) {
        return EmptyMoveList;
    }

    public override List<Square> GetAttackedSquares(Board board, Square from) {
        return EmptySquareList;
    }

    public override PieceType GetPieceType() {
        return PieceType.NONE;
    }
}
using System.Collections.Generic;
using Chess;

/// <summary>
/// Repräsentiert einen König im Schachspiel.
/// </summary>
public class King : Piece {

    private static readonly Direction[] LegalDirections = { Direction.NORTH, Direction.NORTH_EAST, Direction.EAST, 
        Direction.SOUTH_EAST, Direction.SOUTH, Direction.SOUTH_WEST, Direction.WEST, Direction.NORTH_WEST 
    };

    public King(Color color) : base(color) {
        
    }

    public override List<Move> GetPseudoLegalMoves(Board board, Square from) {
        List<Move> pseudoLegalMoves = new List<Move>();

        foreach (Direction direction in LegalDirections) {
            Square to = from + direction;
            
            if(to.GetValue() == SquareValue.NONE) continue;

            if (board.IsEmptyOrOpponent(to, this)) {
                pseudoLegalMoves.Add(new Move(from, to));
            }
        }

        AddCastlingMoves(ref pseudoLegalMoves, board, from);
        return pseudoLegalMoves;
    }

    private void AddCastlingMoves(ref List<Move> pseudoLegalMoves, Board board, Square from) {
        if (board.GetCastling().HasNoCastling()) return;

        CastlingValue[] castlings = Castling.GetCastlings(this.GetColor());

        foreach (CastlingValue castlingValue in castlings) {
            if (board.GetCastling().Has(castlingValue)) {
                byte[] emptySquares = Castling.GetEmptySquares(castlingValue);
                byte[] notAttackedSquares = Castling.GetNotAttackedSquares(castlingValue);
                
                if(!board.AreEmpty(emptySquares)) continue;
                if(board.AreAttacked(notAttackedSquares)) continue;
                
                byte targetKindIndex = Castling.GetEndingKingIndex(castlingValue);
                Square targetSquare = new Square(targetKindIndex);
                
                pseudoLegalMoves.Add(new Move(MoveType.CASTLING, from, targetSquare));
            }
        }
    }

    public override List<Square> GetAttackedSquares(Board board, Square from) {
        List<Square> attackedSquares = new List<Square>();
        
        foreach (Direction direction in LegalDirections) {
            Square to = from + direction;
            
            if(to.GetValue() == SquareValue.NONE) continue;

            if (board.IsEmptyOrOpponent(to, this)) {
                attackedSquares.Add(to);
            }
        }
        return attackedSquares;
    }

    public override PieceType GetPieceType() {
        return PieceType.KING;
    }
}
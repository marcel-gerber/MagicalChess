using Chess;

/// <summary>
/// Die Rechte für die Rochade können in einem einzigen byte gespeichert werden. Dafür werden die ersten 4 bits
/// verwendet. Jeweils 2 (Schwarz und Weiß) für die kurze Rochade (_00) und lange Rochade (_000).
/// </summary>
public enum CastlingValue : byte {
    NO_CASTLING = 0,
    WHITE_00 = 0b00000001,
    WHITE_000 = 0b00000010,
    BLACK_00 = 0b00000100,
    BLACK_000 = 0b00001000
}

/// <summary>
/// Repräsentiert die Rechte für die Rochade. Agiert wie eine Bitmask (Set, Unset) und liefert weitere Hilfsfunktionen.
/// </summary>
public class Castling {

    // Hier werden die aktuellen Rechte für die Rochade gespeichert
    private byte _castlingRights;

    private static readonly CastlingValue[] BlackCastlings = { CastlingValue.BLACK_00, CastlingValue.BLACK_000 };
    private static readonly CastlingValue[] WhiteCastlings = { CastlingValue.WHITE_00, CastlingValue.WHITE_000 };

    // Diese Felder müssen leer sein für die Rochade
    private static readonly byte[] Black00EmptySquares = { 61, 62 };
    private static readonly byte[] Black000EmptySquares = { 57, 58, 59 };
    
    private static readonly byte[] White00EmptySquares = { 5, 6 };
    private static readonly byte[] White000EmptySquares = { 1, 2, 3 };
    
    // Diese Felder dürfen nicht vom Gegner attackiert werden
    private static readonly byte[] Black00NotAttacked = { 61, 62 };
    private static readonly byte[] Black000NotAttacked = { 58, 59 };
    
    private static readonly byte[] White00NotAttacked = { 5, 6 };
    private static readonly byte[] White000NotAttacked = { 2, 3 };

    public Castling() {
        _castlingRights = (byte) CastlingValue.NO_CASTLING;
    }

    // Copy constructor
    public Castling(Castling castling) {
        _castlingRights = castling.GetValue();
    }

    public byte GetValue() {
        return _castlingRights;
    }

    public void Set(CastlingValue value) {
        _castlingRights |= (byte) value;
    }
    
    public void UnSet(CastlingValue value) {
        _castlingRights &= (byte) ~((byte) value);
    }

    public void UnSet(Color color) {
        switch (color) {
            case Color.WHITE:
                UnSet(CastlingValue.WHITE_00);
                UnSet(CastlingValue.WHITE_000);
                return;
            case Color.BLACK:
                UnSet(CastlingValue.BLACK_00);
                UnSet(CastlingValue.BLACK_000);
                return;
            default:
                return;
        }
    }

    public bool Has(CastlingValue value) {
        return (_castlingRights & (byte) value) == (byte) value;
    }

    public bool Has(Color color) {
        foreach (CastlingValue castlingValue in GetCastlings(color)) {
            if (Has(castlingValue)) return true;
        }
        return false;
    }

    public bool HasNoCastling() {
        return _castlingRights == (byte) CastlingValue.NO_CASTLING;
    }

    public static CastlingValue[] GetCastlings(Color color) {
        switch (color) {
            case Color.BLACK:
                return BlackCastlings;
            case Color.WHITE:
                return WhiteCastlings;
            default:
                return null;
        }
    }
    
    /// <summary>
    /// Liefert den Index des Feldes, auf dem der König nach Spielen der Rochade landet.
    /// </summary>
    /// <param name="castlingValue">CastlingValue</param>
    /// <returns>byte</returns>
    public static byte GetEndingKingIndex(CastlingValue castlingValue) {
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
    
    /// <summary>
    /// Liefert den Index des Feldes, auf dem sich der Turm befinden muss, bevor die Rochade gespielt wird.
    /// </summary>
    /// <param name="castling">CastlingValue</param>
    /// <returns>byte</returns>
    public static byte GetStartingRookIndex(CastlingValue castling) {
        switch(castling) {
            case CastlingValue.WHITE_00:
                return 7;
            case CastlingValue.WHITE_000:
                return 0;
            case CastlingValue.BLACK_00:
                return 63;
            case CastlingValue.BLACK_000:
                return 56;
            default:
                return 255;
        }
    }
    
    /// <summary>
    /// Liefert den Index des Feldes, auf dem der Turm landet, nachdem die Rochade gespielt wurde.
    /// </summary>
    /// <param name="castling">CastlingValue</param>
    /// <returns>byte</returns>
    public static byte GetEndingRookIndex(CastlingValue castling) {
        switch(castling) {
            case CastlingValue.WHITE_00:
                return 5;
            case CastlingValue.WHITE_000:
                return 3;
            case CastlingValue.BLACK_00:
                return 61;
            case CastlingValue.BLACK_000:
                return 59;
            default:
                return 255;
        }
    }
    
    /// <summary>
    /// Liefert die Rochade auf Basis des Index des Feldes, auf dem der König landet, nachdem die Rochade gespielt wurde
    /// </summary>
    /// <param name="index">Index des Feldes, auf dem der König landet</param>
    /// <returns>CastlingValue</returns>
    public static CastlingValue GetFromKingIndex(byte index) {
        switch(index) {
            case 2:
                return CastlingValue.WHITE_000;
            case 6:
                return CastlingValue.WHITE_00;
            case 58:
                return CastlingValue.BLACK_000;
            case 62:
                return CastlingValue.BLACK_00;
            default:
                return CastlingValue.NO_CASTLING;
        }
    }

    /// <summary>
    /// Liefert die Rochade auf Basis des Index des Ausgangsfeldes des Turms (bevor die Rochade gespielt wurde).
    /// </summary>
    /// <param name="index">Index des Ausgangsfeldes des Turms</param>
    /// <returns>CastlingValue</returns>
    public static CastlingValue GetFromRookIndex(byte index) {
        switch(index) {
            case 0:
                return CastlingValue.WHITE_000;
            case 7:
                return CastlingValue.WHITE_00;
            case 56:
                return CastlingValue.BLACK_000;
            case 63:
                return CastlingValue.BLACK_00;
            default:
                return CastlingValue.NO_CASTLING;
        }
    }

    /// <summary>
    /// Liefert die Indizes der Felder, die leer sein müssen für die Rochade 'castlingValue'.
    /// </summary>
    /// <param name="castlingValue">CastlingValue</param>
    /// <returns>byte[]</returns>
    public static byte[] GetEmptySquares(CastlingValue castlingValue) {
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
    
    /// <summary>
    /// Liefert die Indizes der Felder, die nicht vom Gegner attackiert werden dürfen,
    /// basierend auf der Rochade 'castlingValue'.
    /// </summary>
    /// <param name="castlingValue">CastlingValue</param>
    /// <returns>byte[]</returns>
    public static byte[] GetNotAttackedSquares(CastlingValue castlingValue) {
        switch (castlingValue) {
            case CastlingValue.BLACK_00:
                return Black00NotAttacked;
            case CastlingValue.BLACK_000:
                return Black000NotAttacked;
            case CastlingValue.WHITE_00:
                return White00NotAttacked;
            case CastlingValue.WHITE_000:
                return White000NotAttacked;
            default:
                return null;
        }
    }


}
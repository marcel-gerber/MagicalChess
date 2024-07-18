using System;

public enum SquareValue : byte {
    A1, B1, C1, D1, E1, F1, G1, H1,
    A2, B2, C2, D2, E2, F2, G2, H2,
    A3, B3, C3, D3, E3, F3, G3, H3,
    A4, B4, C4, D4, E4, F4, G4, H4,
    A5, B5, C5, D5, E5, F5, G5, H5,
    A6, B6, C6, D6, E6, F6, G6, H6,
    A7, B7, C7, D7, E7, F7, G7, H7,
    A8, B8, C8, D8, E8, F8, G8, H8,
    NONE
};

/// <summary>
/// Repräsentiert ein einzelnes Feld auf dem Schachbrett.
/// </summary>
public class Square {
    
    private SquareValue _value;

    // Copy constructor
    public Square(Square square) {
        _value = square.GetValue();
    }
    
    public Square(SquareValue squareValue) {
        _value = squareValue;
    }

    public Square(byte index) {
        if (index > 63) {
            _value = SquareValue.NONE;
            return;
        }
        
        _value = (SquareValue) index;
    }

    /// <summary>
    /// Konstruktor zum Erstellen von 'Square'-Objekten aus Strings.
    /// </summary>
    /// <param name="s">String</param>
    /// <example>"d5"</example>
    public Square(String s) {
        if (s == "-") {
            _value = SquareValue.NONE;
            return;
        }

        byte index = (byte) ((s[0] - 'a') + ((s[1] - '1') * 8));
        _value = (SquareValue) index;
    }

    /// <summary>
    /// Operator, um 'square' einer Direction addieren zu können.
    /// </summary>
    /// <param name="square">Feld</param>
    /// <param name="direction">Richtung</param>
    /// <returns>Square</returns>
    /// <example>d5 + Direction.NORTH => d6</example>
    public static Square operator+(Square square, Direction direction) {
        byte targetIndex = (byte) (square.GetIndex() + direction.GetValue());
        Square targetSquare = new Square(targetIndex);

        if (Math.Abs(square.GetFileIndex() - targetSquare.GetFileIndex()) > 2) {
            return new Square(SquareValue.NONE);
        }

        return targetSquare;
    }
    
    public static int GetRankIndex(char c) {
        switch (c) {
            case '1':
                return 0;
            case '2':
                return 1;
            case '3':
                return 2;
            case '4':
                return 3;
            case '5':
                return 4;
            case '6':
                return 5;
            case '7':
                return 6;
            case '8':
                return 7;
            default:
                return -1;
        }
    }

    public static int GetFileIndex(char c) {
        switch (c) {
            case 'a':
                return 0;
            case 'b':
                return 1;
            case 'c':
                return 2;
            case 'd':
                return 3;
            case 'e':
                return 4;
            case 'f':
                return 5;
            case 'g':
                return 6;
            case 'h':
                return 7;
            default:
                return -1;
        }
    }

    public SquareValue GetValue() {
        return _value;
    }

    public void SetValue(SquareValue squareValue) {
        _value = squareValue;
    }

    public byte GetIndex() {
        return (byte) _value;
    }

    public int GetFileIndex() {
        return GetIndex() & 7;
    }

    public int GetRankIndex() {
        return GetIndex() >> 3;
    }
}
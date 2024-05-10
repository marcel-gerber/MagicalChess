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

public class Square {
    
    private SquareValue _value;
    
    // Valid chars of Squares in Chess Notation
    private static char[] validChars = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
    
    public Square(SquareValue squareValue) {
        this._value = squareValue;
    }

    public Square(byte index) {
        if (index > 63) {
            this._value = SquareValue.NONE;
            return;
        }
        
        this._value = (SquareValue) index;
    }

    public Square(String s) {
        if (s == "-") {
            this._value = SquareValue.NONE;
            return;
        }

        byte index = (byte) ((s[0] - 'a') + ((s[1] - '1') * 8));
        this._value = (SquareValue) index;
    }

    public static Square operator+(Square square, Direction direction) {
        byte targetIndex = (byte) (square.GetIndex() + direction.GetValue());
        Square targetSquare = new Square(targetIndex);

        if (Math.Abs(square.GetFileIndex() - targetSquare.GetFileIndex()) > 2) {
            return new Square(SquareValue.NONE);
        }

        return targetSquare;
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

    public char[] GetValidChars() {
        return validChars;
    }
}
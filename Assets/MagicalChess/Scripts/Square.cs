﻿using System;

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
    
    private SquareValue value;
    
    // Valid chars of Squares in Chess Notation
    private static char[] validChars = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
    
    public Square(SquareValue squareValue) {
        this.value = squareValue;
    }

    public Square(byte index) {
        this.value = (SquareValue) index;
    }

    public Square(String s) {
        if (s == "-") {
            this.value = SquareValue.NONE;
            return;
        }

        byte index = (byte) ((s[0] - 'a') + ((s[1] - '1') * 8));
        this.value = (SquareValue) index;
    }

    public byte GetIndex() {
        return (byte) value;
    }

    public char[] GetValidChars() {
        return validChars;
    }
}
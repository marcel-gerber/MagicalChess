﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StateInfo {

    private readonly Castling _castling;
    private readonly Square _enPassantSquare;
    private readonly Piece _captured;

    public StateInfo(Castling castling, Square enPassantSquare, Piece captured) {
        _castling = castling;
        _enPassantSquare = enPassantSquare;
        _captured = captured;
    }

    public Castling GetCastling() {
        return _castling;
    }

    public Square GetEnPassantSquare() {
        return _enPassantSquare;
    }

    public Piece GetCaptured() {
        return _captured;
    }
}

public class Board {

    private readonly Piece[] _pieces;
    private readonly Castling _castling;
    private Color _sideToMove;
    private Square _enPassantSquare;

    private readonly Stack<StateInfo> _prevStates;

    public Board() {
        _pieces = new Piece[64];
        _castling = new Castling();
        _sideToMove = Color.WHITE;
        _prevStates = new Stack<StateInfo>();
        
        Init();
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

    private bool IsEnPassantPossible(Square to, Piece piece) {
        Piece neighborEast = GetPiece(to + Direction.EAST);
        Piece neighborWest = GetPiece(to + Direction.WEST);

        return (neighborEast is Pawn && neighborEast.GetColor() != piece.GetColor()) || 
               (neighborWest is Pawn && neighborWest.GetColor() != piece.GetColor());
    }

    public void MakeMove(Move move) {
        Square from = move.GetFrom();
        Square to = move.GetTo();
        MoveType moveType = move.GetMoveType();

        Piece moved = GetPiece(from);
        Piece captured = GetPiece(to);

        StateInfo stateInfo = new StateInfo(_castling, _enPassantSquare, captured);
        _prevStates.Push(stateInfo);

        if (_enPassantSquare.GetValue() != SquareValue.NONE) {
            _enPassantSquare.SetValue(SquareValue.NONE);
        }

        if (!(captured is NullPiece)) {
            RemovePiece(to.GetIndex());

            if (captured is Rook) {
                CastlingValue castlingValue = Castling.GetRookFromIndex(to.GetIndex());
                _castling.UnSet(castlingValue);
            }
        }

        if (_castling.Has(_sideToMove)) {
            if (moved is King) {
                _castling.UnSet(_sideToMove);
            }
            else if(moved is Rook) {
                CastlingValue castlingValue = Castling.GetRookFromIndex(from.GetIndex());
                _castling.UnSet(castlingValue);
            }
        }

        if (moved is Pawn) {
            // Double push
            if (Math.Abs(from.GetIndex() - to.GetIndex()) == 16) {
                if (IsEnPassantPossible(to, moved)) {
                    int enPassantIndex = to.GetIndex() ^ 8;
                    _enPassantSquare = new Square((byte) enPassantIndex);
                }
            }
        }

        // TODO
        if (moveType == MoveType.CASTLING) {
            
        }
    }

    public void UnmakeMove(Move move) {
        StateInfo stateInfo = _prevStates.Pop();
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
            if (c == '/') {
                index -= 16;
                continue;
            }

            if (Char.IsDigit(c)) {
                index += (byte) (c - '0');
                continue;
            }
            
            Piece piece = Piece.FromChar(c);
            PlacePiece(index, piece);
            index++;
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

    public void Print() {
        StringBuilder stringBuilder = new StringBuilder("\n---------------------------------\n");
        byte index = 56;

        for (byte i = 0; i < 8; i++) {
            for (byte j = 0; j < 8; j++) {
                stringBuilder.Append("| ");
                stringBuilder.Append(GetPiece(index).GetChar());
                stringBuilder.Append(" ");

                index++;
            }

            stringBuilder.Append("|\n");
            stringBuilder.Append("---------------------------------\n");
            index -= 16;
        }
        Debug.Log(stringBuilder.ToString());
    }

    private void Init() {
        for (byte i = 0; i < 64; i++) {
            _pieces[i] = NullPiece.Instance();
        }
    }

}
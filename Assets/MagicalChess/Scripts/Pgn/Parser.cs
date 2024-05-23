using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class Parser {
    
    private static Parser _instance;
    private Board _board;

    private Parser() {
        _board = new Board();
        _board.SetFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
    }

    public static Parser Instance() {
        return _instance ?? new Parser();
    }
    
    private String RemoveCurlyBracesContent(String input) {
        String pattern = @"\{.*?\}";
        String result = Regex.Replace(input, pattern, "").Trim();
        return result;
    }

    public Pgn Parse(String file) {
        if (!File.Exists(file)) {
            Debug.Log("File does not exist");
            return null;
        }

        Pgn pgn = new Pgn();
        Dictionary<String, String> metadata = new Dictionary<string, string>();

        String[] lines = File.ReadAllLines(file);
        foreach (String line in lines) {
            if (line.StartsWith("[")) {
                String[] metaSplit = line.Split(' ', 2);

                String key = metaSplit[0].Substring(1);
                String value = metaSplit[1];

                value = value.Substring(0, value.Length - 1);
                value = value.Replace("\"", "");

                metadata[key] = value;
                
                continue;
            }
            
            if(String.IsNullOrWhiteSpace(line)) continue;

            String[] moveSplit = Regex.Split(line, @"\d+\.");
            moveSplit = moveSplit.Where(s => !String.IsNullOrWhiteSpace(s)).ToArray();

            for (int i = 0; i < moveSplit.Length; i++) {
                String sMoves = moveSplit[i];
                
                if (sMoves.Contains("{")) {
                    sMoves = RemoveCurlyBracesContent(sMoves);
                }
                
                String[] moveString = sMoves.Split(' ');

                foreach (String sMove in moveString) {
                    if(String.IsNullOrEmpty(sMove)) continue;
                    
                    if(sMove.StartsWith("{")) continue;
                    
                    if (!IsCastling(sMove) && IsResult(sMove)) {
                        pgn.SetMetadata(metadata);
                        return pgn;
                    }

                    Move move = ParseMove(sMove);
                    if (move != null) {
                        Debug.Log(move.GetFrom().GetValue() + " " + move.GetTo().GetValue());
                        _board.MakeMove(move);
                    }
                }
            }
        }
        
        pgn.SetMetadata(metadata);
        return pgn;
    }

    public Pgn Dummy() {
        List<Move> moves = new List<Move>();
        moves.Add(new Move(new Square(SquareValue.E2), new Square(SquareValue.E4)));
        moves.Add(new Move(new Square(SquareValue.C7), new Square(SquareValue.C6)));
        moves.Add(new Move(new Square(SquareValue.D2), new Square(SquareValue.D4)));
        moves.Add(new Move(new Square(SquareValue.D7), new Square(SquareValue.D5)));
        
        Pgn pgn = new Pgn();
        pgn.SetMoves(moves.ToArray());
        return pgn;
    }

    private Move ParseMove(String sMove) {
        if (IsCastling(sMove)) {
            return GetCastling(sMove);
        }
        
        // Pawn move -> d4
        if (sMove.Length == 2) {
            Square to = new Square(sMove);
            Square from = GetFromByTo(PieceType.PAWN, to);
            
            return new Move(from, to);
        }
        
        if (sMove.Length == 3) {
            // Pawn move with additional information -> fe6
            if (Char.IsLower(sMove[0])) {
                char additional = sMove[0];
                Square to = new Square(sMove.Substring(1));
                Square from = GetFromByToWithAdditionalInfo(PieceType.PAWN, to, additional);

                return new Move(from, to);
            }

            // Normal move -> Nc3
            if (Char.IsUpper(sMove[0])) {
                Piece piece = Piece.FromChar(sMove[0]);
                Square to = new Square(sMove.Substring(1));

                Square from = GetFromByTo(piece.GetPieceType(), to);
                return new Move(from, to);
            }
        }

        if (sMove.Length == 4) {
            // capture
            if (sMove[1] == 'x') {
                // Pawn move with additional information -> dxe4
                if (Char.IsLower(sMove[0])) {
                    Char additionalInfo = sMove[0];
                    Square to = new Square(sMove.Substring(2));
                    Square from = GetFromByToWithAdditionalInfo(PieceType.PAWN, to, additionalInfo);
                    return new Move(from, to);
                }

                // Capture move -> Nxe4
                if (Char.IsUpper(sMove[0])) {
                    Piece piece = Piece.FromChar(sMove[0]);
                    Square to = new Square(sMove.Substring(2));
                    
                    Square from = GetFromByTo(piece.GetPieceType(), to);
                    return new Move(from, to);
                }
            }

            // Move that gives check -> Bg6+
            if (sMove[3] == '+') {
                Piece piece = Piece.FromChar(sMove[0]);
                Square to = new Square(sMove.Substring(1, 2));
                
                Square from = GetFromByTo(piece.GetPieceType(), to);
                return new Move(from, to);
            }
            
            // Normal move with additional information -> Ngf6
            Piece pieceFromChar = Piece.FromChar(sMove[0]);
            Char additionalInfoTwo = sMove[1];
            Square toSquare = new Square(sMove.Substring(2));

            Square fromTwo = GetFromByToWithAdditionalInfo(pieceFromChar.GetPieceType(), toSquare, additionalInfoTwo);
            return new Move(fromTwo, toSquare);
        }

        throw new Exception("Unknown Move: " + sMove);
    }

    private Square GetFromByTo(PieceType pieceType, Square to) {
        List<Move> legalMoves = _board.GetLegalMoves(pieceType);
        
        foreach (Move legal in legalMoves) {
            if (legal.GetTo().GetValue() == to.GetValue()) {
                return legal.GetFrom();
            }
        }

        return new Square(SquareValue.NONE);
    }
    
    private Square GetFromByToWithAdditionalInfo(PieceType pieceType, Square to, char additional) {
        List<Move> legalMoves = _board.GetLegalMoves(pieceType);
        
        if (Char.IsLetter(additional)) {
            int fileIndex = Square.GetFileIndex(additional);
            
            foreach (Move legal in legalMoves) {
                if (legal.GetTo().GetValue() == to.GetValue() && legal.GetFrom().GetFileIndex() == fileIndex) {
                    return legal.GetFrom();
                }
            }
        }

        int rankIndex = Square.GetRankIndex(additional);
            
        foreach (Move legal in legalMoves) {
            if (legal.GetTo().GetValue() == to.GetValue() && legal.GetFrom().GetRankIndex() == rankIndex) {
                return legal.GetFrom();
            }
        }
        
        return new Square(SquareValue.NONE);
    }

    private bool IsCastling(String move) {
        switch (move) {
            case "O-O":
                return true;
            case "O-O-O":
                return true;
            default:
                return false;
        }
    }

    private Move GetCastling(String move) {
        switch (move) {
            case "O-O":
                if (_board.GetSideToMove() == Chess.Color.WHITE) {
                    return new Move(MoveType.CASTLING, new Square(SquareValue.E1), new Square(SquareValue.G1));
                }
                return new Move(MoveType.CASTLING, new Square(SquareValue.E8), new Square(SquareValue.G8));
            case "O-O-O":
                if (_board.GetSideToMove() == Chess.Color.WHITE) {
                    return new Move(MoveType.CASTLING, new Square(SquareValue.E1), new Square(SquareValue.C1));
                }
                return new Move(MoveType.CASTLING, new Square(SquareValue.E8), new Square(SquareValue.C8));
            default:
                return null;
        }
    }

    private bool IsResult(String move) {
        return move.StartsWith("1") || move.StartsWith("0");
    }
}
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
    
    /// <summary>
    /// Entfernt geschweifte Klammern und deren Inhalt im String 'input'.
    /// Notwendig zum Entfernen von Kommentaren in PGN-Dateien.
    /// </summary>
    /// <param name="input"></param>
    /// <returns>String</returns>
    private String RemoveCurlyBracesContent(String input) {
        String pattern = @"\{.*?\}";
        String result = Regex.Replace(input, pattern, "").Trim();
        return result;
    }

    /// <summary>
    /// Schnittstelle des Parsers. Erwartet im 'file' eine Datei im PGN-Format. Liefert ein Pgn-Objekt.
    /// </summary>
    /// <param name="file">PGN-Datei</param>
    /// <returns>Pgn</returns>
    public Pgn Parse(String file) {
        if (!File.Exists(file)) {
            Debug.Log("File does not exist");
            return null;
        }

        Pgn pgn = new Pgn();

        String[] lines = File.ReadAllLines(file);
        foreach (String line in lines) {
            if (line.StartsWith("[")) {
                String[] metaSplit = line.Split(' ', 2);

                String key = metaSplit[0].Substring(1);
                String value = metaSplit[1];

                value = value.Substring(0, value.Length - 1);
                value = value.Replace("\"", "");

                pgn.AddMetaData(key, value);
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
                        return pgn;
                    }

                    Move move = ParseMove(sMove);
                    if (move != null) {
                        pgn.AddMove(move);
                        _board.MakeMove(move);
                    }
                }
            }
        }
        return pgn;
    }

    /// <summary>
    /// Erwartet einen String 'sMove' in der verkürzten algebraischen Notation (Schachnotation).
    /// Gibt den Zug als Move-Objekt zurück. <br /><br />
    /// <b>HINWEIS: Es sind nicht alle Sonderfälle implementiert!</b>
    /// </summary>
    /// <param name="sMove">Zug in verkürzter algebraischer Notation</param>
    /// <returns>Move</returns>
    /// <exception cref="Exception"></exception>
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
                    return new Move(MoveType.CAPTURE, from, to);
                }

                // Capture move -> Nxe4
                if (Char.IsUpper(sMove[0])) {
                    Piece piece = Piece.FromChar(sMove[0]);
                    Square to = new Square(sMove.Substring(2));
                    
                    Square from = GetFromByTo(piece.GetPieceType(), to);
                    return new Move(MoveType.CAPTURE, from, to);
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

    /// <summary>
    /// Liefert das Ausgangsfeld auf Basis des Zielfeldes 'to' und der Typ der Figur 'pieceType'
    /// </summary>
    /// <param name="pieceType">Typ der Figur</param>
    /// <param name="to">Zielfeld</param>
    /// <returns>Square</returns>
    private Square GetFromByTo(PieceType pieceType, Square to) {
        List<Move> legalMoves = _board.GetLegalMoves(pieceType);
        
        foreach (Move legal in legalMoves) {
            if (legal.GetTo().GetValue() == to.GetValue()) {
                return legal.GetFrom();
            }
        }

        return new Square(SquareValue.NONE);
    }
    
    /// <summary>
    /// Wenn mehrere Figuren des gleichen Types auf dem gleichen Feld landen können, müssen mehr Informationen
    /// über das Ausgangsfeld vorliegen. Dies wird durch das zusätzliche Argument 'additional' realisiert.
    /// 'additional' kann entweder eine Zahl (Rank) oder ein Buchstabe (File) sein.
    /// </summary>
    /// <param name="pieceType">Typ der Figur</param>
    /// <param name="to">Zielfeld</param>
    /// <param name="additional">Zusätzliche Information über den Zug</param>
    /// <returns></returns>
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

    /// <summary>
    /// Liefert ein Move-Objekt mit dem Zug für die Rochade basierend auf dem String 'move'
    /// </summary>
    /// <param name="move">Rochaden-Zug in Schachnotation ("0-0" oder "0-0-0")</param>
    /// <returns>Move</returns>
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
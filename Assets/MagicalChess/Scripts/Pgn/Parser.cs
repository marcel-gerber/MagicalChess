using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class Parser {
    
    private static Parser _instance;

    private Parser() {
        
    }

    public static Parser Instance() {
        return _instance ?? new Parser();
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

            foreach (String sMoves in moveSplit) {
                String[] moveString = sMoves.Split(' ');

                foreach (String sMove in moveString) {
                    if (IsResult(sMove)) {
                        pgn.SetMetadata(metadata);
                        return pgn;
                    }

                    Move move = ParseMove(sMove);
                    if (move != null) {
                        Debug.Log(move.GetFrom().GetIndex() + " " + move.GetTo().GetIndex());
                    }
                }
                
            }
            
            //Debug.Log(line);
        }
        
        pgn.SetMetadata(metadata);
        return pgn;
    }

    private Move ParseMove(String sMove) {
        // Pawn move -> d4
        if (sMove.Length == 2) {
            Square to = new Square(sMove);
            Square from = new Square(SquareValue.NONE);
            
            Move move = new Move(from, to);
            return move;
        }

        // Pawn move with additional information -> fe6
        if (Char.IsLower(sMove[0])) {
            
        }
        
        return null;
    }

    private bool IsResult(String move) {
        return move.StartsWith("1") || move.StartsWith("0");
    }
}
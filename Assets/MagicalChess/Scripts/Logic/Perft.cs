using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementierung des Perft-Algorithmus zum Testen und Debuggen der Schachlogik.
/// Quelle: https://www.chessprogramming.org/Perft
/// </summary>
public class Perft {

    private readonly Board _board;

    public Perft(Board board) {
        _board = board;
    }

    private int perft(int depth) {
        if (depth == 0) {
            return 1;
        }

        int nodes = 0;
        List<Move> legalMoves = _board.GetPseudoLegalMoves();
        
        foreach (Move move in legalMoves) {
            _board.MakeMove(move);

            if (!_board.IsCheck()) {
                nodes += perft(depth - 1);
            }
            
            _board.UnmakeMove(move);
        }

        return nodes;
    }

    public void Run(int depth) {
        int nodes = perft(depth);
        Debug.Log("Nodes: " + nodes);
    }

}
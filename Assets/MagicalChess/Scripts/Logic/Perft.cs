using System;
using System.Collections.Generic;
using UnityEngine;

public class Perft {

    private Board _board;
    private int _debug;

    public Perft(Board board) {
        _board = board;
        _debug = 0;
    }

    private int perft(int depth) {
        if (depth == 0) {
            return 1;
        }

        int nodes = 0;
        List<Move> legalMoves = _board.GetPseudoLegalMoves();
        
        foreach (Move move in legalMoves) {
            String str = _board.String();
            _board.MakeMove(move);

            if (!_board.IsCheck()) {
                if (move.GetMoveType() == MoveType.CASTLING) {
                    Debug.Log(str);
                    Debug.Log(move.GetFrom().GetValue() + " " + move.GetTo().GetValue());
                }
                
                nodes += perft(depth - 1);
            }
            
            _board.UnmakeMove(move);
        }

        return nodes;
    }

    public void Run(int depth) {
        int nodes = perft(depth);
        Debug.Log("Nodes: " + nodes);
        Debug.Log("Debug: " + _debug);
    }

}
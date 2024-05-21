using System.Collections.Generic;
using UnityEngine;

public class Perft {

    private Board _board;
    private int _checks;

    public Perft(Board board) {
        _board = board;
        _checks = 0;
    }

    private int perft(int depth) {
        if (depth == 0) {
            return 1;
        }

        int nodes = 0;
        List<Move> legalMoves = _board.GetPseudoLegalMoves();
        
        foreach (Move move in legalMoves) {
            _board.MakeMove(move);

            if (_board.IsCheck()) {
                _checks++;
            }
            else {
                nodes += perft(depth - 1);
            }
            
            _board.UnmakeMove(move);
        }

        return nodes;
    }

    public void Run(int depth) {
        int nodes = perft(depth);
        Debug.Log("Nodes: " + nodes);
        Debug.Log("Checks: " + _checks);
    }

}
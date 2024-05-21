using System.Collections.Generic;
using UnityEngine;

public class Perft {

    private Board _board;

    public Perft(Board board) {
        _board = board;
    }

    private int perft(int depth) {
        List<Move> legalMoves = _board.GetPseudoLegalMoves();

        if (depth == 1) {
            return legalMoves.Count;
        }

        int nodes = 0;
        foreach (Move move in legalMoves) {
            _board.MakeMove(move);
            nodes += perft(depth - 1);
            _board.UnmakeMove(move);
        }

        return nodes;
    }

    public void Run(int depth) {
        int nodes = perft(depth);
        Debug.Log("Nodes: " + nodes);
    }

}
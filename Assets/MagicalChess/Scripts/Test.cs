using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Test : MonoBehaviour {
    
    // Start is called before the first frame update
    void Start() {
        Parser parser = Parser.Instance();
        Pgn pgn = parser.Parse(Path.GetFullPath("Assets/MagicalChess/Pgn/game.pgn"));

        Move move;
        while ((move = pgn.GetNextMove()) != null) {
            Debug.Log(move.GetFrom().GetValue() + " " + move.GetTo().GetValue());
            
        }

        // Board board = new Board();
        // board.SetFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        // board.SetFen("r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 0");
        // board.SetFen("8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 0");
        // board.Print();

        // Move move = new Move(MoveType.CASTLING, new Square(SquareValue.E1), new Square(SquareValue.C1));
        // board.MakeMove(move);
        // board.Print();

        // Perft perft = new Perft(board);
        // perft.Run(2);

        // List<Move> moves = board.GetPseudoLegalMoves();
        // board.MakeMove(moves[13]);
        // board.Print();

        // moves = board.GetPseudoLegalMoves();
        // board.MakeMove(moves[10]);
        // board.Print();
        //
        // moves = board.GetPseudoLegalMoves();
        // foreach (Move move in moves) {
        //     Debug.Log(move.GetFrom().GetValue() + " " + move.GetTo().GetValue());
        // }
    }

    // Update is called once per frame
    void Update() {
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Test : MonoBehaviour {
    
    // Start is called before the first frame update
    void Start() {
        /*Parser parser = Parser.Instance();
        Pgn pgn = parser.Parse(Path.GetFullPath("Assets/MagicalChess/Pgn/game.pgn"));*/

        Board board = new Board();
        board.SetFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        board.Print();
    }

    // Update is called once per frame
    void Update() {
    }
}
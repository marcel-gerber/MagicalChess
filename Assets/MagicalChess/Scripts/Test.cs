using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Test : MonoBehaviour {
    
    // Start is called before the first frame update
    void Start() {
        /*Parser parser = Parser.Instance();
        Pgn pgn = parser.Parse(Path.GetFullPath("Assets/MagicalChess/Pgn/game.pgn"));*/

        Square square = new Square(SquareValue.B5);
        Square target = square + Direction.WEST;
        
        Debug.Log(target.GetIndex());
    }

    // Update is called once per frame
    void Update() {
    }
}
using System.IO;
using UnityEngine;

public class Test : MonoBehaviour {
    
    // Start is called before the first frame update
    void Start() {
        Parser parser = Parser.Instance();
        parser.parse(Path.GetFullPath("Pgn/game.pgn"));
    }

    // Update is called once per frame
    void Update() {
    }
}
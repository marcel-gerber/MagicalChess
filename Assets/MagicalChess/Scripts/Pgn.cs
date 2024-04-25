using System;
using System.Collections.Generic;

public class Pgn {
    
    private Dictionary<String, String> metadata;
    private Move[] moves;

    public Pgn() {
        
    }

    public Dictionary<String, String> GetMetadata() {
        return metadata;
    }
    
    public void SetMetadata(Dictionary<String, String> data) {
        this.metadata = data;
    }

    public Move[] GetMoves() {
        return moves;
    }

    public void SetMoves(Move[] moveArray) {
        this.moves = moveArray;
    }
}
using System;
using System.Collections.Generic;

/// <summary>
/// Repräsentiert eine PGN-Datei, bestehend aus Metadaten und Zügen.
/// </summary>
public class Pgn {
    
    private readonly Dictionary<String, String> _metadata;
    private readonly List<Move> _moves;
    private int _position;

    public Pgn() {
        _metadata = new Dictionary<string, string>();
        _moves = new List<Move>();
        _position = 0;
    }

    public Dictionary<String, String> GetMetadata() {
        return _metadata;
    }

    public void AddMetaData(String key, String value) {
        _metadata[key] = value;
    }

    public List<Move> GetMoves() {
        return _moves;
    }

    public void AddMove(Move move) {
        _moves.Add(move);
    }

    public Move GetNextMove() {
        if (_position >= _moves.Count) return null;
        return _moves[_position++];
    }
}
using System;
using System.IO;
using UnityEngine;

public class Parser {
    
    private static Parser _instance;

    private Parser() {
        
    }

    public static Parser Instance() {
        if (_instance == null) {
            return new Parser();
        }
        return _instance;
    }

    public void parse(String file) {
        if (!File.Exists(file)) {
            Debug.Log("File does not exist");
            return;
        }

        String[] lines = File.ReadAllLines(file);
        foreach (String line in lines) {
            Debug.Log(line);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    
    // Start is called before the first frame update
    void Start() {
        Square square = new Square("h8");
        
        Debug.Log("DEBUG: " + square.GetIndex());
    }

    // Update is called once per frame
    void Update() {
    }
}
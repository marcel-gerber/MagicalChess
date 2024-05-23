using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveFigure : MonoBehaviour
{

    public GameObject chessBoard;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick()
    {
        chessBoard.GetComponent<MoveChessFigures>().moveFigures();
        Debug.Log("Clicked Button");
    }
    
    
}

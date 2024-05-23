using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveChessFigures : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject figuresWhite;
    public GameObject figuresBlack;
    public GameObject d5;
    void Start()
    {
        GameObject chessBoard = gameObject;
        MeshRenderer meshRenderer = chessBoard.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveFigures()
    {
        /*
        for (int i = 0; i < figuresBlack.transform.childCount; i++)
        {
            GameObject figure = figuresBlack.transform.GetChild(i).gameObject;
            figure.transform.localPosition += new Vector3(0, 0, 2);
        }
        */
        GameObject testFigure = figuresBlack.transform.GetChild(0).gameObject;
        testFigure.transform.position = d5.transform.position;
    }
}

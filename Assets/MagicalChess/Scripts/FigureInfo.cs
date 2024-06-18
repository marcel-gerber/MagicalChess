using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureInfo : MonoBehaviour {

    [SerializeField] private Chess.Color color;
    [SerializeField] private PieceType pieceType;

    public Chess.Color GetColor() {
        return color;
    }

    public PieceType GetPieceType() {
        return pieceType;
    }
}
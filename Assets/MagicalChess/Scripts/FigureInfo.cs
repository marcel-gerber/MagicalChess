using UnityEngine;

/// <summary>
/// Alle Figuren bekommen ein 'FigureInfo'-Objekt, damit man weiß, um welche Figur es sich konkret handelt 
/// </summary>
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
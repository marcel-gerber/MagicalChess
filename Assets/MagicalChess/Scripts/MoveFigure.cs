using System;
using System.IO;
using UnityEngine;
using Color = Chess.Color;
using Random = UnityEngine.Random;

public class MoveFigure : MonoBehaviour {

    // Fractured Objects
    [SerializeField] private GameObject fracturedWhitePawn;
    [SerializeField] private GameObject fracturedWhiteKnight;
    [SerializeField] private GameObject fracturedWhiteBishop;
    [SerializeField] private GameObject fracturedWhiteRook;
    [SerializeField] private GameObject fracturedWhiteQueen;
    
    [SerializeField] private GameObject fracturedBlackPawn;
    [SerializeField] private GameObject fracturedBlackKnight;
    [SerializeField] private GameObject fracturedBlackBishop;
    [SerializeField] private GameObject fracturedBlackRook;
    [SerializeField] private GameObject fracturedBlackQueen;
    
    public GameObject positions;

    private Pgn pgn;

    // Notwendige Objekte für das Bewegen der Figur
    private bool isMoving;
    private GameObject movingFigure;
    private GameObject capturedFigure;
    private Transform planeFrom;
    private Transform planeTo;
    
    // Notwendige Objekte für die Rochade (Bewegender Turm und zu welchem Feld er sich bewegt)
    private GameObject castlingRook;
    private Transform castlingTo;

    void Start() {
        Parser parser = Parser.Instance();
        pgn = parser.Parse(Path.GetFullPath("Assets/MagicalChess/Pgn/game.pgn"));
        isMoving = false;
    }

    /// <summary>
    /// Wird bei jedem Frame ausgeführt. Sobald 'isMoving' true ist, wird die Figur 'movingFigure' zum Feld
    /// 'planeTo' bewegt
    /// </summary>
    void Update() {
        if (!isMoving) return;
        
        float delta = Time.deltaTime;

        if (movingFigure != null) {
            movingFigure.transform.position =
                Vector3.MoveTowards(movingFigure.transform.position, planeTo.transform.position, delta);
        }
        else {
            castlingRook.transform.position =
                Vector3.MoveTowards(castlingRook.transform.position, castlingTo.transform.position, delta);

            if (castlingRook.transform.position == castlingTo.position) {
                isMoving = false;
            }
            return;
        }

        if (movingFigure.transform.position == planeTo.position) {
            if (castlingRook != null) {
                movingFigure = null;
            }
            else {
                isMoving = false;
            }
            
            if(capturedFigure == null) return;

            // Figur explodieren lassen
            GameObject fracturedFigure = GetFracturedObject(capturedFigure);
            
            capturedFigure.SetActive(false);
            GameObject fractured = Instantiate(fracturedFigure, capturedFigure.transform.position, Quaternion.identity);

            foreach (Transform transform in fractured.transform) {
                Rigidbody rigidbody = transform.GetComponent<Rigidbody>();
                
                rigidbody.AddExplosionForce(Random.Range(10, 10), capturedFigure.transform.position, 10);
            }
        }
    }

    /// <summary>
    /// Holt den nächsten Zug aus dem PGN-Objekt und lässt die Figur bewegen.
    /// Wird beim Betätigen des Buttons 'Nächster Zug' ausgeführt
    /// </summary>
    public void onClick() {
        Debug.Log("Clicked Button");

        if (isMoving) {
            Debug.LogWarning("figure currently moving. please wait");
            return;
        }

        Move nextMove = pgn.GetNextMove();
        movingFigure = null;
        capturedFigure = null;
        castlingRook = null;
        castlingTo = null;

        if (nextMove == null) {
            Debug.LogWarning("no moves left.");
            return;
        }

        Debug.Log("NEW MOVE: " + nextMove.GetFrom().GetValue() + " --> " + nextMove.GetTo().GetValue());
        String positionFrom = nextMove.GetFrom().GetValue().ToString().ToLower();
        String positionTo = nextMove.GetTo().GetValue().ToString().ToLower();

        planeFrom = positions.transform.Find(positionFrom);

        if (planeFrom == null) {
            Debug.LogWarning("no child object.");
            return;
        }

        movingFigure = searchNearestFigure(planeFrom.transform.position);

        if (movingFigure == null) {
            Debug.LogWarning("no nearest figure found.");
            return;
        }

        Debug.Log("Found nearest gameObject: " + movingFigure.name);

        planeTo = positions.transform.Find(positionTo);

        if (planeTo == null) {
            Debug.LogWarning("no child object.");
            return;
        }

        if (nextMove.GetMoveType() == MoveType.CAPTURE) {
            capturedFigure = searchNearestFigure(planeTo.transform.position);
        } else if (nextMove.GetMoveType() == MoveType.CASTLING) {
            // Art der Rochade bestimmen und Feld des Turms holen
            CastlingValue castlingValue = Castling.GetFromKingIndex(nextMove.GetTo().GetIndex());
            byte rookFromIndex = Castling.GetStartingRookIndex(castlingValue);
            Square rookFromSquare = new Square(rookFromIndex);
            
            // Plane, auf der sich der Turm befindet holen und dann den Turm holen
            Transform planeOfRook = positions.transform.Find(rookFromSquare.GetValue().ToString().ToLower());
            castlingRook = searchNearestFigure(planeOfRook.transform.position);
            
            // Plane, zu der sich der Turm bewegt holen
            byte rookToIndex = Castling.GetEndingRookIndex(castlingValue);
            Square rookToSquare = new Square(rookToIndex);
            castlingTo = positions.transform.Find(rookToSquare.GetValue().ToString().ToLower());
        }

        isMoving = true;

        Debug.Log("moved gameObject: " + movingFigure.name);
    }

    /// <summary>
    /// Gibt die Figur, welcher der 'currentPosition' am nächsten ist, zurück
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <returns>GameObject</returns>
    private GameObject searchNearestFigure(Vector3 currentPosition) {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        float closest = 1000;
        GameObject closestObject = null;

        for (int i = 0; i < allObjects.Length; i++) {
            GameObject currentObject = allObjects[i];
            MeshFilter meshFilter = currentObject.GetComponent<MeshFilter>();
            if (meshFilter != null) {
                Mesh mesh = meshFilter.sharedMesh;
                if (mesh != null && mesh.name == "Plane") {
                    continue;
                }
            }

            if (currentObject.name == "Pferd" || currentObject.name == "Directional Light") {
                continue;
            }
            
            if(currentObject.GetComponent<FigureInfo>() == null) continue;

            float dist = Vector3.Distance(allObjects[i].transform.position, currentPosition);
            if (dist < closest) {
                closest = dist;
                closestObject = allObjects[i];
            }
        }

        return closestObject;
    }

    /// <summary>
    /// Gibt das Fractured Object der Figur 'figure' zurück
    /// </summary>
    /// <param name="figure"></param>
    /// <returns>GameObject</returns>
    private GameObject GetFracturedObject(GameObject figure) {
        FigureInfo figureInfo = figure.GetComponent<FigureInfo>();

        if (figureInfo == null) return null;
        Chess.Color color = figureInfo.GetColor();
        
        switch (figureInfo.GetPieceType()) {
            case PieceType.PAWN:
                if (color == Color.WHITE) {
                    return fracturedWhitePawn;
                }
                return fracturedBlackPawn;
            
            case PieceType.KNIGHT:
                if (color == Color.WHITE) {
                    return fracturedWhiteKnight;
                }
                return fracturedBlackKnight;
            
            case PieceType.BISHOP:
                if (color == Color.WHITE) {
                    return fracturedWhiteBishop;
                }
                return fracturedBlackBishop;
            
            case PieceType.ROOK:
                if (color == Color.WHITE) {
                    return fracturedWhiteRook;
                }
                return fracturedBlackRook;
            
            case PieceType.QUEEN:
                if (color == Color.WHITE) {
                    return fracturedWhiteQueen;
                }
                return fracturedBlackQueen;
            
            default:
                return null;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Rendering;
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
    
    // public GameObject chessBoard;
    // public GameObject blackFigures;
    // public GameObject whiteFigures;
    public GameObject positions;

    private Pgn pgn;

    private bool isMoving;
    private GameObject movingFigure;
    private GameObject capturedFigure;
    private Transform planeFrom;
    private Transform planeTo;

    void Start() {
        Parser parser = Parser.Instance();
        pgn = parser.Parse(Path.GetFullPath("Assets/MagicalChess/Pgn/game.pgn"));
        isMoving = false;
    }

    // Update is called once per frame
    void Update() {
        if (!isMoving) return;
        
        float delta = Time.deltaTime;
        movingFigure.transform.position =
            Vector3.MoveTowards(movingFigure.transform.position, planeTo.transform.position, delta);

        if (movingFigure.transform.position == planeTo.position) {
            isMoving = false;
            
            if(capturedFigure == null) return;

            // Figur explodieren lassen
            GameObject fracturedFigure = GetFracturedObject(capturedFigure);
            
            // Destroy(capturedFigure);
            capturedFigure.SetActive(false);
            GameObject fractured = Instantiate(fracturedFigure, capturedFigure.transform.position, Quaternion.identity);

            foreach (Transform transform in fractured.transform) {
                Rigidbody rigidbody = transform.GetComponent<Rigidbody>();
                
                rigidbody.AddExplosionForce(Random.Range(10, 10), capturedFigure.transform.position, 10);
            }
        }
    }

    public void onClick() {
        Debug.Log("Clicked Button");

        if (isMoving) {
            Debug.LogWarning("figure currently moving. please wait");
            return;
        }

        Move nextMove = pgn.GetNextMove();
        movingFigure = null;
        capturedFigure = null;

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
        }

        isMoving = true;

        Debug.Log("moved gameObject: " + movingFigure.name);
    }

    private GameObject searchNearestFigure(Vector3 currentPosition) {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        float closest = 1000;
        GameObject closestObject = null;

        for (int i = 0; i < allObjects.Length; i++) {
            GameObject currentObject = allObjects[i];
            //PrimitiveType type = meshFilter.GetComponent<PrimitiveType>();
            //MeshRenderer meshRenderer = allObjects[i].GetComponent<MeshRenderer>();
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
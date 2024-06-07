using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class MoveFigure : MonoBehaviour {
    
    public GameObject chessBoard;
    public GameObject blackFigures;
    public GameObject whiteFigures;
    public GameObject positions;

    private Pgn pgn;

    private bool isMoving;
    private GameObject nearestFigure;
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
        nearestFigure.transform.position =
            Vector3.MoveTowards(nearestFigure.transform.position, planeTo.transform.position, delta);

        if (nearestFigure.transform.position == planeTo.position) {
            isMoving = false;
        }
    }

    public void onClick() {
        //chessBoard.GetComponent<MoveChessFigures>().moveFigures();
        Debug.Log("Clicked Button");

        if (isMoving) {
            Debug.LogWarning("figure currently moving. please wait");
            return;
        }

        Move next = pgn.GetNextMove();

        if (next == null) {
            Debug.LogWarning("no moves left.");
            return;
        }

        Debug.Log("NEW MOVE: " + next.GetFrom().GetValue() + " --> " + next.GetTo().GetValue());
        String positionFrom = next.GetFrom().GetValue().ToString().ToLower();
        String positionTo = next.GetTo().GetValue().ToString().ToLower();

        planeFrom = positions.transform.Find(positionFrom);

        if (planeFrom == null) {
            Debug.LogWarning("no child object.");
            return;
        }

        nearestFigure = searchNearestFigure(planeFrom.transform.position);

        if (nearestFigure == null) {
            Debug.LogWarning("no nearest figure found.");
            return;
        }

        Debug.Log("Found nearest gameObject: " + nearestFigure.name);
        // planeFrom.transform.position += new Vector3(0, 0.1f, 0);

        // MeshRenderer meshRenderer = currentPlane.GetComponent<MeshRenderer>();
        // if (meshRenderer != null)
        // {
        //     meshRenderer.material.color = Color.blue;
        // }
        // else
        // {
        //     Debug.LogWarning("no meshRenderer.");
        // }

        planeTo = positions.transform.Find(positionTo);

        if (planeTo == null) {
            Debug.LogWarning("no child object.");
            return;
        }

        isMoving = true;

        // nearestFigure.transform.position = planeTo.transform.position;
        Debug.Log("moved gameObject: " + nearestFigure.name);

        // planeTo.transform.position += new Vector3(0, 0.1f, 0);

        // MeshRenderer meshRenderer = currentPlaneTo.GetComponent<MeshRenderer>();
        // if (meshRenderer != null)
        // {
        //     meshRenderer.material.color = Color.red;
        // } 
        // else
        // {
        //     Debug.LogWarning("no meshRenderer.");
        // }
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

            float dist = Vector3.Distance(allObjects[i].transform.position, currentPosition);
            if (dist < closest) {
                closest = dist;
                closestObject = allObjects[i];
            }
        }

        return closestObject;
    }
}
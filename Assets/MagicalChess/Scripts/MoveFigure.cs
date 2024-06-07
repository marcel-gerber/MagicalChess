using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class MoveFigure : MonoBehaviour
{

    public GameObject chessBoard;
    public GameObject blackFigures;
    public GameObject whiteFigures;
    public GameObject positions;

    private Pgn pgn;
    private int moveSize;
    private int currentMove;
    
    void Start()
    {
        Parser parser = Parser.Instance();
        pgn = parser.Parse(Path.GetFullPath("Assets/MagicalChess/Pgn/game.pgn"));
        moveSize = pgn.GetMoves().Count;
        currentMove = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick()
    {
        //chessBoard.GetComponent<MoveChessFigures>().moveFigures();
        Debug.Log("Clicked Button");
        
        if (currentMove < moveSize)
        {
            Move current = pgn.GetMoves()[currentMove];
            Debug.Log("NEW MOVE: " + current.GetFrom().GetValue() + " --> " + current.GetTo().GetValue());
            currentMove++;
            String positionFrom = current.GetFrom().GetValue().ToString().ToLower();
            String positionTo = current.GetTo().GetValue().ToString().ToLower();
            GameObject nearestFigure = null;
            
            Transform currentPlane = positions.transform.Find(positionFrom);
            if (currentPlane != null)
            {
                nearestFigure = searchNearestFigure(currentPlane.transform.position);
                if (nearestFigure != null)
                {
                    //nearestFigure.transform.position += new Vector3(0, 2, 0);
                    Debug.Log("Found nearest gameObject: " + nearestFigure.name);
                }
                currentPlane.transform.position += new Vector3(0, 0.1f, 0);
                
                MeshRenderer meshRenderer = currentPlane.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.material.color = Color.blue;
                } 
                else
                {
                    Debug.LogWarning("no meshRenderer.");
                }
            } 
            else
            {
                Debug.LogWarning("no child object.");
            }
            
            Transform currentPlaneTo = positions.transform.Find(positionTo);
            if (currentPlaneTo != null)
            {
                if (nearestFigure != null)
                {
                    nearestFigure.transform.position = currentPlaneTo.transform.position;
                    Debug.Log("moved gameObject: " + nearestFigure.name);
                }
                currentPlaneTo.transform.position += new Vector3(0, 0.1f, 0);
                MeshRenderer meshRenderer = currentPlaneTo.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.material.color = Color.red;
                } 
                else
                {
                    Debug.LogWarning("no meshRenderer.");
                }
            } 
            else
            {
                Debug.LogWarning("no child object.");
            }
        }
        else
        {
            Debug.LogWarning("no moves left.");  
        }
    }

    private GameObject searchNearestFigure(Vector3 currentPosition)
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() ;
        float closest = 1000;
        GameObject closestObject = null;
        
        for (int i = 0; i < allObjects.Length; i++)
        {
            GameObject currentObject = allObjects[i];
            //PrimitiveType type = meshFilter.GetComponent<PrimitiveType>();
            //MeshRenderer meshRenderer = allObjects[i].GetComponent<MeshRenderer>();
            MeshFilter meshFilter = currentObject.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                Mesh mesh = meshFilter.sharedMesh;
                if (mesh != null && mesh.name == "Plane")
                {
                    continue;
                }
            }

            if (currentObject.name == "Pferd" || currentObject.name == "Directional Light")
            {
                continue;
            }
            
            float dist = Vector3.Distance(allObjects[ i ].transform.position, currentPosition);
            if (dist < closest)
            {
                closest = dist;
                closestObject = allObjects[ i ];
            }
        }
        return closestObject;
    }
    
}

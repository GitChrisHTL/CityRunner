using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Geschwindigkeit, mit der sich die Layer bewegen
    [SerializeField] private GameObject player;   // Referenz auf den Spieler
    [SerializeField] private LayerMask backgroundLayer; // Layer für den Hintergrund
    [SerializeField] private LayerMask groundLayer;     // Layer für den Boden
    [SerializeField] private LayerMask obstacleLayer;   // Layer für die Hindernisse
    [SerializeField] private LayerMask collectableLayer;   // Layer für die Collectables

    private void Update()
    {
        // Hintergrund, Boden und Hindernisse nach links bewegen
        MoveLayers();
    }

    private void MoveLayers()
    {
        // Alle Objekte im "Background" Layer bewegen
        MoveObjectsInLayer(backgroundLayer);

        // Alle Objekte im "Ground" Layer bewegen
        MoveObjectsInLayer(groundLayer);

        // Alle Objekte im "Obstacle" Layer bewegen
        MoveObjectsInLayer(obstacleLayer);

        // Alle Objekte im "Collectable" Layer bewegen
        MoveObjectsInLayer(collectableLayer);
    }

    private void MoveObjectsInLayer(LayerMask layer)
    {
        // Alle Objekte im gegebenen Layer finden und bewegen
        foreach (GameObject obj in FindObjectsInLayer(layer))
        {
            obj.transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
    }

    private GameObject[] FindObjectsInLayer(LayerMask layer)
    {   
    List<GameObject> objects = new List<GameObject>();
    foreach (GameObject obj in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
    {
        if (((1 << obj.layer) & layer) != 0)
        {
            objects.Add(obj);
        }
    }
    return objects.ToArray();
    }
}

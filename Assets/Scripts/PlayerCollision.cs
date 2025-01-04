using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.transform.tag == "Obstacle") {
            Destroy(gameObject);
            // GameManager Set Game Over
            Debug.Log("Player failed!");
            OnPlayerFail();
        }
    }

    private void OnPlayerFail()
    {
        // Logik für das Spielende oder Neustart
        Debug.Log("Game Over!");
        // Hier kannst du das Spiel neu starten oder ein Game-Over-Menü anzeigen
    }
}

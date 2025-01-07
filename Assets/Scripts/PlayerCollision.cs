using UnityEngine;
using UnityEngine.SceneManagement; // Für den Szenen-Neustart
using UnityEngine.UI; // Für UI-Elemente

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI; // Game Over UI, das in Unity verbunden wird

    public CoinController coinController;
    private void Start()
    {
        // Game Over UI zu Beginn des Spiels deaktivieren
        gameOverUI.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Obstacle"))
        {
            Debug.Log("Player hit an obstacle!");
            OnPlayerFail();
        } else if (other.transform.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coinController.coinCount++;
        }
    }

    private void OnPlayerFail()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0; // Spiel anhalten
        gameOverUI.SetActive(true); // Game Over UI anzeigen
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Spielzeit zurücksetzen
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Aktuelle Szene neu laden
    }
}

using UnityEngine;
using UnityEngine.SceneManagement; // Für den Szenen-Neustart
using UnityEngine.UI; // Für UI-Elemente

public class FallDetector : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI; // Game Over UI, das in Unity verbunden wird

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player fell!");
            OnPlayerFail();
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

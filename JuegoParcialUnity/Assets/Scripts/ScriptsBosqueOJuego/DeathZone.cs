using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    [Header("Game Over Target")]
    public string gameOverSceneName = "GameOver";
    public int gameOverSceneIndex = -1; // asigna índice si prefieres (deja -1 para usar name)

    [Header("Behavior")]
    public bool useLivesBeforeGameOver = false; // si quieres gastar vidas antes de gameover
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;

        if (useLivesBeforeGameOver && player.extraLives > 0)
        {
            // consume one extra life and respawn in last saved position if exists
            player.extraLives -= 1;
            Debug.Log("Used one extra life. Remaining: " + player.extraLives);

            // if there's a saved position in SceneState, respawn there
            if (SceneState.lastPlayerPosition != Vector3.zero)
            {
                player.RespawnAt(SceneState.lastPlayerPosition);
                return;
            }
            else
            {
                // fallback: respawn at (0,0)
                player.RespawnAt(Vector3.zero);
                return;
            }
        }

        // no lives left or not using lives: go to Game Over
        if (gameOverSceneIndex >= 0) SceneManager.LoadScene(gameOverSceneIndex);
        else SceneManager.LoadScene(gameOverSceneName);
    }
}

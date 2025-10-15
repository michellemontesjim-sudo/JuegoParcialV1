using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManagerLocal : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform spawnPlayer;
    public Transform spawnEnemy;

    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    private GameObject player;
    private GameObject enemy;
    private int playerHP = 30;
    private int enemyHP = 25;
    private bool playerTurn = true;

    void Start()
    {
        // Spawnear jugador y enemigo
        player = Instantiate(playerPrefab, spawnPlayer.position, Quaternion.identity);
        enemy = Instantiate(enemyPrefab, spawnEnemy.position, Quaternion.identity);

        Debug.Log("Battle started! Player vs Enemy");
        Debug.Log($"Player HP: {playerHP} | Enemy HP: {enemyHP}");
    }

    void Update()
    {
        if (playerHP <= 0)
        {
            Debug.Log("Player defeated!");
            SceneManager.LoadScene("GameOver");
        }
        else if (enemyHP <= 0)
        {
            Debug.Log("Enemy defeated!");
            SceneManager.LoadScene("ForestScene");
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            DoTurn();
        }
    }

    void DoTurn()
    {
        if (playerTurn)
        {
            int dmg = Random.Range(5, 15);
            enemyHP -= dmg;
            Debug.Log($"Player attacks! Deals {dmg} damage. Enemy HP: {enemyHP}");
        }
        else
        {
            int dmg = Random.Range(4, 10);
            playerHP -= dmg;
            Debug.Log($"Enemy attacks! Deals {dmg} damage. Player HP: {playerHP}");
        }

        playerTurn = !playerTurn;
    }
}

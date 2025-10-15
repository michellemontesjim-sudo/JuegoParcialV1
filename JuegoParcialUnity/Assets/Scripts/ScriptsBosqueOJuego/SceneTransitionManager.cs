// SceneTransitionManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [Header("Fade UI")]
    public Image blackOverlay; // assign a UI Image (full screen), set color black and alpha 0
    public float fadeDuration = 0.5f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (blackOverlay != null)
        {
            var c = blackOverlay.color; c.a = 0f; blackOverlay.color = c;
            blackOverlay.gameObject.SetActive(true);
        }
    }

    // Basic fade out -> load -> fade in
    public void LoadSceneWithFade(string sceneName, Vector3? savePlayerPosition = null)
    {
        StartCoroutine(DoLoadScene(sceneName, -1, savePlayerPosition));
    }

    public void LoadSceneWithFade(int sceneIndex, Vector3? savePlayerPosition = null)
    {
        StartCoroutine(DoLoadScene(null, sceneIndex, savePlayerPosition));
    }

    IEnumerator DoLoadScene(string sceneName, int sceneIndex, Vector3? savePlayerPosition)
    {
        yield return StartCoroutine(Fade(0f, 1f)); // fade to black

        // save currently active scene info if provided
        SceneState.lastPlayerPosition = savePlayerPosition ?? SceneState.lastPlayerPosition;
        SceneState.lastSceneName = SceneManager.GetActiveScene().name;
        SceneState.lastSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

        if (!string.IsNullOrEmpty(sceneName))
            SceneManager.LoadScene(sceneName);
        else
            SceneManager.LoadScene(sceneIndex);

        yield return null; // wait a frame for scene load

        yield return StartCoroutine(Fade(1f, 0f)); // fade in
    }

    IEnumerator Fade(float from, float to)
    {
        if (blackOverlay == null) yield break;
        float t = 0f;
        Color col = blackOverlay.color;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(from, to, t / fadeDuration);
            col.a = a;
            blackOverlay.color = col;
            yield return null;
        }
        col.a = to;
        blackOverlay.color = col;
    }

    // Shortcut to go to battle scene: we save player's position automatically
    public void GoToBattleScene(string battleSceneName, Vector3 playerPosition)
    {
        // store player's info to SceneState then load battle scene with fade
        SceneState.lastPlayerPosition = playerPosition;
        SceneState.lastSceneName = SceneManager.GetActiveScene().name;
        SceneState.lastSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneState.lastBattleResult = null; // reset
        StartCoroutine(GoToBattleAsync(battleSceneName));
    }

    IEnumerator GoToBattleAsync(string battleSceneName)
    {
        yield return StartCoroutine(Fade(0f, 1f));
        SceneManager.LoadScene(battleSceneName);
        yield return null;
        yield return StartCoroutine(Fade(1f, 0f));
    }

    // Call this after finishing battle in battle scene:
    // if won -> return to last scene and restore position
    // if lost -> load gameover
    public IEnumerator HandleBattleEndAndReturn(bool won, string gameOverSceneName = "", int gameOverSceneIndex = -1)
    {
        SceneState.lastBattleResult = won;

        if (!won)
        {
            // go to game over
            yield return StartCoroutine(Fade(0f, 1f));
            if (!string.IsNullOrEmpty(gameOverSceneName))
                SceneManager.LoadScene(gameOverSceneName);
            else if (gameOverSceneIndex >= 0)
                SceneManager.LoadScene(gameOverSceneIndex);
            else
                Debug.LogWarning("GameOver scene not configured in HandleBattleEndAndReturn.");
            yield break;
        }

        // won -> return to previous
        yield return StartCoroutine(Fade(0f, 1f));

        // load last scene
        if (!string.IsNullOrEmpty(SceneState.lastSceneName))
            SceneManager.LoadScene(SceneState.lastSceneName);
        else if (SceneState.lastSceneBuildIndex >= 0)
            SceneManager.LoadScene(SceneState.lastSceneBuildIndex);
        else
            Debug.LogWarning("No last scene stored to return to.");

        yield return null;
        // optional: wait a frame then fade in
        yield return StartCoroutine(Fade(1f, 0f));
    }
}

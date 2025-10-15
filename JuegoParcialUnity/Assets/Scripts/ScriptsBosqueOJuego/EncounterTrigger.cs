using Cinemachine.Examples;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterTrigger : MonoBehaviour
{
    [Header("Battle Scene Name")]
    public string battleSceneName = "BattleScene";

    [Header("Transition Settings")]
    public float delayBeforeLoad = 0.5f; // medio segundo antes de cambiar de escena

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (!other.CompareTag("Player")) return;

        triggered = true;
        Debug.Log(" Encounter triggered! Preparing battle...");

        // 1) Try direct typed access (most common)
        if (other.TryGetComponent<PlayerController>(out var pc))
        {
            // try public field or property
            try { pc.canMove = false; } catch { /* ignore */ }
        }
        else if (other.TryGetComponent<PlayerMovement>(out var pm))
        {
            try { pm.canMove = false; } catch { /* ignore */ }
        }
        else
        {
            // 2) Best-effort: call a named method if exists (no error if not)
            other.SendMessage("SetCanMove", false, SendMessageOptions.DontRequireReceiver);
            other.SendMessage("DisableMovement", SendMessageOptions.DontRequireReceiver);
        }

        StartCoroutine(LoadBattleScene());
    }

    private IEnumerator LoadBattleScene()
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(battleSceneName);
    }
}

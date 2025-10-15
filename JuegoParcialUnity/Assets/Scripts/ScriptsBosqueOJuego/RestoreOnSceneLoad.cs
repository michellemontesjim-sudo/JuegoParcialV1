// RestoreOnSceneLoad.cs
using UnityEngine;

public class RestoreOnSceneLoad : MonoBehaviour
{
    void Start()
    {
        if (SceneState.lastBattleResult == true && SceneState.lastPlayerPosition != Vector3.zero)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
                player.RespawnAt(SceneState.lastPlayerPosition);
            // reset optional flags
            SceneState.lastBattleResult = null;
            SceneState.lastPlayerPosition = Vector3.zero;
        }
        else if (SceneState.lastBattleResult == false)
        {
            // should not happen because HandleBattleEndAndReturn direct-loaded GameOver
            SceneState.lastBattleResult = null;
        }
    }
}

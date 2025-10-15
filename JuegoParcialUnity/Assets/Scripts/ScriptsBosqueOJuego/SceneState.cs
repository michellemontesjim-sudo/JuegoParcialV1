// SceneState.cs
using UnityEngine;

public static class SceneState
{
    // position in world to restore after battle
    public static Vector3 lastPlayerPosition = Vector3.zero;
    public static string lastSceneName = "";
    public static int lastSceneBuildIndex = -1;

    // battle result (true = win, false = lose, null = none)
    public static bool? lastBattleResult = null;

    // optional: id of encounter to restore context (not used now)
    public static string encounterId = "";
}

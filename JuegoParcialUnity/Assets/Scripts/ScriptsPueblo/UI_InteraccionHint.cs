using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_InteraccionHint : MonoBehaviour
{
    public static UI_InteraccionHint Instance;
    public TMP_Text hintText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        hintText.gameObject.SetActive(false);
    }

    public void ShowHint(string message)
    {
        hintText.text = message;
        hintText.gameObject.SetActive(true);
    }

    public void HideHint()
    {
        hintText.gameObject.SetActive(false);
    }
}

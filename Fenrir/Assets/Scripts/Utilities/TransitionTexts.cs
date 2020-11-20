using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionTexts : MonoBehaviour
{
    public TransitionText[] texts;

    [System.Serializable]
    public class TransitionText
    {
        public string text;
        public string refernc;
    }

    public Text textUI;
    public Text refUI;

    private void OnEnable()
    {
        int r = Random.Range(0, texts.Length);
        textUI.text = texts[r].text;
        refUI.text = texts[r].refernc;
    }
}

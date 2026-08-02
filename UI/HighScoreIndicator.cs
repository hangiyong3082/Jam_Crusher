using TMPro;
using UnityEngine;

public class HighScoreIndicator : MonoBehaviour
{
    TMP_Text text;
    float bestScore;
    Color itsColor;
    bool isShown = false;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        bestScore = PlayerPrefs.GetFloat("BestScore", 0);
        itsColor = text.color;
        text.color = new Color(itsColor.r, itsColor.g, itsColor.b, 0);
    }

    private void Update()
    {
        if (!isShown && GameManager.Instance.score > bestScore)
        {
            text.color = new Color(itsColor.r, itsColor.g, itsColor.b, 1);
            isShown = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrownAchivUITargetScoreIndicatorGenerator : MonoBehaviour
{
    [SerializeField] QuestManger questManager;
    [SerializeField] Transform bg;
    [SerializeField] GameObject targetScoreIndicator;
    [SerializeField] int maxScore;


    private void Start()
    {
        foreach (Quest quest in questManager.quests)
        {
            //setting
            Rect bgRect = bg.GetComponent<RectTransform>().rect;
            float bgHeight = bgRect.height;
            float yPosDelta = -bgHeight / 2 + (((float)quest.target / (float)maxScore) * bgHeight);
            //generating
            GameObject generatedIndicator = Instantiate(targetScoreIndicator,transform);
            generatedIndicator.transform.localPosition = Vector3.up * yPosDelta;
            TMP_Text scoreText = generatedIndicator.transform.GetChild(0).GetComponent<TMP_Text>();
            scoreText.text = quest.target.ToString();
        }
    }
}

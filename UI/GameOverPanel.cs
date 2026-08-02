using DarkTonic.MasterAudio;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject scoreBoard;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] Button restartButton;
    [SerializeField] GameObject highlightEffect;

    string scoreBoardAnimID;

    private void Awake()
    {
        scoreBoardAnimID = scoreBoard.GetComponent<DOTweenAnimation>().id;
    }

    public void ButtonSetup()
    {
        restartButton.onClick.AddListener(delegate { GameManager.Instance.LoadMenu(); });
    }

    private void Start()
    {
        scoreBoard.SetActive(false);
    }

    private void OnEnable()
    {   
        StartCoroutine(ActiveScoreBoard());
    }

    IEnumerator ActiveScoreBoard()
    {
        yield return new WaitForSeconds(1);
        scoreText.text = "";
        scoreBoard.SetActive(true);
        restartButton.gameObject.SetActive(false);
        highlightEffect.SetActive(false);

        MasterAudio.PlaySound("GameOver_ShowScoreBoard");
        DOTween.Play(scoreBoardAnimID);

        yield return new WaitForSeconds(0.5f);
        scoreText.DOText($"{string.Format("{0:F1}", GameManager.Instance.score)}", 1, scrambleMode: ScrambleMode.Numerals).SetEase(Ease.Linear).SetId(scoreBoardAnimID);
        MasterAudio.PlaySound("GameOver_ShowScoreText");

        yield return new WaitForSeconds(1);
        restartButton.gameObject.SetActive(true);
        highlightEffect.SetActive(true);
        MasterAudio.PlaySound("GameOver_TypingScoreFinished");
    }
}

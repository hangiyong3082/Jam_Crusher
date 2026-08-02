using DarkTonic.MasterAudio;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("References")]
    [SerializeField] public GameObject gamePanel;
    [SerializeField] public GameObject gameoverPanel, menuPanel, pausePanel, buyConfirmationPanel;
    [SerializeField] public GameObject bombBoxHighlightUI, timeBar;
    [SerializeField] public GameObject coinUI, collectedCoin_p, crownAchievementUI; //p : prefab
    [SerializeField] public TMP_Text scoreText, nextSpeedUpScoreText, startFrom200CostText;
    [SerializeField] public Button pauseButton, settingButton, tutorialSkipButton, menuTutorialButton, startButton, startFrom200Button;

    TimeBar timeBar_s;
    GameManager gameManager;

    private void Awake()
    {
        timeBar_s = timeBar.GetComponent<TimeBar>();
        gameManager = GameManager.Instance;
        ButtonSetup();
    }

    void ButtonSetup()
    {
        pauseButton.onClick.AddListener( delegate { 
            gameManager.PauseGame(); 
            settingButton.gameObject.SetActive(true);
            pausePanel.SetActive(true);

            pauseButton.gameObject.SetActive(false);
        });
        startButton.onClick.AddListener(() => gameManager.StartGame());
        //startFrom200Button.onClick.AddListener(() => GameManager.Instance.StartFrom200());
        pausePanel.GetComponent<PausePanel>().ButtonSetup();
        menuPanel.GetComponent<MenuPanel>().ButtonSetup();
        gameoverPanel.GetComponent<GameOverPanel>().ButtonSetup();
    }

    //GameManager GM = GameManager.Instance;

    private void Start()
    {
        //Panel
        ActivePanel(new GameObject[] { menuPanel });
        //Button
        ActiveSettingOrPauseBtn(settingButton); ;
        //Language
        ChangeLocale(PlayerPrefs.GetInt("Language", 0));
        //Text
        TextSetup();

        UpdateNSST();
        
    }

    void TextSetup()
    {
        startFrom200CostText.text = gameManager.startFrom200Cost.ToString();
    }

    #region MGUI
    public void MGUI()
    {
        MGUI_AvailableTileSpnList();
        MGUI_AddCoin();
        MGUI_ResetCoin();
        MGUI_ResetAll();
    }
    void MGUI_AvailableTileSpnList()
    {
        if (GUI.Button(new Rect(10, 10, 300, 100), "AvailableTileSpnList.Instance.CheckList();"))
        {
            AvailableTileSpnList.Instance.CheckList();
        }
    }
    void MGUI_AddCoin()
    {
        if (GUI.Button(new Rect(300, 10, 100, 100),"Add Coins"))
        {
            CoinManager.Instance.AddCoins(100);
        }
    }
    void MGUI_ResetCoin()
    {
        if (GUI.Button(new Rect(400, 10, 100, 100), "Reset Coin"))
        {
            PlayerPrefs.SetInt("Coins", 0);
        }
    }
    void MGUI_ResetAll()
    {
        if (GUI.Button(new Rect(500, 10, 100, 100), "Reset All"))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadSceneAsync(0);
            //GameManager.Instance.AddCoins(100);
        }
    }
    #endregion

    private void Update()
    {
        if (gameManager.State == GameState.Playing)
        {
            scoreText.text = $"{string.Format("{0:F1}", gameManager.score)}";
            UpdateNSST();
            timeBar_s.Update_();

            pauseButton.enabled = !DOTween.IsTweening("GameUI");
            
        }
        SetBombBoxHightlight(); //setactive 스스로 함
        //language
        //ChangeLocale(PlayerPrefs.GetInt("Language", 0));
    }
    /// <summary> 게임 시작할 때 실행 </summary>
    public void WhenGameStart()
    {
        //Panel
        ActivePanel(new GameObject[] { gamePanel});
        //timeBar
        timeBar_s.StartWorking();
        //Button
        ActiveSettingOrPauseBtn(pauseButton);

    }
    /// <summary>게임 오버일 때 실행</summary>
    public void WhenGameOver()
    {
        ActivePanel(new GameObject[] { gameoverPanel });

        ActiveSettingOrPauseBtn();
    }

    void ActivePanel(GameObject[] gameObjects)
    {
        gamePanel.SetActive(false);
        //gamePanel3d.SetActive(false);
        gameoverPanel.SetActive(false);
        //menuPanel.SetActive(false);
        pausePanel.SetActive(false);

        foreach (GameObject obj in gameObjects)
        {
            obj.SetActive(true);
        }
    }
    
    void ActiveSettingOrPauseBtn(Button button = null)
    {
        settingButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);

        if (button != null)
        {
            button.gameObject.SetActive(true);
        }
        
    }
    
    public void NextTurn()
    {
        timeBar_s.NextTrun();
    }

    public void AddCoinsUIEffect(Vector3 worldObjectsPos, int amount)
    {
        StartCoroutine(IAddCoinsUIEffect(worldObjectsPos, amount));
    }

    public IEnumerator IAddCoinsUIEffect(Vector3 worldObjectsPos, int amount)
    {
        ParticleSystem.MainModule collectedCoin_p_PS = gameManager.coinParticle.GetComponent<ParticleSystem>().main;

        yield return new WaitForSeconds(collectedCoin_p_PS.startLifetime.constant/collectedCoin_p_PS.simulationSpeed);

        float moveTime = 1f;
        for (int i = 0; i < amount; i++)
        {
            Vector3 startPos = Camera.main.WorldToScreenPoint(worldObjectsPos);
            GameObject coin = Instantiate(collectedCoin_p, startPos, Quaternion.identity, gamePanel.transform);

            //coin.GetComponent<Material>().DOFade(0f, 0f);
            // coin.GetComponent<Material>().DOFade(1f, 0f).SetDelay(3f);
            
            coin.transform.DOMove(coinUI.transform.position, moveTime).SetEase(Ease.InBack).SetDelay(i*0.1f);
            Destroy(coin, moveTime);
        }
        yield return new WaitForSeconds(moveTime);
        MasterAudio.PlaySound("Game_CollectCoins");
    }

    /// <summary>폭탄 박스 ui 강조 표시를 키거나 끄기</summary>
    void SetBombBoxHightlight()
    {
        bombBoxHighlightUI.SetActive(false);
        if (gameManager.State != GameState.Playing)
        {
            return;
        }
        if (!gameManager.placementMode && gameManager.bombBoxItemCount > 0)
        {
            bombBoxHighlightUI.SetActive(true);
        }
        
    }

    public void ClickSound()
    {
        MasterAudio.PlaySound("UI_ButtonClick");
    }

    public void ChangeLocale(int indexInParent)
    {
        StartCoroutine(SetLocale(indexInParent));
        PlayerPrefs.SetInt("Language", indexInParent);
        
    }
    IEnumerator SetLocale(int localID)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localID];
    }

    //NSST : next speedup score text
    void UpdateNSST()
    {
        if (gameManager.speedUpScoreList.Count == 0 && nextSpeedUpScoreText.text == "")
        {
            return;
        }

        if (DOTween.IsTweening("SpeedUpTextAnim")) nextSpeedUpScoreText.text = "";
        else if (gameManager.speedUpScoreList.Count > 0) nextSpeedUpScoreText.text = $"/{gameManager.speedUpScoreList[0]}";

        if (gameManager.speedUpScoreList.Count == 0)
        {
            nextSpeedUpScoreText.text = "";
        }
    }



}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DarkTonic.MasterAudio;
using DG.Tweening;
using UnityEngine.SceneManagement;

public enum GameState
{
    Menu,
    Intro,
    Playing,
    Pause,
    GameOver
}
public class GameManager : Singleton<GameManager>
{
    public GameState State { get; private set; }
    //tile
    [HideInInspector] public readonly int tileCount = 5;
    [HideInInspector] public readonly int tileScale = 2;

    //score
    public float score = 0;
    public float scorePerSec { get; private set; }
    //gameSpeed
    public int difficulty { get; private set; }
    int carForceSpawnDiffLimit = 1;
    public List<float> speedUpScoreList { get; private set; } = new List<float>() { 15, 50, 100, 150, 300, 500 };
    //이하는 speedUpScoreList보다 요소가 한 개 많아야함. (0점부터 시작)
    List<float> turnTimesByScore = new List<float>() { 2.7f, 2.3f, 2f, 1.8f, 1.6f, 1.4f, 1.2f };
    List<int> spawncarCountsByScore = new List<int>() { 1, 1, 2, 2, 2, 2, 2 };
    public Dictionary<string, List<int>> spawnOnTileByScore = new Dictionary<string, List<int>>()
    {
        //맵에 올라오는 장애물 최대 개수
        {"Tree",new List<int>() { 0, 0, 1, 1, 2, 2, 2 } },
        {"PlayerBannedSign",new List<int>() { 0, 0,1, 1, 2, 2, 2 } },

    };
    List<SpawnerOnTile> spawnersOnTile = new List<SpawnerOnTile>();
    //tutorial
    public bool isTutorial;
    public bool spawnObstacles;
    public bool spawnItem;


    //[HideInInspector] public List<int> spawntreeCountsByScore { get; private set; } = new List<int>() { 5, 1, 1, 2, 2, 2, 2 };

    //ui
    [SerializeField] ShopController shopController;

    //player
    [SerializeField] GameObject playerPrefab;
    public bool isMoveable { get; set; }
    public int remainingMoveCountInit { get; private set; }
    [SerializeField] GameObject moveButtonGroup;

    //camera
    [SerializeField] public Animator stateDrivenCamera;

    //map
    [SerializeField] GameObject road;

    //item
    [HideInInspector] public int bombBoxItemCount;
    [HideInInspector] public bool placementMode;
    [HideInInspector] public int maxbBItemCount { get; private set; }

    //coin
    [SerializeField] public GameObject coinParticle;

    //issue
    public bool rTheCarsWaiting;
    public bool isVibrate { get; set; }
    public bool isPCControllAvailable { get; set; } = false;

    //time
    public float turnTimeInit { get; private set; }
    public float turnTime
    {
        get;
        private set;
        
    }
    public int turnCount { get; private set; }
    [HideInInspector] public float initPlacementTime { get; private set; }

    //spawn car
    bool startGameSpawnCar = false;

    //startFrom200
    public int startFrom200Cost { get; set; } = 50;

    //achievement
    [SerializeField] public QuestController achieveController;

    PlayerController playerController;
    TutorialManager tutorialManager;
    AudioManager audioManager;

    public void GameSetting()
    {
        Application.targetFrameRate = 180;
        //score
        scorePerSec = 1.2f;
        //time
        turnTimeInit = turnTimesByScore[0];
        initPlacementTime = 5f;
        //item
        bombBoxItemCount = 1;
        maxbBItemCount = 3;
        //car
        rTheCarsWaiting = true;
        //player
        isMoveable = true;
        remainingMoveCountInit = 99; //99 : infinite
        //tutorial
        isTutorial = false;
        spawnObstacles = true;
        spawnItem = true;
        
    }

    public void StartFrom200()
    {
        //turnTimesByScore = new List<float>() { 1.5f,1.4f,1.3f,1.2f,1.1f,1.05f, 1f };

        //int coins = PlayerPrefs.GetInt("Coins", 0);
        //if (coins >= startFrom200Cost)
        //{
        //    PlayerPrefs.SetInt("Coins", coins - startFrom200Cost);
        //    score = 200;
        //    StartGame();       
        //}
        score = 200;
        PlayerPrefs.SetInt("Pass200AndDidntQuickStartYet", 1);
        StartGame();    

        //spawncarCountsByScore = new List<int>() { 2,2, 3, 3, 4, 4, 4 };
        //GameSetting();

    }

    private void Awake()
    {
        GameSetting();
        tutorialManager = FindObjectOfType<TutorialManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void Start()
    {
        State = GameState.Menu;
        Time.timeScale = 1f;

        //ui
        //timeBar.SetActive(false);

        //transparency
        foreach (var obj in FindObjectsByType<SetTransparency>(0))
        {
            obj.GetComponent<SetTransparency>().OnMenu();
        }
        //player
        GameObject p = Instantiate(playerPrefab);
        p.GetComponent<PlayerSpawn>().SpawnPlayer();
        playerController = p.GetComponent<PlayerController>();
        shopController.SetSkin();
        //quest
        achieveController.SetProgress(QuestType.score, (int)GetBestScore());
        //spawner
        foreach (var spawners in FindObjectsByType<SpawnerOnTile>(0))
            spawnersOnTile.Add(spawners);

        
    }

    public void DisableVibrate()
    {
        isVibrate = false;
    }

    private void Update()
    { 
        if (State == GameState.Playing)
        {
            Game();
        }
    }
    
    public void StartGame()
    {
        #region Setting
        State = GameState.Playing;
        //ui
        UIManager.Instance.WhenGameStart();
        Destroy(UIManager.Instance.menuPanel);
        //timeBar_s.StartWorking();
        DOTween.Play("GameUI");
        //time
        turnTime = 1f; //For fast start
        //trancparency
        foreach (var obj in FindObjectsByType<SetTransparency>(0))
        {
            obj.GetComponent<SetTransparency>().OnGame();
        }
        //tutorial
        if (isTutorial)
        {
            spawnObstacles = false;
            spawnItem = false;
        }
        //sound
        MasterAudio.PlaySound("Game_Start");
            
            

        AddScore(0);
        MasterAudio.StopPlaylist();
        #endregion
    }

    public void PauseGame()
    {
        State = GameState.Pause;
        Time.timeScale = 0f;
        tutorialManager.tutorialPanel.SetActive(false);
    }

    public void ResumeGame()
    {
        State = GameState.Playing;
        Time.timeScale = 1f;
        tutorialManager.tutorialPanel.SetActive(true);
    }

    public void Game()
    {
        //movebutton ui
        //bool isMoveButtonAvailable = (!placementMode && playerController.remainingMoveCount > 0) ? true : false;
        //moveButtonGroup.GetComponent<ToggleMoveButtons>().Work(true); //원래는 위에 있는 isMoveButtonAvailable 넣어야함

        //time
        if (!placementMode)
        {
            turnTime -= Time.deltaTime;
            if (!isTutorial)
            {
                AddScore(Time.deltaTime * scorePerSec);

            }
        }

        //Difficulty
        float textWaitingTime = 0;
        if (turnTime <= 0)
        {       
            textWaitingTime = 0.2f;
            ProceedRound();
        }

        //next turn? *************************************************************
        SetDifficulty(textWaitingTime);
        //if (turnTime > 0 || placementMode)
        //{
        //SetDifficulty(textWaitingTime);
        //    return;
        //}
        

        
        
    }

    void ProceedRound()
    {
        //audio
        if (FindAnyObjectByType<Car>() == null)
        {
            MasterAudio.PlaySound("Game_NextQuarter");
        }

        #region Work Systems
        //remainingPlayerMoveCount
        playerController.remainingMoveCount = remainingMoveCountInit;
        var rmcScript = GameObject.FindWithTag("Player").GetComponentInChildren<RemainingMoveCountText>();
        rmcScript.InitCountAnim();
        //Car
        foreach (GameObject car in GameObject.FindGameObjectsWithTag("Car"))
            car.GetComponent<Car>().CarWork();
        //CarSpawnDelayer
        foreach (GameObject cs in GameObject.FindGameObjectsWithTag("CarSpawnDelayer"))
            cs.GetComponent<CarSpawnDelayer>().CarSpawnDelayerWork();
        //CarSpawner
        CarSpawner carSpawner = GameObject.Find("CarSpawner").GetComponent<CarSpawner>();
        for (int i = 0; i < Random.Range(1, spawncarCountsByScore[0] + 1); i++)
        {
            if (difficulty <= carForceSpawnDiffLimit)
            {
                carSpawner.SpawnCar(100);
            }
            else
            {
                carSpawner.SpawnCar();
            }

        }
        foreach (int i in carSpawner.diagonalSpn)
        {
            carSpawner.ReturnSpawnPointNum(i);
        }
        carSpawner.diagonalSpn.Clear();


        //DetectCarJamManager
        DetectCarJamManager.Instance.DetectCarJam();
        //ItemManager
        if (spawnItem)
        {
            ItemManager.Instance.SpawnItem();
        }

        //Bomb Box
        foreach (GameObject bombBox in GameObject.FindGameObjectsWithTag("BombBox"))
            bombBox.GetComponent<BombBox>().Work();
        //Obstacle
        foreach (var obs in FindObjectsByType<Obstacle>(0))
            obs.Work();
        //SpawnerOnTile (Obstacle 보다 아래) 
        if (spawnObstacles)
        {
            //키 리스트 설정
            List<string> keys = new List<string>();
            foreach (var key in spawnOnTileByScore.Keys)
                keys.Add(key);
            //랜덤 키로 접근해서 장애물 스폰
            string randomKey = keys[Random.Range(0, keys.Count)];
            foreach (var spawner in spawnersOnTile)
                if (spawner.prefabsTag == randomKey)
                    spawner.Spawn(!isTutorial ? spawnOnTileByScore[randomKey][0] : 1
                        , tutorialManager.obstacleForcedSpawn);
        }
        //Tutorial
        tutorialManager.CarMission(playerController);
        tutorialManager.ObstacleMission();

        #endregion

        //차가 게임지역에 있을때만 rtcw false로 바꿈
        foreach (GameObject car in GameObject.FindGameObjectsWithTag("Car"))
        {
            if (car.GetComponent<Car>().isOnRoad)
            {
                //rtcw
                rTheCarsWaiting = false;
                break;
            }
        }
        //SetDifficulty(textWaitingTime);

        //UI
        UIManager.Instance.NextTurn();

        turnTime = turnTimeInit;
        turnCount++;
    }

    public void GameOver()
    {
        State = GameState.GameOver;

        UIManager.Instance.WhenGameOver();
        BombBoxManager.Instance.DisableOA();

        SetBestScore();
        achieveController.SetProgress(QuestType.score, (int)GetBestScore());

        Vibrate();
    }

    public void SetBestScore()
    {
        float bestScore = PlayerPrefs.GetFloat("BestScore", 0);
        if (score > bestScore)
        {
            PlayerPrefs.SetFloat("BestScore", score);

        }
    }

    public float GetBestScore()
    {
        return PlayerPrefs.GetFloat("BestScore", 0);

    }

    public void LoadMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    /// <summary>
    /// 점수를 추가함.
    /// </summary>
    /// <param name="addScore">값</param>
    public void AddScore(float addScore)
    {
        score += addScore;
    }

    /// <summary>
    /// 모바일 기기를 진동함
    /// </summary>
    public void Vibrate()
    {
        if (isVibrate) Handheld.Vibrate();
    }

    void SetDifficulty(float textWaitingTime_m)
    {
        bool IsScoreExceedspeedUpScore()
        {
            return speedUpScoreList.Count > 0 && score >= speedUpScoreList[0];
        }
        if (IsScoreExceedspeedUpScore())
        {
            turnTimeInit = turnTimesByScore[0];

            difficulty++;
            speedUpScoreList.RemoveAt(0);
            turnTimesByScore.RemoveAt(0);
            spawncarCountsByScore.RemoveAt(0);
            foreach (var spawners in FindObjectsByType<SpawnerOnTile>(0))
                spawnOnTileByScore[spawners.prefabsTag].RemoveAt(0);

            #if DEBUG_TURNTIME          
            print($"GameManager!!! {turnTime} -> {textWaitingTime_m}");
            #endif
            StartCoroutine(SpeedUpTextWork(textWaitingTime_m));
        }
    }

    IEnumerator SpeedUpTextWork(float textWaitingTime_m)
    {
        var speedUpTextScript = GameObject.Find("SpeedUpText").GetComponent<SpeedUpTextAnim>();
        
        yield return new WaitForSeconds(0);
        speedUpTextScript.Work();
        MasterAudio.PlaySound("Game_SpeedUp");

        
    }
}

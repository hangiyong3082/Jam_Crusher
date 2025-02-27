using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DarkTonic.MasterAudio;
using DG.Tweening;
using Unity.VisualScripting;
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
    //float[] speedUpScoreList = new float[6] {15,50,100,150,300,500};
    public List<float> speedUpScoreList { get; private set; } = new List<float>() { 15, 50, 100, 150, 300, 500 };
    //이하는 speedUpScoreList보다 요소가 한 개 많아야함. (0점부터 시작)
    List<float> turnTimesByScore = new List<float>() { 2.2f, 2f, 1.8f, 1.6f, 1.4f, 1.2f, 1f };
    List<int> spawncarCountsByScore = new List<int>() { 1, 2, 2, 3, 4, 4, 4 };
    public Dictionary<string, List<int>> spawnOnTileByScore = new Dictionary<string, List<int>>()
    {
        {"Tree",new List<int>() { 1, 1, 1, 2, 2, 2, 2 } },
        {"PlayerBannedSign",new List<int>() { 1, 1, 1, 2, 2, 2, 2 } },

    };


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

    //issue
    public bool rTheCarsWaiting;
    public bool isVibrate { get; set; }

    //time
    public float turnTimeInit { get; private set; }
    public float turnTime { get; private set; }
    [HideInInspector] public float initPlacementTime { get; private set; }

    //spawn car
    bool startGameSpawnCar = false;

    public void GameSetting()
    {
        Application.targetFrameRate = 180;
        //score
        scorePerSec = 0.7f;
        //time
        turnTimeInit = turnTimesByScore[0];
        initPlacementTime = 5f;
        //item
        bombBoxItemCount = 2;
        maxbBItemCount = 5;
        //car
        rTheCarsWaiting = true;
        //player
        isMoveable = true;
        remainingMoveCountInit = 2;
    }

    private void Awake()
    {
        GameSetting();
    }

    private void Start()
    {
        State = GameState.Menu;
        isVibrate = true;

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
        shopController.SetSkin();
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
        //timeBar_s.StartWorking();
        DOTween.Play("GameUI");
        //time
        turnTime = 1f; //For fast start
        //trancparency
        foreach (var obj in FindObjectsByType<SetTransparency>(0))
        {
            obj.GetComponent<SetTransparency>().OnGame();
        }

        AddScore(0);
        MasterAudio.StopPlaylist();
        #endregion
    }

    public void PauseGame()
    {
        State = GameState.Pause;
    }

    public void ResumeGame()
    {
        State = GameState.Playing;
    }

    public void Game()
    {       
        //get player controller
        var pcScript = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        //movebutton ui
        bool isMoveButtonAvailable = (!placementMode && pcScript.remainingMoveCount > 0) ? true : false;
        moveButtonGroup.GetComponent<ToggleMoveButtons>().Work(isMoveButtonAvailable);

        //time
        if (!placementMode)
        {
            turnTime -= Time.deltaTime;
            AddScore(Time.deltaTime*scorePerSec);
        }
        //Difficulty
        float textWaitingTime = 0;
        if (turnTime <= 0)
        {       
            textWaitingTime = 0.2f;
        }

        //next turn? *************************************************************
        if (turnTime > 0 || placementMode)
        {
            SetDifficulty(textWaitingTime);
            return;
        }    

        #region Work Systems
        //remainingPlayerMoveCount
        pcScript.remainingMoveCount = remainingMoveCountInit;
        var rmcScript = GameObject.FindWithTag("Player").GetComponentInChildren<RemainingMoveCountText>();
        rmcScript.InitCountAnim();
        //Car
        foreach (GameObject car in GameObject.FindGameObjectsWithTag("Car"))
            car.GetComponent<Car>().CarMove();
        //CarSpawnDelayer
        foreach (GameObject cs in GameObject.FindGameObjectsWithTag("CarSpawnDelayer"))
            cs.GetComponent<CarSpawnDelayer>().CarSpawnDelayerWork();
        //CarSpawner
        CarSpawner carSpawner = GameObject.Find("CarSpawner").GetComponent<CarSpawner>();
        for (int i = 0; i < spawncarCountsByScore[0]; i++)
        {        
            carSpawner.SpawnCar();      
        }
        foreach (int i in carSpawner.diagonalSpn)
        {
            carSpawner.ReturnSpawnPointNum(i);         
        }
        carSpawner.diagonalSpn.Clear();


        //DetectCarJamManager
        DetectCarJamManager.Instance.DetectCarJam();
        //ItemManager
        ItemManager.Instance.SpawnItem();

        //Bomb Box
        foreach (GameObject bombBox in GameObject.FindGameObjectsWithTag("BombBox"))
            bombBox.GetComponent<BombBox>().Work();
        //Obstacle
        foreach (var obs in FindObjectsByType<Obstacle>(0))
            obs.Work();
        //SpawnerOnTile (Obstacle 보다 아래) 
        List<string> keys = new List<string>();
        foreach (var key in spawnOnTileByScore.Keys)
            keys.Add(key);
        string randomKeey = keys[Random.Range(0,keys.Count)];
        foreach (var spawners in FindObjectsByType<SpawnerOnTile>(0))
            if (spawners.prefabsTag == randomKeey) spawners.Spawn(spawnOnTileByScore[spawners.prefabsTag][0]);

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
        SetDifficulty(textWaitingTime);

        //UI
        UIManager.Instance.NextTurn();

        turnTime = turnTimeInit;

        MasterAudio.PlaySound("Game_NextQuarter");
    }

    public void GameOver()
    {
        State = GameState.GameOver;

        UIManager.Instance.WhenGameOver();
        BombBoxManager.Instance.DisableOA();

        Vibrate();
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
        //if (isVibrate) Handheld.Vibrate();
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

            speedUpScoreList.RemoveAt(0);
            turnTimesByScore.RemoveAt(0);
            spawncarCountsByScore.RemoveAt(0);
            foreach (var spawners in FindObjectsByType<SpawnerOnTile>(0))
                spawnOnTileByScore[spawners.prefabsTag].RemoveAt(0);


            print($"GameManager!!! {turnTime} -> {textWaitingTime_m}");
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

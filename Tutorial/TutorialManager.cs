using DarkTonic.MasterAudio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Steps
{
    timer = 1,
    timer2,
    timer3,
    carMission,
    box,
    waitingBox,
    bombItemMission,
    bombMission,
    obstacleMission,
    breakObstacleMission,
    finish
}

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] public GameObject tutorialPanel;
    Image tutorialPanelImg;
    GameObject[] sequences;
    //spawner
    List<SpawnerOnTile> spawnersOnTile = new List<SpawnerOnTile>();
    //check
    public Steps currentStep = 0;
    bool box1Runned = false;
    //misson
    int carMissionTarget = 10; //10
    int carMissionProgress;
    int bombMissionTarget = 3; //5
    int _bombMissionProgress;
    int bombMissionProgress
    {
        get
        {
            if (_bombMissionProgress > bombMissionTarget) return bombMissionTarget;
            else return _bombMissionProgress;
        }
        set { _bombMissionProgress = value; }
    }
    int obstacleMissionTarget = 10; //10
    int obstacleMissionProgress; 
    int breakObsMissionTarget = 2; //2
    int _breakObsMissionProgress;
    int breakObsMissionProgress
    {
        get
        {
            if (_breakObsMissionProgress > breakObsMissionTarget) return breakObsMissionTarget;
            else return _breakObsMissionProgress;
        }
        set { _breakObsMissionProgress = value; }
    }
    public bool obstacleForcedSpawn { get; private set; } = false;
    [SerializeField] TMP_Text carMissionText;
    [SerializeField] TMP_Text bombMissionText;
    [SerializeField] TMP_Text obstacleMissionText;
    [SerializeField] TMP_Text breakObstacleMissionText;
    

    [SerializeField] GameObject bombBoxButtonGroup;

    GameManager gameManager;
    
    public bool placedBox;

    public float requiredScoreToSkipTutorial { get; private set; } = 50f;


    private void Awake()
    {
        gameManager = GameManager.Instance;
        tutorialPanelImg = tutorialPanel.GetComponent<Image>();
    }
    
    private void Start()
    {
        sequences = GetChildren(tutorialPanel);
        foreach (var sequence in sequences)
        {
            sequence.SetActive(false);
        }

        placedBox = false;
        tutorialPanelImg.raycastTarget = false;

        if (gameManager.GetBestScore() >= requiredScoreToSkipTutorial) SetTutorialDone();
        UIManager.Instance.menuTutorialButton.gameObject.SetActive(!IsTutorialDone());
        foreach (var spawner in FindObjectsByType<SpawnerOnTile>(0))
            spawnersOnTile.Add(spawner);
    }

    public void SetTutorialDone()
    {
        PlayerPrefs.SetInt("IsTutorialDone", 1);
    }

    public bool IsTutorialDone()
    {
        return PlayerPrefs.GetInt("IsTutorialDone", 0) == 1 ? true : false;
    }

    public void SkipTutorial()
    {
        gameManager.LoadMenu();
    }

    public void StartTutorial()
    {
        gameManager.isTutorial = true;
        gameManager.StartGame();
        if (FindObjectOfType<SettingUI>() != null)FindObjectOfType<SettingUI>().transform.parent.gameObject.SetActive(false);
        UIManager.Instance.pauseButton.gameObject.SetActive(false);
        UIManager.Instance.tutorialSkipButton.gameObject.SetActive(true);

        var playerController = FindObjectOfType<PlayerController>();
        //playerController.transform.GetComponent<BoxCollider>().enabled = false;
    }

    public GameObject[] GetChildren(GameObject parent)
    {
        GameObject[] children = new GameObject[parent.transform.childCount];

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            children[i] = parent.transform.GetChild(i).gameObject;
        }

        return children;
    }

    private void Update()
    {
        if (!gameManager.isTutorial) return;

        var playerController = FindObjectOfType<PlayerController>();

        if (currentStep == 0)
        {
            bombBoxButtonGroup.SetActive(false);
        }
        if (currentStep == 0 && gameManager.turnCount == 1)
        {
            GoNextStep();
        }
        if (currentStep == Steps.carMission && carMissionProgress >= carMissionTarget)
        {
            GoNextStep(true);
        }
        switch (currentStep)
        {
            case Steps.timer:
                Time.timeScale = 0;
                tutorialPanelImg.raycastTarget = true;
                break;
            case Steps.carMission:
                Time.timeScale = 1;
                tutorialPanelImg.raycastTarget = false;
                carMissionText.text = $"{carMissionProgress}/{carMissionTarget}";
                break;
            case Steps.box:
                StartCoroutine(Box1());
                break;
            case Steps.waitingBox:
                Time.timeScale = 1;
                gameManager.isMoveable = true;
                gameManager.spawnItem = true;
                break;
            case Steps.bombItemMission:  
                break;
            case Steps.bombMission:
                bombMissionText.text = $"{bombMissionProgress}/{bombMissionTarget}";
                ItemManager.Instance.spawnProbability = 20;
                break;
            case Steps.obstacleMission:
                gameManager.spawnObstacles = true;
                obstacleMissionText.text = $"{obstacleMissionProgress}/{obstacleMissionTarget}";
                obstacleForcedSpawn = true;
                
                break;
            case Steps.breakObstacleMission:
                breakObstacleMissionText.text = $"{breakObsMissionProgress}/{breakObsMissionTarget}";
                
                break;
            case Steps.finish:
                SetTutorialDone();
                Time.timeScale = 0;
                //보상 추가
                break;
        }
    }

    public void CarMission(PlayerController playerController)
    {
        if (currentStep != Steps.carMission) return;

        bool isDead = false;
        Collider[] colliders = Physics.OverlapBox(playerController.gameObject.transform.position, Vector3.one);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Car"))
            {
                isDead = true;
            }
        }
        if (!isDead)
        {
            carMissionProgress++;
            PublicFunctions.UIEffect(carMissionText.gameObject, this, "carMission", UIAnim.ScaleHighlight);
        }
    }

    IEnumerator Box1()
    {
        if (!box1Runned)
        {
            box1Runned = true;
            yield return new WaitForSeconds(0.5f);
            Time.timeScale = 0;
            gameManager.isMoveable = false;
            bombBoxButtonGroup.SetActive(true);
        }    
    }

    public void BombItemMission()
    {
        if (currentStep != Steps.bombItemMission) return;
        GoNextStep(true);

    }
    
    public void BombMission(int carCount)
    {
        if (currentStep != Steps.bombMission) return;
        bombMissionProgress += carCount;
        PublicFunctions.UIEffect(bombMissionText.gameObject, this, "bombMission", UIAnim.ScaleHighlight);

        if (bombMissionProgress >= bombMissionTarget)
        {
            StartCoroutine(GoNextStepWDelay(true));
        }

    }

    public void ObstacleMission()
    {
        if (currentStep != Steps.obstacleMission) return;
        obstacleMissionProgress++;
        PublicFunctions.UIEffect(obstacleMissionText.gameObject, this, "obstacleMission", UIAnim.ScaleHighlight);

        if (obstacleMissionProgress >= obstacleMissionTarget) GoNextStep(true);

    }

    public void BreakObsMission(int obsCount)
    {
        if (currentStep != Steps.breakObstacleMission) return;
        breakObsMissionProgress += obsCount;
        PublicFunctions.UIEffect(breakObstacleMissionText.gameObject, this, "obstacleMission", UIAnim.ScaleHighlight);

        if (breakObsMissionProgress >= breakObsMissionTarget) StartCoroutine(GoNextStepWDelay(true));

    }

    public void PlayerDead(PlayerController playerController)
    {
        //respawn
        Vector3 randomSpawnPos = PublicFunctions.RandomPosition();
        playerController.gameObject.transform.position =
            new Vector3(randomSpawnPos.x,FindObjectOfType<PlayerSpawn>().spawnPosY,randomSpawnPos.z);
        //misson
        carMissionProgress = 0;
        obstacleMissionProgress = obstacleMissionProgress != -1 ? 0 : -1;
        PublicFunctions.UIEffect(carMissionText.gameObject, this, "carMission", UIAnim.CancelHighlightWShake);
        PublicFunctions.UIEffect(obstacleMissionText.gameObject, this, "obstacleMission", UIAnim.CancelHighlightWShake);
    }

    public void GoNextStep(bool completionSound = false)
    {
        currentStep++;

        if ((int)currentStep > 1) sequences[(int)currentStep - 2].SetActive(false);
        sequences[(int)currentStep - 1].SetActive(true);

        if (completionSound)
        {
            MasterAudio.PlaySound("Game_MissionCompleted");

        }

        //if (((int)currentStep) >= System.Enum.GetValues(typeof(Steps)).Length)
        //{
        //    GameManager.Instance.LoadMenu();
        //}
    }

    public IEnumerator GoNextStepWDelay(bool completionSound = false)
    {
        yield return new WaitForSeconds(0.5f);
        GoNextStep(completionSound);
    }
}

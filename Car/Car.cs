using DarkTonic.MasterAudio;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Car : MonoBehaviour //CarSpawnDelayer.cs에서 생성됨
{
    public int spn;

    [Header("child 0~2는 건들지 말 것.")]
    //manager
    [Header("child 0")] public CarSetup carSetup;

    //pos
    [Header("child 1")]  public GameObject nextPosObj;
    Vector3 nextPos, prevPos;

    //effect
    [Header("child 2")] public GameObject dangerAreaObj;

    //skin
    GameObject currentModel = null;
    Transform Models;
    ThemeManager themeManager;
    [SerializeField] CarSkinManager carSkinManager;

    //count
    [SerializeField] public int carLength;
    public int moveCount;
    /// <summary>
    /// 스폰킬 방지를 위해 쓰임
    /// </summary>
    public int turnCountAfterSpawn;

    //move
    bool isMoving = false, reachedNextPos;
    
    //issue
    public bool isBrake, isOnRoad;
    int keepBrakeCount = 0;
    int keepBrakeCountMax = 5;
    GameObject carSpawnDeathPreventer;
    public int forCheckAllCarsFinishMove;// { get; private set; } //Update에서 보면 알음. 1일때만 실행

    //darkeness
    Color originColor;
    Color darkenColor = new Color(0.4f,0.4f,0.4f,1);
    [ReadOnly] public string frontObjName;// { get; private set; }
    public bool _darken;// { get; private set; }

    private void Awake()
    {
        carSetup = transform.GetChild(0).GetComponent<CarSetup>();
        nextPosObj = transform.GetChild(1).gameObject;
        dangerAreaObj = transform.GetChild(2).gameObject;
        reachedNextPos = false;
        isOnRoad = false;

        themeManager = FindObjectOfType<ThemeManager>();

        Models = transform.Find("Models_");
    }

    private void Start()
    {
        CarManager.Instance.AddSpn(spn);
        SetSkin();
        SpawnAnim();
        originColor = GetColor();
        dangerAreaObj.SetActive(true);
        prevPos = transform.position;
        
    }

    void Update()
    {
        Debug.DrawRay(transform.position, (nextPosObj.transform.position - transform.position) * 2, Color.red);
        if (isMoving && DOTween.IsTweening("MoveCar") == false)
        {
            WhenReachNextPos();
        }
        if (DOTween.IsTweening("MoveCar") == false)
        {
            GameManager.Instance.rTheCarsWaiting = true;
        }

        if (carSpawnDeathPreventer != null)
        {
            carSpawnDeathPreventer.transform.position = nextPosObj.transform.position;
        }

        SetDangerArea();

        #region 차 색깔 흐리게
        if (!isOnRoad) 
            foreach (Collider collider in GetForwardObjects())
        {

            if (collider.transform.GetComponent<BombBox>()) frontObjName = "BombBox";
            else if (collider.transform.GetComponent<Obstacle>()) frontObjName = "Obstacle";
            else frontObjName = "Another";

            if (
                !dangerAreaObj.activeSelf &&
                frontObjName == "Another"
                )
            {
                SetDarkeness(true);
            }
            else
            {
                SetDarkeness(false);

            }

            if (frontObjName != "Another") break;
        }
        #endregion

        #region 모든 차가 움직임 끝냈을 때 한 번 실행
        if (!DOTween.IsTweening("MoveCar"))
        {
            forCheckAllCarsFinishMove = Mathf.Min(++forCheckAllCarsFinishMove, 2);
        }
        else
        {
            forCheckAllCarsFinishMove = 0;
        }
        if (forCheckAllCarsFinishMove == 1) //모든 차가 움직임 끝냈을 때 한 번 실행
        {
            
        }      
        #endregion

        /*int numOfChild = this.transform.childCount;
        for (int i = 1; i <= numOfChild - 1; i++) //0번 자식 오브젝트는 NextPos이기 때문에 1부터 시작
            transform.GetChild(i).GetComponent<CarTrasnparent>().CarTransparent();*/
    }

    public void CarWork()
    {
        CarMove();
        forCheckAllCarsFinishMove = 0;
    }

    void CarMove()
    {
        turnCountAfterSpawn++;
        isBrake = false;
        BlockCarMove();

        if (isBrake)
        {
            return;
        }
        
        moveCount++;
        if (moveCount == 1)
        {
            isOnRoad = true;
            carSpawnDeathPreventer
            = Instantiate(carSetup.carSpawnDeathPreventerPfb, nextPosObj.transform.position, Quaternion.identity);
            SetDarkeness(false);
        }

        //moveCount 늘어난 후
        IfMoveWhenMoving();
        nextPos = nextPosObj.transform.position;
        nextPos = new Vector3(Mathf.Round(nextPos.x), transform.position.y, Mathf.Round(nextPos.z));
        isMoving = true;
        transform.DOMove(nextPos, 0.4f).SetId("MoveCar").SetEase(Ease.OutBack);
        if (moveCount == GameManager.Instance.tileCount + carLength)
        {
            DestroyCar(false);
        }
        else
        {
            MasterAudio.PlaySound("Car_Move");
        }
    }
    private void BlockCarMove()
    {

        #region 스폰할 때 충돌 방지
        carSetup.carSpawnDeathPreventerWall.SetActive(false);
        if (isOnRoad == false)
        {
            foreach (Collider collider in GetForwardObjects())
            {
                if (collider.transform.CompareTag("CarSpawnDeathPreventer"))
                {
                    isBrake = true;
                    //carSetup.carSpawnDeathPreventerWall.SetActive(true);
                }
            }
        }
        #if DEBUG_DETECT_BRAKE
        print(GetType().Name + spn + " -> " + (!isOnRoad && isBrake).ToString());
        #endif
        #endregion

        #region 앞에 비통과성 오브젝트가 있을 시 이동x, 충돌

        bool isCarAhead = false;
        foreach (Collider collider in GetForwardObjects())
        {
            if (collider.transform.CompareTag("Car") ||
                collider.transform.CompareTag("BombBox") ||
                (collider.transform.GetComponent<Obstacle>() != null && collider.GetComponent<Obstacle>().isPassableForCar == false))
            {
                isBrake = true;
                if (isOnRoad) keepBrakeCount++;
                if (collider.transform.CompareTag("Car")) isCarAhead = true;

            }
            
            if (collider.transform.GetComponent<Obstacle>() != null && collider.GetComponent<Obstacle>().countToReveal == 1)
            {
                isBrake = true;
            }
            if (collider.transform.CompareTag("Car")) DetectCarJamManager.Instance.AddDetectedCars(this.gameObject, collider.gameObject);
            else if (collider.transform.CompareTag("BombBox")) collider.transform.GetComponent<BombBox>().Crashed();
            else if (collider.transform.GetComponent<Obstacle>() != null) collider.transform.GetComponent<Obstacle>().Crashed();

            
        } 
        #endregion

        //anim
        if (isBrake && (isOnRoad || !isCarAhead) && !_darken) 
        {
            CrashAnim();
        }

        #region 연속 n번 막히면 혼자 터짐
        if (!isBrake)
        {
            //keepBrakeCount = 0;
            //carSetup.damageEffect.SetActive(false);
            //carSetup.bigdamageEffect.SetActive(false);
        }
        
        if (keepBrakeCount >= keepBrakeCountMax)
        {
            DestroyCar(true);            
        }
        else if (keepBrakeCount >= keepBrakeCountMax - 1)
        {
            carSetup.bigdamageEffect.SetActive(true);
            Models.DOShakePosition(1, new Vector3(0.03f, 0, 0.03f), randomness: 30, fadeOut: false).SetLoops(-1, LoopType.Restart).SetId("ShakeCar");
        }
        else if (keepBrakeCount >= keepBrakeCountMax - 2)
        {
            carSetup.damageEffect.SetActive(true);
        }       
        #endregion
    }
    private void IfMoveWhenMoving()
    {
        if (isMoving)
        {
            DOTween.Kill("MoveCar", false);
            transform.position = nextPos;
        }
    }
    private void WhenReachNextPos()
    {
        isMoving = false;
        transform.position = nextPos;
        prevPos = transform.position;
        reachedNextPos = true;
    }
    public void DestroyCar(bool destroyEffect,bool addCoins = false)
    {
        CarManager.Instance.RemoveSpn(spn);
        Destroy(gameObject);
        Destroy(carSpawnDeathPreventer);
        this.DOKill();

        if (destroyEffect) DestroyEffect();
        if (addCoins)
        {
            int amount = (int)Mathf.Floor(Random.Range(0, carLength*2 + 1)/2); //차 길이 두배에서 랜덤 -> 다시 반으로 나누고 반내림
            if (amount > 0 && !GameManager.Instance.isTutorial)
            {
                CoinManager.Instance.AddCoinsWith3DEffect(amount, transform.position,2.5f);
                UIManager.Instance.AddCoinsUIEffect(gameObject.transform.position, amount);
            }      
        }

        GameObject[] csds = GameObject.FindGameObjectsWithTag("CarSpawnDelayer");
        foreach (GameObject csd in csds)
        {
            CarSpawnDelayer carSpawnDelayerScript = csd.GetComponent<CarSpawnDelayer>();
            if (carSpawnDelayerScript.spn == spn)
            {
                carSpawnDelayerScript.DestroyCarSpawnDelayer();
            }
        }
    }

    private void DestroyEffect()
    {
        Instantiate(carSetup.crashWCarEffect, transform.position, Quaternion.identity);
        //sfx
        MasterAudio.PlaySound3DAtTransform("Car_Crash", transform);
    }

    void SetDarkeness(bool darken)
    {
        _darken = darken;
        var renderer = currentModel.transform.GetChild(0).GetComponent<Renderer>();
        if (renderer != null)
        {
            // 인스턴스 머티리얼 사용
            var mat = renderer.material;
            

            var newColor = darken ? originColor * darkenColor : originColor;
            newColor.a = originColor.a;

            if (mat.HasProperty("_BaseColor"))
                mat.SetColor("_BaseColor", newColor);
            else
                mat.SetColor("_Color", newColor);
        }
    }

    Color GetColor()
    {
        var renderer = currentModel.transform.GetChild(0).GetComponent<Renderer>();

        var mat = renderer.material;
        var baseColor = mat.HasProperty("_BaseColor")
            ? mat.GetColor("_BaseColor")
            : mat.GetColor("_Color");
        return baseColor;
       
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            DestroyCar(true);     
        }
        else if (other.CompareTag("BombBox"))
        {
            isMoving = false;
            transform.position = prevPos;
            other.GetComponent<BombBox>().Crashed();
        }
        else if (other.GetComponent<Obstacle>() != null && other.GetComponent<Obstacle>().isPassableForCar == false)
        {
            DestroyCar(true);
        }
    }
    
    private void SetDangerArea()
    {
        bool isEnable = true;
        if (!isOnRoad)
        {
            foreach (Collider collider in GetForwardObjects())
            {
                if (collider.transform.CompareTag("Car") ||
                    collider.transform.CompareTag("CarSpawnDeathPreventer") ||
                    collider.transform.CompareTag("BombBox")||
                    collider.transform.CompareTag("ObstacleDangerArea") ||
                    (collider.GetComponent<Obstacle>() != null &&
                    collider.transform.GetComponent<Obstacle>().isPassableForCar == false))      
                {
                    isEnable = false;
                }
                else if (collider.transform.IsChildOf(transform) == false && collider.name == "DangerArea_" &&
                    collider.GetComponent<Car>().spn > spn)
                {
                    isEnable = false;
                }
                
            }
        }
        else isEnable = false;

        dangerAreaObj.SetActive(isEnable);
    }

    private Collider[] GetForwardObjects()
    {
        return Physics.OverlapBox(nextPosObj.transform.position, Vector3.one / 1);
    }

    private void CrashAnim()
    {
        Models.DOShakePosition(0.3f, strength: 0.2f, vibrato: 30)
                        .SetId("MoveCar");
    }

    private void SpawnAnim()
    {
        Models.DOPunchScale(Vector3.one*0.1f,0.3f,1,0)
                        .SetId("MoveCar");
    }

    private void SetSkin()
    {
        GameObject modelGroupForOneSkinType = null;
        CarSkin[] skins = carSkinManager.carSkins;
        CarSkin currentSkin = null;

        //모델 그룹 설정
        foreach (var skin in skins)
        {
            if (skin.skinName == themeManager.themeType.ToString())
            {
                modelGroupForOneSkinType = Models.Find(skin.skinName).gameObject;
                currentSkin = skin;
                break;
            }
        }
        //모델 설정
        int randomModelIndex = Random.Range(0, modelGroupForOneSkinType.transform.childCount);
        currentModel = modelGroupForOneSkinType.transform.GetChild(randomModelIndex).gameObject;
        //모델에 따른 차 색상 설정
        var currentColorGroup = FindColorGroup();
        int randomColorIndex = Random.Range(0, currentColorGroup.Length);
        currentModel.transform.GetChild(0).GetComponent<MeshRenderer>().material = currentColorGroup[randomColorIndex];
        //완료
        currentModel.SetActive(true);
        
        Material[] FindColorGroup()
        {
            switch (carLength)
            {
                case 1: return currentSkin.smallCarColorGroups[randomModelIndex].colors;
                case 2: return currentSkin.mediumCarColorGroups[randomModelIndex].colors;
                case 3: return currentSkin.largeCarColorGroups[randomModelIndex].colors;
                default: return null;
            }
        }
    }
}

using DarkTonic.MasterAudio;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour //CarSpawnDelayer.cs에서 생성됨
{
    public int spn;

    //manager
    [Header("child 0")] public CarSetup carSetup;

    //pos
    [Header("child 1")]  public GameObject nextPosObj;
    Vector3 nextPos, prevPos;

    //effect
    [Header("child 2")] public GameObject dangerAreaObj;

    //skin
    [SerializeField] List<GameObject> skinTypes = new List<GameObject>();
    GameObject currentSkin = null;
    ThemeManager themeManager;

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

    private void Awake()
    {
        carSetup = transform.GetChild(0).GetComponent<CarSetup>();
        nextPosObj = transform.GetChild(1).gameObject;
        dangerAreaObj = transform.GetChild(2).gameObject;
        reachedNextPos = false;
        isOnRoad = false;

        themeManager = FindObjectOfType<ThemeManager>();
    }

    private void Start()
    {
        CarManager.Instance.AddSpn(spn);
        SetSkin();
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

        /*int numOfChild = this.transform.childCount;
        for (int i = 1; i <= numOfChild - 1; i++) //0번 자식 오브젝트는 NextPos이기 때문에 1부터 시작
            transform.GetChild(i).GetComponent<CarTrasnparent>().CarTransparent();*/
    }

    public void CarMove()
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
    }
    private void BlockCarMove()
    {

        #region 스폰할 때 충돌 방지
        if (isOnRoad == false)
        {
            foreach (Collider collider in GetForwardObjects())
            {
                if (collider.transform.CompareTag("CarSpawnDeathPreventer"))
                {
                    isBrake = true;
                }
            }
        }
        #endregion

        #region 앞에 비통과성 오브젝트가 있을 시 이동x, 충돌
      
        foreach (Collider collider in GetForwardObjects())
        {
            if (collider.transform.CompareTag("Car") ||
                collider.transform.CompareTag("BombBox") ||
                (collider.transform.GetComponent<Obstacle>() != null && collider.GetComponent<Obstacle>().isPassableForCar == false))
            {
                isBrake = true;
                if (isOnRoad) keepBrakeCount++;

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
        if (isBrake)
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
    public void DestroyCar(bool destroyEffect)
    {
        CarManager.Instance.RemoveSpn(spn);
        Destroy(gameObject);
        Destroy(carSpawnDeathPreventer);
        this.DOKill();

        if (destroyEffect) DestroyEffect();

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
        transform.Find("Models_").transform.DOShakePosition(0.3f, strength: 0.2f, vibrato: 30)
                        .SetId("MoveCar");
    }

    private void SetSkin()
    {
        GameObject skinType = null;
        for (int i = 0; i<skinTypes.Count; i++)
        {
            if (skinTypes[i].name == themeManager.themeType.ToString())
            {
                skinType = skinTypes[i];
                break;
            }
        }
        int randomSkinIndex = Random.Range(0, skinType.transform.childCount);
        currentSkin = skinType.transform.GetChild(randomSkinIndex).gameObject;
        currentSkin.SetActive(true);
    }
}

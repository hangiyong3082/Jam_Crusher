using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarSpawnDelayer : MonoBehaviour //CarSpawner.cs에서 생성됨
{
    //spn
    [HideInInspector] public int spn;
    [HideInInspector] public int acrossSpn; 

    //manager
    CarSpawner carSpawnerScript;

    //spawn car
    [SerializeField] List<GameObject> carPrefab;
    int carPrefabCount;
    int carLength;
    [SerializeField] List<GameObject> carLengthText;
    int randomCar;

    //icon
    [SerializeField] GameObject dangerIcon;

    //instantiate car
    GameObject instantiatedCarLengthText;
    GameObject instantiatedCar;

    //count
    int spawningCarCount, carLeftMoveCount, spawnRemainingCount;
    [SerializeField] TMP_Text remainingCountText;   
    

    private void Awake()
    {
        carSpawnerScript = GameObject.Find("CarSpawner").GetComponent<CarSpawner>();
        carPrefabCount = carPrefab.Count;
        remainingCountText.gameObject.transform.rotation = Quaternion.Euler(90,0,0);
    }

    private void Start()
    {
        carSpawnerScript.ExcludeSpawnPointNum(spn);
        carSpawnerScript.ExcludeSpawnPointNum(acrossSpn);

        spawnRemainingCount = 1;
        randomCar = Random.Range(0,carPrefabCount);
        carLength = randomCar + 1;
        spawningCarCount = carLength;
        carLeftMoveCount = GameManager.Instance.tileCount + carPrefab[randomCar].GetComponent<Car>().carLength;


        RemainingCountTextUpdate();
        dangerIcon.SetActive(true);
    }

    public void CarSpawnDelayerWork()
    {
        spawnRemainingCount--;
        //spawnRemainingCount가 0이 됐을 때 바로 carLeftMoveCount--가 실행되면 안됨.
        if (spawnRemainingCount > 0)
        {
            RemainingCountTextUpdate();
        }
        else if (spawnRemainingCount < 0)
        {
            if (instantiatedCar != null)
            {
                if (instantiatedCar.GetComponent<Car>().isBrake == false)
                {
                    carLeftMoveCount--;
                    spawningCarCount--;
                }
            } 
        }
        else if (spawnRemainingCount == 0)
        {
            instantiatedCar = Instantiate(carPrefab[randomCar],transform.position,transform.rotation);
            instantiatedCar.GetComponent<Car>().spn = spn;

            dangerIcon.SetActive(false);
            remainingCountText.text = "";
            //instantiatedCarLengthText = Instantiate(carLengthText[carLength - 1], transform.position, Quaternion.identity);
        }
        if (carLeftMoveCount == 0)
        {
              DestroyCarSpawnDelayer();
        }
        if (spawningCarCount == 0)
        {
            Destroy(instantiatedCarLengthText);
            dangerIcon.SetActive(false);
        }
    }

    public void DestroyCarSpawnDelayer()
    {
        ReturnSpawnPoint(spn);
        ReturnSpawnPoint(acrossSpn);

        Destroy(instantiatedCarLengthText);
        
        //print($"return : spn={spn},acrossSpn:{acrossSpn}");
        Destroy(gameObject);
    }

    void RemainingCountTextUpdate()
    {
        remainingCountText.text = "";
    }

    public virtual void ReturnSpawnPoint(int spn) => carSpawnerScript.ReturnSpawnPointNum(spn);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CarSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject carSpawnDelayerPrefab;

    int spawnProbability = 70;

    float spawnPosY = 1.5f;
    float middleToSideLength;

    int randomSpn = 0;
    public List<int> availableSpnList = new List<int>();
    public List<int> diagonalSpn = new List<int>();
    int[,] lawSpnList = new int[4,5];
    Vector3[] spnList = new Vector3[20];
    Quaternion[] spawnCarRotationList = new Quaternion[20];

    int tileCount;
    int tileScale;

    private void Awake()
    {
        tileCount = GameManager.Instance.tileCount;
        tileScale = GameManager.Instance.tileScale;
        middleToSideLength = tileScale * Mathf.CeilToInt(tileCount / 2f);
    }

    private void Start()
    {
        StartSetting();
    }

    public void SpawnCar()
    {
        if (Random.Range(1, 101) > spawnProbability) return;
        /*0 1 2 3 4 
        10         15
        11         16
        12         17
        13         18
        14         19
         5 6 7 8 9*/
        if (availableSpnList.Count != 0)
        {
            //랜덤 스폰 지점
            randomSpn = availableSpnList[Random.Range(0, availableSpnList.Count)];
           
            #region Prevent Across Spawn
            //<차 스폰시 반대편에서는 스폰 못하게 막음 (차 충돌 방지)
            int rsp = randomSpn; //변수 간략화용
            int acrossSpn = 0;
            if (rsp>=0 && rsp<=4) acrossSpn = rsp + 5;
            if (rsp>=5 && rsp<=9) acrossSpn = rsp - 5;
            if (rsp>=10 && rsp<=14) acrossSpn = rsp + 5;
            if (rsp>=15 && rsp<=19) acrossSpn = rsp - 5;

            //>
            #endregion

            #region Prevent Diagonal Spawn

            AddDiagonalSpn(19 - randomSpn);
            if (randomSpn >= 0 && randomSpn <= 9)
            {
                AddDiagonalSpn(randomSpn+10);
            }
            else if (randomSpn >= 10 && randomSpn <= 19)
            {
                AddDiagonalSpn(randomSpn-10);
            }

            void AddDiagonalSpn(int spn)
            {
                if (availableSpnList.Contains(spn))
                {
                    diagonalSpn.Add(spn);
                }
            }
            #endregion

            GameObject carSpawnDelayer = Instantiate(carSpawnDelayerPrefab, spnList[randomSpn], spawnCarRotationList[randomSpn]);
            carSpawnDelayer.GetComponent<CarSpawnDelayer>().spn = randomSpn;
            carSpawnDelayer.GetComponent<CarSpawnDelayer>().acrossSpn = acrossSpn;

            ExcludeSpawnPointNum(randomSpn);
            ExcludeSpawnPointNum(acrossSpn);
            foreach (int i in diagonalSpn) ExcludeSpawnPointNum(i);

            string a = "";
            foreach (int i in availableSpnList)
            {
                a += i.ToString()+" ";
            }
            //print(a);
        } //선택된 스폰 지점은 스폰가능 지점 리스트에서 삭제

        //스폰너 없어지면 다시 리스트에 값 추가
    }

    public void ReturnSpawnPointNum(int spawnPointNum)
    {
        if (availableSpnList.Contains(spawnPointNum)) return;
        availableSpnList.Add(spawnPointNum);
    }

    public void ExcludeSpawnPointNum(int spawnPointNum)
    {
        try
        {
            availableSpnList.Remove(spawnPointNum); 
        }
        catch
        {
            return;
        }
    }

    void StartSetting()
    {
        //스폰가능 지점 리스트
        for (int i = 0; i < tileCount * 4; i++)
        {
            availableSpnList.Add(i);
        }
        //

        //모서리마다의 스폰 지점번호 묶기
        int n = 0;
        for (int i = 0; i <= 3; i++)
        {
            for (int j = 0; j < tileCount; j++)
            {
                lawSpnList[i, j] = n;
                n++;
            }
        }
        //

        //스폰 지점당 위치값
        int startArrayNum = 0;
        for (int i = 0; i < 4; i++)
        {
            int pos = -(tileScale * Mathf.FloorToInt(tileCount/2f));
            for (int j = startArrayNum; j < (startArrayNum + tileCount); j++)
            {
                if (startArrayNum == 0) { spnList[j] = new Vector3(pos, spawnPosY, middleToSideLength); spawnCarRotationList[j].eulerAngles = new Vector3(0, -180, 0); }
                if (startArrayNum == tileCount) { spnList[j] = new Vector3(pos, spawnPosY, -middleToSideLength); spawnCarRotationList[j].eulerAngles = new Vector3(0, 0, 0); }
                if (startArrayNum == tileCount*2) { spnList[j] = new Vector3(-middleToSideLength, spawnPosY, -pos); spawnCarRotationList[j].eulerAngles = new Vector3(0, 90, 0); }
                if (startArrayNum == tileCount*3) { spnList[j] = new Vector3(middleToSideLength, spawnPosY, -pos); spawnCarRotationList[j].eulerAngles = new Vector3(0, -90, 0); }
                pos += 2;
            }
            startArrayNum += tileCount;
        }
        ///
    }
}

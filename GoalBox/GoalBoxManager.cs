using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBoxManager : MonoBehaviour
{
    [SerializeField]
    GameObject goalBoxPrefab;

    int randomSpawnPointNum = 0;
    public List<int> spawnAvailablePointNumList = new List<int>();
    Vector3[] spawnPointPosList = new Vector3[25];

    private void Awake()
    {
        //스폰가능 지점 리스트
        for (int i = 0; i < 25; i++)
        {
            spawnAvailablePointNumList.Add(i);
        }
        //
        //스폰 지점당 위치값
        int startArrayNum = 0;
        int posZ = 4;
        for (int i = 0; i < 5; i++)
        {
            int posX = -4;
            for (int j = startArrayNum; j <= startArrayNum + 4; j++)
            {
                spawnPointPosList[j] = new Vector3(posX, 1, posZ);
                posX += 2;
            }
            posZ += -2;
            startArrayNum += 5;
        }
        //
    }

    IEnumerator SpawnGoalBox()
    {
        yield return null;

        if (spawnAvailablePointNumList.Count != 0)
        {
            randomSpawnPointNum = spawnAvailablePointNumList[Random.Range(0, spawnAvailablePointNumList.Count)]; //랜덤 스폰 지점
            ExcludeSpawnPointNum(randomSpawnPointNum);
            GameObject goalBox = Instantiate(goalBoxPrefab, spawnPointPosList[randomSpawnPointNum], Quaternion.identity);
            goalBox.GetComponent<GoalBox>().pointNum = randomSpawnPointNum;
        } 
        //선택된 스폰 지점은 스폰가능 지점 리스트에서 삭제
        //플레이어가 있는 지점은 제외

        //플레이어가 도달하면 다시 리스트에 값 추가
    }

    public void ReturnSpawnPointNum(int spawnPointNum)
    {
        spawnAvailablePointNumList.Add(spawnPointNum);
    }

    public void ExcludeSpawnPointNum(int spawnPointNum)
    {
        spawnAvailablePointNumList.Remove(spawnPointNum);
    }
}

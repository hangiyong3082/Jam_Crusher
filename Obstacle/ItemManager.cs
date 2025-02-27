using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    //item
    [SerializeField] GameObject itemPrefab;
    public int itemsOnRoadCount;

    //spawn probability
    [SerializeField] float spawnProbability;
    public int spawnCancleStrike = 0;
    [SerializeField] int spawnCancleStrikeMax = 10;

    //spawn
    int randomSpn = 0;
    Vector3[] spawnPosList = new Vector3[25];

    //item count text
    [SerializeField] public GameObject bBitemCountText; //{ get; private set; }

    private void Awake()
    {
        //스폰 지점당 위치값
        int startArrayNum = 0;
        int posZ = 4;
        for (int i = 0; i < 5; i++)
        {
            int posX = -4;
            for (int j = startArrayNum; j <= startArrayNum + 4; j++)
            {
                spawnPosList[j] = new Vector3(posX, 1, posZ);
                posX += 2;
            }
            posZ += -2;
            startArrayNum += 5;
        }
        //
    }

    public void SpawnItem()
    {
        if (spawnCancleStrike >= spawnCancleStrikeMax)
        {        
            Work();
            spawnCancleStrike = 0;

        }
        if (Random.Range(0f,1f) <= spawnProbability)
        {
            Work();
            spawnCancleStrike = 0;
        }
        else
        {
            spawnCancleStrike++;
        }
        
        void Work()
        {
            var playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            int itemsInGame = GameManager.Instance.bombBoxItemCount + itemsOnRoadCount;

            if (AvailableTileSpnList.Instance.list.Count != 0 && itemsInGame < GameManager.Instance.maxbBItemCount)
            {
                int playerPointNum = playerController.pointNum;
                AvailableTileSpnList.Instance.ExcludeSpn(playerPointNum);

                randomSpn = AvailableTileSpnList.Instance.RandomSpn(); //랜덤 스폰 지점 
                GameObject item = Instantiate(itemPrefab, spawnPosList[randomSpn], Quaternion.identity);
                itemsOnRoadCount++;

                item.GetComponent<Item>().spn = randomSpn;
                AvailableTileSpnList.Instance.ExcludeSpn(randomSpn);

                AvailableTileSpnList.Instance.ReturnSpn(playerPointNum);
            }
            //선택된 스폰 지점은 스폰가능 지점 리스트에서 삭제
            //플레이어가 있는 지점은 제외

            //플레이어가 도달하면 다시 리스트에 값 추가
        }
    }
}

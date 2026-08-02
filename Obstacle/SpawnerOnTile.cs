using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerOnTile : MonoBehaviour, ISpawner
{
    [Header("References")]
    [Header("prefab must have tag!")]
    [SerializeField] GameObject prefab;

    int tileCount, tileScale;

    [Header("Settings")]
    [SerializeField] float spawnPosY; //Tree : 0.8f
    [SerializeField] float spawnProbability; //Tree : 20

    [Header("Public")]
    public string prefabsTag;


    private void Awake()
    {
        tileCount = GameManager.Instance.tileCount;
        tileScale = GameManager.Instance.tileScale;
        prefabsTag = prefab.tag;
        if (prefabsTag == null) throw new System.Exception("프리팹에 gamemanager 딕셔너리 key 이름과 같게 태그 붙여야함");
    }

    private void Update()
    {
        
    }

    public void Spawn(int maxCountOnBoard = 1, bool forcedSpawn = false)
    {
        var playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        for (int i = 0; i < 1; i++)
        {
            if (GameObject.FindGameObjectsWithTag(prefabsTag).Length>= maxCountOnBoard ||
                AvailableTileSpnList.Instance.list.Count == 0)
            {
                return;
            }
            if (!forcedSpawn)
            {
                if (Random.Range(1, 101) > spawnProbability)
                {
                    continue;
                }
            }       
            int randomPoint = AvailableTileSpnList.Instance.RandomSpn();
            var gameObject = Instantiate(prefab, CalculatePos(randomPoint), Quaternion.identity);
            gameObject.GetComponent<Obstacle>().pointNum = randomPoint;
            AvailableTileSpnList.Instance.ExcludeSpn(randomPoint);
        }
    }

    Vector3 CalculatePos(int tileIndex)
    {
        Vector3 result;

        int numberInRow;
        int numberInLine;
        float posX;
        float posZ;

        float halfTiles = Mathf.Ceil(tileCount / 2f);

        numberInRow = (tileIndex + 1) - tileCount * Mathf.FloorToInt(tileIndex / (float)tileCount);
        numberInLine = Mathf.CeilToInt((tileIndex + 0.1f) / tileCount);

        posX = (numberInRow - halfTiles) * tileScale;
        posZ = (halfTiles - numberInLine) * tileScale;


        result = new Vector3(posX, spawnPosY, posZ);
        print($"{tileIndex} -> {posX}, {posZ}\n row : {numberInRow}, line {numberInLine}");

        return result;
    }
}

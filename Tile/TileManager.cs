using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    int tileCount;
    float tileWidth;
    float halfLength;

    [SerializeField] GameObject tentativeTilePrefab;

    private void Start()
    {
        SpawnTiles(tentativeTilePrefab);
    }


    public void SpawnTiles(GameObject tilePrefab)
    {
        tileCount = GameManager.Instance.tileCount;
        tileWidth = tilePrefab.transform.localScale.x;

        halfLength = (tileCount - 1) / 2 * tileWidth;

        int startIndex = 0;
        float posZ = halfLength;
        for (int i = 0; i < tileCount; i++)
        {
            float posX = -halfLength;
            for (int j = startIndex; j <= startIndex + (tileCount - 1); j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(posX, -0.3f, posZ), Quaternion.identity);
                tile.GetComponent<Tile>().pointNum = j;
                posX += 2;
            }
            posZ += -2;
            startIndex += tileCount;
        }
    }

    public void DestroyTiles()
    {
        foreach (var tile in GameObject.FindGameObjectsWithTag("Ground"))
        {
            Destroy(tile);
        }
    }
}

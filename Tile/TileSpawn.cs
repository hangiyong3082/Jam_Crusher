using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawn : MonoBehaviour
{
    [SerializeField]
    GameObject tilePrefab;

    int tileCount;
    public float tileWidth;
    public float halfLength;
    
    void Awake()
    {
        tileCount = GameManager.Instance.tileCount;
        tileWidth = tilePrefab.transform.localScale.x;

        halfLength = (tileCount-1)/2*tileWidth; 
    }

    void Start()
    {
        int startIndex = 0;
        float posZ = halfLength;
        for (int i = 0; i < tileCount; i++)
        {
            float posX = -halfLength;
            for (int j = startIndex; j <= startIndex + (tileCount-1); j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(posX, -0.3f, posZ), Quaternion.identity);
                tile.GetComponent<Tile>().pointNum = j;
                posX += 2;
            }
            posZ += -2;
            startIndex += tileCount;
        }
    }
}

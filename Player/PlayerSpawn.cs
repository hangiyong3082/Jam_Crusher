using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject tilePrefab;
    public int sideTileCount;
    float upRightTilePos;
    float spawnPosY = 1.5f;

    // Start is called before the first frame update
    void Awake()
    {
        BoxCollider tileBc = tilePrefab.GetComponent<BoxCollider>();
        float tileWidth = tileBc.size.x * 2;

        upRightTilePos = (sideTileCount - 1) / 2 * tileWidth;
    }
    // Start is called before the first frame update
    public void SpawnPlayer()
    {
        transform.position = new Vector3(0,spawnPosY,0);
    }
}

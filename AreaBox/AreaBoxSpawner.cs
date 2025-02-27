using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBoxSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject areaBoxPrefab;
    [SerializeField] GameObject tilePrefab;
    // Start is called before the first frame update
    void Awake()
    {
        int tileCount = GameManager.Instance.tileCount;
        float tileWidth = (int)tilePrefab.transform.localScale.x;
        float halfLength = (tileCount - 1) / 2 * tileWidth;

        int startIndex = 0;
        float posZ = halfLength;
        for (int i = 0; i < tileCount; i++)
        {
            float posX = -halfLength;
            for (int j = startIndex; j <= startIndex + (tileCount-1); j++)
            {
                GameObject areaBox = Instantiate(areaBoxPrefab, new Vector3(posX, 0, posZ), Quaternion.identity);
                areaBox.GetComponent<AreaBox>().areaNum = j;
                posX += 2;
            }
            posZ += -2;
            startIndex += tileCount;
        }
    }
}

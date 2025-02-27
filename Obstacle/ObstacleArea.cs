using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleArea : MonoBehaviour
{
    [SerializeField] GameObject mainObject;
    [SerializeField] GameObject bombBoxPrefab;

    public int pointNum;
    public bool isOnPlaceMode = false;

    BoxCollider boxCollider;
    public bool isEnable;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    
    private void Start()
    {
        mainObject.SetActive(false);
    }
    
    private void Update()
    {
        if (!isOnPlaceMode)
        {
            mainObject.SetActive(false);
            return;
        }

        Collider[] colliders = Physics.OverlapBox(transform.position, boxCollider.size + new Vector3(0, 5, 0));
        
        isEnable = true;
        foreach (var collider in colliders)
        {
            if (collider.transform.CompareTag("Player")||
                collider.transform.CompareTag("Car")||
                collider.transform.CompareTag("BombBox") ||
                collider.transform.CompareTag("Item") ||
                collider.transform.GetComponent<Obstacle>() != null) 

            {
                isEnable = false; break;
            }
        }
        if (isEnable) mainObject.SetActive(true);
        else mainObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        /*
        if (!GameManager.Instance.placementMode || !isEnable)
        {
            return;
        }
        Instantiate(obstaclePrefab, transform.position+Vector3.up, Quaternion.identity);
        GameManager.Instance.obstacleItemCount--;

        oAMScript.TogglePlacementMode();
        */
    }
}

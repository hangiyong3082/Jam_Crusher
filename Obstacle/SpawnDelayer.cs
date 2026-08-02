using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  직접적으로 타겟 오브젝트에 붙음. 스포너 x
/// </summary>
public class SpawnDelayer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject dangerIcon;
    [SerializeField] GameObject dangerArea;
    bool isAlreadyPlayedIconAnim;

    [Header("Public")]
    public int _remainingCount;
    /// <summary>
    /// (위에 차 있을 때 경우 처리됨)
    /// </summary>
    public int remainingCount
    {
        get => _remainingCount;
        set
        {
            // 한 번 남았는데 위에 차 있으면 안 나옴
            if (value == 1 && DetectCarAbove()) _remainingCount = 1;
            else _remainingCount = value;
        }
    }
    


    private void Awake()
    {
        
    }
    private void Start()
    {
        dangerArea.SetActive(false);
        dangerIcon.SetActive(true);
        

    }

    private void Update()
    {
        if (remainingCount > 1)
        {
            dangerIcon.SetActive(true);
        }
        else if (remainingCount == 1)
        {
            //dangerIcon.SetActive(false);
            dangerArea.SetActive(true);
            if (!isAlreadyPlayedIconAnim)
            {
                //dangerIcon.transform.DOScale(1.1f, 0.25f).SetLoops(-1, LoopType.Yoyo);
                isAlreadyPlayedIconAnim = true;
            }
                
        }
        else if (remainingCount == 0)
        {
            dangerIcon.SetActive(false);
            dangerArea.SetActive(false);
        }
    }

    bool DetectCarAbove()
    {
        bool result = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position,Vector3.up,out hit, 100))
        {
            if (hit.collider.CompareTag("Car"))
            {
                result = true;
            }
        }
        return result;
    }
}

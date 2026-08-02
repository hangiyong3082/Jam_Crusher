using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Image timeBarImg;
    [SerializeField] DOTweenAnimation timeUpAnim;


    private void Awake()
    {
        gameObject.SetActive(false);  
    }

    public void Update_()
    {
        if (GameManager.Instance.placementMode)
        {
            timeBarImg.color = new Color(timeBarImg.color.r, timeBarImg.color.g, timeBarImg.color.b, 0.5f);
            return;
            
        }
        timeBarImg.color = new Color(timeBarImg.color.r, timeBarImg.color.g, timeBarImg.color.b, 1);
        timeBarImg.fillAmount = GameManager.Instance.turnTime / GameManager.Instance.turnTimeInit;
    }

    public void NextTrun()
    {
        InitSize();

        timeUpAnim.DORestart();
    }

    void InitSize()
    {
        timeBarImg.fillAmount = 1;
    }

    public void StartWorking()
    {
        gameObject.SetActive(true);
    }
}

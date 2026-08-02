using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Image title;
    [SerializeField] public Button startButton;
    [SerializeField] public Button startFrom200Button;
    [SerializeField] Button shopButton;
    [SerializeField] ShopUI shopUI;
    [SerializeField] public GameObject bottomUI;
    [SerializeField] LocalizeStringEvent bestScore;
    //[SelectionBase(true)] PausePanel

    private void Start()
    {
        shopUI.gameObject.SetActive(false);
        startFrom200Button.interactable =  (GameManager.Instance.GetBestScore() >= 200f);
        LocalizationFunctions.UpdateVariable(bestScore, GameManager.Instance.GetBestScore().ToString("F1"));
    }

    public void ButtonSetup()
    {
        shopButton.onClick.AddListener(delegate
        {
            shopUI.gameObject.SetActive(true);
            bottomUI.SetActive(false);
            FindAnyObjectByType<ShopController>().GenerateItems();
            //shopUI.OpenCloseAnim(shopUI.transform, 1, true);
            //shopUI.OpenCloseAnim(bottomUI, 0);
            shopUI.OpenShop();
            shopUI.ActiveOptionSection();

            UIManager.Instance.ClickSound();
            GameManager.Instance.stateDrivenCamera.Play("ZoomViewCamera");
        });
        shopUI.ButtonSetup();
    }
}

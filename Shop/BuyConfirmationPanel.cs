using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class BuyConfirmationPanel : MonoBehaviour
{
    [Header("References")]

    [SerializeField] Button buyButton;
    [SerializeField] Button cancelButton;

    [SerializeField] Image profileImg;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text costText;
    //
    Skin skin_m;
    SkinShopItem skinShopItem_m;


    private void Awake()
    {
        buyButton.onClick.AddListener(delegate {BuyItem(); });
        cancelButton.onClick.AddListener(delegate { ClosePanel(); });
    }

    private void Start()
    {
        gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
    }

    public void OpenPanel(Skin skin, SkinShopItem skinShopItem)
    {
        gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);

        profileImg.sprite = skin.profileImg;
        nameText.text = LocalizationFunctions.GetLocalizedString(skin.localizedString);
        costText.text = skin.cost.ToString();

        skin_m = skin;
        skinShopItem_m = skinShopItem;
    }

    void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    void BuyItem()
    {
        int coins = PlayerPrefs.GetInt("Coins", 0);

        PlayerPrefs.SetInt("Coins", coins - skin_m.cost);
        skinShopItem_m.skinManager.Unlock(skinShopItem_m.skinIndex);
        skinShopItem_m.costArea.SetActive(false);
        MasterAudio.PlaySound("Shop_Purchase");

        ClosePanel();
    }
    



}
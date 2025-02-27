using DarkTonic.MasterAudio;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SkinManager skinManager;
    [SerializeField] Button buyButton;
    [SerializeField] TMP_Text costText;
    [SerializeField] GameObject costArea;
    [SerializeField] GameObject selectionEffect;
    [SerializeField] bool isOriginal = false;

    [Header("Public")]
    public int skinIndex;

    private Skin skin;
    ShopController shopController;
    BuyConfirmationPanel buyConfirmationPanel;
    
    

    private void Awake()
    {
        //reference
        shopController = GameObject.Find("ShopController").GetComponent<ShopController>();
        buyConfirmationPanel = GameObject.Find("BuyConfirmationPanel").GetComponent<BuyConfirmationPanel>();

        skinIndex = transform.GetSiblingIndex();

        buyButton.onClick.AddListener(OnSkinPressed);
        
    }

    void Start()
    {
        skin = skinManager.skins[skinIndex];

        GetComponent<Image>().sprite = skin.profileImg;
        selectionEffect.SetActive(false);

        if (isOriginal)
        {
            skinManager.Unlock(skinIndex);
            selectionEffect.SetActive(true);
        }

        if (skinManager.IsUnlocked(skinIndex))
        {
            costArea.SetActive(false);
            if (skin == skinManager.GetSelectedSkin())
            {
                selectionEffect.SetActive(true);
            }
        }
        else
        {
            costText.text = skin.cost.ToString();
        }
    }
 
    void OnSkinPressed()
    {
        if (skinManager.IsUnlocked(skinIndex))
        {
            skinManager.SelectSkin(skinIndex);
            selectionEffect.SetActive(true);
            shopController.SetSkin();
            
            selectionEffect.transform.DOPunchScale(Vector3.one * -0.2f, 0.2f, vibrato: 1);
        }
        else
        {
            BuyItem();
        }
    }

    void BuyItem()
    {
    int coins = PlayerPrefs.GetInt("Coins", 0);

    // Unlock the skin
    if (coins >= skin.cost && !skinManager.IsUnlocked(skinIndex))
    {
        PlayerPrefs.SetInt("Coins", coins - skin.cost);
        skinManager.Unlock(skinIndex);
        costArea.SetActive(false);
        MasterAudio.PlaySound("Shop_Purchase");
    }
    else
    {
        Debug.Log("Not enough coins :(");
    }
    }

    private void Update()
    {
        if (skin != skinManager.GetSelectedSkin())
        {
            selectionEffect.SetActive(false);
        }
    }
}

using DarkTonic.MasterAudio;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class SkinShopItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public SkinManager skinManager;
    [SerializeField] Image profileImg;
    [SerializeField] Button buyButton;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text costText;
    [SerializeField] public GameObject costArea;
    [SerializeField] GameObject selectionEffect;
    [SerializeField] GameObject lockEffect;
    [SerializeField] LocalizeStringEvent localizeStringEvent;

    [Header("Public")]
    public int skinIndex;

    private Skin skin;
    ShopController shopController;
    BuyConfirmationPanel buyConfirmationPanel;

    int runLocalizationCount = 0;


    private void Awake()
    {
        //reference
        shopController = FindFirstObjectByType<ShopController>();
        buyConfirmationPanel = FindAnyObjectByType<BuyConfirmationPanel>(FindObjectsInactive.Include);

        skinIndex = transform.GetSiblingIndex();

        buyButton.onClick.AddListener(OnSkinPressed);
    }

    void Start() //매니저가 설정됐을 때
    {
        selectionEffect.SetActive(false);
        lockEffect.SetActive(false);

        while (skinManager == null) { }

        skin = skinManager.skins[skinIndex];
        profileImg.sprite = skin.profileImg;
        

        if (skin.isOriginal)
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
            if (skin.cost == -1)
            {
                costArea.SetActive(false);
                lockEffect.SetActive(true);
            }
            else
            {
                costText.text = skin.cost.ToString();
            }
        }

        DoLocalize();
        Invoke("DoLocalize", 0.1f);

    }

    private void OnEnable()
    {
        if (skinManager == null) return;

        if (skinManager.IsUnlocked(skinIndex))
        {
            costArea.SetActive(false);
        }
    }
    
    public void DoLocalize()
    {
        IEnumerator enumerator = LocalizationFunctions.SynchronizeStringWEvent(skin.localizedString, localizeStringEvent);
        StartCoroutine(enumerator);
    }

    void OnSkinPressed()
    {
        if (skinManager.IsUnlocked(skinIndex))
        {
            skinManager.SelectSkin(skinIndex);
            selectionEffect.SetActive(true);
            shopController.SetSkin();
            
            selectionEffect.transform.localScale = Vector3.one;
            DOTween.Complete(GetType().Name + "selectionEffect");
            selectionEffect.transform.DOPunchScale(Vector3.one * -0.2f, 0.2f, vibrato: 1).SetId(GetType().Name+"selectionEffect");
            print("anim 실행");
        }
        else
        {
            BuyItem();
        }
        UIManager.Instance.ClickSound();
    }

    void BuyItem()
    {
        int coins = PlayerPrefs.GetInt("Coins", 0);

        // Unlock the skin
        if (coins >= skin.cost && !skinManager.IsUnlocked(skinIndex))
        {
            UIManager.Instance.buyConfirmationPanel.GetComponent<BuyConfirmationPanel>().OpenPanel(skin, this);
        }
        else
        {
            string animID = GetType().Name + "NotEnoughCoins";
            DOTween.Kill(animID, true);
            costText.DOColor(new Color(1, 0.45f, 0.45f), 0.2f)
                .SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo).SetId(animID);
            costText.DOScale(1.2f,0.2f)
                .SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo).SetId(animID);

            MasterAudio.PlaySound("Shop_NotEnoughCoins");
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

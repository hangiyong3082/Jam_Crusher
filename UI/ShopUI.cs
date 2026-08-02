using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum BuySectionState
{
    Patterns,
    Cosmetics,
    Themes,
}
public enum ShopState
{
    None,
    Option,
    BuySection,
}
public class ShopUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject option;
    [SerializeField] GameObject buySection;
    [SerializeField] GameObject viewPort;
    [SerializeField] Button undoButton;
    [SerializeField] MenuPanel menuPanel;

    [Header("Public")]
    public ShopState shopState;

    private void Start()
    {
        ActiveSection(new GameObject[] {option});
    }
    
    public void ButtonSetup()
    {
        //buy section
        for (int i = 0; i < option.transform.childCount; i++)
        {
            var iLocal = i;
            option.GetComponentsInChildren<Button>()[i].onClick.AddListener(delegate 
            {
                ClickSound(); 
                shopState = ShopState.BuySection; 
                ActiveBuySection(state: (BuySectionState)iLocal); 
            });
        }
        //undo
        undoButton.onClick.AddListener(delegate 
        {
            ClickSound();
            UndoState();
        });

        void ClickSound()
        {
            UIManager.Instance.ClickSound();
        }

        void UndoState()
        {

            if ((int)shopState > 0)
            {
                shopState = (ShopState)((int)shopState - 1);
                print(shopState.ToString());
                switch (shopState)
                {
                    case ShopState.None:
                        CloseShop();
                        return;
                    case ShopState.Option:
                        ActiveOptionSection();
                        return;
                }
            }
        }

    }

    

    /// <param name="option">0 : close, 1 : open</param>
    public void OpenCloseAnim(RectTransform trans, int option, bool isFrom = false)
    {
        string animId = nameof(ShopUI) + nameof(OpenCloseAnim) + nameof(trans.gameObject.name);

        //DOTween.Kill(animId, true);
        switch (option)
        {
            case 0:
                if (isFrom) trans.DOAnchorPosY(700,0.5f).SetRelative();
                trans.DOAnchorPosY(-700, 0.5f).SetRelative();

                return;
            case 1:
                if (isFrom) trans.DOAnchorPosY(-700, 0.5f).SetRelative();
                trans.DOAnchorPosY(700, 0.5f).SetRelative();
                return;
        }       
    }

    public void OpenShop()
    {
        gameObject.SetActive(true);
        ActiveSection(new GameObject[] { });
        menuPanel.startButton.enabled = false;
        menuPanel.title.enabled = false;
        UIManager.Instance.crownAchievementUI.SetActive(false);

        //animation
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.DOPunchPosition(Vector3.up * 0.5f, 0.5f, 0, 0).SetId(this.GetType().Name + "OpenBuySection");
        //player.transform.DORotate(Vector3.up * 180, 0.5f).SetId(this.GetType().Name + "OpenBuySection");
    }
    public void CloseShop()
    {
        gameObject.SetActive(false);
        menuPanel.bottomUI.SetActive(true);
        menuPanel.startButton.enabled = true;
        menuPanel.title.enabled = true;
        UIManager.Instance.crownAchievementUI.SetActive(true);
        GameManager.Instance.stateDrivenCamera.Play("QuarterViewCamera");
        //animation
        DOTween.Complete(this.GetType().Name + "OpenBuySection");
        //GameObject.FindWithTag("Player").transform.rotation = Quaternion.identity;
    }

    public void ActiveOptionSection()
    {
        ActiveSection(new GameObject[] { option });
        shopState = ShopState.Option;
    }

    /// <summary>
    /// 둘 중에 하나만 하면 됨
    /// </summary>
    /// <param name="state"></param>
    /// <param name="stateInt"></param>
    public void ActiveBuySection(BuySectionState state)
    {
        ActiveSection(new GameObject[] { buySection });
        shopState = ShopState.BuySection;

        int stateInt = (int)state;

        for (int i = 0; i < viewPort.transform.childCount; i++)
        {
            viewPort.transform.GetChild(i).gameObject.SetActive(false);
        }
        viewPort.transform.GetChild(stateInt).gameObject.SetActive(true);
        buySection.GetComponent<ScrollRect>().content 
            = viewPort.transform.GetChild(stateInt).GetComponent<RectTransform>();
    }

    void ActiveSection(GameObject[] gameObjects)
    {
        option.SetActive(false);
        buySection.SetActive(false);
        foreach (GameObject gameObj in gameObjects)
        {
            gameObj.SetActive(true);
        }  
    }
}

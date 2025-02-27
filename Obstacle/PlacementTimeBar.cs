using UnityEngine;
using UnityEngine.UI;

public class PlacementTimeBar : MonoBehaviour
{
    //bar
    [SerializeField] Image mainBar;
    float initTime;
    float time;
    Vector2 initBarSize;

    //ui
    [SerializeField] GameObject bBItemCountText;

    private void Awake()
    {
        

        initBarSize = mainBar.rectTransform.sizeDelta;
    }

    private void Start()
    {
        Init();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //Init();
    }

    private void Update()
    {
        if (!GameManager.Instance.placementMode)
        {
            return;
        }

        Vector2 barSize = mainBar.rectTransform.sizeDelta;
        mainBar.rectTransform.sizeDelta
            = new Vector2(barSize.x - (initBarSize.x * Time.deltaTime / initTime), barSize.y);

        time -= Time.deltaTime;
        if (time <= 0)
        {
            GameManager.Instance.bombBoxItemCount--;
            //anim
            bBItemCountText.GetComponent<ItemCountAnimation>().RemoveItemAnim();
            //setting
            BombBoxManager.Instance.TogglePlacementMode();
            BombBoxManager.Instance.SetUI();
            
        }
    }
  
    public void Init()
    {
        //time
        initTime = GameManager.Instance.initPlacementTime;
        time = initTime;
        //size
        mainBar.rectTransform.sizeDelta = initBarSize;
    }
}

using DarkTonic.MasterAudio;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BombBoxManager : Singleton<BombBoxManager>
{
    //bombBox
    [SerializeField] GameObject obstacleAreaPfb;
    [SerializeField] GameObject bombBoxPfb;
    [SerializeField] GameObject bombBoxButton;
    Button bombBoxItemButton;
    [SerializeField] GameObject bBItemCountText;
    //placement time bar
    [SerializeField] GameObject placementTimeBar;
    //ui
    [SerializeField] GameObject itemCountGroup;
    //Camera
    [SerializeField] GameObject stateDrivenCamera;
    Animator cameraAnimator;

    GameManager gm;
    

    void Awake()
    {
        SpawnOA();

        gm = GameManager.Instance;
        bombBoxItemButton = bombBoxButton.GetComponent<Button>();
        cameraAnimator = stateDrivenCamera.GetComponent<Animator>();
    }

    private void Start()
    {
        SetUI();
        
    }

    private void Update()
    {
        //controll
        if (gm.placementMode)
        {
            PlaceBombBox();
        }
        if (Input.GetKeyDown(KeyCode.Space) && bombBoxItemButton.interactable)
        {
            TogglePlacementMode();
        }
    }

    public void TogglePlacementMode()
    {
        SwipeToMove s2m = GameObject.FindWithTag("Player").GetComponent<SwipeToMove>();
        //placement mode
        if (gm.placementMode == false) //on
        {
            gm.placementMode = true;
            bombBoxItemButton.interactable = false;
            placementTimeBar.SetActive(true);
            placementTimeBar.GetComponent<PlacementTimeBar>().Init();
            itemCountGroup.SetActive(false);   
            gm.isMoveable = false;

            s2m.clickedButton = true;

            MasterAudio.PlaySound("BombBox_Button");
            cameraAnimator.Play("TopViewCamera");
        }
        else //off
        {
            gm.placementMode = false;
            bombBoxItemButton.interactable = GameManager.Instance.bombBoxItemCount > 0 ? true : false;
            placementTimeBar.SetActive(false);
            itemCountGroup.SetActive(true);
            gm.isMoveable = true;

            s2m.clickedButton = false;

            cameraAnimator.Play("QuarterViewCamera");
        }
        
        print($"S:{s2m.startPos} E:{s2m.endPos}");
        s2m.InitSwipe();
        //obstacle areas
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ObstacleArea");
        foreach (GameObject obj in objs) 
        { 
            ObstacleArea objScript = obj.GetComponent<ObstacleArea>();
            if (objScript.isOnPlaceMode == false) objScript.isOnPlaceMode = true;
            else objScript.isOnPlaceMode = false;
        }
    }

    public void DisableOA()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ObstacleArea");
        foreach (GameObject obj in objs)
        {
            ObstacleArea objScript = obj.GetComponent<ObstacleArea>();
            objScript.isOnPlaceMode = false;
        }

        //gameObject.SetActive(false);
    }
    
    private void PlaceBombBox()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Vector3.zero);
        int layerMask = 1 << LayerMask.NameToLayer("ObstacleArea");

        //mobile setting
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0));   
        }
        //pc setting
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        if ((Input.touchCount > 0||Input.GetMouseButtonDown(0))&&
            Physics.Raycast(ray, out hit, 100, layerMask))
        {
            ObstacleArea oAScript = hit.transform.GetComponent<ObstacleArea>();
            if (gm.placementMode && oAScript.isEnable)
            {
                Instantiate(bombBoxPfb, hit.transform.position + Vector3.up, Quaternion.identity);
                GameManager.Instance.bombBoxItemCount--;
                //anim
                bBItemCountText.GetComponent<ItemCountAnimation>().UseItemAnim();
                //audio
                MasterAudio.PlaySound("BombBox_Place");
                //setting
                TogglePlacementMode();
                SetUI();
            }     
            
        }
    }
    
    private void SpawnOA()
    {
        int startIndex = 0;
        int posZ = 4;
        for (int i = 0; i < 5; i++)
        {
            int posX = -4;
            for (int j = startIndex; j <= startIndex + 4; j++)
            {
                GameObject tile = Instantiate(obstacleAreaPfb, new Vector3(posX, 0.7f, posZ), Quaternion.identity);
                tile.GetComponent<ObstacleArea>().pointNum = j;
                posX += 2;
            }
            posZ += -2;
            startIndex += 5;
        }
    }
    
    public void SetUI()
    {
        bBItemCountText.GetComponent<TMP_Text>().text = $"x{gm.bombBoxItemCount}/{GameManager.Instance.maxbBItemCount}";

        if (gm.bombBoxItemCount == 0) bombBoxItemButton.interactable = false;
        else bombBoxItemButton.interactable = true;
    }
}

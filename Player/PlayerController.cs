using DarkTonic.MasterAudio;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(JumpToMove))]
[RequireComponent(typeof(SwipeToMove))]
public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    BoxCollider boxCollider;
    Vector3 currentPos = Vector3.zero;   
    //move
    JumpToMove jumpToMoveScript;
    float hr, vr;
    public int remainingMoveCount;
    bool moveCancel = false;
    RaycastHit hit;
    //position
    public int pointNum;

    [Header("References")]
    //particle
    [SerializeField] ParticleSystem gameoverParticle;
    public RemainingMoveCountText remainingMoveCountText_s;


    void Awake()
    {
        jumpToMoveScript = GetComponent<JumpToMove>();
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider.enabled == false) throw new System.Exception($"{nameof(PlayerController)} : collider 꺼짐");
    }

    private void Start()
    {
        remainingMoveCount = GameManager.Instance.remainingMoveCountInit;
        GetCurrentPointNum();
    }


    void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
        {
            return;
        }
        MovePC();
        Debug.DrawRay(transform.position, new Vector3(hr, 0, vr)*2,Color.red);
    
        if (!DOTween.IsTweening("BlockedMoveEffect") && !jumpToMoveScript.isMoving)
        {
            DOTween.Kill("BlockedMoveEffect", true);
        }
    }

 
    public void MovePC()
    {
        
        if (Input.GetKeyDown(KeyCode.W)||
            Input.GetKeyDown(KeyCode.S)||
            Input.GetKeyDown(KeyCode.A)||
            Input.GetKeyDown(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.W)) vr = 1;
            if (Input.GetKeyDown(KeyCode.S)) vr = -1;
            if (Input.GetKeyDown(KeyCode.A)) hr = -1;
            if (Input.GetKeyDown(KeyCode.D)) hr = 1;
            

            Move(hr, vr);
            hr = 0;
            vr = 0;

        }
            
    }
    public void MoveMobile(int h, int v)
    {
        Move(h, v);
    }

    public void Move(float h, float v)
    {
        moveCancel = false;
        //limit movement
        if (!GameManager.Instance.isMoveable || remainingMoveCount <= 0)
        {
            CancelMovement();
            //
            if (remainingMoveCount <= 0)
            {
                MasterAudio.PlaySound3DAtTransform("Player_LimitMoveCount", transform);
            }
            
        }
        Boundary(h, v);
        BlockMove(h, v);
        //move
        if (!moveCancel)
        {
            GetNextPointNum();
            //move
            jumpToMoveScript.PlayerMove(h, v);
            remainingMoveCount--;
            MasterAudio.PlaySound3DAtTransform("Player_Move", transform);
        }
    }
    void Boundary(float h,float v)
    {
        //가려는 방향에 타일이 없으면 이동 제한
        Vector3 center;
        Vector3 relativeDetectPos = new Vector3(2 * h, -1, 2 * v);
        if (jumpToMoveScript.isMoving)
        {
            center = jumpToMoveScript.nextPos + relativeDetectPos;
        }
        else center = transform.position + relativeDetectPos;

        bool detectTile = false;
        foreach (Collider collider in Physics.OverlapBox(center, transform.localScale / 2))
        {
            if (collider.CompareTag("Ground"))
            {
                detectTile = true; break;
            }
        }
        if (!detectTile)
        {
            CancelMovement();
        }
    }
    void BlockMove(float h, float v)
    {
        //가려는 방향에 차 or 장애물이 있으면 움직임 막기
        if (Physics.Raycast(transform.position, new Vector3(h, 0, v), out hit, 2f))
        {
            if (hit.collider.CompareTag("BombBox") ||
                hit.collider.GetComponent<Obstacle>() != null)
            {
                CancelMovement();
                if (hit.collider.GetComponent<Obstacle>() != null) hit.collider.GetComponent<Obstacle>().WhenCancelPlayerMove();
            }
        }
    }
    void CancelMovement()
    {
        moveCancel = true;
        remainingMoveCountText_s.CantMoveAnim();
        //BlockedMoveEffect();
    }
    void BlockedMoveEffect()
    {
        if (!DOTween.IsTweening("BlockedMoveEffect") && !jumpToMoveScript.isMoving)
        {
            currentPos = transform.position;
            transform.DOPunchPosition(new Vector3(0.2f, 0, 0), 0.3f, 30, 1).SetId("BlockedMoveEffect");
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car") || other.GetComponent<Obstacle>() != null)
        {
            GameOver();
            DOTween.Kill("MovePlayer");
            MasterAudio.PlaySound3DAtTransform("Player_CrashWCar", transform);
        }
    }
    void GameOver()
    {
        Instantiate(gameoverParticle, transform.position+Vector3.up, Quaternion.identity);
        gameObject.SetActive(false);
        GameManager.Instance.GameOver();
    }

    void GetNextPointNum()
    {
        Vector3 center = transform.position + new Vector3(2 * hr, -1, 2 * vr);
        foreach (Collider collider in Physics.OverlapBox(center, transform.localScale / 2))
        {
            if (collider.CompareTag("AreaBox"))
            {
                pointNum = collider.gameObject.GetComponent<AreaBox>().areaNum;
            }
        }
    }
    void GetCurrentPointNum()
    {
        if (Physics.Raycast(transform.position,Vector3.down,out hit))
        {
            if (hit.transform.CompareTag("AreaBox"))
            {
                pointNum = hit.transform.GetComponent<AreaBox>().areaNum;
            }
        }
    }

    public void SetOutline(bool boolean)
    {
        GetComponent<Outline>().enabled = boolean;
    }
    void Init()
    {
        GetCurrentPointNum();
    }
}

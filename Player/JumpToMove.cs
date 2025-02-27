using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpToMove : MonoBehaviour
{
    float moveSpeed = 9, rotateSpeed = 3;
    float degreeOfMove = 2;
    float moveDeg = 0, moveRad = 0;
    float playerYPos = 1.7f;

    public Vector3 nextPos;
    public bool isOnGround = false;
    public bool isMoving = false;

    Rigidbody rb;

    RemainingMoveCountText remainingMoveCountText_s;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        remainingMoveCountText_s = GetComponent<PlayerController>().remainingMoveCountText_s;
        transform.position = new Vector3(0,playerYPos,0);
    }

    void Update()
    {   
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, nextPos, moveSpeed * Time.deltaTime);

            var distance = Vector3.Distance(transform.position, nextPos);
            switch (distance)
            {
                case <= 0.01f:
                    InitMovement();
                    isMoving = false;
                    break;
                case < 0.5f:
                    moveSpeed = 11; break;
                case < 1:
                    moveSpeed = 9; break;
                case >= 1:
                    moveSpeed = 7; break;           
           }

        }
    }


    public void PlayerMove(float h,float v)
    {
        if (h != 0 && v != 0)
        {
            return;
        }

        if (isMoving)
        {
            InitMovement();          
        }
        
        nextPos = new Vector3(transform.position.x + degreeOfMove * h, playerYPos, transform.position.z + degreeOfMove * v);
        nextPos = new Vector3(Mathf.Round(nextPos.x), playerYPos, Mathf.Round(nextPos.z));
        isMoving = true;

        RotatePlayer(h,v);
        //print(moveRad);

        

        //transform.DOMove(nextPos, 0.4f).SetId("MovePlayer");
        //transform.DORotate(Vector3.up * moveAngle, 0.2f).SetId("MovePlayer");
        //transform.DOPunchScale(new Vector3(0, 0.7f, 0), 0.4f, 3, 1).SetId("MovePlayer");

        //GameManager.Instance.PlayerMoved();
    }

    void RotatePlayer(float h,float v)
    {
        if (h == 1) moveDeg = 90;
        if (h == -1) moveDeg = -90;
        if (v == 1) moveDeg = 0;
        if (v == -1) moveDeg = -180;
        moveRad = moveDeg * Mathf.Deg2Rad;

        transform.rotation = Quaternion.Euler(0, moveDeg, 0);
        remainingMoveCountText_s.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void InitMovement()
    {
        //DOTween.Kill("MovePlayer", true);
        //transform.rotation = new Quaternion(0, moveRad, 0, 0);
        //transform.DOScale(Vector3.one, 0);
        transform.position = nextPos;
    }

    private Collider[] GetAreaBoxPlayerGoing(float h,float v)
    {
        return Physics.OverlapBox(new Vector3(0,-0.4f,2), Vector3.one);
    }
}

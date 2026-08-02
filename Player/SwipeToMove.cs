using DarkTonic.MasterAudio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class SwipeToMove : MonoBehaviour
{
    PlayerController playerController;

    Touch touch;
    public Vector2 startPos = Vector2.zero;
    public Vector2 endPos = Vector2.zero;
    float swipeAngle = 0;

    public bool startedTouch;
    public bool clickedButton;

    private void Awake()
    {
        clickedButton = false;
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (clickedButton) InitSwipe();
        if (!clickedButton)
        {
            Swipe();
        }
    }

    void Swipe()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
        }

        if (Input.touchCount == 1 && !startedTouch && touch.phase == TouchPhase.Began)
        {
            startPos = touch.position;
            startedTouch = true;
        }
        if (startedTouch && touch.phase == TouchPhase.Moved)
        {
            endPos = touch.position;
        }
        if (Input.touchCount == 0 && startedTouch && Vector2.Distance(startPos,endPos) != 0 && endPos.x >= 0.01f)
        {
            startedTouch = false;
            clickedButton = false;
            if (Vector2.Distance(startPos, endPos) > 0 == false)
            {
                return;
            }
            Vector2 v2 = endPos - startPos;
            swipeAngle = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;

            if (swipeAngle > 40 && swipeAngle < 140) Main(0, 1);
            if (swipeAngle > -140 && swipeAngle < -40) Main(0, -1);
            if (swipeAngle > -40 && swipeAngle < 40) Main(1, 0);
            if (swipeAngle > 140 || swipeAngle < -140) Main(-1, 0);

            startPos = Vector2.zero; endPos = Vector2.zero;
        }
    }

    void Main(int h, int v)
    {
        try
        {
            if (Input.touchCount > 1) return;
            playerController.MoveMobile(h, v);
        }
        catch { }
        MasterAudio.PlaySound3DAtTransform("Player_Move", transform);
    }

    public void InitSwipe()
    {
        startedTouch = false;
        startPos = Vector2.zero;
        endPos = Vector2.zero;
    }
}

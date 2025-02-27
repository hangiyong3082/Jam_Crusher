using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class PlayerMoveButton : MonoBehaviour
{
    PlayerController playerController_s;

    private void Awake()
    {
       
    }

    private void Update()
    {
        try
        {
            playerController_s = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }
        catch { } 
    }
    public void MoveUp()
    {
        Main(0, 1);
    }
    public void MoveDown()
    {
        Main(0, -1);
    }
    public void MoveRight()
    {
        Main(1, 0);
    }
    public void MoveLeft()
    {
        Main(-1, 0);
    }

    void Main(int h,int v)
    {
        try
        {
            if (Input.touchCount > 1) return;
            playerController_s.MoveMobile(h, v);
        }
        catch { }
    }
}

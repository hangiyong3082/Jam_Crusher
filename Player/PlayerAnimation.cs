using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAnimState
{
    None,
    Down,
    Up
}

[Obsolete("사용되지 않음")]
public class PlayerAnimation:MonoBehaviour
{
    float playerYPos = 1.7f;
    public bool isMoving;

    public PlayerAnimState state;
    public Vector3 currentTarget;
    Vector3 goingDownTarget = new Vector3(1.2f, 0.8f, 1.2f);
    Vector3 goingUpTarget = Vector3.one;
    public float currentDuration;
    float goingDownDuration = 1f;
    float goingUpDuration = 1.1f;

    private void Awake()
    {
        currentTarget = goingUpTarget;
    }

    void MovingAnim()
    {
        //if (!isMoving) return;
        float previousYScale = transform.localScale.y;
        Vector3 currentVelocity = Vector3.zero;

        //transform.localScale = Vector3.SmoothDamp(transform.localScale, currentTarget, ref currentVelocity,currentDuration);
        transform.localScale = Vector3.Lerp(transform.localScale, currentTarget, 5*Time.deltaTime);
        transform.position += Vector3.down * (previousYScale - transform.localScale.y)/2;
    }

    IEnumerator SwitchState()
    {
        state = PlayerAnimState.Down;
        currentDuration = goingDownDuration;
        currentTarget = goingDownTarget;

        yield return new WaitForSeconds(goingDownDuration);

        state = PlayerAnimState.Up;
        currentDuration = goingUpDuration;
        currentTarget = goingUpTarget;

        yield return new WaitForSeconds(goingUpDuration);

        state = PlayerAnimState.None;
        isMoving = false;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isMoving)
            {
                isMoving = false;
                StopCoroutine(SwitchState());
            }
            else
            {
                isMoving = true;
                StartCoroutine(SwitchState());
                
            }
                
        }

        MovingAnim();
    }
}

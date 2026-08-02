using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;

public class SwipeDetection : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Vector2 startPos;
    public bool swiping = false;
    public bool swipped = false;
    public float swipingTime;
    float swipingTimeInit = 0.4f;

    private enum DraggedDirection
    {
        Up,
        Down,
        Right,
        Left
    }

    private DraggedDirection GetDragDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        DraggedDirection draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
        }
        swipped = false;
        if (positiveX > 50 || positiveY > 50)
        {
            swipped = true;
        }
        return draggedDir;
    }

    void Task(DraggedDirection draggedDir)
    {
        PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>(); 
        if (swipped)
        {
            switch (draggedDir)
            {
                case DraggedDirection.Left:
                    playerController.Move(-1, 0);
                    break;
                case DraggedDirection.Right:
                    playerController.Move(1, 0);
                    break;
                case DraggedDirection.Up:
                    playerController.Move(0, 1);
                    break;
                case DraggedDirection.Down:
                    playerController.Move(0, -1);
                    break;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = eventData.position;
        SwipingTimerStart();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!swiping) return;
        Vector2 dragVectorDirection = eventData.position - startPos;
        Task(GetDragDirection(dragVectorDirection));
    }

    void SwipingTimerStart()
    {
        swipingTime = swipingTimeInit;
        swiping = true;
    }

    void SwipingTimer()
    {
        if (!swiping) return;

        if (swipingTime > 0)
        {
            swipingTime -= Time.deltaTime;
        }
        else
        {
            swipingTime = 0;
            swiping = false;
        }
    }

    private void Update()
    {
        SwipingTimer();
    }
}

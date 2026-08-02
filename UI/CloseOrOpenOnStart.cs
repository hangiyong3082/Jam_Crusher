using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseOrOpenOnStart : MonoBehaviour
{
    enum State
    {
        Open,
        Close
    }
    [SerializeField] State state;

    private void Start()
    {
        gameObject.SetActive(state == State.Open ? true : false);
    }
}

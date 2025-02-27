using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMoveButtons : MonoBehaviour
{
    public void Work(bool boolean)
    {
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i).GetComponent<Button>().interactable = boolean;
        }
    }
}

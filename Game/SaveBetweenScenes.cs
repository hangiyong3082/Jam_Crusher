using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveBetweenScenes : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}

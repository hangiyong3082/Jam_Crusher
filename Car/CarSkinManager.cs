using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSkinManager : Singleton<CarSkinManager>
{
    /// <summary>
    /// Classic, 
    /// </summary>
    public string currentSkinType;

    private void Awake()
    {
        currentSkinType = "Classic";
    }
}

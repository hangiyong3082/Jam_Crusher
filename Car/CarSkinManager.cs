using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CarSkinManager",menuName = "CarSkin Manager")]
public class CarSkinManager : ScriptableObject
{
    [SerializeField] public CarSkin[] carSkins;

    
}

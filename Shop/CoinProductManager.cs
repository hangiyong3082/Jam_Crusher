using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CoinProductManager", menuName = "CoinProduct Manager")]
public class CoinProductManager : ScriptableObject
{
    [SerializeField] public CoinProduct[] items;
}

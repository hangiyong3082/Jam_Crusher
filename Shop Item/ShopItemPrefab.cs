using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemPrefab : MonoBehaviour
{
    [SerializeField] SkinType skinType;
    [SerializeField] new string name;
    [HideInInspector] public string Name { get; private set; }

    private void Awake()
    {
        Name = skinType.ToString() + name;
    }


}

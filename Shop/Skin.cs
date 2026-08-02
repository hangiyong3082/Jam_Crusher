using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public enum SkinType
{
    Pattern,
    Cosmetic,
    Aura,
    Theme,
}

[System.Serializable]
public class Skin
{
    [Header("References")]
    public string name;
    public int cost;
    public SkinType type;
    public Sprite profileImg;
    public bool isOriginal = false;
    public LocalizedString localizedString;
    [Header("Select one thing")]
    public Material pattern;
    public GameObject cosmetic;
    public GameObject aura;
    public GameObject theme;
    
}

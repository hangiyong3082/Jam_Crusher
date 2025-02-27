using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkinType
{
    Pattern,
    Costemic,
    Theme,
}

[System.Serializable]
public class Skin
{
    [Header("References")]
    public int cost;
    public SkinType type;
    public Sprite profileImg;
    [Header("Select one thing")]
    public Material pattern;
    public GameObject costemic;
    public GameObject theme;
}

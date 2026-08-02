using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarSkin
{
    [Header("ThemeType과 같아야 함")]
    public string skinName;
    [Header("스킨 그룹에 있는 차 모델들의 순서에 맞춰서 할당")]
    public CarColorGroup[] smallCarColorGroups;
    public CarColorGroup[] mediumCarColorGroups;
    public CarColorGroup[] largeCarColorGroups;

    [System.Serializable]
    public struct CarColorGroup
    {
        public Material[] colors;
    }
}


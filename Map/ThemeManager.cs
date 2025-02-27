using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ThemeType
{
    Traffic,
    Desert,
}
public class ThemeManager : MonoBehaviour
{
    [SerializeField] public ThemeType themeType;
    
}

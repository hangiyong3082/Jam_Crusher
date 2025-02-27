using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinManager", menuName = "Skin Manager")]
public class SkinManager : ScriptableObject
{
    [SerializeField] public SkinType skinType;
    [SerializeField] public Skin[] skins;

    string GetSkinStateName(int skinIndex)
    {
        return skinType.ToString() + "_" + skinIndex.ToString();
    }
    string GetSelectedSkinName()
    {
        return "Selected" + skinType.ToString();
    }

    public void SelectSkin(int skinIndex) => PlayerPrefs.SetInt(GetSelectedSkinName(), skinIndex);

    public Skin GetSelectedSkin()
    {
        int skinIndex = PlayerPrefs.GetInt(GetSelectedSkinName(), 0);
        if (skinIndex >= 0 && skinIndex < skins.Length)
        {
            return skins[skinIndex];
        }
        else
        {
            return null;
        }
    }

    public void Unlock(int skinIndex) => PlayerPrefs.SetInt(GetSkinStateName(skinIndex), 1);

    public bool IsUnlocked(int skinIndex) => PlayerPrefs.GetInt(GetSkinStateName(skinIndex), 0) == 1;
}

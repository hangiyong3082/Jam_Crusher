using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "SkinManager", menuName = "Skin Manager")]
public class SkinManager : ScriptableObject
{
    [SerializeField] public SkinType skinType;
    [SerializeField] public List<Skin> skins;

    public string GetSkinStateName(int skinIndex)
    {
        return skinType.ToString() + "_" + skinIndex.ToString();
    }
    public string GetSkinStateName(string skinName)
    {
        return skinType.ToString() + "_" + skins.FindIndex(s => s.name == skinName).ToString();
    }
    string GetSelectedSkinType()
    {
        return "Selected" + skinType.ToString();
    }

    public void SelectSkin(int skinIndex) => PlayerPrefs.SetInt(GetSelectedSkinType(), skinIndex);

    public Skin GetSelectedSkin()
    {
        int skinIndex = PlayerPrefs.GetInt(GetSelectedSkinType(), 0);
        if (skinIndex >= 0 && skinIndex < skins.Count)
        {
            return skins[skinIndex];
        }
        else
        {
            return null;
        }
    }

    public void Unlock(int skinIndex)
    {
        Debug.Log(GetSkinStateName(skinIndex) + " 잠금해제");
        PlayerPrefs.SetInt(GetSkinStateName(skinIndex), 1);
    }

    public void Unlock(string skinName)
    {
        Debug.Log(GetSkinStateName(skinName) + " 잠금해제");
        PlayerPrefs.SetInt(GetSkinStateName(skinName), 1);
    }

    public bool IsUnlocked(int skinIndex) => PlayerPrefs.GetInt(GetSkinStateName(skinIndex), 0) == 1;
}

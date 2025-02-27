using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ShopController : MonoBehaviour
{
    [SerializeField] private TMP_Text coinsText;
    [Header("순서대로")]
    [SerializeField] private SkinManager[] skinManagers;

    void Update()
    {
        coinsText.text = PlayerPrefs.GetInt("Coins").ToString();
    }

    public void SetSkin()
    {
        foreach (var skinManager in skinManagers)
        {
            var player = GameObject.FindWithTag("Player");
            switch (skinManager.skinType)
            {
                case SkinType.Pattern:
                    player.GetComponent<MeshRenderer>().material = skinManager.GetSelectedSkin().pattern;
                    return;
                case SkinType.Costemic:
                    Instantiate(skinManager.GetSelectedSkin().costemic,player.transform);
                    return;
                case SkinType.Theme:
                    return;
            }
        }
    }
}

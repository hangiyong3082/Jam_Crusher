using DarkTonic.MasterAudio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SettingUI: MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject vibrationMain;
    [SerializeField] Sprite vibrationOn;
    [SerializeField] Sprite vibrationOff;
    [SerializeField] GameObject vibrationImg;
    [SerializeField] GameObject languageOption;
    [SerializeField] GameObject languageContent;
    [SerializeField] GameObject languageMain;

    bool vibration;

    private void OnEnable()
    {
        InitSetting();
    }

    private void Awake()
    {
        InitSetting();
        AddListener2Button(); 
    }

    private void Update()
    {
        //update vibration
        int _vibration = PlayerPrefs.GetInt("Vibration", 0);
        vibration = _vibration == 0 ? false : true;
        vibrationImg.GetComponent<Image>().sprite = vibration ? vibrationOn : vibrationOff;

    }
    #region 시작
    void InitSetting()
    {
        languageOption.SetActive(false);
    }

    void AddListener2Button()
    {
        Action buttonClick = () => { UIManager.Instance.ClickSound(); };
        vibrationMain.GetComponent<Button>().onClick.AddListener(delegate { buttonClick(); ToggleVibration(); });
        languageMain.GetComponent<Button>().onClick.AddListener(delegate { buttonClick();  ToggleLanguageOptionWdw(); });
        for (int i = 0; i < languageContent.transform.childCount; i++)
        {
            languageContent.transform.GetChild(i).GetComponent<Button>()
                .onClick.AddListener(delegate { buttonClick(); ToggleLanguageOptionWdw();});
        }
    }
    #endregion

    #region 버튼 실행 함수
    void ToggleVibration()
    {
        vibration = !vibration;

        GameManager.Instance.isVibrate = vibration;

        int _vibration = vibration == false ? 0 : 1;
        PlayerPrefs.SetInt("Vibration", _vibration);
    }

    void ToggleLanguageOptionWdw()
    {
        languageOption.SetActive(!languageOption.activeSelf);
    }

    #endregion 

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Button resumeButton;
    [SerializeField] Button mainmenuButton;

    public void ButtonSetup()
    {
        resumeButton.onClick.AddListener(delegate
        {
            GameManager.Instance.ResumeGame();
            UIManager.Instance.settingButton.gameObject.SetActive(false);
            gameObject.SetActive(false);
            UIManager.Instance.pauseButton.gameObject.SetActive(true);
        });

        mainmenuButton.onClick.AddListener(delegate
        {
            GameManager.Instance.LoadMenu();
        });
    }
}

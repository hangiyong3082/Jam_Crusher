using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingButton : MonoBehaviour
{
    [SerializeField] GameObject menuSettingUI, menuBottonUI;

    private void Start()
    {
        if (menuSettingUI.activeSelf == true) menuSettingUI.SetActive(false);
    }
    public void Active()
    {
        menuSettingUI.SetActive(true);
    }

    public void ToggleSettingUI()
    {
        if (menuSettingUI.activeSelf == true)
        {
            
            menuSettingUI.SetActive(false);
            //일단 지금은 안 씀
            //menuBottonUI.SetActive(true);
        }
        else
        {
            menuSettingUI.SetActive(true);
            //일단 지금은 안 씀
            //menuBottonUI.SetActive(false);


        }
    }
}

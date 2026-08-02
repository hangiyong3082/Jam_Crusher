using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateChecker : MonoBehaviour
{
    private const string dateKey = "LastSavedDate";
    private bool _initialized = false;
    private bool _isNewDate = false;

    public bool isNewDate
    {
        get
        {
            if (!_initialized) throw new InvalidOperationException("DateChecker √ ±‚»≠ æ» µ !");
            return _isNewDate;
        }
    }

    private void Awake()
    {
        CheckDate();
        print(CheckDate());
    }

    public string CheckDate()
    {
        string lastDate = PlayerPrefs.GetString(dateKey, "");
        string todayDate = DateTime.Now.ToShortDateString();

        if (lastDate != todayDate)
        {
            PlayerPrefs.SetString(dateKey, todayDate);
            PlayerPrefs.Save();
            _isNewDate = true;          
        }
        _initialized = true;

        return $"lastDate : {lastDate}, todayDate : {todayDate}";
    }
}

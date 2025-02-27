using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableTileSpnList : Singleton<AvailableTileSpnList>
{
    public List<int> list = new List<int>();

    bool initSetngCompleleted = false;

    private void Awake()
    {
        InitialSetting();
    }
    /// <summary>
    /// 무조건!!
    /// </summary>
    void InitialSetting()
    {
        for (int i = 0; i < Mathf.Pow(GameManager.Instance.tileCount, 2); i++)
        {
            list.Add(i);
        }
        initSetngCompleleted = true;
    }

    void CheckInitSetng()
    {
        if (!initSetngCompleleted)
        {
            throw new System.Exception($"{nameof(InitialSetting)} 실행 안 함");
        }
    }

    public void ResetSpn()
    {
        CheckInitSetng();
        list.Clear();
        for (int i = 0; i < Mathf.Pow(GameManager.Instance.tileCount, 2); i++)
        {
            list.Add(i);
        }
    }
    public void ExcludeSpn(int spn)
    {
        CheckInitSetng();
        
        if (list.Contains(spn)) list.Remove(spn);

    }
    public void ReturnSpn(int spn)
    {
        CheckInitSetng();

        if (!list.Contains(spn)) list.Add(spn);
       
    }
    public void CheckList()
    {
        string str = "";
        foreach (var i in list)
        {
            str += " " + i.ToString();
        }
        Debug.Log(str);
    }

    public int RandomSpn()
    {
        return list[Random.Range(0 ,list.Count)];
    }

    
}

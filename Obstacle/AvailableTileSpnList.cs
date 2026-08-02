using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableTileSpnList : Singleton<AvailableTileSpnList>
{
    public List<int> list = new List<int>();

    bool initSetngCompleleted = false;

    GameManager gameManager = null;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        InitialSetting();
        
    }
    /// <summary>
    /// 무조건!!
    /// </summary>
    void InitialSetting()
    {
        for (int i = 0; i < Mathf.Pow(gameManager.tileCount, 2); i++)
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

    public int GetSpn(Vector3 pos)
    {
        int result = 0;
        int tileSize = gameManager.tileScale;
        int tileCount = gameManager.tileCount;
        float posZ = Mathf.Round(pos.z), posX = Mathf.Round(pos.x);
        float minSpnPos = tileCount / 2 * tileSize * -1;

        result = (int)((posZ / -2 + 2)*tileCount + (posX / 2 + 2));

        return result;
    }

    public void ResetSpn()
    {
        CheckInitSetng();
        list.Clear();
        for (int i = 0; i < Mathf.Pow(gameManager.tileCount, 2); i++)
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

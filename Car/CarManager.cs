using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : Singleton<CarManager>
{
    public List<int> inGameCarsSpn = new List<int>();

    private void Update()
    {
        
    }

    public void AddSpn(int spn)
    {
        inGameCarsSpn.Add(spn);
    }

    public void RemoveSpn(int spn)
    {
        inGameCarsSpn.Remove(spn);
    }
}

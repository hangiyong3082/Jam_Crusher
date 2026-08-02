using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCarJamManager : Singleton<DetectCarJamManager>
{
    public GameObject[] detectedCars = new GameObject[21];
    public List<int> blockedCarsSpnList = new List<int>();
    public List<int> blockingCarsSpnList = new List<int>();

    public List<int> matchedSpnList = new List<int>();

    /// <summary>
    /// <para> GameManger에서 사용</para>
    /// blocked/ingCarsSpn에서 일치하는값이 4개라면 막힘 현상이 일어났다고 인식.
    /// </summary>
    public void DetectCarJam()
    {
        #if !DEBUG_DETECT_CARJAM
        return;
        #endif

        for (int i = 0; i < blockedCarsSpnList.Count; i++)
        {
            if (blockingCarsSpnList.Contains(blockedCarsSpnList[i]))
            {
                matchedSpnList.Add(blockedCarsSpnList[i]);
            }
        }
        if (matchedSpnList.Count == 4)
        {
            for (int i = 0; i < matchedSpnList.Count; i++)
            {
                //detectedCars[matchedSpnList[i]].GetComponent<Car>().DestroyCar();
            }
        }
        InitLists();
    }

    void InitLists()
    {
        for (int i = 0; i < detectedCars.Length; i++)
        {
            detectedCars[i] = null;
        }
        blockedCarsSpnList.Clear();
        blockingCarsSpnList.Clear();
        matchedSpnList.Clear();
    }

    /// <summary>
    /// <para>Car에서 사용</para>
    /// detectedCars배열에 막히거나 막은 차를 할당하고, blocked/ingCarsSpn리스트에 그 차들의 spn을 넣음
    /// </summary>
    /// <param name="blockedCar">막힌 차</param>
    /// <param name="blockingCar">막고있는 차</param>
    public void AddDetectedCars(GameObject blockedCar, GameObject blockingCar)
    {
        #if !DEBUG_CARJAM   
        return;
        #endif

        int blockedCarSpn = blockedCar.GetComponent<Car>().spn;
        int blockingCarSpn = blockingCar.GetComponent<Car>().spn;

        detectedCars[blockedCarSpn] = blockedCar;
        detectedCars[blockingCarSpn] = blockingCar;

        if (!blockedCarsSpnList.Contains(blockedCarSpn))
        {
            blockedCarsSpnList.Add(blockedCarSpn);
        }
        if (!blockingCarsSpnList.Contains(blockingCarSpn))
        {
            blockingCarsSpnList.Add(blockingCarSpn);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    [SerializeField] GameObject mainObj;
    [SerializeField] GameObject[] inactivedObjs;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(delegate { Work(); });
    }

    void Work()
    {
        foreach (var obj in inactivedObjs)
        {
            obj.SetActive(true);
        }
        mainObj.SetActive(false);
    }
}

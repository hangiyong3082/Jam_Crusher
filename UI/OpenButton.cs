using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OpenButton : MonoBehaviour
{
    [SerializeField] GameObject mainObj;
    [SerializeField] GameObject[] Objs2BInactived;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(delegate { Work(); });
    }

    void Work()
    {
        foreach (var obj in Objs2BInactived)
        {
            obj.SetActive(false);
        }
        mainObj.SetActive(true);
    }
}

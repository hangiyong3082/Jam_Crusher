using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTransparent : MonoBehaviour
{
    [SerializeField]
    int partNumber;
    [SerializeField] GameObject mainObject; //자식 오브젝트

    private void Start()
    {
        mainObject.SetActive(false);
    }

    private void Update()
    {
        SetCarTransparent();
    }

    public void SetCarTransparent()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, Vector3.up * 5);
        foreach (Collider collider in colliders)
        {
            if (collider.transform.CompareTag("Ground"))
            {
                mainObject.SetActive(true);
                break;
            }
            else
            {
                mainObject.SetActive(false);
            }
        }
    }
}

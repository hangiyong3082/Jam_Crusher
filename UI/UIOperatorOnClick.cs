using UnityEngine;
using UnityEngine.UI;

public class UIOperatorOnClick : MonoBehaviour
{
    [SerializeField] GameObject[] beingActivedObjects;
    [SerializeField] GameObject[] beingInactivedObjects;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate { ActiveObjects(); });
        GetComponent<Button>().onClick.AddListener(delegate { InactiveObjects(); });

    }

    void ActiveObjects()
    {
        if (beingActivedObjects.Length == 0) return;
        foreach (GameObject go in beingActivedObjects)
        {
            go.SetActive(true);
        }
    }
    void InactiveObjects()
    {
        if (beingInactivedObjects.Length == 0) return;
        foreach (GameObject go in beingInactivedObjects)
        {
            go.SetActive(false);
        }
    }
}

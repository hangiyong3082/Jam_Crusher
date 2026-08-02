using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoalBox : MonoBehaviour
{
    [SerializeField] ParticleSystem spawnParticle;
    [SerializeField] ParticleSystem goalParticle;
    GoalBoxManager goalBoxManager;

    public int pointNum;

    private void Awake()
    {
        goalBoxManager = GameObject.Find("GoalBoxManager").GetComponent<GoalBoxManager>();
    }

    private void Start()
    {
        Instantiate(spawnParticle,transform.position+Vector3.up*2,Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            goalBoxManager.StartCoroutine("SpawnGoalBox");
            goalBoxManager.ReturnSpawnPointNum(pointNum);
            GameManager.Instance.AddScore(1);

            //Instantiate(goalParticle, transform.position + Vector3.up * 2, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

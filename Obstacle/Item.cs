using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ParticleSystem spawnParticle;
    [SerializeField] ParticleSystem takenParticle;

    ParticleSystem spawnParticle_i;
    //ui

    public int spn;

    private void Awake()
    {

    }

    private void Start()
    {
        //spawnParticle_i = Instantiate(spawnParticle, transform.position + Vector3.up * 0.5f, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //variable
            GameManager.Instance.bombBoxItemCount++;
            AvailableTileSpnList.Instance.ReturnSpn(spn);
            ItemManager.Instance.itemsOnRoadCount--;
            //ui
            ItemManager.Instance.bBitemCountText.GetComponent<ItemCountAnimation>().AddItemAnim();
            BombBoxManager.Instance.SetUI();
            
            //effect
            Instantiate(takenParticle, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            //sfx
            MasterAudio.PlaySound3DAtTransform("BombBox_PickedUp", transform);
            //spawnParticle_i.Stop();
            
            Destroy(gameObject);
        }
    }
}

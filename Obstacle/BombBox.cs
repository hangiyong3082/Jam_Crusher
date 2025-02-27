using DarkTonic.MasterAudio;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BombBox : MonoBehaviour
{
    [SerializeField] GameObject normalBombBox;
    [SerializeField] GameObject damagedBombBox;

    [SerializeField] GameObject bombScoreTextPfb;
    [SerializeField] ParticleSystem crashEffect;
    [SerializeField] ParticleSystem explodeEffect;

    [HideInInspector] public int health = 2;
    int scoreOfEachPart = 1;

    public void Work()
    {
        if (health > 1)
        {
            normalBombBox.SetActive(true);
            damagedBombBox.SetActive(false);
        }
        else if (health == 1)
        {
            normalBombBox.SetActive(false);
            damagedBombBox.SetActive(true);
        }
        else if (health <= 0)
        {
            //when explode
            Collider[] colliders = Physics.OverlapBox(transform.position, Vector3.one * 3f);
            foreach (Collider collider in colliders)
            {
                if (collider.transform.CompareTag("Car") && collider.GetComponent<Car>().turnCountAfterSpawn > 0)
                {
                    //setting
                    var carScript = collider.GetComponent<Car>();
                    int scoreToAdded = scoreOfEachPart * carScript.carLength;
                    //add score
                    GameManager.Instance.AddScore(scoreToAdded);
                    //visual score
                    var instantiatedBST = 
                        Instantiate(bombScoreTextPfb,collider.transform.position+Vector3.up*2, bombScoreTextPfb.transform.rotation);
                    instantiatedBST.GetComponent<TextMeshPro>().text = $"+{scoreToAdded}";
                    Destroy(instantiatedBST, 2);
                    //destroy the car
                    carScript.DestroyCar(false);
                }
                else if (collider.transform.GetComponent<Obstacle>() != null)
                {
                    collider.GetComponent<Obstacle>().Crashed(collider.GetComponent<Obstacle>().initialhealth);
                }
            }   
            ExplodeEffect();
            //score text effect
            var gamescoreText = GameObject.Find("ScoreText_");
            DOTween.Kill("ScoreTextAnim", true);
            gamescoreText.transform.DOScale(0.25f, 0.3f).SetRelative(true)
                .SetLoops(2, LoopType.Yoyo).SetId("ScoreTextAnim");

            Destroy(gameObject);
        }
    }

    public void Crashed()
    {
        health--;
        Instantiate(crashEffect,transform.position+Vector3.up*0.5f, Quaternion.identity);
        //sfx
        if (health > 0)
        {
            MasterAudio.PlaySound3DAtTransform("BombBox_Hit", transform);
        }
    }

    public void ExplodeEffect()
    {
        Instantiate(explodeEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        MasterAudio.PlaySound3DAtTransform("BombBox_Explosion", transform);
        GameManager.Instance.Vibrate();
    }
}

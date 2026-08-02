using DarkTonic.MasterAudio;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class BombBox : MonoBehaviour
{
    [SerializeField] GameObject[] bombBoxModelsByHealth;
    [SerializeField] GameObject model;
    [SerializeField] GameObject bombScoreTextPfb;
    [SerializeField] ParticleSystem crashEffect;
    [SerializeField] ParticleSystem alternativeCrashEffect;
    [SerializeField] ParticleSystem explodeEffect;
    [SerializeField] ParticleSystem _carDebrisEffect;

    [HideInInspector] public int health = 2;
    [ReadOnly] public int spn = -1;
    int scoreOfEachPart = 1;

    GameManager gameManager = null;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void Start()
    {
        SetModel();
    }

    void SetModel()
    {
        for (int i = 0; i < bombBoxModelsByHealth.Length; i++)
        {
            bombBoxModelsByHealth[i].SetActive(i == health -1);
        }
       
    }

    public void Work()
    {
        if (health >= 1)
        {
            SetModel();
        }
        else if (health <= 0)
        {
            //when explode
            int explodedCarCnt = 0;
            int explodedObsCnt = 0;
            int carDebrisCnt = 0;
            Collider[] colliders = Physics.OverlapBox(transform.position, Vector3.one * 3f);
            foreach (Collider collider in colliders)
            {
                if (collider.transform.CompareTag("Car") && collider.GetComponent<Car>().turnCountAfterSpawn > 0)
                {
                    //setting
                    var carScript = collider.GetComponent<Car>();
                    int scoreToAdded = scoreOfEachPart * carScript.carLength;
                    carDebrisCnt += scoreToAdded;
                    //add score
                    if (!gameManager.isTutorial)
                    {
                        gameManager.AddScore(scoreToAdded);

                    }
                    //visual score
                    var instantiatedBST =
                        Instantiate(bombScoreTextPfb, collider.transform.position + Vector3.up * 2, bombScoreTextPfb.transform.rotation);
                    instantiatedBST.GetComponent<TextMeshPro>().text = $"+{scoreToAdded}";
                    Destroy(instantiatedBST, 2);
                    //destroy the car
                    carScript.DestroyCar(false, true);
                    explodedCarCnt++;
                }
                else if (collider.transform.GetComponent<Obstacle>() != null)
                {
                    collider.GetComponent<Obstacle>().Crashed(collider.GetComponent<Obstacle>().initialhealth);
                    explodedObsCnt++;
                }
                else if (collider.transform.GetComponent<PlayerController>() != null)
                {
                    //collider.GetComponent<PlayerController>().GameOver();
                }
            }   
            
            //score text effect
            var gamescoreText = GameObject.Find("ScoreText_");
            PublicFunctions.UIEffect(gamescoreText, this,"ScoreText", UIAnim.ScaleHighlight);

            FindObjectOfType<TutorialManager>().BombMission(explodedCarCnt);
            FindObjectOfType<TutorialManager>().BreakObsMission(explodedObsCnt);
            Destroy(gameObject);
            ExplodeEffect(carDebrisCnt);
            AvailableTileSpnList.Instance.ReturnSpn(spn);
        }
    }

    public void Crashed()
    {
        health--;
        //Instantiate(crashEffect,transform.position+Vector3.up*0.5f, Quaternion.identity);
        Instantiate(alternativeCrashEffect,transform.position+Vector3.up*1f, alternativeCrashEffect.transform.rotation);
        //anim
        model.transform.DOPunchPosition(Vector3.up * 0.3f, 0.3f, 1, 0);
        //sfx
        if (health > 0)
        {
            MasterAudio.PlaySound3DAtTransform("BombBox_Hit", transform);
        }
    }

    public void ExplodeEffect(int explodedObjCnt)
    {
        if (explodeEffect != null)
            Instantiate(explodeEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        var carDebrisEffect = Instantiate(_carDebrisEffect, transform.position + Vector3.up * 1f, _carDebrisEffect.transform.rotation);
        //carDebrisEffect.Emit(explodedObjCnt);
        carDebrisEffect.Play();
        MasterAudio.PlaySound3DAtTransform("BombBox_Explosion", transform);
        gameManager.Vibrate();
    }
}

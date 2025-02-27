using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpawnDelayer))]
public class Obstacle : MonoBehaviour
{
    //[Header("References")]
    protected GameObject model;
    BoxCollider cd;
    SpawnDelayer spawnDelayerScript;
    Tweener crashedAnim;
    string crashedAnimID = "ObstacleCrashedAnim";

    [Header("Settings")]
    public int countToReveal; //±âº» : 2
    public bool isPassableForCar;
    [Tooltip("-1 : ½º½º·Î ¾È ±úÁü")] [SerializeField] int _countToDestroy;
    /// <summary>
    /// (À§¿¡ Â÷ ÀÖÀ» ¶§ °æ¿ì Ã³¸®µÊ)
    /// </summary>
    public int countToDestroy
    {
        get => _countToDestroy;
        set
        {
            if (countToDestroy == -1) _countToDestroy = -1;
            else _countToDestroy = value;
        }
    }   
    [Tooltip("-1 : ¹«Àû")] public int initialhealth; 
    

    [Header("Public")]
    public int pointNum;
    public int health;
    
    public virtual void Awake()
    {
        spawnDelayerScript = GetComponent<SpawnDelayer>();
        cd = GetComponent<BoxCollider>();
        model = transform.Find("Model_").gameObject;
        health = initialhealth;
        //if (model.activeSelf == true) throw new System.Exception("model_ ²¨¾ßµÊ");
        if (model.GetComponent<DOTweenAnimation>().autoPlay == true) throw new System.Exception("autoplay ²¨¾ßµÊ");
        if (transform.Find("DangerArea_").tag == null) throw new System.Exception("obstacledangerarea ÅÂ±× ºÙ¿©¾ßµÊ");
        //crashedAnim = transform.DOShakePosition(0.3f, strength: 0.2f, vibrato: 30).SetId(crashedAnimID);
    }
    
    public virtual void Start()
    {
        cd.enabled = false;
        model.SetActive(false);
        spawnDelayerScript.remainingCount = countToReveal;
    }

    public virtual void Work()
    {
        if (countToReveal > -1) countToReveal--;
        spawnDelayerScript.remainingCount = countToReveal;

        if (countToReveal <= -1)
        {
            countToDestroy--;
        }
        else if (countToReveal <= 0)
        {
            Reveal(); 
        }
        DestroyAccToHealth();
        DestroyAccToCount();
    }

    public virtual void Reveal()
    {
        cd.enabled = true;
        model.SetActive(true);
        foreach (var dotAnims in model.GetComponents<DOTweenAnimation>())
            dotAnims.DOPlay();
    }

    public virtual void Crashed(int damage = 1)
    {
        if (initialhealth == -1)
        {
            return;
        }
        health -= damage;

        DOTween.Kill(crashedAnimID,true);
        transform.DOShakePosition(0.3f, strength: 0.2f, vibrato: 30).SetId(crashedAnimID);
        DestroyAccToHealth();
    }

    public virtual void WhenCancelPlayerMove()
    {

    }

    public void PlayParticle(GameObject obj)
    {
        obj.GetComponent<ParticleSystem>().Play();
    }

    void DestroyAccToHealth() //AccTo : according to
    {
        if (model.activeSelf == true && health == 0)
        {
            DestroyObstacle();
        }
    }

    void DestroyAccToCount()
    {
        if (model.activeSelf == true && countToDestroy == 0)
        {
            DestroyObstacle();
        }
    }

    public virtual void DestroyObstacle()
    {
        Destroy(gameObject);
        AvailableTileSpnList.Instance.ReturnSpn(pointNum);
        DOTween.Kill(crashedAnimID);
    }
}

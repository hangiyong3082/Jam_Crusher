using DarkTonic.MasterAudio;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBannedSign : Obstacle
{
    [Header("Reference")]
    [SerializeField] GameObject bannedIcon;
    [SerializeField] GameObject signModel;
    
    string countAnimId;
    string bannedAnimID;

    public override void Awake()
    {
        base.Awake();
        bannedAnimID = nameof(PlayerBannedSign) + nameof(WhenCancelPlayerMove);
        countAnimId = PublicFunctions.GetAnimId(this, "goDown" + Random.Range(0f, 1f).ToString());

    }
    public override void Start()
    {
        base.Start();
        bannedIcon.SetActive(false);
    }

    public override void Work()
    {  
        base.Work();
        if (countToReveal <= -1)
        {
            DOTween.Kill(countAnimId, true);
            signModel.transform.DOMoveY(-0.7f, 0.3f).SetRelative().SetEase(Ease.OutBounce).SetId(countAnimId);

        }

    }

    public override void Reveal()
    {
        base.Reveal();
        bannedIcon.SetActive(true);
        foreach (var dotAnims in bannedIcon.GetComponents<DOTweenAnimation>())
            dotAnims.DOPlay();

        MasterAudio.PlaySound("Object_SpawnSign");
    }

    public override void WhenCancelPlayerMove()
    {
        base.WhenCancelPlayerMove();
        DOTween.Complete(bannedAnimID);
        model.transform.DOPunchScale(new Vector3(0.2f, 0, 0.2f), 0.3f, vibrato: 1).SetId(bannedAnimID); //스크립트 이릅이랑 함수 앞 글자 따온거임
        MasterAudio.PlaySound3DAtTransform("Player_BannedMovement", transform);
    }

    public override void DestroyObstacle()
    {
        base.DestroyObstacle();
        DOTween.Kill(bannedAnimID);
    }
}

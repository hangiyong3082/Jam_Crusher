using DarkTonic.MasterAudio;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBannedSign : Obstacle
{
    [Header("Reference")]
    [SerializeField] GameObject bannedIcon;

    string bannedAnimID;

    public override void Awake()
    {
        base.Awake();
        bannedAnimID = nameof(PlayerBannedSign) + nameof(WhenCancelPlayerMove);
    }
    public override void Start()
    {
        base.Start();
        bannedIcon.SetActive(false);
    }

    public override void Reveal()
    {
        base.Reveal();
        bannedIcon.SetActive(true);
        foreach (var dotAnims in bannedIcon.GetComponents<DOTweenAnimation>())
            dotAnims.DOPlay();
    }

    public override void WhenCancelPlayerMove()
    {
        base.WhenCancelPlayerMove();
        model.transform.DOPunchScale(new Vector3(0.2f, 0, 0.2f), 0.3f, vibrato: 1).SetId(bannedAnimID); //스크립트 이릅이랑 함수 앞 글자 따온거임
        MasterAudio.PlaySound3DAtTransform("Player_BannedMovement", transform);
    }

    public override void DestroyObstacle()
    {
        base.DestroyObstacle();
        DOTween.Kill(bannedAnimID);
    }
}

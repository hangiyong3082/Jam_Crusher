using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RemainingMoveCountText : MonoBehaviour
{
    [SerializeField] TMP_Text text;   

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
        {
            if (GameManager.Instance.State == GameState.Menu)
            {
                text.GetComponent<MeshRenderer>().enabled = false;
            }
            return;   
        }
        text.GetComponent<MeshRenderer>().enabled = true;
        //get player information
        var player = GameObject.FindWithTag("Player");
        var pcScript = player.GetComponent<PlayerController>();
        var playerPos = player.transform.position;
        //position

        text.text = $"{pcScript.remainingMoveCount}";
    }

    public void InitCountAnim()
    {
        DOTween.Kill("RPMC", true); //RPMC : RemainingMoveCount
        transform.DOScale(0.08f, 0.15f).SetRelative(true).SetLoops(2, LoopType.Yoyo).SetId("RPMC");
    }

    public void CantMoveAnim()
    {
        DOTween.Kill("RPMC", true);
        text.GetComponent<TMP_Text>().DOColor(new Color(1, 0.45f, 0.45f), 0.3f)
            .SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo).SetId("RPMC");
        transform.DOPunchPosition(new Vector3(1,0,1)*0.2f, 0.5f, 15).SetId("RPMC");
    }
}

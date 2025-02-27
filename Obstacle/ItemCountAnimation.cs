using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemCountAnimation : MonoBehaviour
{
    public void AddItemAnim()
    {
        DOTween.Kill("BombBoxItemAnim", true);
        transform.DOScale(Vector3.one, 0.3f).SetRelative(true)
            .SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad).SetId("BombBoxItemAnim");
    }
    public void RemoveItemAnim()
    {
        //ColorAnim(new Color(1, 0.33f, 0.33f));
        ColorAnim(new Color(1, 0.33f, 0.33f));
    }
    public void UseItemAnim()
    {
        ColorAnim(new Color(1,1,1));
    }

    Tween ColorAnim(Color color)
    {
        DOTween.Kill("BombBoxItemAnim", true);
        return gameObject.transform.GetComponent<TMP_Text>().DOColor(color, 0.3f)
            .SetLoops(2, LoopType.Yoyo).SetEase(Ease.Linear).SetId("BombBoxItemAnim");
    }
}

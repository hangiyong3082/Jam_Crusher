using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UIAnim
{
    ScaleHighlight,
    CancelHighlightWShake
}
public static class PublicFunctions
{
    public static Vector3 RandomPosition()
    {
        int tileCount = GameManager.Instance.tileCount;
        int tileScale = GameManager.Instance.tileScale;
        int halfLength = tileScale * (tileCount / 2);
        Vector3[] spawnPosList = new Vector3[(int)Mathf.Pow(GameManager.Instance.tileCount, 2)];

        int startArrayNum = 0;
        int posZ = halfLength;
        for (int i = 0; i < tileCount; i++)
        {
            int posX = -halfLength;
            for (int j = startArrayNum; j < startArrayNum + tileCount; j++)
            {
                spawnPosList[j] = new Vector3(posX, 1, posZ);
                posX += tileScale;
            }
            posZ -= tileScale;
            startArrayNum += tileCount;
        }

        return spawnPosList[Random.Range(0, (int)Mathf.Pow(GameManager.Instance.tileCount, 2))];

    }

    public static void SetScrollViewGridSize(GameObject scrollViewContent)
    {
        var rectSize = scrollViewContent.GetComponent<RectTransform>().sizeDelta;
        var gridLayoutGroup = scrollViewContent.GetComponent<GridLayoutGroup>();
        int contentCount = scrollViewContent.transform.childCount;
        scrollViewContent.GetComponent<RectTransform>().sizeDelta =
            new Vector2(rectSize.x, gridLayoutGroup.cellSize.y * contentCount + (gridLayoutGroup.spacing.y * contentCount));
    }
    
    public static void UIEffect(GameObject gameObject, object obj, string id, UIAnim uiAnim)
    {
        string fixedId = GetAnimId(obj, id);
        DOTween.Kill(fixedId, true);
        switch (uiAnim)
        {
            case UIAnim.ScaleHighlight:
                gameObject.transform.DOScale(0.25f, 0.3f).SetRelative(true)
                    .SetLoops(2, LoopType.Yoyo).SetId(fixedId);
                break;
            case UIAnim.CancelHighlightWShake:
                gameObject.GetComponent<TMP_Text>().DOColor(new Color(1, 0.45f, 0.45f), 0.3f)
                    .SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo).SetId(fixedId);
                gameObject.transform.DOPunchPosition(Vector3.one * 10f, 0.5f, 15).SetId(fixedId);
                break;
        }
    }

    public static string GetAnimId(object obj,string id)
    {
        return obj.GetType().Name + id;
    }
}

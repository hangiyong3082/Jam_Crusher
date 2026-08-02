using UnityEngine;

public class DarkenModel : MonoBehaviour
{
    public Color darkenColor = new Color(0.6f, 0.6f, 0.6f, 1f); // 약간 어두운 회색

    void Start()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            // 인스턴스 머티리얼 사용
            var mat = renderer.material;
            var baseColor = mat.HasProperty("_BaseColor")
                ? mat.GetColor("_BaseColor")
                : mat.GetColor("_Color");

            var newColor = baseColor * darkenColor;
            newColor.a = baseColor.a;

            if (mat.HasProperty("_BaseColor"))
                mat.SetColor("_BaseColor", newColor);
            else
                mat.SetColor("_Color", newColor);
        }
    }
}

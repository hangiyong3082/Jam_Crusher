using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTransparency : MonoBehaviour
{
    [SerializeField] List<Material> materials = new List<Material>();

    [SerializeField]float alphaOnMenu = 0.3f;
    [SerializeField] float alphaOnGame = 0.06f;

    public void OnMenu()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            var color = materials[i].color;
            materials[i].color =
                new Color(color.r, color.g, color.b, alphaOnMenu);
        }
    }

    public void OnGame()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            var color = materials[i].color;
            //roadMaterials[i].color =
              //  new Color(color.r, color.g, color.b, alphaOnGame);
            materials[i].DOColor(new Color(color.r, color.g, color.b, alphaOnGame), 0.5f);
        }
    }
}

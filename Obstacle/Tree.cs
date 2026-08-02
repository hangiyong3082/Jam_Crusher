using DarkTonic.MasterAudio;
using UnityEngine;

public class Tree : Obstacle
{
    [Header("Reference")]
    [SerializeField] GameObject originalState;
    [SerializeField] GameObject crackedState;

    public override void Start()
    {
        base.Start();
        crackedState.SetActive(false);
    }
    public override void Work()
    {
        base.Work();
        if (health == 1)
        {
            originalState.SetActive(false);
            crackedState.SetActive(true);
        }
    }

    public override void Reveal()
    {
        base.Reveal();
        MasterAudio.PlaySound("Object_GrowTree");
    }

    public override void Crashed(int damage = 1)
    {
        base.Crashed(damage);
        MasterAudio.PlaySound3DAtTransform("Tree_Crashed", transform);

    }
}
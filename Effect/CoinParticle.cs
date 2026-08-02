using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinParticle : MonoBehaviour
{
    private void Start()
    {
        float particlesLifetime = GetComponent<ParticleSystem>().main.startLifetime.constant;
        Destroy(gameObject, particlesLifetime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [SerializeField] ParticleSystem effect;

    public void BallCollide(Vector3 pos)
    {
        ParticleSystem eff = Instantiate(effect, pos, Quaternion.identity);
    }
}
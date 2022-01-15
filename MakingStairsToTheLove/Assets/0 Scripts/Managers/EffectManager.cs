using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [SerializeField] GameObject brickDestroy;

    public void BrickDestroy(Vector3 pos)
    {
        GameObject eff = Instantiate(brickDestroy, pos, Quaternion.identity);
    }
}
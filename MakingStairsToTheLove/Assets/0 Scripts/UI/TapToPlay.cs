using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TapToPlay : MonoBehaviour
{
    Sequence sequence;
    void Start()
    {
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(Vector3.one * 1.1f, 1).SetLoops(-1, LoopType.Yoyo));
    }
    private void OnDisable()
    {
        sequence.Kill();
    }
}
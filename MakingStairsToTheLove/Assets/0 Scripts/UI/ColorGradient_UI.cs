using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ColorGradient_UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI thisTMP;
    VertexGradient textGradient;
    float a, b, c, d;

    private void OnEnable()
    {
        textGradient = thisTMP.colorGradient;

        a = 0;
        b = 0;
        c = 0;
        d = 0;

        DOTween.To(() => a, x => a = x, 1, 0.0f);
        DOTween.To(() => b, x => b = x, 1, 0.0f);
        DOTween.To(() => c, x => c = x, 1, 0.4f);
        DOTween.To(() => d, x => d = x, 1, 0.4f);

        StartCoroutine(Close());
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(0.5f);

        DOTween.To(() => a, x => a = x, 0, 0.3f);
        DOTween.To(() => b, x => b = x, 0, 0.3f);
        DOTween.To(() => c, x => c = x, 0, 0.3f);
        DOTween.To(() => d, x => d = x, 0, 0.3f);

        yield return new WaitForSeconds(0.4f);

        gameObject.SetActive(false);
    }

    private void Update()
    {
        textGradient.bottomLeft = new Color32(255, 255, 255, (byte)(a * 255));
        textGradient.bottomRight = new Color32(255, 255, 255, (byte)(b * 255));
        textGradient.topLeft = new Color32(255, 255, 255, (byte)(c * 255));
        textGradient.topRight = new Color32(255, 255, 255, (byte)(d * 255));

        thisTMP.colorGradient = textGradient;
    }
}

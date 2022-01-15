using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class PointManager : Singleton<PointManager>, IWinObserver
{

    [SerializeField] RectTransform canvasRectT;
    [SerializeField] GameObject[] pointPopUps;
    [SerializeField] int pointPopUpIndex = 0;

    [SerializeField] GameObject goldImage;
    [SerializeField] TextMeshProUGUI goldTMP;

    private void Start()
    {
        UpdateGoldTMP();

        Observers.Instance.Add_WinObserver(this);
    }

    public void PointPopUp(Transform targetTr, int addingPoint)
    {
        pointPopUps[pointPopUpIndex].SetActive(true); // Going to false in ColorGradient_UI script
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, targetTr.position + new Vector3(0, 2, 2));
        pointPopUps[pointPopUpIndex].GetComponent<TextMeshProUGUI>().text = "+" + addingPoint.ToString() + "$";
        pointPopUps[pointPopUpIndex].GetComponent<RectTransform>().anchoredPosition = screenPoint - canvasRectT.sizeDelta / 2f;

        IncreasePopUpIndex();

        Globals.totalPoint += addingPoint;
        UpdateGold();
        // InputHandler.Instance.UpdateTotalPointText();
    }

    public void UpdateGold()
    {
        UpdateGoldTMP();
        GoldImageAnimation();
    }

    private void UpdateGoldTMP()
    {
        goldTMP.text = Globals.totalPoint.ToString();
    }

    private void GoldImageAnimation()
    {
        goldImage.transform.DOScale(Vector3.one * 1.2f, 0.1f).OnComplete(() => goldImage.transform.DOScale(Vector3.one, 0.1f));
    }

    void IncreasePopUpIndex()
    {
        pointPopUpIndex++;
        if (pointPopUpIndex == pointPopUps.Length)
            pointPopUpIndex = 0;
    }



    public void WinScenario()
    {

    }
}
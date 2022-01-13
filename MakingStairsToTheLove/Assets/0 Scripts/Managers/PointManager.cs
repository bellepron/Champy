using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointManager : Singleton<PointManager>, IWinObserver
{

    [SerializeField] RectTransform canvasRectT;
    [SerializeField] GameObject[] pointPopUps;
    [SerializeField] int pointPopUpIndex = 0;

    private void Start()
    {
        Globals.totalPoint = 1;
        Globals.multiplyPoint = 2;

        Observers.Instance.Add_WinObserver(this);
    }

    public void PointPopUp(Transform oreoTr, int addingPoint)
    {
        pointPopUps[pointPopUpIndex].SetActive(true); // Going to false in ColorGradient_UI script
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, oreoTr.position + new Vector3(0, 7, 5));
        pointPopUps[pointPopUpIndex].GetComponent<TextMeshProUGUI>().text = "+" + addingPoint.ToString() + "$";
        pointPopUps[pointPopUpIndex].GetComponent<RectTransform>().anchoredPosition = screenPoint - canvasRectT.sizeDelta / 2f;

        IncreasePopUpIndex();

        Globals.totalPoint += addingPoint;
        // InputHandler.Instance.UpdateTotalPointText();
    }

    public void PointPopUpHand(Transform handTr, int showPoint)
    {
        pointPopUps[pointPopUpIndex].SetActive(true); // Going to false in ColorGradient_UI script
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, handTr.position);
        pointPopUps[pointPopUpIndex].GetComponent<TextMeshProUGUI>().text = "+" + showPoint.ToString() + "$";
        pointPopUps[pointPopUpIndex].GetComponent<RectTransform>().anchoredPosition = screenPoint - canvasRectT.sizeDelta / 2f;

        IncreasePopUpIndex();
    }
    void IncreasePopUpIndex()
    {
        pointPopUpIndex++;
        if (pointPopUpIndex == pointPopUps.Length)
            pointPopUpIndex = 0;
    }

    public void WinScenario()
    {
        Globals.totalPoint = 0;
    }
}
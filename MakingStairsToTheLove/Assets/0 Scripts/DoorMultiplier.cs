using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DoorMultiplier : MonoBehaviour, IInteractable, IWinObserver
{
    public enum GlassType { FIRST, SECOND }
    [SerializeField] GlassType glassType;
    [SerializeField] int multiplyValue;
    [SerializeField] TextMeshPro multiplyTMP;
    [SerializeField] GameObject door;
    [SerializeField] GameObject brokenDoor;

    Vector3 startPos;

    private void Start()
    {
        door.SetActive(false);
        startPos = transform.position;

        multiplyTMP.text = multiplyValue.ToString() + "x";
        multiplyTMP.enabled = false;

        if (glassType == GlassType.FIRST)
            transform.position += transform.right * 3;
        else
            transform.position -= transform.right * 3;

        Observers.Instance.Add_WinObserver(this);
    }

    void IWinObserver.WinScenario()
    {
        door.SetActive(true);
        multiplyTMP.enabled = true;
        transform.DOMove(startPos, 1);
    }

    void IInteractable.Interact(Player player)
    {
        BeBroken();
        MultiplyGold();
        multiplyTMP.enabled = false;

        if (glassType == GlassType.SECOND)
            Globals.hasReachTOLover = true;
    }

    void BeBroken()
    {
        door.SetActive(false);
        brokenDoor.SetActive(true);
    }

    void MultiplyGold()
    {
        Globals.totalPoint *= multiplyValue;
        PointManager.Instance.UpdateGold();
    }
}
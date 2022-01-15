using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Lover : MonoBehaviour, IWinObserver, ILoseObserver, ILevelEndObserver
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        Observers.Instance.Add_WinObserver(this);
        Observers.Instance.Add_LoseObserver(this);
        Observers.Instance.Add_LevelEndObserver(this);
    }

    void IWinObserver.WinScenario()
    {
        anim.SetTrigger("happy");
    }

    void ILoseObserver.LoseScenario()
    {
        anim.SetTrigger("sad");
    }

    void ILevelEndObserver.LevelEnd()
    {
        if (Globals.hasReachTOLover == true)
        {
            // anim.SetTrigger("dancePose");
            // transform.DORotate(Vector3.zero, 1.5f, RotateMode.FastBeyond360);
            // transform.DOLocalJump(Player.Instance.dancePoseHand.position, 1.5f, 1, 1.5f).OnComplete(() => SetParentPlayersDancePoseHand());


            anim.SetTrigger("dancePose");
            transform.DORotate(new Vector3(172.104f, -54.01999f, -3.85199f), 1.5f, RotateMode.FastBeyond360);
            transform.DOJump(Player.Instance.dancePoseHand.position, 1.5f, 1, 1.5f).OnComplete(() => SetParentPlayersDancePoseHand());
            StartCoroutine(Player.Instance.RotateLover());
        }
        else
        {
            anim.SetTrigger("sad");
        }
    }

    void SetParentPlayersDancePoseHand()
    {
        transform.parent = Player.Instance.dancePoseHand;
    }
}
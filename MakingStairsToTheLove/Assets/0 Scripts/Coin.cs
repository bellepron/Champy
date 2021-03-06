using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IInteractable
{
    void IInteractable.Interact(Player player)
    {
        gameObject.SetActive(false);
        PointManager.Instance.PointPopUp(transform, 1);
        SoundManager.Instance.Coin();
    }
}
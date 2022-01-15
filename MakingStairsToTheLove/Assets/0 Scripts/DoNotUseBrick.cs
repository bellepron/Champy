using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotUseBrick : MonoBehaviour, IInteractable
{
    void IInteractable.Interact(Player player)
    {
        player.InteractWithDoNotUseBrick();
    }
}
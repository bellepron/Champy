using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour, IInteractable
{
    bool interacted;
    void IInteractable.Interact(Player player)
    {
        if (interacted == true) return;
        interacted = true;

        player.InteractWithBrick(gameObject);
    }

    private void OnBecameInvisible()
    {
        interacted = false;
    }
}
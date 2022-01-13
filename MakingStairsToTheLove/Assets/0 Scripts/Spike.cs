using UnityEngine;

public class Spike : MonoBehaviour, IInteractable
{
    void IInteractable.Interact(Player player)
    {
        player.InteractWithSpike();
    }
}
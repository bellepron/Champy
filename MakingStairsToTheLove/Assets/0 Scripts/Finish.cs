using UnityEngine;

public class Finish : MonoBehaviour, IInteractable
{
    void IInteractable.Interact(Player player)
    {
        Observers.Instance.Notify_WinObservers();
    }
}
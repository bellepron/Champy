using UnityEngine;

public class Finish : MonoBehaviour, IInteractable
{
    bool triggered;
    void IInteractable.Interact(Player player)
    {
        if (triggered == false)
            Observers.Instance.Notify_WinObservers();

        triggered = true;
    }
}
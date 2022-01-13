using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Events : MonoBehaviour
{
    public static UnityEvent m_MyOneShotEvent = null;

    void Start()
    {
        m_MyOneShotEvent = new UnityEvent();
    }

    private void OnDisable()
    {
        m_MyOneShotEvent.RemoveAllListeners();
    }
}
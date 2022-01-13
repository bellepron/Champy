using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwipingHand : MonoBehaviour
{
    // Swipe Control
    Vector2 firstPos;
    Vector2 secondPos;
    Vector2 currentSwipe;
    bool isSwiped;

    void Start()
    {
        transform.DOMoveX(transform.position.x + 300, 1f).SetLoops(-1);
        StartCoroutine(CheckSwipe());
    }
    IEnumerator CheckSwipe()
    {
        while (isSwiped == false)
        {
            Swipe();
            yield return null;
        }
    }
    void Swipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstPos = (Vector2)Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            secondPos = (Vector2)Input.mousePosition;
            if (Vector2.Distance(firstPos, secondPos) > 2)
            {
                FindObjectOfType<GameManager>().StartPanel();
                isSwiped = true;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UI_Hand : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [SerializeField] GameObject clickingHand;
    [SerializeField] float time = 0.05f;
    void Start()
    {
        // hand.SetActive(false);
        clickingHand.SetActive(false);
    }

    void Update()
    {
        hand.transform.position = Input.mousePosition;
        clickingHand.transform.position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            hand.SetActive(false);
            clickingHand.SetActive(true);
            // StartCoroutine(ClickingHand());
        }
        if (Input.GetMouseButtonUp(0))
        {
            hand.SetActive(true);
            clickingHand.SetActive(false);
        }
    }

    IEnumerator ClickingHand()
    {
        yield return new WaitForSeconds(time);
        hand.SetActive(false);
        clickingHand.transform.position = Input.mousePosition;
        clickingHand.SetActive(true);
        yield return new WaitForSeconds(time);
        clickingHand.SetActive(false);
        hand.SetActive(true);

        // gameObject.SetActive(false);
    }
}
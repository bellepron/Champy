using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKLook : MonoBehaviour
{
    Animator animator;
    float weight = 1;

    private GameObject target;

    void Start()
    {
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetLookAtWeight(1f * weight, .2f * weight, 0.2f * weight, .1f * weight, .8f * weight);

        Vector3 direction = target.transform.position + new Vector3(0, 1.2f, 0) - transform.position;

        Ray lookAtRay = new Ray(transform.position, direction);
        animator.SetLookAtPosition(lookAtRay.GetPoint(25));
    }

    public void OpenIKSlightly()
    {
        weight = Mathf.Lerp(weight, 1f, Time.fixedDeltaTime);
    }
    public void CloseIKSlightly()
    {
        weight = Mathf.Lerp(weight, 0f, Time.fixedDeltaTime);
    }
}

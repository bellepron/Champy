using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollToggle : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody Rigidbody;
    protected CapsuleCollider capsuleCollider;
    protected Collider[] childrenCollider;
    protected Rigidbody[] childrenRigidbody;
    [SerializeField] GameObject pelvis;

    void Awake()
    {
        animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        childrenCollider = pelvis.GetComponentsInChildren<Collider>();
        childrenRigidbody = pelvis.GetComponentsInChildren<Rigidbody>();
    }

    private void Start()
    {
        RagdollActivate(false);
    }

    public void RagdollActivate(bool active)
    {
        //children
        foreach (var collider in childrenCollider)
            collider.enabled = active;
        foreach (var rigidb in childrenRigidbody)
        {
            rigidb.detectCollisions = active;
            rigidb.isKinematic = !active;
        }

        //rest
        animator.enabled = !active;
        Rigidbody.detectCollisions = !active;
        Rigidbody.isKinematic = active;
        capsuleCollider.enabled = !active;
        //script.enabled = !active;
    }
}
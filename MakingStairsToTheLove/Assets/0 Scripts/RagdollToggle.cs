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
    [HideInInspector] public Rigidbody pelvisRb;

    void Awake()
    {
        animator = transform.parent.GetComponent<Animator>();
        Rigidbody = transform.parent.GetComponent<Rigidbody>();
        capsuleCollider = transform.parent.GetComponent<CapsuleCollider>();

        childrenCollider = pelvis.GetComponentsInChildren<Collider>();
        childrenRigidbody = pelvis.GetComponentsInChildren<Rigidbody>();

        pelvisRb = pelvis.GetComponent<Rigidbody>();
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
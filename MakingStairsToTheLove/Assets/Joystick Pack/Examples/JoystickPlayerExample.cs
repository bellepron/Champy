using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public DynamicJoystick dynamicJoystick;
    public Rigidbody rb;
    Animator anim;
    Vector3 _velocity;

    public void FixedUpdate()
    {
        Vector3 direction = Vector3.forward * dynamicJoystick.Vertical + Vector3.right * dynamicJoystick.Horizontal;
        // // // rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        rb.velocity = direction * speed * Time.fixedDeltaTime;
        _velocity = rb.velocity;

        // RotateToVelocityDirection();
        // UpdateAnimator();
    }

    private void RotateToVelocityDirection()
    {
        if (_velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_velocity, Vector3.up);
        }
    }

    private void UpdateAnimator()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(_velocity);
        float _speed = localVelocity.z;
        anim.SetFloat("moveSpeed", _speed);
    }
}
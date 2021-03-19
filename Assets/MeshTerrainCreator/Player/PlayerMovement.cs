using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
     private const float gravity = -9.82f;
    [SerializeField] private float WalkSpeed = 1.5f;
    [SerializeField] private float SprintSpeed = 3.0f;
    [SerializeField] private float RotateSpeed = 55;
    [SerializeField] private float m_jumpHeight = 3.0f;
    [SerializeField] private float DistToGround = 0.9f;

    [SerializeField] private CharacterController m_characterController;
    [SerializeField] private Camera m_playerCamera;

    [SerializeField] Transform m_modelTransform;

    public event Action Jumping;  

    public bool IsSprinting() => m_sprinting;
    private bool m_sprinting;

    private float yaw;

    private Vector3 drawDir;
    private Vector3 velocity;

    private bool wasGrounded = false;

    private void Update() 
    {
        SetSprinting();
        AxesMovement();
        YOrientation();
        
        if (m_characterController.velocity.y > .25f)
        {
            Debug.Log("Rising");
        }
        else if (m_characterController.velocity.y < -.25f)
        {
            Debug.Log("Falling");
        }

        if (!IsGrounded() && wasGrounded)
        {
            Debug.Log("LIFT OFF!");
        }
        else if (IsGrounded() && !wasGrounded)
        {
            Debug.Log("Landing");
        }


        wasGrounded = IsGrounded();
    }

    private void SetSprinting() => m_sprinting = Input.GetKey(KeyCode.LeftShift) ? true : false;

    private void YOrientation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Rotate(true);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            Rotate(false);
        }
        else 
        {
            yaw += RotateSpeed * 0.035f * Input.GetAxis("Mouse X");
            transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
        }
    }

    private void AxesMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 _forward = new Vector3(transform.forward.x, m_playerCamera.transform.forward.y, transform.forward.z);
        Vector3 _right = new Vector3(transform.right.x, m_playerCamera.transform.right.y, transform.right.z);

        Vector3 forward;
        Vector3 right;
        
        var speed = m_sprinting ? SprintSpeed : WalkSpeed;

        forward = _forward * v;
        right = _right * h;

        Vector3 moveDir = forward + right;

        if (moveDir.sqrMagnitude > 1)
            moveDir.Normalize();
        
        SetModelRotation(moveDir);
        m_characterController.Move(moveDir * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                Jumping?.Invoke();
                velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * gravity);
            }
        }

        velocity.y += gravity * Time.deltaTime;

        m_characterController.Move(velocity * Time.deltaTime);
    }

    private void Rotate(bool right)
    {
        int dir = right ? -1 : 1;

        yaw += RotateSpeed * 0.035f * dir;
        transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
    }
    
    private void SetModelRotation(Vector3 v)
    {
        v = new Vector3(v.x, 0.0f, v.z);
        drawDir = v * 5;

        if (m_sprinting) 
        {
            if (v != Vector3.zero) {
                Quaternion q = Quaternion.LookRotation(v, Vector3.up);
                m_modelTransform.rotation = q;
            }
        }
        else
        {
            m_modelTransform.localRotation = Quaternion.identity; 
        }
    }

    private bool IsGrounded() => Physics.Raycast(transform.position, -Vector3.up, DistToGround + 0.01f);
    
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, drawDir);
    }
}

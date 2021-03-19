using System;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private Animator m_anim;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        m_anim.SetTrigger("Attack");
    }
}

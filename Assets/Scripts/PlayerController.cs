using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigid;

    Vector2 moveDir;

    void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer spriter;
    [SerializeField] Animator animator;

    [Header("Property")]
    [SerializeField] float movePower;
    [SerializeField] float breakPower;
    [SerializeField] float maxXSpeed;
    [SerializeField] float maxYSpeed;

    [SerializeField] float jumpSpeed;
    bool isGround;

    Vector2 moveDir;

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (moveDir.x < 0 && rigid.velocity.x > -maxXSpeed)
        {
            rigid.AddForce(Vector2.right * moveDir.x * movePower);
        }
        else if (moveDir.x > 0 && rigid.velocity.x < maxXSpeed)
        {
            rigid.AddForce(Vector2.right * moveDir.x * movePower);
        }        
        else if (moveDir.x == 0 && rigid.velocity.x > 0.1f)
        {
            rigid.AddForce(Vector2.left * breakPower);
        }        
        else if (moveDir.x == 0 && rigid.velocity.x < -0.1f)
        {
            rigid.AddForce(Vector2.right * breakPower);
        }

        if (rigid.velocity.y < -maxYSpeed)
        {
            Vector2 velocity = rigid.velocity;
            velocity.y = -maxYSpeed;
            rigid.velocity = velocity;
        }

        animator.SetFloat("YSpeed", rigid.velocity.y);
    }

    void Jump()
    {
        Vector2 velocity = rigid.velocity;
        velocity.y = jumpSpeed;
        rigid.velocity = velocity;
    }

    void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();

        if (moveDir.x < 0 )
        {
            spriter.flipX = true;
            animator.SetBool("Run", true);
        }
        else if (moveDir.x > 0)
        {
            spriter.flipX = false;
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }


    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && isGround)
        {
            Jump();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGround = false;
    }
}

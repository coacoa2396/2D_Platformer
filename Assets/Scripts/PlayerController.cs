using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] GameObject player;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer spriter;
    [SerializeField] Animator animator;
    [SerializeField] LayerMask groundCheckLayer;
    [SerializeField] LayerMask downLayerCheck;
    

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

    GameObject downJumpObject;

    private void OnCollisionStay2D(Collision2D collision)
    {
        downJumpObject = collision.gameObject;
    }

   

    void DownJump()
    {
        Debug.Log("´Ù¿îÁ¡ÇÁ");
        StartCoroutine(Down());
    }

    IEnumerator Down()
    {
        if (!downLayerCheck.Contain(downJumpObject.layer))
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), downJumpObject.GetComponent<Collider2D>(), true);
            yield return new WaitForSeconds(0.5f);
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), downJumpObject.GetComponent<Collider2D>(), false);
        }
        else
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), downJumpObject.GetComponent<CompositeCollider2D>(), true);
            yield return new WaitForSeconds(0.5f);
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), downJumpObject.GetComponent<CompositeCollider2D>(), false);

        }
    }

    void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();

        if (moveDir.x < 0)
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

        if (moveDir.y == -1 && value.isPressed && isGround)
        {
            DownJump();
            Debug.Log(moveDir.y);
        }
        else if (value.isPressed && isGround)
        {
            Jump();
        }
    }


    

    int groundCount;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (groundCheckLayer.Contain(collision.gameObject.layer))
        {
            groundCount++;
            isGround = groundCount > 0;
            Debug.Log("¶¥ ¹âÀ½");
            animator.SetBool("isGround", true);
        }

        /*
        if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Ground", "Default", "Obsticle")) != 0)
        {
            Debug.Log("¶¥¿¡¼­ ¶³¾îÁü");
        }
        */
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (groundCheckLayer.Contain(collision.gameObject.layer))
        {
            groundCount--;
            isGround = groundCount > 0;
            Debug.Log("¶¥¿¡¼­ ¶³¾îÁü");
            animator.SetBool("isGround", false);
        }
    }
}

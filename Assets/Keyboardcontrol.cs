using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardControl : MonoBehaviour
{
    public float jumpHigh = 8f;
    public float moveSpeed = 5f;

    public float secondJumptime = 0.5f;//二段跳窗口时间，策划可调整

    //发射子弹
    public GameObject bulletPrefab;
    public Transform fashe;
    public float bulletSpeed = 10f;
 

    private Rigidbody2D body;
    private bool isGrounded;

    private int jumpCount = 0;
    private float secondJumpTime = 0f;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Keyboard.current == null)
        return;
        //二段跳计时
        if (secondJumpTime > 0)
        {
            secondJumpTime -= Time.deltaTime;
        }
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            //一段跳
            if (isGrounded)
            {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpHigh);   
            jumpCount = 1;
            secondJumpTime = secondJumptime;
            }
            //二段跳
            else if (jumpCount == 1 && secondJumpTime > 0)
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpHigh);
                jumpCount = 2;
                secondJumpTime = 0f;
            }
        }
        //发射子弹
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        float move = 0f;

        if (Keyboard.current != null && isGrounded )
        {
            if (Keyboard.current.aKey.isPressed)
                move -= 1f;

            if (Keyboard.current.dKey.isPressed)
                move += 1f;
        }

        body.linearVelocity = new Vector2(move * moveSpeed, body.linearVelocity.y);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab,fashe.position,Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.linearVelocity = Vector2.right * bulletSpeed;
    }
}

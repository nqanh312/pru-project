using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class fully describes player parameters, movement and animations logic
/// </summary>
public class Player : MonoBehaviour
{
    public static Player instance;

    public float health = 3f;
    public float damage = 1f;
    public int coins = 0;

    public float speed = 300f;
    public int extraJumps = 1;
    public float jumpPower = 7;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public bool isAlive = true;

    public GameObject bullet;
    public Camera cam;

    private Animator animator;
    private Rigidbody2D body;

    private bool isGrounded = true;
    private bool moveRight = true;
    private int jumpsLeft;

    private Vector3 spawnPosition;
    private Vector3 spawnOffset = new Vector3(0, 1.5f, 0);

    /// <summary>
    /// Player is spawned at the green flag position
    /// </summary>
    private void Start()
    {
        instance = this;

        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();

        jumpsLeft = extraJumps;

        SetSpawnPosition(GameObject.FindGameObjectWithTag("Start Point").transform.position);
        transform.position = spawnPosition;

        cam = Camera.main;

        Sounds.instance.MuteMusicBoss(false);
    }

    /// <summary>
    /// Move player with A/D or Left/Right arrows. Space to jump. Ctrl to fire.
    /// </summary>
    void Update()
    {
        if (isAlive)
        {
            float movement = Input.GetAxis("Horizontal");

            body.velocity = new Vector2(movement * speed * Time.deltaTime, body.velocity.y);

            // Rotate player according to movement direction
            if (movement > 0 && !moveRight)
                Flip();
            else if (movement < 0 && moveRight)
                Flip();

            MoveAnimation(Mathf.Abs(movement));

            // Jump and double jump logic
            if (Input.GetKeyDown(KeyCode.Space) && jumpsLeft > 0)
            {
                body.velocity = new Vector2(body.velocity.x, jumpPower);
                jumpsLeft--;
                JumpAnimation();
                Sounds.instance.PlayJump();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && jumpsLeft == 0 && isGrounded)
            {
                body.velocity = new Vector2(body.velocity.x, jumpPower);
                JumpAnimation();
                isGrounded = false;
                Sounds.instance.PlayJump();
            }

            // Makes player to fall more realistic according to real world physics
            if (body.velocity.y < 0)
                body.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
            else if (body.velocity.y > 0 && !Input.GetButton("Jump"))
                body.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;

            // Attack logic
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (transform.localScale.x > 0)
                    Instantiate(bullet, transform.position + new Vector3(1f, 0, 0), Quaternion.identity);
                else
                    Instantiate(bullet, transform.position - new Vector3(1f, 0, 0), Quaternion.identity);

                Sounds.instance.PlayShot();
            }

            // Move camera with player
            cam.transform.position = new Vector3(transform.position.x, transform.position.y / 2, cam.transform.position.z);

            // Respawn player if it falls below the screen
            if (transform.position.y < -10)
                Respawn();
        }
    }

    /// <summary>
    /// Respawn player at the last checkpoint flag
    /// </summary>
    public void Respawn()
    {
        transform.position = spawnPosition + spawnOffset;
    }

    /// <summary>
    /// Set new spawn position
    /// </summary>
    /// <param name="position"></param>
    public void SetSpawnPosition(Vector3 position)
    {
        spawnPosition = position + spawnOffset;
    }

    /// <summary>
    /// Call it after player reaches ground
    /// </summary>
    public void Grounded()
    {
        isGrounded = true;
        GroundedAnimation();
        jumpsLeft = extraJumps;
    }

    /// <summary>
    /// Rotate player to 180 degrees
    /// </summary>
    private void Flip()
    {
        moveRight = !moveRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    /// <summary>
    /// Update player coins amount
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateCoins(int amount)
    {
        Sounds.instance.PlayCoin();

        coins += amount;
        GameManager.instance.UpdateCoins(coins);
    }

    /// <summary>
    /// Call it when player gets a damage
    /// </summary>
    /// <param name="damage"></param>
    public void Hurt(float damage)
    {
        if (isAlive)
        {
            Sounds.instance.PlayHurt();

            body.velocity = Vector2.up * 10;
            HurtAnimation();

            health -= damage;

            GameManager.instance.UpdateHealth(health);

            if (health <= 0)
            {
                GameManager.instance.GameOver();
                DeadAnimation();
                Destroy(body);
                isAlive = false;
            }
        }
    }

    /// <summary>
    /// Player has two movement animations bases on current speed, walking below 0.9 and run above 0.9
    /// </summary>
    /// <param name="speed"></param>
    public void MoveAnimation(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    /// <summary>
    /// Call player jump animation
    /// </summary>
    public void JumpAnimation()
    {
        animator.SetBool("IsHurting", false);
        animator.SetBool("IsJumping", true);
    }

    /// <summary>
    /// Call player hurt animation
    /// </summary>
    public void HurtAnimation()
    {
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsHurting", true);
    }

    /// <summary>
    /// Call player grounded animation
    /// </summary>
    public void GroundedAnimation()
    {
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsHurting", false);
    }

    /// <summary>
    /// Call player death animation
    /// </summary>
    public void DeadAnimation()
    {
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsHurting", false);
        animator.SetBool("IsDead", true);
    }

    /// <summary>
    /// Stop all animations and back to walking/running
    /// </summary>
    public void StopAllAnimations()
    {
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsHurting", false);
        animator.SetBool("IsDead", false);
    }

    /// <summary>
    /// Check if player steps on ground
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            Grounded();
        }
        
    }
}

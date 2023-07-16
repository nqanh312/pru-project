using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class describes enemies parameters and contains interactions logic
/// </summary>
public class GreenMonster : MonoBehaviour
{
    public float damage = 0.5f;
    public float health = 1f;
    public float distance = 1f;
    public float speed = 5f;

    public float jumpSpeed;
    public int startJumpingAt;
    public int jumpDelay;

    public GameObject bossBullet;
    public float delayBeforeFiring;
    public Slider bossHealth;
    Vector3 bulletSpawnPos;
    bool canFire, isJumping;

    Rigidbody2D rb;
    SpriteRenderer sr;
    private Animator animator;

    public bool isStatic = false;
    public bool isInvincible = false;
    public bool isHorizontalMovement = false;

    private Vector3 basePosition;
    private bool moveRight = true;
    private float previousOffset = 0;
    private bool isAlive = true;
    private bool startBoss;

    void Start()
    {
        animator = GetComponent<Animator>();
        basePosition = transform.position;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        bossHealth.value = (float) health;
        bossHealth.interactable = false;
        startBoss = false;
        bossHealth.gameObject.SetActive(false);


        canFire = false;
        bulletSpawnPos = gameObject.transform.Find("BulletSpawnPos").transform.position;

        Sounds.instance.MuteMusicBoss(true);    

        Invoke("Reload", Random.Range(1f, delayBeforeFiring));
    }

    /// <summary>
    /// If an enemy is alive and non-static then move it according to horizontal or vertical option
    /// </summary>
    void Update()
    {   
        

    if(startBoss == false) {
        Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(transform.position, 10f);
                for (int i = 0; i < collidersEnemies.Length; i++)
                {
                    if (collidersEnemies[i].gameObject.tag == "Player")
                    {
                        startBoss = true;
                    }
                }
        }

        if (startBoss)
        {
            
            if (canFire && isAlive)
            {
                FireBullets();
                canFire = false;

                if (health < startJumpingAt && !isJumping)
                {
                    InvokeRepeating("Jump", 0, jumpDelay);
                    isJumping = true;
                }
            }

            if (isAlive && !isStatic)
            {   
                bossHealth.gameObject.SetActive(true);
                Sounds.instance.MuteMusicBoss(false);
                Sounds.instance.MuteMusic(true);
                float offset = Mathf.Sin(Time.time * speed) * distance;

                if (isHorizontalMovement)
                    transform.position = new Vector3(basePosition.x + offset, basePosition.y, basePosition.z);
                else
                    transform.position = new Vector3(basePosition.x, basePosition.y + offset, basePosition.z);

                if (!moveRight && previousOffset > offset)
                    Flip();
                else if (moveRight && previousOffset < offset)
                    Flip();

                previousOffset = offset;
            }
        }
    }

    void Reload()
    {   
        canFire = true;
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0, jumpSpeed));
    }

    void FireBullets()
    {   
        Instantiate(bossBullet, bulletSpawnPos, Quaternion.identity);

        Invoke("Reload", delayBeforeFiring);
    }

    /// <summary>
    /// Check if enemy hits player or if it's hit with a bullet
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") && isAlive)
        {
            Player.instance.Hurt(damage);
        }
        else if (collision.transform.CompareTag("Bullet"))
        {
            Sounds.instance.PlayHit();
            sr.color = Color.red;
            Invoke("RestoreColor", 0.1f);

            if (isAlive && !isInvincible)
            {
                health -= Player.instance.damage;
                bossHealth.value = (float) health;

                if (health <= 0)
                {
                    isAlive = false;
                    //gameObject.AddComponent<Rigidbody2D>();
                    animator.SetBool("IsDead", true);
                    bossHealth.gameObject.SetActive(false);
                    Sounds.instance.MuteMusicBoss(true);
                    Sounds.instance.MuteMusic(false);


                    // effect explosion
                    Vector3 pos = collision.transform.position;
                    pos.z = 20f;
                    SFXCtrl.instance.EnemyExplosion(pos);
                    

                    StartCoroutine(Delete());
                }
            }

            Destroy(collision.gameObject);
        }
    }

    void RestoreColor()
    {
        sr.color = Color.white;
    }


    /// <summary>
    /// Rotate enemy to 180 degrees
    /// </summary>
    private void Flip()
    {
        moveRight = !moveRight;

        if (isHorizontalMovement)
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
    }

    /// <summary>
    /// Remove enemy game object from the scene
    /// </summary>
    IEnumerator Delete()
    {
        yield return new WaitForSeconds(0f);

        Destroy(gameObject);
    }
}

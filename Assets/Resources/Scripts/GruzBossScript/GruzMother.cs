using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class GruzMother : MonoBehaviour
{
    [Header("Idel")]
    [SerializeField] float idelMovementSpeed;
    [SerializeField] Vector2 idelMovementDirection;

    [Header("AttackUpNDown")]
    [SerializeField] float attackMovementSpeed;
    [SerializeField] Vector2 attackMovementDirection;

    [Header("AttackPlayer")]
    [SerializeField] float attackPlayerSpeed;
    [SerializeField] Transform player;

    [Header("Other")]
    [SerializeField] Transform goundCheckUp;
    [SerializeField] Transform goundCheckDown;
    [SerializeField] Transform goundCheckWall;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    private bool isTouchingUp;
    private bool isTouchingDown;
    private bool isTouchingWall;
    private bool hasPlayerPositon;

    private Vector2 playerPosition;

    private bool facingLeft = true;
    private bool goingUp = true;
    private Rigidbody2D enemyRB;
    private Animator enemyAnim;
    SpriteRenderer sr;
    
    private bool isAlive = true;
    public Slider bossHealth;
    public float damage = 0.5f;
    public float health = 10f;
    private bool startBoss;


    void Start()
    {
        idelMovementDirection.Normalize();
        attackMovementDirection.Normalize();
        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        bossHealth.value = (float) health;
        bossHealth.interactable = false;
        bossHealth.gameObject.SetActive(false);
        startBoss = false;
    }

    // Update is called once per frame
    void Update()
    {   
        if(startBoss == false) 
        {
            Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(transform.position, 5f);
            for (int i = 0; i < collidersEnemies.Length; i++)
                {
                    if (collidersEnemies[i].gameObject.tag == "Player")
                    {
                        startBoss = true;
                    }
                }
        }
        if (startBoss) {
            bossHealth.gameObject.SetActive(true);
            isTouchingUp = Physics2D.OverlapCircle(goundCheckUp.position, groundCheckRadius, groundLayer); 
            isTouchingDown = Physics2D.OverlapCircle(goundCheckDown.position, groundCheckRadius, groundLayer); 
            isTouchingWall = Physics2D.OverlapCircle(goundCheckWall.position, groundCheckRadius, groundLayer);
        }

    }

    void RandomStatePicker()
    {
        int randomState = Random.Range(0, 2);
        if (randomState == 0)
        {
            enemyAnim.SetTrigger("AttackUpNDown");
        }
        else if (randomState == 1)
        {
            enemyAnim.SetTrigger("AttackPlayer");
        }
    }

   public void IdelState()
    {
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }

        if (isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }
        enemyRB.velocity = idelMovementSpeed * idelMovementDirection;
    } 
   public void AttackUpNDownState()
    {
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }

        if (isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }
        enemyRB.velocity = attackMovementSpeed * attackMovementDirection;
    }

    public void AttackPlayerState()
    {
       
        if (!hasPlayerPositon)
        {
            FlipTowardsPlayer();
             playerPosition = player.position - transform.position;
            playerPosition.Normalize();
            hasPlayerPositon = true;
        }
        if (hasPlayerPositon)
        {
            enemyRB.velocity = attackPlayerSpeed * playerPosition;
           
        }
        

        if (isTouchingWall || isTouchingDown)
        {
            //play Slam animation
            enemyAnim.SetTrigger("Slamed");
            enemyRB.velocity = Vector2.zero;
            hasPlayerPositon = false;
        }
    }

    void FlipTowardsPlayer()
    {
        float playerDirection = player.position.x - transform.position.x;

        if (playerDirection>0 && facingLeft)
        {
            Flip();
        }
        else if (playerDirection<0 && !facingLeft)
        {
            Flip();
        }
    }

    void ChangeDirection()
    {
        goingUp = !goingUp;
        idelMovementDirection.y *= -1;
        attackMovementDirection.y *= -1;
        Sounds.instance.PlayHit();
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        idelMovementDirection.x *= -1;
        attackMovementDirection.x *= -1;
        transform.Rotate(0, 180, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(goundCheckUp.position, groundCheckRadius);
        Gizmos.DrawWireSphere(goundCheckDown.position, groundCheckRadius);
        Gizmos.DrawWireSphere(goundCheckWall.position, groundCheckRadius);
    }

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

            if (isAlive)
            {
                health -= Player.instance.damage;
                bossHealth.value = (float) health;

                if (health <= 0)
                {
                    isAlive = false;
                    //gameObject.AddComponent<Rigidbody2D>();
                    bossHealth.gameObject.SetActive(false);

                    Sounds.instance.WinGame(); 
                    Sounds.instance.MuteMusic(true)   ;

                    //effect explosion
                    Vector3 pos = collision.transform.position;
                    Debug.Log(pos);
                    pos.z = 20f;
                    //SFXCtrl.instance.ShowPlayerLanding(pos);


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

    IEnumerator Delete()
    {
        yield return new WaitForSeconds(0f);

        Destroy(gameObject);
    }
}

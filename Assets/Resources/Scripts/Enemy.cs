using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class describes enemies parameters and contains interactions logic
/// </summary>
public class Enemy : MonoBehaviour
{
    public float damage = 0.5f;
    public float health = 1f;
    public float distance = 1f;
    public float speed = 5f;

    private Animator animator;

    public bool isStatic = false;
    public bool isInvincible = false;
    public bool isHorizontalMovement = false;

    private Vector3 basePosition;
    private bool moveRight = true;
    private float previousOffset = 0;
    private bool isAlive = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        basePosition = transform.position;
    }

    /// <summary>
    /// If an enemy is alive and non-static then move it according to horizontal or vertical option
    /// </summary>
    void Update()
    {
        if (isAlive && !isStatic)
        {
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

            if (isAlive && !isInvincible)
            {
                health -= Player.instance.damage;

                if (health <= 0)
                {
                    isAlive = false;
                    gameObject.AddComponent<Rigidbody2D>();
                    animator.SetBool("IsDead", true);

                    StartCoroutine(Delete());
                }
            }

            Destroy(collision.gameObject);
        }
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
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }
}

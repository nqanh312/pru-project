using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// handles the player's bullet movement and collision with the enemy
/// </summary>
public class BossBulletCtrl : MonoBehaviour
{
    public Vector2 velocity;
    Rigidbody2D rb;

    private bool isAlive = true;
    public float damage = 0.5f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") && isAlive)
        {
            Player.instance.Hurt(damage);
            Destroy(gameObject);
        }
    }

}

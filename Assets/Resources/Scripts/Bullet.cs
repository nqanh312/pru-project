using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class describes bullet parameters and contains interactions logic
/// </summary>
public class Bullet : MonoBehaviour
{
    public int damage = 1;

    public float direction;

    /// <summary>
    /// Spawn bullet near to player
    /// </summary>
    void Start()
    {
        direction = Player.instance.transform.localScale.x;
    }

    /// <summary>
    /// Move bullet forward
    /// </summary>
    void Update()
    {
        transform.position += new Vector3(0.5f, 0, 0) * direction;
    }

    /// <summary>
    /// Delete bullet if it hits ground
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
            Destroy(gameObject);
    }
}

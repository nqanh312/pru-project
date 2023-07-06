using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class describes coin parameters and contains interactions logic
/// </summary>
public class Coin : MonoBehaviour
{
    public int value;

    public GameObject effect;

    /// <summary>
    /// Smooth movement up and down
    /// </summary>
    private void Update()
    {
        float newY = Mathf.Sin(Time.time * 5) / 25f;
        transform.position = new Vector3(transform.position.x, transform.position.y + newY, transform.position.z);
    }

    /// <summary>
    /// Check if player gets the coin
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.instance.UpdateCoins(value);
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

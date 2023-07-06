using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class describes flags parameters and contains interactions logic
/// </summary>
public class Flag : MonoBehaviour
{
    /// <summary>
    /// Check if it collides with a player. Set a new checkpoint or finish the level
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Sounds.instance.PlayFlag();
            GetComponentInChildren<ParticleSystem>().Play();
        }

        if (gameObject.CompareTag("Save Point") && collision.CompareTag("Player"))
        {
            Sounds.instance.PlayFlag();
            Player.instance.SetSpawnPosition(transform.position);
        }
        else if (gameObject.CompareTag("Finish Point") && collision.CompareTag("Player"))
        {
            Sounds.instance.PlayFlag();
            Player.instance.isAlive = false;
            Player.instance.StopAllAnimations();
            GameManager.instance.LevelEnd();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for playing all sound effects
/// </summary>
public class Sounds : MonoBehaviour
{
    public static Sounds instance;

    public AudioSource background;
    public AudioSource coin;
    public AudioSource flag;
    public AudioSource hit;
    public AudioSource hurt;
    public AudioSource jump;
    public AudioSource shot;
    public AudioSource win;
    public AudioSource bgBoss;

    void Start()
    {
        instance = this;
    }

    /// <summary>
    /// Play this sound when player collects coin
    /// </summary>
    public void PlayCoin()
    {
        coin.Play();
    }

    /// <summary>
    /// Play this sound when player reaches flag
    /// </summary>
    public void PlayFlag()
    {
        flag.Play();
    }

    /// <summary>
    /// Play this sound when player hits an enemy
    /// </summary>
    public void PlayHit()
    {
        hit.Play();
    }

    /// <summary>
    /// Play this sound when player is attacked
    /// </summary>
    public void PlayHurt()
    {
        hurt.Play();
    }

    /// <summary>
    /// Play this sound when player jumps
    /// </summary>
    public void PlayJump()
    {
        jump.Play();
    }

    /// <summary>
    /// Play this sound when player shoots
    /// </summary>
    public void PlayShot()
    {
        shot.Play();
    }

    public void WinGame()
    {
        win.Play();
    }

    /// <summary>
    /// Mute or unmute all sounds
    /// </summary>
    /// <param name="mute"></param>
    public void MuteSounds(bool mute)
    {
        if (mute)
        {
            coin.mute = true;
            flag.mute = true;
            hit.mute = true;
            hurt.mute = true;
            jump.mute = true;
            shot.mute = true;
            
        }
        else
        {
            coin.mute = false;
            flag.mute = false;
            hit.mute = false;
            hurt.mute = false;
            jump.mute = false;
            shot.mute = false;
        }
    }

    /// <summary>
    /// Mute or unmute background music
    /// </summary>
    /// <param name="mute"></param>
    public void MuteMusic(bool mute)
    {
        if (mute) {
            background.mute = true;
            
        }
            
        else
            background.mute = false;
    }

    public void MuteMusicBoss(bool mute)
    {
        if (mute) {
            bgBoss.mute = true;
            
        }
            
        else
            bgBoss.mute = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Clase para gestionar el Audio del juego
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Sonido de salto
    /// </summary>
    [SerializeField]
    private AudioSource jumpSFX;

    /// <summary>
    /// Sonido de salto
    /// </summary>
    [SerializeField]
    private AudioSource BlockHitSFX;
    /// <summary>
    /// Sonido de muerte del Goomba
    /// </summary>
    [SerializeField]
    private AudioSource goombaJumpedSFX;
    /// <summary>
    /// Música de fondo
    /// </summary>
    [SerializeField]
    private AudioSource levelMusic;
    /// <summary>
    /// Sonido al morir
    /// </summary>
    [SerializeField]
    private AudioSource deathSFX;
    /// <summary>
    /// Sonido al alcanzar el final del nivel
    /// </summary>
    [SerializeField]
    private AudioSource goalSFX;
    /// <summary>
    /// Sonido de recogida de monedas
    /// </summary>
    [SerializeField]
    private AudioSource coinPickSFX;

    void Start()
    {
        PlayMusic();
    }


    /// <summary>
    /// Reproduce el sonido de salto
    /// </summary>
    public void PlayJump()
    {
        jumpSFX.Play();
    }

    /// <summary>
    /// Reproduce el sonido de golpear un bloque
    /// </summary>
    public void PlayBlockHit()
    {
        BlockHitSFX.Play();
    }

    /// <summary>
    /// Reproduce el sonido de muerte del Goomba
    /// </summary>
    public void PlayGoombaJumped()
    {
        goombaJumpedSFX.Play();
    }


    /// <summary>
    /// Reproduce la música del nivel
    /// </summary>
    public void PlayMusic()
    {
        levelMusic.Play();
    }

    /// <summary>
    /// Reproduce el sonido de recogida de monedas
    /// </summary>
    public void PlayCoin()
    {
        coinPickSFX.Play();
    }


    /// <summary>
    /// Para todo el audio
    /// </summary>
    public void StopAllAudios()
    {
        jumpSFX.Stop();
        BlockHitSFX.Stop();
        goombaJumpedSFX.Stop();
        goalSFX.Stop();
        deathSFX.Stop();
        coinPickSFX.Stop();
        levelMusic.Stop();
    }

    /// <summary>
    /// Reproduce el sonido de muerte
    /// </summary>
    public void PlayDeathSFX()
    {
        StopAllAudios();
        deathSFX.Play();
    }


    /// <summary>
    /// Reproduce el sonido al llegar al final del nivel
    /// </summary>
    public void PlayGoalSFX()
    {
        StopAllAudios();
        goalSFX.Play();
    }
}

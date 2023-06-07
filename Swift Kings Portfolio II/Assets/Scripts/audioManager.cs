using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class audioManager : MonoBehaviour {
    [Header("Singleton")]
    public static audioManager instance;

    [Header ("Components")]
    [SerializeField] AudioSource audSFX;
    [SerializeField] AudioSource audMusic;

    [Header("SFX Audio")]
    [SerializeField] AudioClip audHover;
    [SerializeField] AudioClip audSelect;
    [SerializeField] AudioClip audBack;

    [Header("Music Audio")]
    [SerializeField] AudioClip audMenu;
    [SerializeField] AudioClip[] audGame;

    [Header("Settings")]
    [SerializeField][Range(0,1)] float audSFXVol = 1f;
    [SerializeField][Range(0,1)] float audMusicVol = 0.5f;

    /**
     * Awake
     */
    void Awake() {
        instance = this; //Only one instance of singleton
    }

    /**
     * SFX Audio
     */
    public void BtnHover() {
        audSFX.PlayOneShot(audHover, audSFXVol);
    }

    public void BtnSelect() {
        audSFX.PlayOneShot(audSelect, audSFXVol);
    }

    public void BtnBack() {
        audSFX.PlayOneShot(audBack, audSFXVol);
    }

    // Music Audio
    public void playMenuMenu() {
        audMusic.clip = audMenu;
        audMusic.volume = audMusicVol;
        audMusic.loop = true;
        audMusic.Play();
    }

    public void playRandomGame() {
        audMusic.clip = audGame[Random.Range(0, audGame.Length)];
        audMusic.volume = audMusicVol;
        audMusic.loop = true;
        audMusic.Play();
    }

    public void playLevelOne() {
        audMusic.clip = audGame[0];
        audMusic.volume = audMusicVol;
        audMusic.loop = true;
        audMusic.Play();
    }
}

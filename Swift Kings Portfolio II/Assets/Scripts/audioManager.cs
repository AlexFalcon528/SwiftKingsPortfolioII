using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class audioManager : MonoBehaviour {
    [Header("Singleton")]
    public static audioManager instance;

    [Header("Components")]
    [SerializeField] AudioSource audSFX;
    [SerializeField] AudioSource audMusic;

    [Header("Options Components")]
    [SerializeField] Slider musicVolSlider;
    [SerializeField] TMP_InputField musicVolInput;
    [SerializeField] Slider sfxVolSlider;
    [SerializeField] TMP_InputField sfxVolInput;

    [Header("SFX Audio")]
    [SerializeField] AudioClip audHover;
    [SerializeField] AudioClip audSelect;
    [SerializeField] AudioClip audBack;

    [Header("Music Audio")]
    [SerializeField] AudioClip audMenu;
    [SerializeField] AudioClip[] audGame;

    [Header("Settings")]
    [SerializeField][Range(0,1)] public float audSFXVol = 1f;
    [SerializeField][Range(0,1)] float audMusicVol = 0.5f;

    /**
     * Awake
     */
    void Awake() {
        instance = this; //Only one instance of singleton
    }

    /**
     * Start
     */
    void Start() {
        // Get Values from Player Prefs
        bool hasMusicAud = playerPrefsManager.instance.HasMusicAud();
        if (hasMusicAud) {
            audMusicVol = playerPrefsManager.instance.GetMusicAud();
        } else {
            playerPrefsManager.instance.SetMusicAud(audMusicVol);
        }

        bool hasSFXAud = playerPrefsManager.instance.HasSFXAud();
        if (hasSFXAud) {
            audSFXVol = playerPrefsManager.instance.GetSFXAud();
        } else {
            playerPrefsManager.instance.SetSFXAud(audSFXVol);
        }

        // Add Listeners to the Sliders & Inputs
        musicVolSlider.onValueChanged.AddListener(delegate { MusicVolChange(true); });
        musicVolInput.onValueChanged.AddListener(delegate { MusicVolChange(false); });
        sfxVolSlider.onValueChanged.AddListener(delegate { SFXVolChange(true); });
        sfxVolInput.onValueChanged.AddListener(delegate { SFXVolChange(false); });

        // Set Options Values
        musicVolSlider.value = audMusicVol;
        sfxVolSlider.value = audSFXVol;
        musicVolInput.text = audMusicVol.ToString();
        sfxVolInput.text = audSFXVol.ToString();
    }

    /**
     * Volume Settings Change
     */
    void MusicVolChange(bool wasSlider) {
        if (wasSlider) {
            audMusicVol = musicVolSlider.value;
            musicVolInput.text = audMusicVol.ToString();
        } else {
            // Validate if its a float
            float isFloat;
            float.TryParse(musicVolInput.text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out isFloat);
            if (isFloat != 0) {
                float f = float.Parse(musicVolInput.text);
                if (f > 1) f = 1f;
                else if (f < 0) f = 0f;
                musicVolSlider.value = f;
                audMusicVol = f;
            } else {
                musicVolInput.text = audMusicVol.ToString();
            }
        }
        playerPrefsManager.instance.SetMusicAud(audMusicVol);
        audMusic.volume = audMusicVol;
    }

    void SFXVolChange(bool wasSlider) {
        if (wasSlider) {
            audSFXVol = sfxVolSlider.value;
            sfxVolInput.text = audSFXVol.ToString();
        } else {
            // Validate if its a float
            float isFloat;
            float.TryParse(sfxVolInput.text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out isFloat);
            if (isFloat != 0) {
                float f = float.Parse(sfxVolInput.text);
                if (f > 1) f = 1f;
                else if (f < 0) f = 0f;
                sfxVolSlider.value = f;
                audSFXVol = f;
                // audSFX.PlayOneShot(audSelect, audSFXVol);
            } else {
                sfxVolInput.text = audSFXVol.ToString();
            }
        }
        playerPrefsManager.instance.SetSFXAud(audSFXVol);
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

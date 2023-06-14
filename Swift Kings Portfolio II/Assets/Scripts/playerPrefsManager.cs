using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPrefsManager : MonoBehaviour
{
    [Header("Singleton")]
    public static playerPrefsManager instance;

    /**
     * Awake
     */
    void Awake() {
        instance = this; //Only one instance of singleton
    }

    public bool HasSFXAud() {
        return PlayerPrefs.HasKey("AudSFXVol");
    }

    public float GetSFXAud() {
        return PlayerPrefs.GetFloat("AudSFXVol");
    }

    public void SetSFXAud(float vol) {
        PlayerPrefs.SetFloat("AudSFXVol", vol);
    }

    public bool HasMusicAud() {
        return PlayerPrefs.HasKey("AudMusicVol");
    }

    public float GetMusicAud() {
        return PlayerPrefs.GetFloat("AudMusicVol");
    }

    public void SetMusicAud(float vol) {
        PlayerPrefs.SetFloat("AudMusicVol", vol);
    }

    public bool HasDifficulty() {
        return PlayerPrefs.HasKey("Difficulty");
    }

    public void SetDifficulty(int difficulty) {
        PlayerPrefs.SetInt("Difficulty", difficulty);
    }

    public int GetDifficulty() {
        return PlayerPrefs.GetInt("Difficulty");
    }
}

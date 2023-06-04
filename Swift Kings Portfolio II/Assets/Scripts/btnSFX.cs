using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class btnSFX : MonoBehaviour {
    [SerializeField] AudioSource audSFX;
    [SerializeField] AudioClip audHover;
    [SerializeField] AudioClip audSelect;
    [SerializeField] AudioClip audBack;

    /**
     * Audio
     */
    public void BtnHover() {
        audSFX.PlayOneShot(audHover);
    }

    public void BtnSelect() {
        audSFX.PlayOneShot(audSelect);
    }

    public void BtnBack() {
        audSFX.PlayOneShot(audBack);
    }
}

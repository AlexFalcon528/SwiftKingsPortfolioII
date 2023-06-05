using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class gunStats : ScriptableObject
{
    [Range(2, 300)] public int shootDist;
    [Range(0.1f, 3)] public float shootRate;
    [Range(1, 10)] public int shootDamage;
    [Range(1, 4)] public int reloadRate;
    [Range(1, 100)] public int maxAmmo;
    public int currAmmo;
    public GameObject model;
    public GameObject hitEffect;
    public GameObject muzzleFlash;
    public AudioClip gunshotAud;
    [Range(0, 1)] public float gunshotAudVol;
    public AudioClip reloadAud;
    [Range(0, 1)] public float reloadAudVol;
    public AudioClip reloadOverAud;
    [Range(0, 1)] public float reloadOverAudVol;
    void Start()
    {
        currAmmo = maxAmmo;
    }
}

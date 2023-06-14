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
    [Range(1, 100)] public float recoil;
    public bool isScatter;
    public int currAmmo;
    public GameObject model;
    public GameObject hitEffect;
    public GameObject muzzleFlash;
    public AudioClip gunshotAud;
    public AudioClip reloadAud;
    public AudioClip reloadOverAud;
    void Start()
    {
        currAmmo = maxAmmo;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class playerController : MonoBehaviour, IDamage, IPhysics
{
    [Header("~~~~~~~Components~~~~~~~")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource aud;
    [SerializeField] VisualEffect sprintEffect;
    [SerializeField] public GameObject futurePos;
    [SerializeField] Transform itemPos;
    [SerializeField] GameObject OrbWeapon;
    [SerializeField] GameObject pushBackSphere;
    [SerializeField] GameObject grenade;


    [Header("\n~~~~~~~Stats~~~~~~~")]
    [Header("~~~Player~~~")]
    [SerializeField] int hp;
    [SerializeField] float speed;
    [SerializeField] float sprintMult;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravity;
    [SerializeField] int jumps;
    [SerializeField][Range(3, 9)] float jumpVelocity;
    [SerializeField] float pushBackResolve;
    [SerializeField][Range(0,60)] int pushBackAttackCD;
    [SerializeField] int grenadeCount;

    [Header("\n~~~Weapon~~~")]
    public List<gunStats> guns = new List<gunStats>();
    [SerializeField] int heldAmmo;
    [SerializeField] int orbCount;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    [SerializeField] int shootDamage;
    [SerializeField] int push;
    [SerializeField] MeshFilter gunModel;
    [SerializeField] MeshRenderer gunMat;
    public int selectedGun;
    Vector3 pushBack;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audJump;
    [SerializeField] AudioClip[] audDamage;
    [SerializeField] AudioClip[] audSteps;

    int grenadeNum;
    int jumped;
    Vector3 move;
    Vector3 velocity;
    bool isGrounded;
    bool isSprinting;
    bool isShooting;
    bool isReloading;
    int hpOriginal;
    bool stepIsPlaying;
    public bool isPoweredUp;
    bool isUsingItem;
    bool jumpPeak;
    bool dPadPressed;
    bool isPushing;

    // Start is called before the first frame update
    void Start()
    {
        hpOriginal = hp; //Store original health for respawn system
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        Sprint(); //Find out if the player is sprinting
        if (menuManager.instance.activeMenu == null)
        {
            Movement(); //Move player
            ChangeGun();
            if (menuManager.instance.activeMenu == null)
            {
                if (!isShooting && !isReloading && !isUsingItem) //If the player is pressing the shoot button and not already shooting
                {
                    if (guns.Count > 0 && (Input.GetButton("Shoot") || Input.GetAxis("Shoot") == 1)) //If the player is pressing the shoot button and not already shooting
                    {
                        StartCoroutine(Shoot());
                    }
                    if (guns.Count > 0 && Input.GetButton("Reload") && heldAmmo > 0)
                    {
                        StartCoroutine(Reload());
                    }
                    if (Input.GetButtonDown("UseItem"))
                    {
                        StartCoroutine(UseItem());
                    }
                }
                if(Input.GetButton("Push back Attack"))
                {
                    StartCoroutine(PushBackAttack());
                }
                if(Input.GetButtonDown("Grenade Throw"))
                {
                    ThrowGrenade();
                }

            }
        }
    }
    void Movement()
    {
        isGrounded = controller.isGrounded; //Check for grounded
        if (isGrounded) //If grounded and experiencing gravity
        {

            if (!stepIsPlaying && move.normalized.magnitude > 0.5f)
                StartCoroutine(playSteps());
           
            if(velocity.y < 0)
            {
                velocity.y = 0f; //Reset vertical velocity
                jumped = 0; //Reset times jumped
                jumpPeak = false;
            }

        }
        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * speed);
        futurePos.transform.position = controller.transform.localPosition + move * (speed * 0.3f);

        //Jump functionality
        if (!jumpPeak && Input.GetButton("Jump"))  //If press jump and haven't jumped more than jumps
        {
            if (Input.GetButtonDown("Jump") && jumped < jumps) //If press jump and haven't jumped more than jumps
            {
                aud.PlayOneShot(audJump[UnityEngine.Random.Range(0, audJump.Length)], audioManager.instance.audSFXVol);
                jumped++; //Jump

            }
            if (transform.position.y >= jumpHeight || Input.GetButtonUp("Jump"))
            {
                jumpPeak = true;
            }
            velocity.y = jumpVelocity; //Move up

        }

        //Gravity
        velocity.y -= gravity * Time.deltaTime;
        controller.Move((velocity + pushBack) * Time.deltaTime);
        pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackResolve);
    }
    IEnumerator DamageFlash()
    {
        menuManager.instance.damageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        menuManager.instance.damageFlash.SetActive(false);
    }
    void Sprint()
    {
        //If holding down sprint button
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true; //Sprint
            speed *= sprintMult; //Apply speed multiplier
            sprintEffect.enabled = true; // turns on sprint effect
        }
        else if (Input.GetButtonUp("Sprint")) //Once sprint button is let go
        {
            isSprinting = false; //Stop sprinting
            speed /= sprintMult; //Unapply speed multiplier
            sprintEffect.enabled = false;//turns off sprint effect
        }
    }

    IEnumerator playSteps()
    {
        stepIsPlaying = true;
        float stepVol = audioManager.instance.audSFXVol - 0.4f;
        if(stepVol < 0 && audioManager.instance.audSFXVol > 0) stepVol = 0.1f;
        aud.PlayOneShot(audSteps[UnityEngine.Random.Range(0, audSteps.Length)], stepVol);
        if (!isSprinting)
            yield return new WaitForSeconds(0.5f);
        else
            yield return new WaitForSeconds(0.3f);
        stepIsPlaying = false;
    }
    public void gunPickup(gunStats gunStat)
    {
        guns.Add(gunStat);

        shootDamage = gunStat.shootDamage;
        shootDist = gunStat.shootDist;
        shootRate = gunStat.shootRate;
        gunModel.transform.localScale = gunStat.model.transform.localScale;

        gunModel.mesh = gunStat.model.GetComponent<MeshFilter>().sharedMesh;
        gunMat.material = gunStat.model.GetComponent<MeshRenderer>().sharedMaterial;

        selectedGun = guns.Count - 1;
        UpdateUI();
    }
    public void ammoPickup(int ammo)
    {
        heldAmmo += ammo;
        orbCount++;
        UpdateUI();
    }

    IEnumerator Shoot()
    {
        if (guns[selectedGun].currAmmo > 0)
        {
            guns[selectedGun].currAmmo--;

            aud.PlayOneShot(guns[selectedGun].gunshotAud, audioManager.instance.audSFXVol);

            isShooting = true;
            UpdateUI();

            RaycastHit hit;
            if (!guns[selectedGun].isScatter)
            {
                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
                {
                    IDamage damageable = hit.collider.GetComponent<IDamage>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(shootDamage);
                    }
                    IPhysics physicsable = hit.collider.GetComponent<IPhysics>();
                    if (physicsable != null)
                    {
                        Vector3 dirPush = hit.transform.position - transform.position;//push direction
                        physicsable.TakePushBack(dirPush * push);//push them
                    }
                    Instantiate(guns[selectedGun].hitEffect, hit.point, guns[selectedGun].hitEffect.transform.rotation);
                }
            }
            else
            {
                Vector3 aimDirection = Camera.main.gameObject.transform.forward;
                Vector3 spread = Vector3.zero;
                for (int i = 0; i <= 4; i++)
                {
                    aimDirection = Camera.main.gameObject.transform.forward;
                    spread += Vector3.up * UnityEngine.Random.Range(-1f, 1f);
                    spread += Vector3.right * UnityEngine.Random.Range(-1f, 1f);
                    aimDirection += spread.normalized * UnityEngine.Random.Range(0, 0.3f);
                    if (Physics.Raycast(Camera.main.gameObject.transform.position + (Camera.main.gameObject.transform.forward * 0.3f), aimDirection, out hit, shootDist))
                    {
                        //UnityEngine.Debug.DrawLine(Camera.main.gameObject.transform.position, hit.point, Color.green, 1f);
                        IDamage damageable = hit.collider.GetComponent<IDamage>();
                        if (damageable != null)
                        {
                            damageable.TakeDamage(shootDamage);
                        }
                        IPhysics physicsable = hit.collider.GetComponent<IPhysics>();
                        if (physicsable != null)
                        {
                            Vector3 dirPush = hit.transform.position - transform.position;//push direction
                            physicsable.TakePushBack(dirPush * push);//push them
                        }
                        Instantiate(guns[selectedGun].hitEffect, hit.point, guns[selectedGun].hitEffect.transform.rotation);
                    }
                    /*else
                    {
                        UnityEngine.Debug.DrawLine(Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.position + aimDirection*shootDist, Color.red, 1f);
                    }*/
                }
            }

            gameManager.instance.cScript.xRotation -= guns[selectedGun].recoil * 0.1f;
            controller.Move(transform.forward * (guns[selectedGun].recoil *-0.5f) * Time.deltaTime);
            yield return new WaitForSeconds(shootRate);

            isShooting = false;
        }
        else
        {
            if (heldAmmo > 0)
            {
                StartCoroutine(Reload());
            }
            
        }
    }
    IEnumerator Reload()
    {
        isReloading = true;
        while (guns[selectedGun].currAmmo < guns[selectedGun].maxAmmo && heldAmmo>0)
        {
            guns[selectedGun].currAmmo++;
            heldAmmo--;
            if (guns[selectedGun].currAmmo == guns[selectedGun].maxAmmo)
            {
                aud.PlayOneShot(guns[selectedGun].reloadOverAud, audioManager.instance.audSFXVol);
            }
            else
            {
                aud.PlayOneShot(guns[selectedGun].reloadAud, audioManager.instance.audSFXVol);
            }

            UpdateUI();
            yield return new WaitForSeconds((float)guns[selectedGun].reloadRate / guns[selectedGun].maxAmmo);

        }

        isReloading = false;
    }
    IEnumerator UseItem()
    {
        isUsingItem = true;
        if (orbCount > 0)
        {
            //aud.PlayOneShot();
            CreateOrb();
            yield return new WaitForSeconds(2);
            orbCount--;
        }
        isUsingItem = false;
    }

    public void CreateOrb()
    {
        Instantiate(OrbWeapon, itemPos.position, Camera.main.transform.rotation);
    }
    void ChangeGun()
    {

        if (Input.GetAxis("Mouse ScrollWheel") > 0 || (Input.GetAxis("Swap Weapon") > 0 && !dPadPressed))
        {
            if(Input.GetAxis("Swap Weapon") > 0) dPadPressed = true;
            if(selectedGun < guns.Count - 1)
            {
                selectedGun++;
            }
            else
            {
                selectedGun = 0;
            }
            
            ChangeGunStats();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 || (Input.GetAxis("Swap Weapon") < 0 && !dPadPressed))
        {
            if (Input.GetAxis("Swap Weapon") < 0) dPadPressed = true;
            if (selectedGun > 0)
            {
                selectedGun--;
            }
            else
            {
                selectedGun = guns.Count-1; ;
            }
            ChangeGunStats();
        } else if (Input.GetAxis("Swap Weapon") == 0 && dPadPressed) dPadPressed= false;

    }
    void ChangeGunStats()
    {
        if (guns[selectedGun] != null)
        {
            shootDamage = guns[selectedGun].shootDamage;
            shootDist = guns[selectedGun].shootDist;
            shootRate = guns[selectedGun].shootRate;

            gunModel.mesh = guns[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
            gunMat.material = guns[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;
            gunModel.transform.localScale = guns[selectedGun].model.transform.localScale;
            UpdateUI();
        }
    }
    public void TakeDamage(int dmg)
    {
        hp -= dmg;//Subtract damage taken
        if(hp <= hpOriginal * 0.3)
        {
            gameManager.instance.LowHealth();
        }
        else
        {
            gameManager.instance.HighHealth();
        }
        if (hp <= 0)//If hp is less than or = to 0
        {
            gameManager.instance.HighHealth();
            if (!gameManager.instance.isDead)
            {
                gameManager.instance.isDead = true;
                gameManager.instance.YouLose(); //Lose the game
            }
        }
        else {StartCoroutine(DamageFlash());
    }
        UpdateUI();
    }
    public void TakePushBack(Vector3 dir)
    {
        pushBack -= dir;// bullets and explosions push 

        UpdateUI(); // Update the UI since variables updated
    }
    public void SpawnPlayer()
    {
        controller.enabled = false; //Disable CharacterController to allow manual position setting
        transform.position = gameManager.instance.spawnPoint.transform.position; //Set the position to where the player is supposed to spawn
        velocity = Vector3.zero; //Reset all velocity
        controller.enabled = true; //Reenable controller to allow for the movement functions to work
        hp = hpOriginal; //Reset the player's hp to the original amount
        UpdateUI(); // Update the UI since variables updated
        gameManager.instance.isDead = false;
    }
    public void HealPlayer(int amount)
    {
        hp += amount; //Increase hp
        UpdateUI(); // Update the UI since variables updated
    }

    void UpdateUI() {
        gameManager.instance.Healthbar.fillAmount = (float)hp / hpOriginal; // Set Healthbar fill to the amount of hp compared to original
        gameManager.instance.healthBarText.text = hp.ToString(); // numerical display of hp
        gameManager.instance.heldAmmo.text = $"{heldAmmo}";

        // Update Weapon Ammo Display
        
        if (guns.Count > 0)
            gameManager.instance.weaponAmmoText.text = $"{guns[selectedGun].currAmmo} / {guns[selectedGun].maxAmmo}";
        
     }
    public Vector3 getVelocity()
    {
        return controller.velocity;
    }

    public void powerUpSpeed()
    {
        speed += 2;
        isPoweredUp = true;
    }


    public void powerUpHP()
    {
        hp += 5;
        isPoweredUp = true;
        UpdateUI();
    }

    public void powerUpDMG()
    {
        shootDamage += 5;
        isPoweredUp = true;
    }
    IEnumerator PushBackAttack()
    {
        if (!isPushing)
        {
            isPushing = true;
            pushBackSphere.SetActive(true);
            yield return new WaitForSeconds(.5f);
            pushBackSphere.SetActive(false);
            yield return new WaitForSeconds(pushBackAttackCD);
            isPushing = false;
        }
    }
    public void GrenadePickUp()
    {
        grenadeCount++;
       
    }
    void ThrowGrenade()
    {
        if(grenadeCount>0)
        {
            grenadeCount--;
            Instantiate(grenade, itemPos.position, Camera.main.transform.rotation); 
            UpdateUI();
            
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.Http.Headers;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [Range(1, 5)][SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [Range(1, 10)][SerializeField] int hp;
    [Range(1, 5)][SerializeField] float playerSpeed;
    [Range(2, 5)][SerializeField] float sprintMod;
    [Range(8, 25)][SerializeField] float jumpHeight;
    [Range(10, 50)][SerializeField] float gravityValue;
    [Range(1, 3)][SerializeField] int maxJumps;

    [Header("----- Weapon Stats -----")]
    [Range(2,300)][SerializeField] int shootDist;
    [Range(0.1f,3)][SerializeField] float shootRate;
    [Range(1,10)][SerializeField] int shootDamage;

    private int timesJumped;
    private Vector3 move;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool isSprinting;
    private bool isShooting;
    private int hpOrig;

    private void Start()
    {
        hpOrig = hp;
        spawnPlayer();
    }

    void Update()
    {

        Sprint();
        if (gameManager.instance.activeMenu == null)
        {
            Movement();
            if (Input.GetButton("Shoot") && !isShooting)
            {
                StartCoroutine(Shoot());
            }
        }

        if(Input.GetButton("Shoot") && !isShooting)
        {
            StartCoroutine(Shoot());
        }
        
    }
    void Movement(){
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            timesJumped = 0;
        }

        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);



        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && timesJumped < maxJumps)
        {
            timesJumped++;
            playerVelocity.y += jumpHeight;
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            playerSpeed *= sprintMod;
        }
        else if(Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            playerSpeed /= sprintMod;
        }
    }
    IEnumerator Shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f,0.5f)), out hit, shootDist)) 
        {
            IDamage damageable = hit.collider.GetComponent<IDamage>();
            if(damageable != null)
            {
                damageable.takeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }
    public void takeDamage(int dmg)
    {
        hp-=dmg;
        if (hp <= 0)
        {
            gameManager.instance.youLose();
        }
    }
    public void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
        hp = hpOrig;
    }
    public void playerHeal(int amount)
    {
        hp += amount;
    }
}

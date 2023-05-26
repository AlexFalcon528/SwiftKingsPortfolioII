using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    [Header("~~~~~~~Singleton~~~~~~~")]
    public static gameManager instance;
    [Header("\n~~~~~~~Player~~~~~~~")]
    public GameObject player;
    public playerController pScript;
    public GameObject spawnPoint;
    [Header("\n~~~~~~~User Interface~~~~~~~")]
    public GameObject activeMenu;
    public GameObject pause;
    public GameObject lose;
    public GameObject win;
    public GameObject mainMenu;
    public GameObject gamemodes;
    public GameObject reticle;
    public GameObject objectiveParent;
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI waveText;
    public GameObject HealthbarParent;
    public Image Healthbar;
    public TextMeshProUGUI healthBarText;
    public GameObject weaponAmmoParent;
    public TextMeshProUGUI weaponAmmoText;
    public bool isPaused; 
    public int enemiesRemaining;
    public GameObject playerDamageFlash;
    [Header("\n~~~~~~~~Gameplay~~~~~~~~~~~")]
    public int wave;
    [SerializeField] int finalWave;
    float originalTimeScale;
    public bool nextWave;
    [Header("\n~~~~~~~~Minions Tracker~~~~~~~~~~~")]
    public int numberOfMinions;
    [Range(1, 30)] [SerializeField] public int maxNumberOfMinions;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this; //Only one instance of singleton
        player = GameObject.FindWithTag("Player"); //Find player
        spawnPoint = GameObject.FindWithTag("Spawnpoint"); //Find spawnpoint
        if(SceneManager.GetActiveScene().name != "LandingScene") pScript = player.GetComponent<playerController>();
        originalTimeScale = Time.timeScale; //Save original time scale for later use
        nextWave = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel")&&activeMenu == null) //When pressing cancel button and not in a menu already
        {
            PauseState();//Pause game
            activeMenu = pause; //Store pause menu as active menu
            activeMenu.SetActive(isPaused); //Activate active menu
            
        }
    }

    public void PauseState()
    {
        isPaused = true;//Pause
        Time.timeScale = 0; //Stop physics and time
        Cursor.visible = true; //Make cursor visible but confined within the screen of the game
        Cursor.lockState = CursorLockMode.Confined;
        handleGameUI(false); // Disable Gameplay UI Component
    }

    public void UnpauseState()
    {
        Time.timeScale = originalTimeScale; //Reset time
        Cursor.visible = false; //Relock mouse and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false; //Unpause
        activeMenu.SetActive(false); //Deactivate current menu
        activeMenu = null;//Unstore current menu
        handleGameUI(true); // Reactivate Gameplay UI Component
    }

    public void HandleReturnMenu() {
        Time.timeScale = originalTimeScale; //Reset time
        isPaused = false; //Unpause
        activeMenu.SetActive(false); //Deactivate current menu
        activeMenu = null; //Unstore current menu
    }

    public void YouLose()
    {
        PauseState(); //Pause
        activeMenu = lose; //Set current menu to the lose menu
        activeMenu.SetActive(true); //Show lose menu
        playerDamageFlash.SetActive(false);
    }

    public void UpdateMinionsCounter(int amount)
    {
        numberOfMinions += amount;
    }

    public void UpdateGameGoal(int amount) {
        enemiesRemaining += amount;
        if (enemiesRemaining <= 0)
        {
            nextWave = true;
            wave++;
            if (wave > finalWave)
            {
                StartCoroutine(YouWin());
            }
        }
        objectiveText.text = $"Enemies: {enemiesRemaining}";
        waveText.text = $"Wave: {wave}";
    }

    IEnumerator YouWin() {
        yield return new WaitForSeconds(1);
        PauseState();
        activeMenu = win;
        activeMenu.SetActive(true);
    }

    void handleGameUI(bool active) {
        reticle.SetActive(active); // Active/Deactivate the reticle
        HealthbarParent.SetActive(active); // Active/Deactivate the healthbar
        weaponAmmoParent.SetActive(active); // Active/Deactivate the Weapons Ammo Display
        objectiveParent.SetActive(active); // Active/Deactivate the Objective
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    [Header("~~~~~~~Singleton~~~~~~~")]
    public static gameManager instance;
    [Header("\n~~~~~~~Player~~~~~~~")]
    public GameObject player;
    public Camera mCamera;
    public playerController pScript;
    public GameObject spawnPoint;
    [Header("\n~~~~~~~User Interface~~~~~~~")]
    private float origTimeScale;

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
    [Header("\n~~~~~~~~Gameplay~~~~~~~~~~~")]
    public int wave;
    [SerializeField] int finalWave;
    public bool nextWave;
    [Header("\n~~~~~~~~Minions Tracker~~~~~~~~~~~")]
    public int numberOfMinions;
    [Range(1, 30)] [SerializeField] public int maxNumberOfMinions;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this; //Only one instance of singleton
        player = GameObject.FindWithTag("Player"); //Find player
        spawnPoint = GameObject.FindWithTag("Spawnpoint"); //Find spawnpoint
        mCamera = player.gameObject.GetComponent<Camera>();

        if (SceneManager.GetActiveScene().name != "LandingScene")
            pScript = player.GetComponent<playerController>();

        nextWave = true;
    }

    private void Start() {
        StartCoroutine(menuManager.instance.WaitToUnfade());
        // Start Menu Music if on Main Menu
        if (SceneManager.GetActiveScene().name == "LandingScene")
        {
            audioManager.instance.playMenuMenu();
        }
        origTimeScale = Time.timeScale;
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

    /**
     * Pause/Unpause State Handling
     */
    public void PauseState() {
        isPaused = true;// Pause
        origTimeScale = Time.timeScale; // Set current timescale
        Time.timeScale = 0; // Stop physics and time
        Cursor.visible = true; // Make cursor visible but confined within the screen of the game
        Cursor.lockState = CursorLockMode.Confined;
        // handleGameUI(false); // Disable Gameplay UI Component
    }

    public void UnpauseState() {
        Time.timeScale = origTimeScale; // Reset time
        Cursor.visible = false; // Relock mouse and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false; // Unpause
        menuManager.instance.DeactiveAllMenus();
        // handleGameUI(true); // Reactivate Gameplay UI Component
    }

    public void HandleReturnMenu() {
        Time.timeScale = origTimeScale; //Reset time
        isPaused = false; //Unpause
        menuManager.instance.DeactiveAllMenus(); // Deactive Menus
    }


    /**
     * Win / Lose Condition Handling
     */
    public void YouLose() {
        PauseState(); //Pause
        menuManager.instance.OpenLose();
    }

    public IEnumerator YouWin() {
        yield return new WaitForSeconds(1);
        PauseState();
        menuManager.instance.OpenWin();
    }
}

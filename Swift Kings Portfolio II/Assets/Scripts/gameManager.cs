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
    [Header("~~~~~~~Player~~~~~~~")]
    public GameObject player;
    public Camera mCamera;
    public playerController pScript;
    public GameObject spawnPoint;
    public bool isDead = false;

    [Header("~~~~~~~User Interface~~~~~~~")]
    private float origTimeScale = 1;
    private float timescale;

    [SerializeField] GameObject lowHealthIndicator;
    public GameObject reticle;
    public GameObject objectiveParent;
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI waveText;
    public GameObject HealthbarParent;
    public Image Healthbar;
    public TextMeshProUGUI healthBarText;
    public GameObject weaponAmmoParent;
    public TextMeshProUGUI weaponAmmoText;
    public TextMeshProUGUI heldAmmo;
    public TextMeshProUGUI  currPoints;
    public bool isPaused;
    public int enemiesRemaining;
    public int highScore;
    public int currentScore;
    public int points;
    [Header("\n~~~~~~~~Gameplay~~~~~~~~~~~")]
    public int wave;
    [SerializeField] int finalWave;
    public bool nextWave;
    [Range(1, 3)]public int difficulty = 2; 
    [Header("\n~~~~~~~~Minions Tracker~~~~~~~~~~~")]
    public int numberOfMinions;
    [Range(1, 30)] [SerializeField] public int maxNumberOfMinions;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this; //Only one instance of singleton
        player = GameObject.FindWithTag("Player"); //Find player
        spawnPoint = GameObject.FindWithTag("Spawnpoint"); //Find spawnpoint
        
        if (SceneManager.GetActiveScene().name != "LandingScene") {
            mCamera = player.gameObject.GetComponent<Camera>();
            pScript = player.GetComponent<playerController>();
        }

        nextWave = true;
    }

    private void Start() {
        points = 50;
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            highScore = PlayerPrefs.GetInt("HighScore");
        }
        else if (SceneManager.GetActiveScene().name == "Survive")
        {
            highScore = PlayerPrefs.GetInt("SurviveHighScore");
        }

        StartCoroutine(menuManager.instance.WaitToUnfade());
        // Start Menu Music if on Main Menu
        if (SceneManager.GetActiveScene().name == "LandingScene")
        {
            audioManager.instance.playMenuMenu();
        } else {
            audioManager.instance.playRandomGame();
        }
    }

    private void Update() {
        timescale = Time.timeScale;
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
        Time.timeScale = 0; // Stop physics and time
        Cursor.visible = true; // Make cursor visible but confined within the screen of the game
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void UnpauseState() {
        Time.timeScale = origTimeScale; // Reset time
        Cursor.visible = false; // Relock mouse and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        menuManager.instance.DeactiveAllMenus();
        isPaused = false;
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
        if (highScore < currentScore)
        {
            highScore = currentScore;
            if (SceneManager.GetActiveScene().name == "SampleScene")
            {
                PlayerPrefs.SetInt("HighScore",highScore);
            }
            else if (SceneManager.GetActiveScene().name == "Survive")
            {
                PlayerPrefs.SetInt("SurviveHighScore",highScore);
            }
        }
        menuManager.instance.OpenLose();
    }

    public IEnumerator YouWin() {
        yield return new WaitForSeconds(1);
        PauseState();
        menuManager.instance.OpenWin();
    }

    public void LowHealth()
    {
        lowHealthIndicator.SetActive(true);
        StartCoroutine(HeartBeat());
        
    }
    IEnumerator HeartBeat() 
    {
        Image image = lowHealthIndicator.GetComponent<Image>();

        while (true)
        {
            image.CrossFadeAlpha(0.1f, 2f, false);
            yield return new WaitForSeconds(2f);
            image.CrossFadeAlpha(0.5f, 2f, false);
            yield return new WaitForSeconds(2f);
        }
    }
    public void HighHealth()
    {
        StopCoroutine(HeartBeat());
        lowHealthIndicator.SetActive(false);
        
    }
}

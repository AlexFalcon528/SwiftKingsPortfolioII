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
    public cameraController cScript;
    public GameObject spawnPoint;
    public bool isDead = false;

    [Header("~~~~~~~User Interface~~~~~~~")]
    private float origTimeScale = 1;

    [SerializeField] GameObject lowHealthIndicator;
    public GameObject reticle;
    public GameObject gameplayUI;
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI waveText;
    public GameObject HealthbarParent;
    public Image Healthbar;
    public TextMeshProUGUI healthBarText;
    public GameObject weaponAmmoParent;
    public TextMeshProUGUI weaponAmmoText;
    public TextMeshProUGUI heldAmmo;
    public TextMeshProUGUI heldGrenades;
    public TextMeshProUGUI heldOrbs;
    public TextMeshProUGUI  currPoints;
    public GameObject gunPickupIcon;
    public TextMeshProUGUI gunPrice;
    public bool isPaused;
    public int enemiesRemaining;
    public int highScore;
    public int currentScore;
    public int points;
    [Header("\n~~~~~~~~Gameplay~~~~~~~~~~~")]
    [Range(1, 3)]public int difficulty = 2;
    public int wave;
    public int finalWave;
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
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 50;

        if (SceneManager.GetActiveScene().name != "LandingScene") {
            gameplayUI.SetActive(true);
            mCamera = player.gameObject.GetComponentInChildren<Camera>();
            cScript = mCamera.GetComponent<cameraController>();
            player.TryGetComponent<playerController>(out pScript);
        }

        nextWave = true;
    }

    private void Start() {
        bool hasDifficulty = playerPrefsManager.instance.HasDifficulty();
        if (hasDifficulty)
        {
            difficulty = playerPrefsManager.instance.GetDifficulty();
        }
        else
        {
            playerPrefsManager.instance.SetDifficulty(difficulty);
        }

        if(!playerPrefsManager.instance.HasLevel()) playerPrefsManager.instance.SetLevel(1);
        if (!playerPrefsManager.instance.HasSurviveLevel()) playerPrefsManager.instance.SetSurviveLevel(1);

        points = 50;
        LoadHighScore();

        StartCoroutine(menuManager.instance.WaitToUnfade());
        // Start Menu Music if on Main Menu
        if (SceneManager.GetActiveScene().name == "LandingScene")
        {
            audioManager.instance.playMenuMenu();
        } else {
            audioManager.instance.playRandomGame();
        }
    }
    public void LoadHighScore()
    {
        PlayerPrefs.GetInt($"{SceneManager.GetActiveScene().name}", highScore);
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
            pScript.GrenadePickUp();
            pScript.GrenadePickUp();
            points += 25;
            currentScore += 25;
            currPoints.text = $"{points}";
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
            PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().name}", highScore);
        }
        menuManager.instance.OpenLose();
    }

    public IEnumerator YouWin() {
        yield return new WaitForSeconds(1);
        PauseState();
        if (highScore < currentScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().name}", highScore);
        }
        menuManager.instance.OpenWin();
        WinProgression();
    }

    public void WinProgression() {
        // Get Currently Level & Scene
        int eliminateLvl = playerPrefsManager.instance.GetLevel();
        int surviveLvl = playerPrefsManager.instance.GetSurviveLevel();
        string currentScene = SceneManager.GetActiveScene().name;

        // Update Progression based off current level and completed scene
        if (eliminateLvl < 2 && currentScene == "SampleScene") playerPrefsManager.instance.SetLevel(eliminateLvl + 1);
        else if (eliminateLvl == 2 && currentScene == "Exterminate") playerPrefsManager.instance.SetLevel(eliminateLvl + 1);
        else if (surviveLvl < 2 && currentScene == "Survive") playerPrefsManager.instance.SetSurviveLevel(surviveLvl + 1);
    }

    /**
     * Health Feedback
     */
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
